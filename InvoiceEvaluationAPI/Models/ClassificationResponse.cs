using System.Text.Json.Serialization;

namespace InvoiceEvaluationAPI.Models
{
    public class ClassificationResponse
    {
        public string Classification { get; set; }

        public string RiskLevel { get; set; }
    }
}
