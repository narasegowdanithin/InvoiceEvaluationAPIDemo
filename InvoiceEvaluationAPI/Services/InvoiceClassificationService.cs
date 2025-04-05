using Polly;
using RestSharp;

namespace InvoiceEvaluationAPI.Services
{
    public class InvoiceClassificationService
    {
        private readonly IRestClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public InvoiceClassificationService(IRestClient client, ILogger<InvoiceClassificationService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<string> ClassifyInvoiceAsync(string invoiceId)
        {
            try
            {
                // Simulating calling a mock 3rd-party API
                //var request = new RestRequest("mockapi/classify", Method.Get); //From postman for getting classification data
                var request = new RestRequest("classify", Method.Get); //locally created api for getting the classification data
                request.AddQueryParameter("invoiceId", invoiceId);
                

                // Using Polly to retry 3 times in case of failure
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                // Make the actual request with retry logic
                var response = await policy.ExecuteAsync(async () =>
                {
                    var apiResponse = await _client.ExecuteAsync(request);
                    if (!apiResponse.IsSuccessful)
                    {
                        _logger.LogError("Failed to call 3rd-party API: {StatusCode} - {Message}",
                            apiResponse.StatusCode, apiResponse.Content);
                        throw new Exception("Failed to get a response from the classification API.");
                    }
                    _logger.LogInformation("API Response: {ApiResponse}", apiResponse.Content);
                    return apiResponse.Content;
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during invoice classification.");
                throw;  // Rethrow exception to be handled by the calling method
            }
        }
    }
}
