using Contracts.Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Mvc.Services.Api.Helpers;

internal static class ApiErrorMessageReader
{
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
