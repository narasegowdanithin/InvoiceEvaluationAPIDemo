using InvoiceEvaluationService.Helpers;

namespace InvoiceEvaluationAPI.Test.IntegrationTests
{
    [TestFixture]
    public class EvaluationHelperTests
    {
        [Test]
        public void GenerateEvaluationFile_ShouldCreateFileWithCorrectContent()
        {
            // Arrange
            string evaluationId = "EVAL123";
            string invoiceId = "12345";
            string classification = "WaterLeakDetection";
            var rulesApplied = new List<string> { "Approve" };

            // Act
            string filePath = EvaluationFileHelper.GenerateEvaluationFile(evaluationId, invoiceId, classification, rulesApplied);

            // Assert
            Assert.That(File.Exists(filePath), Is.True, "The evaluation file should be created.");

            // Read file content and check if it contains expected information
            string fileContent = File.ReadAllText(filePath);
            Assert.That(fileContent, Does.Contain("Evaluation Summary Report"));
            Assert.That(fileContent, Does.Contain($"Evaluation ID: {evaluationId}"));
            Assert.That(fileContent, Does.Contain($"Invoice ID: {invoiceId}"));
            Assert.That(fileContent, Does.Contain($"Classification: {classification}"));
            Assert.That(fileContent, Does.Contain("Rules Applied:"));
            Assert.That(fileContent, Does.Contain("Approve"));
        }
    }
}
