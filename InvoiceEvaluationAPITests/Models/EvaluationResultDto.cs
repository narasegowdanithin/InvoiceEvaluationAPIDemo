using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceEvaluationAPI.Tests.Models
{
    public class EvaluationResultDto
    {
        public string EvaluationId { get; set; }
        public string InvoiceId { get; set; }
        public List<string> RulesApplied { get; set; }
        public string Classification { get; set; }
        public string EvaluationFile { get; set; }
    }

}
