using Contracts.Dtos.Common;
using Mvc.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Mvc.Clients.Helpers;

internal static class ApiErrorResponseHandler
{
    public static async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        string fallbackMessage
    )
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        throw await CreateExceptionAsync(response, fallbackMessage);
    }

    public static async Task<Exception> CreateExceptionAsync(
        HttpResponseMessage response,
        string fallbackMessage
    )
    {
        var message = await ReadAsync(response, fallbackMessage);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                return new KeyNotFoundException(message);

            case HttpStatusCode.Conflict:
                return new BusinessRuleException(message);

            default:
                return new ApplicationException(message);
        }
    }

    public static async Task<string> ReadAsync(
        HttpResponseMessage response,
        string fallbackMessage
    )
    {
        var errorContent = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(errorContent))
        {
            return fallbackMessage;
        }

        try
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorResponseDto>(
                errorContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (errorResponse?.Errors is { Count: > 0 })
            {
                return string.Join(", ", errorResponse.Errors);
            }

            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(
                errorContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (!string.IsNullOrWhiteSpace(problemDetails?.Detail))
            {
                return problemDetails.Detail;
            }

            if (!string.IsNullOrWhiteSpace(problemDetails?.Title))
            {
                return problemDetails.Title;
            }
        }
        catch (JsonException)
        {
        }

        return errorContent;
    }
}
