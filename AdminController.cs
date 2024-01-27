using CommonUtil.ListViews;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using JOIN_Data.Models.ViewModels;
using JOIN_Data.Enums;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Claims;
using JOIN_Data.Services.Interfaces;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Extensions;
using DocumentFormat.OpenXml.InkML;

namespace JOINBoard.Controllers;

public class AdminController : Controller
{
    private readonly IListViewFactory listViewFactory;
    private readonly IListViewService listViewService;
    private readonly IUserService userSettingService;
    private readonly IApplicantService applicantService;
    private readonly IApplicationService applicationService;
    private string defaultSortColumn = "DateCreated";
    public AdminController(IListViewFactory listViewFactory, IListViewService listViewService, 
        IUserService userSettingService, IApplicantService applicantService, IApplicationService applicationService)
    {
        this.listViewFactory = listViewFactory;
        this.listViewService = listViewService;
        this.userSettingService = userSettingService;
        this.applicantService = applicantService;
        this.applicationService = applicationService;

    }

    [Route("Admin/BoardManagement")]
    public IActionResult BoardManagement()
    {
        ViewBag.LeftNavKey = LeftNavConstants.Board;
        return View("BoardManagement");
    }

    [HttpGet]
    [Route("Admin/AppManagement")]
    public IActionResult AppManagement()
    {
        ViewBag.LeftNavKey = LeftNavConstants.Applications;
        return View("AppManagement");
    }

    [HttpGet, HttpPost]
    [Route("Admin/Details/{id}")]   
    public async Task<IActionResult> Details(int id, bool isFullDetail = false, PagingInfo pageInfo = null)
    {
        var model = await applicationService.GetApplicationByName_IdAsync(id:id);
        ViewBag.LeftNavKey = "";      
       
        if (Request.IsAjaxRequest() && isFullDetail == false)
        {
            return PartialView("_appRead", model);
        }
        ViewBag.Route = "Admin/App";
        ViewBag.Container = "recordListContainer";
        return PartialView("AppFullDetail", model);
    }

    [HttpGet, HttpPost]
    [Route("Admin/GetAppList/update-display")]
    public async Task<IActionResult> UpdateAppDisplay(PagingInfo pageInfo = null)
    {
        var listView = listViewFactory.GetListView<ApplicationViewMode, ApplicationViewMode>(pageInfo.ViewKey);
        var vm = await getAppsListViewModel(pageInfo);

        vm.RecordRoute = "Admin/Details";
        vm.PartialURL = "Admin/GetAppList";
        AppUtils.SetObjectAsJson(HttpContext.Session, "DisplayType", pageInfo.DisplayType);
        await listView.UpdateDisplayType(HttpContext, userSettingService);

        if (pageInfo.DisplayType == DisplayMode.Table)
        {
            var showPreview = vm.ListView.ShowPreview(HttpContext);
            if (showPreview == false)
            {
                vm.ListView.JavascriptPreviewFunction = "loadLinkPartial('/Admin/Details', this, {'isFullDetail': true}, 'recordListContainer')";
                vm.ListView.IsJavascriptPreview = true;
            }
            vm.ListView.AllowExtSearch = true;
            return PartialView("ListView/_listView", vm);
        }
        vm.ListView.AllowExtSearch = false;
        return PartialView("ListView/_listView_VerticalSplit", vm);
    }

    [HttpGet]
    public async Task<IActionResult> GetAppList(string category = null, PagingInfo pageInfo = null)
    {
        ViewBag.AppManagementNavKey = AppManagementNavConstants.Applications;
        if (HttpContext.Request.Method == "GET")
        {
            if (ViewBag.pageInfo != null)
            {
                pageInfo = ViewBag.pageInfo;
            }
        }
        pageInfo.ViewKey = LeftNavConstants.Applications;
        pageInfo.CategoryKey = category;
        var listView = listViewFactory.GetListView<ApplicationViewMode, ApplicationViewMode>(pageInfo.ViewKey);
        pageInfo = await listView.CheckToggle(pageInfo, HttpContext, userSettingService);
        var vm = await getAppsListViewModel(pageInfo);
        ViewBag.LeftNavKey = pageInfo.ViewKey;
        vm.RecordRoute = "Admin/Details";
        vm.PartialURL = "Admin/GetAppList";
       
        var userSetting = AppUtils.GetObjectFromJson<UserSetting>(SessionKeyConstants.USER_SETTING, HttpContext.Session);
        if (userSetting is not null && userSetting.DefaultToVerticalSplitView)
        {
            pageInfo.DisplayType = DisplayMode.VerticalSplit;
            vm.ListView.AllowExtSearch = false;
        }

        //category.Replace("_", " ") + " " + vm.ListView.ViewTitle;
        vm.ListView.ViewTitle = vm.ListView.ViewTitle;
        
        if (Request.IsAjaxRequest())
        {
            if (pageInfo.DisplayType == DisplayMode.Table)
            {
                var showPreview = vm.ListView.ShowPreview(HttpContext);
                if (showPreview == false)
                {
                    vm.ListView.JavascriptPreviewFunction = "loadLinkPartial('/Admin/Details', this, {'isFullDetail': true}, 'recordListContainer')";
                    vm.ListView.IsJavascriptPreview = true;
                }            
                vm.ListView.AllowExtSearch = true;
                return PartialView("ListView/_listView", vm);
                //return PartialView("ListView/_listViewTableData", vm);
            }
            vm.ListView.AllowExtSearch = false;
            return PartialView("ListView/_listViewTableData_VerticalSplit", vm);
        }
        return View("/Index", vm);
    }

