using InvoiceEvaluationService.Helpers;

namespace InvoiceEvaluationAPI.Test.IntegrationTests
{
    [TestFixture]
    public class FileHelperTests
    {
        [Test]
        public void ConvertFileToBase64_ShouldReturnValidBase64String()
        {

            string filePath = "test_evaluation_file.txt";
            File.WriteAllText(filePath, "Sample Evaluation Content");


            string base64String = EvaluationFileHelper.ConvertFileToBase64(filePath);

            // Assert
            Assert.That(string.IsNullOrEmpty(base64String), Is.False, "Base64 string should not be null or empty.");
            Assert.DoesNotThrow(() => Convert.FromBase64String(base64String), "Base64 string should be valid.");

            // Clean up
            File.Delete(filePath);
        }
    }
}
