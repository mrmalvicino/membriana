using Microsoft.AspNetCore.Mvc;
using Mvc.Exceptions;

namespace Mvc.Controllers;

public abstract class MvcControllerBase : Controller
{
    protected bool TryAddModelError(Exception exception)
    {
        if (exception is not (BusinessRuleException or ApplicationException))
        {
            return false;
        }

        ModelState.AddModelError(string.Empty, exception.Message);
        return true;
    }

    protected bool TrySetDeleteError(Exception exception)
    {
        if (exception is not (BusinessRuleException or ApplicationException))
        {
            return false;
        }

        TempData["BusinessError"] = exception.Message;
        return true;
    }
}