    private async Task<ListViewModel<ApplicationViewMode, ApplicationViewMode>> getAppsListViewModel(PagingInfo pageInfo)
    {

        var listView = listViewFactory.GetListView<ApplicationViewMode, ApplicationViewMode>(pageInfo.ViewKey);
        Expression<Func<ApplicationViewMode, object>> sortColumn = o => o.Status;
        if (!string.IsNullOrEmpty(pageInfo.SortColumn))
        {
            sortColumn = listView.GetSortExpression(pageInfo.SortColumn);
        }
        else
        {
            pageInfo.SortColumn = "Status";// defaultSortColumn;
            pageInfo.SortOrder = ListViewConstants.SORT_ORDER_DESC;
        }

        listView.DefaultCriteria = GetDefaultCriteria(pageInfo);

        await _setListViewProps(pageInfo, listView);
        var appList = await applicationService.GetAppsPagedListAsync(pageInfo, listView, sortColumn);
        pageInfo.TotalRecords = appList.TotalCount;

        ListViewModel<ApplicationViewMode, ApplicationViewMode> vm = new(ControllerContext)
        {
            ListView = listView,
            PagedList = appList,
            PagingInfo = pageInfo
        };
        AppUtils.SetObjectAsJson(HttpContext.Session, pageInfo.ViewKey, pageInfo);
        return vm;
    }
    [HttpGet]
    [Route("Admin/Create")]
    public IActionResult CreateTest()
    {
        ViewBag.AppManagementNavKey = AppManagementNavConstants.Applications;
        return ViewComponent("ApplicationCreate"); 
    }

    [HttpGet]
    [Route("Admin/GetAppList/New")]
    public IActionResult Create()
    {
        ViewBag.AppManagementNavKey = AppManagementNavConstants.Applications;
        return PartialView("ApplicationCreate"); 
    }
    [HttpPost]
    [Route("Admin/create-new-app")]
    public IActionResult createNewApp(string/*NewAppViewModel*/ model)
    {
        ViewBag.AppManagementNavKey = AppManagementNavConstants.Applications;
        return View();
    }

    private async Task _setListViewProps(PagingInfo pageInfo, IListView listView)
    {
        listView.UserListView = await listViewService.GetModelAsync(pageInfo.ViewKey, User.GetJAGCProfileUUID());
        if (Request.IsAjaxRequest())
        {
            if (listView.UserListView == null && !string.IsNullOrEmpty(pageInfo.CategorizedColumn))
            {
                listView.UserListView = await listViewService.GetModelAsync(pageInfo.ViewKey, User.GetJAGCProfileUUID(), true);
            }
            if (listView.UserListView != null && listView.UserListView.CategorizedColumn != pageInfo.CategorizedColumn)
            {
                listView.UserListView.CategorizedColumn = pageInfo.CategorizedColumn;
                await listViewService.UpdateCategorizedColumnAsync(pageInfo.ViewKey, User.GetJAGCProfileUUID(), listView.UserListView.CategorizedColumn);
            }
        }
        else
        {
            if (listView.UserListView != null)
            {
                pageInfo.CategorizedColumn = listView.UserListView.CategorizedColumn;
            }
        }
        listView.CategorizedColumn = listView.UserListView?.CategorizedColumn;
    }
    private Expression<Func<ApplicationViewMode, bool>> GetDefaultCriteria(PagingInfo pageInfo)
    {
        Expression<Func<ApplicationViewMode, bool>> defaultCriteria;
        
        if (pageInfo.ViewKey == "RelatedClaimsMed")
        {
            defaultCriteria = t => t.AppId == 33;
            if (pageInfo.CategoryKey == "potential_open")
            {
                defaultCriteria = defaultCriteria.AndPredicate(t => t.Status == "Test");
            }
        }
        defaultCriteria = t => t.AppId == 33;
        if (pageInfo.CategoryKey == "potential_open")
        {
            defaultCriteria = defaultCriteria.AndPredicate(t => t.AppName == "Test");
        }

        return defaultCriteria;
    }

