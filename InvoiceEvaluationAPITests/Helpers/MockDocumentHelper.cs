//using MigraDoc.DocumentObjectModel;
//using MigraDoc.Rendering;
//using Microsoft.AspNetCore.Http;
//using System.IO;
//using System.Net.Http;
using System.Net.Http.Headers;

namespace InvoiceEvaluationAPI.Test.Helpers
{
    public static class MockDocumentHelper
    {
        public static StreamContent CreateMockDocument()
        {

            var bytes = System.Text.Encoding.UTF8.GetBytes("This is a fake PDF");
            var stream = new MemoryStream(bytes);
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return fileContent;
        }
    }
}
