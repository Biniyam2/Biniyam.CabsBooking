using JOIN_Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using JOIN_Data.Services;

namespace JOINBoard.Components;

public class SectionCard : ViewComponent
{
    private readonly IServiceService _serviceService;
    public SectionCard(IServiceService applicationService)
    {
        _serviceService = applicationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int appId)
    {
        var result = await _serviceService.GetSectionsByAppId(appId);
        return View(result);
    }
}
