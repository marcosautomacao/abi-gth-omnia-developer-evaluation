using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }

        public ErrorResponse(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public ErrorResponse(string type, string message, IDictionary<string, string[]> errors)
            : this(type, message)
        {
            Errors = errors;
        }
    }
} 