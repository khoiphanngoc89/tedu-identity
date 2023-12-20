namespace Tedu.Identity.Infrastructure.Models;

public sealed class ApiErrorResult<T> : ApiResult<T>
{
    public List<string> Errors { get; set; } = new List<string>();

    public ApiErrorResult(List<string> errors)
        : base(false)
    {
        this.Errors = errors;
    }

    public ApiErrorResult(string? message)
        : base(false, message)
    {
    }
}
