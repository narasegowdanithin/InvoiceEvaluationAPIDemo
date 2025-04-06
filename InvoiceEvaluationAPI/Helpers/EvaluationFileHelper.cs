using System;
using System.IO;
using System.Text;

namespace InvoiceEvaluationService.Helpers
{
    public static class EvaluationFileHelper
    {
        // Method to generate the evaluation summary file and return the file path
        public static string GenerateEvaluationFile(string evaluationId, string invoiceId, string classification, List<string> rulesApplied)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Evaluation Summary Report");
            sb.AppendLine("-------------------------");
            sb.AppendLine($"Evaluation ID: {evaluationId}");
            sb.AppendLine($"Invoice ID: {invoiceId}");
            sb.AppendLine($"Classification: {classification}");
            sb.AppendLine("Rules Applied:");

            foreach (var rule in rulesApplied)
            {
                sb.AppendLine($"- {rule}");
            }

            sb.AppendLine("\nEvaluation Result: The evaluation was successfully processed.");
            sb.AppendLine("\nThank you for using our evaluation service.");

            // File path where the evaluation file will be saved
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Evaluations");

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Generate a file path using the evaluation ID and invoice ID
            string filePath = Path.Combine(directoryPath, $"{evaluationId}_{invoiceId}_evaluation_summary.txt");

            // Write the evaluation content to the file
            File.WriteAllText(filePath, sb.ToString());

            return filePath; // Return the local file path where the file is saved
        }
        public static string ConvertFileToBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }
    }
}
