using DocumentFormat.OpenXml.Office2010.Excel;
using JOIN_Data.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JOIN_Data.Services.Interfaces;
using System.Collections.Generic;
using JOIN_Data.Services;
using System.Linq;

namespace JOINBoard.Components;

public class DashboardChart : ViewComponent
{
    private readonly IApplicationService applicationService;
    public DashboardChart(IApplicationService applicationService)
    {
        this.applicationService = applicationService;
    }
    public async Task<IViewComponentResult> InvokeAsync(string appId, string appName, Dictionary<string, int> chartData = null)
    {
        ChartViewModel vm = new ChartViewModel()
        {
            ChartId = "noData",
            Name = "No Data",
            Labels = new List<string> { "No Data" },
            Counts = new List<int> { 1 }
        };
        if (chartData is null)
        {
            chartData = await applicationService.GetAppApplicantsStatusAsync(null, appName);
            appName = $"{appName} (Users Status)";
        }
        if (chartData is not null)
        {
            vm.ChartId = appId;
            vm.Name = appName;
            vm.Labels = chartData.Keys.ToList();
            vm.Counts = chartData.Values.ToList();
        }
        return View(vm);
    }
}
