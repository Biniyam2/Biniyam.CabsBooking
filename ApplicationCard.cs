using JOIN_Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JOINBoard.Components;

public class ApplicationCard : ViewComponent
{
    private readonly IApplicationService _applicationService;
    public ApplicationCard(IApplicationService applicationService)
    {
        this._applicationService = applicationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string[] statuses = null, string appName = null, string section = null,
        DateTime? createdDate = null, DateTime? lastUpdatedDate = null, string createdBy = null, string lastUpdatedBy = null,
        int pageSize = 20, int pageNum = 1)
    {
        var result = await _applicationService.GetApplicationsAsync(statuses, appName, section, createdDate, lastUpdatedDate, createdBy, lastUpdatedBy
            ,pageSize , pageNum);
        return View(result.listResult);
    }
}
