using CommonUtil.ListViews;
using JAGCNet.Library.Services.Interfaces;
using JOINBoard.ActionFilters;
using JOIN_Data.Enums;
using JOIN_Data.Extensions;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Models.UtilityModels;
using JOIN_Data.Persistence;
using JOIN_Data.Services.Interfaces;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JOINBoard.Controllers
{
    [Authorize(Policy = AuthConstants.POLICY_ACTIVE_USER)]
    public class PresentationsController : Controller
    {
        private readonly IJAGCProfilesService profilesService;
        private readonly IPresentationService recordService;
        private readonly IListViewFactory listViewFactory;
        private readonly IUserService userSettingService;
        private readonly IKeyValueService keywordService;
        private string viewKey = "Presentations";
        private string defaultSortColumn = "Title";
        public PresentationsController(IJAGCProfilesService profilesService,
            IPresentationService recordService,
            IListViewFactory listViewFactory,
            IUserService userSettingService,
            IKeyValueService keywordService)
        {
            this.profilesService = profilesService;
            this.recordService = recordService;
            this.listViewFactory = listViewFactory;
            this.userSettingService = userSettingService;
            this.keywordService = keywordService;
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

            ListView<Presentation, Presentation> listView = listViewFactory.GetListView<Presentation, Presentation>(pageInfo.ViewKey);
            pageInfo = await listView.CheckToggle(pageInfo, HttpContext, userSettingService);
            ViewBag.CategoryKey = pageInfo.CategoryKey;
            ListViewModel<Presentation, Presentation> vm = await getListViewModel(pageInfo);
            ViewBag.LeftMenu = pageInfo.ViewKey;
            vm.RecordRoute = "presentations/edit";
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListViewTableData", vm);
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
            ListViewModel<Presentation, Presentation> vm = await getListViewModel(pageInfo);
            ListView<Presentation, Presentation> listView = listViewFactory.GetListView<Presentation, Presentation>(pageInfo.ViewKey);
            return listView.Export(vm, pageInfo.DisplayCriteria);
        }

        private async Task<ListViewModel<Presentation, Presentation>> getListViewModel(PagingInfo pageInfo)
        {
            ListView<Presentation, Presentation> listView = listViewFactory.GetListView<Presentation, Presentation>(pageInfo.ViewKey);
            Expression<Func<Presentation, object>> sortColumn = o => o.Title;
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
            ListViewModel<Presentation, Presentation> vm = new ListViewModel<Presentation, Presentation>(ControllerContext)
            {
                ListView = listView,
                PagedList = recordList,
                PagingInfo = pageInfo
            };
            AppUtils.SetObjectAsJson(HttpContext.Session, pageInfo.ViewKey, pageInfo);
            return vm;
        }


    }
}
