# InvoiceEvaluationAPI

A .NET 8 Web API project that processes insurance invoice evaluations, integrates with a mock third-party classification service, applies decision rules, and returns a structured evaluation summary.

---

## 🧩 Features

- 📥 **POST /evaluate** endpoint to receive invoice PDF and JSON data.
- ✅ Input validation for:
  - Required fields
  - Invoice number format (e.g., "S12345")
  - PDF format check
  - Max file size: 5MB
- 🔗 Integration with a **mock 3rd-party API** for invoice classification using RestSharp & Polly.
- 📊 Rule engine to evaluate invoice based on editable JSON rules.
- 🧾 Returns a structured JSON response with evaluation result.
- 🧪 Includes unit and integration tests (NUnit).
- 🪵 Logging to `app.log` using Serilog.

---

### POST /evaluate
    
    
       {
          "Document": "file",
          "InvoiceId": "12345",
          "InvoiceNumber": "S12345",
          "InvoiceDate": "2025-04-03",
          "Comment": "Test invoice",
          "Amount": 500
        }
    
1. Document: PDF document (file upload).

2. InvoiceId: Unique identifier for the invoice.

3. InvoiceNumber: The invoice number, must start with "S" followed by 5 digits.

4. InvoiceDate: The date the invoice was issued.

5. Comment: A comment regarding the invoice.

6. Amount: The total amount on the invoice (must be greater than 0).


This endpoint receives an invoice along with the document (PDF) and invoice details in JSON format.

---

## 🔌 MockExternalApi Integration

This project uses a separate mock service to simulate a 3rd-party classification API.

📦 External Project Repository:  
[👉 MockExternalApi (GitHub)](https://github.com/narasegowdanithin/MockExternalApi)

### 🧪 Purpose

This mock API returns a simulated classification and risk level based on the invoice being evaluated.

### 🚀 How to Run the Mock API

1. Clone the repo:

   ```bash
   git clone https://github.com/narasegowdanithin/MockExternalApi.git
   ```
2. Navigate to the folder and run the project:
  
   ```bash
   dotnet run
   ```
 3.Find the Port:(which is already included in the project)
   Now listening on: https://localhost:5001

   
---

### Rule engine to evaluate invoice based on editable JSON rule

simple decision table in JSON format that a non-technical operation manager could edit

It will have: 
##### RuleId, Condition, Action

Using which service will evaluates invoice data.

---

### Returns a structured JSON response with evaluation result.

JSON summary report as the response of POST /evaluate:

    
      {
        "evaluationId": "EVAL001", 
        "invoiceId": "IncoiceId",
        "rulesApplied": "decision",
        "classification": "classification",
        “evaluationFile”: “Base64String”
      }
    
    
In addition to this Evulation file will be saved to the Evaluation directory of project.

---

### 🚀 How to Run the test for getting Test and Code Coverage report.
Open the Powershell and run the tsetcoverage.ps1

   ```bash
   tsetcoverage.ps1
   ```

Reports will be created inside TestReport and CoverageReport directory of test project in .html format.



