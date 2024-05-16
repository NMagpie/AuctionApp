using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Common.Abstractions
{
    public abstract class AppBaseController : ControllerBase
    {
        protected int GetUserId() => int.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    }
}
