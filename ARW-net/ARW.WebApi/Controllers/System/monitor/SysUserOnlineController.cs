using Microsoft.AspNetCore.Mvc;
using ARW.Admin.WebApi.Filters;

namespace ARW.Admin.WebApi.Controllers.monitor
{
    [Verify]
    [Route("monitor/online")]
    public class SysUserOnlineController : BaseController
    {
        [HttpGet("list")]
        public IActionResult Index()
        {
            return SUCCESS(null);
        }
    }
}
