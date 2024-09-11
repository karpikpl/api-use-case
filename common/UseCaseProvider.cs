using Microsoft.AspNetCore.Http;

namespace common;

public static class UseCaseProvider
{

    public static UseCaseName GetUseCase(IHttpContextAccessor contextAccessor, int pathSegmentIndex)
    {
        var requestPath = contextAccessor.HttpContext?.Request.Path.ToString() ?? string.Empty;
        var pathSegments = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
       
        if (pathSegmentIndex < 0 || pathSegmentIndex >= pathSegments.Length)
        {
            return UseCaseName.None;
        }

        var segment = pathSegments[pathSegmentIndex];
        if (Enum.TryParse<UseCaseName>(segment, true, out var useCase))
        {
            return useCase;
        }

        return UseCaseName.None;
    }
}