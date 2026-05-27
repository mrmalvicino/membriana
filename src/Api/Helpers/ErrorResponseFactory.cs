using Contracts.Dtos.Common;

namespace Api.Helpers;

internal static class ErrorResponseFactory
{
    public static ErrorResponseDto Create(params string[] errors)
    {
        return new ErrorResponseDto
        {
            Errors = errors.ToList()
        };
    }
}
