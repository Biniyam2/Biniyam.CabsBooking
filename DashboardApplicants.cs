using JOIN_Data.Models.ViewModels;
using JOIN_Data.Services;
using JOIN_Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JOINBoard.Components;

public class DashboardApplicants : ViewComponent
{
    private IDashboardService dashboardService;
    private IApplicantService applicantService;
    public DashboardApplicants(IApplicantService applicantService, IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
        this.applicantService = applicantService;
    }
    public async Task<IViewComponentResult> InvokeAsync(string[] appNames = null)
    {
        var appUsers = await applicantService.GetApplicants(appNames: appNames);

        if (appUsers == null)
        {
            appUsers = new List<ApplicantView>();
        }

        return View(appUsers);
    }
}
