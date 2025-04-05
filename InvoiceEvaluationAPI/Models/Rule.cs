namespace InvoiceEvaluationAPI.Models
{
    public class Rule
    {
        public int RuleId { get; set; }
        public string Condition { get; set; }
        public string Action { get; set; }
    }
}
