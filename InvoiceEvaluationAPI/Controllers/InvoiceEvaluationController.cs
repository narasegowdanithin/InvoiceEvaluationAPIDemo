using InvoiceEvaluationAPI.Models;
using InvoiceEvaluationAPI.Services;
using InvoiceEvaluationService.Helpers;
using InvoiceEvaluationService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InvoiceEvaluationAPI.Controllers
{
    [ApiController]
    [Route("api/evaluate")]
    public class InvoiceEvaluationController : ControllerBase
    {
        private readonly ILogger<InvoiceEvaluationController> _logger;
        private readonly InvoiceClassificationService _classificationService;
        private readonly RuleEngineService _ruleEngine;



        public InvoiceEvaluationController(ILogger<InvoiceEvaluationController> logger, InvoiceClassificationService classificationService, RuleEngineService ruleEngine)
        {
            _logger = logger;
            _classificationService = classificationService;
            _ruleEngine = ruleEngine;
        }

        [HttpPost]
        public async Task<IActionResult> EvaluateInvoice([FromForm] InvoiceEvaluationRequest request)
        {
            try
            {
                _logger.LogInformation("Received invoice evaluation request for Invoice ID: {InvoiceId}", request.InvoiceId);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validation failed: {Errors}", ModelState);
                    return BadRequest(ModelState);
                }

                // Save the file
                var filePath = Path.Combine("uploads", $"{request.InvoiceId}.pdf");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Document.CopyToAsync(stream);
                }

                // Classify the invoice using the mock API
                // Call the service to classify the invoice and get the raw JSON response
                var apiResponse = await _classificationService.ClassifyInvoiceAsync(request.InvoiceId);
                var classificationData = JsonSerializer.Deserialize<ClassificationResponse>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ensures it matches JSON fields even if casing differs
                });

                _logger.LogInformation("Invoice document saved successfully at {FilePath}", filePath);


                // Apply decision rules
                var rulesApplied = _ruleEngine.ApplyRules(request.Amount, classificationData.Classification, classificationData.RiskLevel);
                _logger.LogInformation("Rules applied: {RulesApplied}", string.Join(", ", rulesApplied));

                string evaluationId = $"EVAL{Guid.NewGuid().ToString().Substring(0, 6)}";
                string filepath = EvaluationFileHelper.GenerateEvaluationFile(evaluationId, request.InvoiceId, classificationData.Classification, rulesApplied);

                string base64File = EvaluationFileHelper.ConvertFileToBase64(filePath);

                // Create the result object
                var evaluationResult = new InvoiceEvaluationResult
                {
                    EvaluationId = evaluationId,
                    InvoiceId = request.InvoiceId,
                    RulesApplied = rulesApplied,
                    Classification = classificationData.Classification,
                    EvaluationFile = base64File
                };

                return Ok(evaluationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invoice.");
                return StatusCode(500, new { message = "Internal server error. Please try again later." });
            }
        }
    }
}
