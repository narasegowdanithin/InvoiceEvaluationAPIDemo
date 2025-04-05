using System;
using System.Collections.Generic;

namespace InvoiceEvaluationService.Models
{
    public class InvoiceEvaluationResult
    {
        public string EvaluationId { get; set; }  // Unique ID for this evaluation
        public string InvoiceId { get; set; }     // The Invoice ID
        public List<string> RulesApplied { get; set; }  // List of rules applied to this invoice
        public string Classification { get; set; }  // Classification result (e.g., "WaterLeakDetection")
        public string EvaluationFile { get; set; }  // Base64-encoded evaluation file (text summary)
    }
}