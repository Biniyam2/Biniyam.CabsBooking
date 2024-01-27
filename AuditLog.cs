
using JOIN_Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JOINBoard.Web.Components
{
    public class AuditLog : ViewComponent
    {
        IAuditLogService logService;
        public AuditLog(IAuditLogService logService)
        {
            this.logService = logService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string className, int primaryKey, bool showAccordian = true)
        {
            var logList = await logService.GetListAsync(className, primaryKey);
            ViewBag.ShowAccordian = showAccordian;
            return View(logList);
        }
    }
}
