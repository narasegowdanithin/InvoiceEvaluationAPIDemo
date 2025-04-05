using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace InvoiceEvaluationAPI.Validation
{
    public class InvoiceValidationAttribute : ValidationAttribute
    {
        private readonly ILogger<InvoiceValidationAttribute> _logger;

        public InvoiceValidationAttribute()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<InvoiceValidationAttribute>();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                _logger.LogWarning("No document uploaded.");
                return new ValidationResult("Document is required.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                _logger.LogWarning("Invalid document type.");
                return new ValidationResult("Only PDF files are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
            {
                _logger.LogWarning("Document size exceeds limit.");
                return new ValidationResult("File size must be less than 5MB.");
            }

            _logger.LogInformation("Document validation passed.");
            return ValidationResult.Success;
        }
    }
}
