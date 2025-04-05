using System.ComponentModel.DataAnnotations;
using InvoiceEvaluationAPI.Validation;

namespace InvoiceEvaluationAPI.Models
{
    public class InvoiceEvaluationRequest
    {
        [Required]
        [InvoiceValidation] //Uses custom validation attribute
        public IFormFile Document { get; set; }

        [Required]
        public string InvoiceId { get; set; }

        [Required]
        [RegularExpression("^S\\d{5}$", ErrorMessage = "Invoice number must start with 'S' followed by 5 digits.")]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        public string Comment { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
