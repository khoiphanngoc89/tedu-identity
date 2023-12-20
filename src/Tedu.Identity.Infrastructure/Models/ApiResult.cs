using Microsoft.AspNetCore.Mvc;

namespace Tedu.Identity.Infrastructure.Models;

public class ApiResult<T> : IActionResult
{
    public bool IsSucceeded { get; set; }
    public string? Message { get; set; }
    public T? Result { get; set; }

    public ApiResult(bool isSucceeded)
    {
        this.IsSucceeded = isSucceeded;
    }
    public ApiResult(bool isSucceeded, string? message)
    {
        this.IsSucceeded = isSucceeded;
        this.Message = message;
    }

    public ApiResult(bool isSucceeded, T result, string? message = null)
    {
        this.Result = result;
        this.IsSucceeded = isSucceeded;
        this.Message = message;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objResult = new ObjectResult(this);
        await objResult.ExecuteResultAsync(context);
    }
}
