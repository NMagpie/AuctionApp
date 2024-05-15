using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common.Abstractions
{
    public abstract class AppBaseController : ControllerBase
    {
        protected int GetUserId() => int.Parse(User.Claims.First().Value);
    }
}
