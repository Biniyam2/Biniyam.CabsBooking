using JOIN_Data.Services.Interfaces;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JOINBoard.Controllers
{
    //[Authorize(Policy = AuthConstants.POLICY_ACTIVE_USER)]
    public class DashboardController : Controller
    {
        private readonly IUserService userSettingService;
        private readonly IAuthorizationService authorizationService;


        public DashboardController(
            IUserService userSettingService,
            IAuthorizationService authorizationService
            )
        {
            this.userSettingService = userSettingService;
            this.authorizationService = authorizationService;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LeftNavKey = LeftNavConstants.HOME;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("[controller]/dashboard-chart")]
        public IActionResult GetDashboardChart(string appId, string appName, Dictionary<string, int> chartData = null)
        {
            return ViewComponent("DashboardChart", new { appId, appName, chartData });
        }
        [Route("[controller]/Applicants")]
        public IActionResult GetDashboardApplicants(string[] appNames = null)
        {
            return ViewComponent("DashboardApplicants", new { appNames });
        }
    }
}
