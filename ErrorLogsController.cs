using CommonUtil.ListViews;
using JAGCNet.Library.Services.Interfaces;
using JOINBoard.ActionFilters;
using JOIN_Data.Enums;
using JOIN_Data.Extensions;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Models.LogModels;
using JOIN_Data.Models.UtilityModels;
using JOIN_Data.Persistence;
using JOIN_Data.Services.Interfaces;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JOINBoard.Controllers
{
    [Authorize(Policy = AuthConstants.POLICY_MANAGE_KEYWORDS)]
    public class ErrorLogsController : Controller
    {
        private readonly IJAGCProfilesService profilesService;
        private readonly IErrorLogService recordService;
        private readonly IListViewFactory listViewFactory;
        private string viewKey = "ErrorLogs";
        private string defaultSortColumn = "LogDate";
        public ErrorLogsController(IJAGCProfilesService profilesService,
            IErrorLogService recordService,
            IListViewFactory listViewFactory)
        {
            this.profilesService = profilesService;
            this.recordService = recordService;
            this.listViewFactory = listViewFactory;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LeftNavKey = LeftNavConstants.ERROR_LOGS;
            ViewBag.LeftNav = "_left_Nav_Admin";
        }
        [HttpPost, HttpGet]
        [ServiceFilter(typeof(IndexActionFilter))]
        public async Task<IActionResult> Index(PagingInfo pageInfo)
        {
            if (HttpContext.Request.Method == "GET")
            {
                if (ViewBag.pageInfo != null)
                {
                    pageInfo = ViewBag.pageInfo;
                }
            }

            if (String.IsNullOrEmpty(pageInfo.ViewKey))
            {
                pageInfo.ViewKey = viewKey;
            }

            ListView<ErrorLog, ErrorLog> listView = listViewFactory.GetListView<ErrorLog, ErrorLog>(pageInfo.ViewKey);
            ListViewModel<ErrorLog, ErrorLog> vm = await getListViewModel(pageInfo);
            vm.RecordRoute = "errorlogs";
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListViewTableData_VerticalSplit", vm);
            }
            return View("Index", vm);
        }
        [HttpGet]
        public async Task<IActionResult> Export(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                key = viewKey;
            }
            PagingInfo pageInfo = AppUtils.GetObjectFromJson<PagingInfo>(key, HttpContext.Session);
            pageInfo.PageSize = -1;
            ListViewModel<ErrorLog, ErrorLog> vm = await getListViewModel(pageInfo);
            ListView<ErrorLog, ErrorLog> listView = listViewFactory.GetListView<ErrorLog, ErrorLog>(pageInfo.ViewKey);
            return listView.Export(vm, pageInfo.DisplayCriteria);
        }

        private async Task<ListViewModel<ErrorLog, ErrorLog>> getListViewModel(PagingInfo pageInfo)
        {
            ListView<ErrorLog, ErrorLog> listView = listViewFactory.GetListView<ErrorLog, ErrorLog>(pageInfo.ViewKey);
            Expression<Func<ErrorLog, object>> sortColumn = o => o.LogDate;
            if (!String.IsNullOrEmpty(pageInfo.SortColumn))
            {
                sortColumn = listView.GetSortExpression(pageInfo.SortColumn);
            }
            else
            {
                pageInfo.SortColumn = defaultSortColumn;
                pageInfo.SortOrder = ListViewConstants.SORT_ORDER_DESC;
            }
            var recordList = await recordService.GetPagedListAsync(pageInfo, listView, sortColumn);
            pageInfo.TotalRecords = recordList.TotalCount;
            ListViewModel<ErrorLog, ErrorLog> vm = new ListViewModel<ErrorLog, ErrorLog>(ControllerContext)
            {
                ListView = listView,
                PagedList = recordList,
                PagingInfo = pageInfo
            };
            AppUtils.SetObjectAsJson(HttpContext.Session, pageInfo.ViewKey, pageInfo);
            return vm;
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ErrorLog model = await recordService.GetModelAsync(id);
            return PartialView("_details", model);
        }
        [HttpGet]
        public IActionResult New()
        {
            return PartialView("_edit");
        }
    }
}
