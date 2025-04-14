using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = exception switch
            {
                ValidationException validationEx => HandleValidationException(validationEx),
                DomainException domainEx => HandleDomainException(domainEx),
                _ => HandleUnknownException(exception)
            };

            response.StatusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                DomainException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            if (_environment.IsDevelopment() && exception.StackTrace != null)
            {
                errorResponse.StackTrace = exception.StackTrace;
            }

            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(result);
        }

        private ErrorResponse HandleValidationException(ValidationException exception)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var error in exception.Errors)
            {
                var propertyName = error.PropertyName;
                if (!errors.ContainsKey(propertyName))
                {
                    errors[propertyName] = new[] { error.ErrorMessage };
                }
            }

            return new ErrorResponse(
                "Validation Error",
                "One or more validation errors occurred.",
                errors);
        }

        private ErrorResponse HandleDomainException(DomainException exception)
        {
            return new ErrorResponse(
                "Domain Error",
                exception.Message);
        }

        private ErrorResponse HandleUnknownException(Exception exception)
        {
            return new ErrorResponse(
                "Server Error",
                _environment.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred.");
        }
    }
} 