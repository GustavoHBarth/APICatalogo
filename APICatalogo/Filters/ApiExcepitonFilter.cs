using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiExcepitonFilter : IExceptionFilter
{
    private readonly ILogger<ApiExcepitonFilter> _logger;

    public ApiExcepitonFilter(ILogger<ApiExcepitonFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada: Status Code 500");

        context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitação.")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}
