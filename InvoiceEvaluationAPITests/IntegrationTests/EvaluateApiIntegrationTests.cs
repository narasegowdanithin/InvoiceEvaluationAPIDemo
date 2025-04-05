using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using InvoiceEvaluationAPI.Test.Helpers;
using InvoiceEvaluationAPI.Tests.Models;

namespace InvoiceEvaluationAPI.Test.IntegrationTests
{
    [TestFixture]
    public class InvoiceEvaluationIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task Evaluate_ValidInvoiceData_ShouldReturnOkResponse()
        {
            // Arrange: Create a valid invoiceDto
            var invoiceDto = new
            {
                InvoiceId = "12345",
                InvoiceNumber = "S12345",
                InvoiceDate = "2025-04-03",
                Comment = "Test invoice",
                Amount = 500
            };

            // Create the JSON content for the invoiceDto
            var jsonContent = new StringContent(JsonConvert.SerializeObject(invoiceDto), Encoding.UTF8, "application/json");

            // Create a MultipartFormDataContent to simulate the file upload
            var multipartContent = new MultipartFormDataContent();

            // Add the mock document to the multipart content
            var documentContent = MockDocumentHelper.CreateMockDocument();
            documentContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            multipartContent.Add(documentContent, "document", "mock_invoice.pdf"); // "document" is the form field name
            multipartContent.Add(new StringContent(invoiceDto.InvoiceId), "InvoiceId");
            multipartContent.Add(new StringContent(invoiceDto.InvoiceNumber), "InvoiceNumber");
            multipartContent.Add(new StringContent(invoiceDto.InvoiceDate), "InvoiceDate");
            multipartContent.Add(new StringContent(invoiceDto.Comment), "Comment");
            multipartContent.Add(new StringContent(invoiceDto.Amount.ToString()), "Amount");
   
            var response = await _client.PostAsync("/api/evaluate", multipartContent);
        
            // Assert: Verify that the response is OK (200)
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var responseBody = await response.Content.ReadAsStringAsync();
            var evaluationResult = JsonConvert.DeserializeObject<EvaluationResultDto>(responseBody);

            // Assert: Ensure the evaluation result contains the expected fields
            Assert.That(evaluationResult, Is.Not.Null);
            Assert.That(evaluationResult.EvaluationId, Does.StartWith("EVAL"));
            Assert.That(evaluationResult.EvaluationFile, Is.Not.Null);
        }
    }
}