    #region test
    //[Route("Admin/ReportList/{appParts}")]
    //[HttpPost, HttpGet]
    //public async Task<IActionResult> ReportList(AppParts appParts, PagingInfo pageInfo = null, bool initialLoad = false, ReportParam param = null)
    //{
    //    if (pageInfo == null)
    //    {
    //        pageInfo = AppUtils.GetObjectFromJson<PagingInfo>("officeKey", HttpContext.Session);
    //    }

    //    if (HttpContext.Request.Method == "GET")
    //    {
    //        if (ViewBag.pageInfo != null)
    //        {
    //            pageInfo = ViewBag.pageInfo;
    //        }
    //        pageInfo.FieldFilters = SetReportParam(param);
    //    }

    //    if (HttpContext.Request.Method == "POST")
    //    {
    //        if (ViewBag.pageInfo != null)
    //        {
    //            pageInfo = ViewBag.pageInfo;
    //        }

    //        param = GetReportParam(pageInfo.FieldFilters);
    //    }

    //    switch (appParts)
    //    {
    //        case AppParts.Applications:
    //            pageInfo.ViewKey = AppManagementNavConstants.Applications;
    //            break;
    //        case AppParts.Board:
    //            pageInfo.ViewKey = AppManagementNavConstants.Board;
    //            break;
    //        case AppParts.Validation:
    //            pageInfo.ViewKey = AppManagementNavConstants.Validation;
    //            break;
    //        case AppParts.Sections:
    //            pageInfo.ViewKey = AppManagementNavConstants.Sections;
    //            break;
    //        default:
    //            break;
    //    }

    //    var vm = await getReportViewModel(pageInfo, appParts, false, param);

    //    ViewBag.LeftNavKey = pageInfo.ViewKey;
    //    vm.RecordRoute = "report/report-selector";
    //    vm.PartialURL = "Report/ReportList/" + appParts;
    //    vm.ListView.OuterContainerId = "report-list-container";
    //    //vm.ListView.ReportTypes = appParts;

    //    if (initialLoad && vm.PagedList.TotalCount <= 25)
    //        return PartialView("ListView/_listView", vm);

    //    return PartialView("ListView/_listViewTableData", vm);
    //}
    //private ReportParam GetReportParam(List<FieldFilter> fieldFilters)
    //{
    //    var result = new ReportParam();
    //    if (fieldFilters is not null && fieldFilters.Count > 0)
    //    {
    //        foreach (var item in fieldFilters)
    //        {
    //            switch (item.FieldName)
    //            {
    //                case "DateCriteria":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.DateCriteria = item.Values[0];
    //                    break;
    //                case "IsActive":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.IsActive = bool.Parse(item.Values[0]);
    //                    break;
    //                case "OpenCloseClaim":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.OpenCloseClaim = item.Values[0];
    //                    break;
    //                case "SelectedMonth":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.SelectedMonth = int.Parse(item.Values[0]);
    //                    break;
    //                case "RecoveryType":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.RecoveryType = int.Parse(item.Values[0]);
    //                    break;
    //                case "Year":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.Year = int.Parse(item.Values[0]);
    //                    break;
    //                case "StartFiscalYear":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.StartFiscalYear = int.Parse(item.Values[0]);
    //                    break;
    //                case "EndFiscalYear":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.EndFiscalYear = int.Parse(item.Values[0]);
    //                    break;
    //                case "AssertionAmount":
    //                    if (item.Values is not null && item.Values.Count > 0)
    //                        result.AssertionAmount = decimal.Parse(item.Values[0]);
    //                    break;
    //                case "Date":
    //                    if (item.Values is not null && item.Values.Count > 0 && string.IsNullOrWhiteSpace(item.Values[0]) == false)
    //                        result.Date = DateTime.Parse(item.Values[0]);
    //                    break;
    //                case "StartDate":
    //                    if (item.Values is not null && item.Values.Count > 0 && string.IsNullOrWhiteSpace(item.Values[0]) == false)
    //                        result.StartDate = DateTime.Parse(item.Values[0]);
    //                    break;
    //                case "EndDate":
    //                    if (item.Values is not null && item.Values.Count > 0 && string.IsNullOrWhiteSpace(item.Values[0]) == false)
    //                        result.EndDate = DateTime.Parse(item.Values[0]);
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //    return result;
    //}

    //private List<FieldFilter> SetReportParam(ReportParam param)
    //{
    //    var FieldFilters = new List<FieldFilter>();
    //    if (param is not null)
    //    {
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "DateCriteria",
    //            ShowAsTag = false,
    //            Values = new List<string>() { param.DateCriteria }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "IsActive",
    //            ShowAsTag = false,
    //            Values = new List<string>() { param.IsActive.ToString() }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "OpenCloseClaim",
    //            ShowAsTag = false,
    //            Values = new List<string>() { param.OpenCloseClaim }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "SelectedMonth",
    //            ShowAsTag = false,
    //            Values = new List<string>() { $"{param.SelectedMonth}" }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "RecoveryType",
    //            ShowAsTag = false,
    //            Values = new List<string>() { $"{param.RecoveryType}" }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "Year",
    //            ShowAsTag = false,
    //            Values = new List<string>() { $"{param.Year}" }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "StartFiscalYear",
    //            ShowAsTag = false,
    //            Values = new List<string>() { $"{param.StartFiscalYear}" }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "EndFiscalYear",
    //            ShowAsTag = false,
    //            Values = new List<string>() { $"{param.EndFiscalYear}" }
    //        });
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "AssertionAmount",
    //            ShowAsTag = false,
    //            Values = new List<string>() { $"{param.AssertionAmount}" }
    //        });
    //        var dd = param.Date.HasValue ? param.Date.Value.ToString() : "";
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "Date",
    //            ShowAsTag = false,
    //            Values = new List<string>() { dd }
    //        });
    //        var sd = param.StartDate.HasValue ? param.StartDate.Value.ToString() : "";
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "StartDate",
    //            ShowAsTag = false,
    //            Values = new List<string>() { sd }
    //        });
    //        var ed = param.EndDate.HasValue ? param.EndDate.Value.ToString() : "";
    //        FieldFilters.Add(new FieldFilter
    //        {
    //            FieldName = "EndDate",
    //            ShowAsTag = false,
    //            Values = new List<string>() { ed }
    //        });
    //    }
    //    return FieldFilters;
    //}

    //private async Task<ListViewModel<ApplicationViewMode, ApplicationViewMode>> getReportViewModel(PagingInfo pageInfo, AppParts appParts, bool export = false, ReportParam param = null)
    //{
    //    var listView = listViewFactory.GetListView<ApplicationViewMode, ApplicationViewMode>(pageInfo.ViewKey);

    //    await _setListViewProps(pageInfo, listView);

    //    var recordList = await GetReportPagedListAsync(pageInfo, listView, appParts, export, param);
    //    pageInfo.TotalRecords = recordList.TotalCount;
    //    //listView.CategorizedColumn = "FiscalYear";
    //    ListViewModel<ApplicationViewMode, ApplicationViewMode> vm = new(ControllerContext)
    //    {
    //        ListView = listView,
    //        PagedList = recordList,
    //        PagingInfo = pageInfo
    //    };
    //    AppUtils.SetObjectAsJson(HttpContext.Session, pageInfo.ViewKey, pageInfo);
    //    return vm;
    //}

    //private async Task<PagedList<ApplicationViewMode>> GetReportPagedListAsync(PagingInfo pageInfo, ListView<ApplicationViewMode, ApplicationViewMode> listView, AppParts appParts, bool export = false, ReportParam param = null)
    //{
    //    var recordList = new List<ApplicationViewMode>();
    //    int totalCount = 0;
    //    switch (appParts)
    //    {
    //        case AppParts.Applications:
    //            var fopx = await FieldOfficePropertyRecoveries(pageInfo, export, param);
    //            recordList = fopx.items;
    //            totalCount = fopx.totalCount;
    //            break;
    //        case AppParts.Sections:
    //            //var medx = Get_Med_AffirmativeClaimStatus(pageInfo, export, param);
    //            //recordList = medx.items;
    //            //totalCount = medx.totalCount;
    //            break;
    //        default:
    //            break;
    //    }

    //    return new PagedList<ApplicationViewMode>
    //    {
    //        TotalCount = totalCount,
    //        Items = recordList
    //    };
    //}

    //private async Task<(int totalCount, List<ApplicationViewMode> items)> FieldOfficePropertyRecoveries(PagingInfo pageInfo, bool export = false, ReportParam param = null)
    //{
    //    var listView = listViewFactory.GetListView<ApplicationViewMode, ApplicationViewMode>(pageInfo.ViewKey);

    //    Expression<Func<ApplicationViewMode, object>> sortColumn = o => o.CreatedDate;
    //    if (!string.IsNullOrEmpty(pageInfo.SortColumn))
    //    {
    //        sortColumn = listView.GetSortExpression(pageInfo.SortColumn);
    //    }
    //    else
    //    {
    //        pageInfo.SortColumn = "OfficeName";
    //    }

    //    //var result = await fact_ClaimTransactionRepository.GetFieldOfficePropertyRecoveries(reportOffice.ID, export, sortColumn, pageInfo.SortOrder != "asc", pageInfo.Page, pageInfo.PageSize, param);
    //    //return (result.totalCount, result.items);

    //    return (40, new List<ApplicationViewMode>());
    //}
    #endregion
}

