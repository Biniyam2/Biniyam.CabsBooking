using CommonUtil.ListViews;
using JAGCNet.Library.Services.Interfaces;
using JOIN_Data.Enums;
using JOIN_Data.Extensions;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Models.UtilityModels;
using JOIN_Data.Services.Interfaces;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JOINBoard.Controllers
{
    [Authorize(Policy = AuthConstants.POLICY_MANAGE_KEYWORDS)]
    public class KeyValuesController : Controller
    {
        private readonly IJAGCProfilesService profilesService;
        private readonly IKeyValueService keyValueService;
        private readonly IListViewFactory listViewFactory;

        public KeyValuesController(IJAGCProfilesService profilesService, IKeyValueService keyValueService, IListViewFactory listViewFactory)
        {
            this.profilesService = profilesService;
            this.keyValueService = keyValueService;
            this.listViewFactory = listViewFactory;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LeftNavKey = LeftNavConstants.KEYWORDS;
            ViewBag.LeftNav = "_left_Nav_Admin";

        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Index(PagingInfo pageInfo)
        {
            pageInfo.ViewKey = "KeyValues";

            ListView<KeyValueLookup, KeyValueLookup> listView = listViewFactory.GetListView<KeyValueLookup, KeyValueLookup>(pageInfo.ViewKey);
            if (String.IsNullOrEmpty(pageInfo.CategoryKey))
            {
                pageInfo.CategoryKey = "1";
            }
            ViewBag.CategoryKey = pageInfo.CategoryKey;
            ListViewModel<KeyValueLookup, KeyValueLookup> vm = await getListViewModel(pageInfo);
            if (Request.IsAjaxRequest())
            {
                return PartialView("ListView/_ListViewTableData", vm);
            }
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Export()
        {
            PagingInfo pageInfo = AppUtils.GetObjectFromJson<PagingInfo>("KeyValues", HttpContext.Session);
            pageInfo.ViewKey = "KeyValues";
            pageInfo.PageSize = -1;
            ListViewModel<KeyValueLookup, KeyValueLookup> vm = await getListViewModel(pageInfo);
            ListView<KeyValueLookup, KeyValueLookup> listView = listViewFactory.GetListView<KeyValueLookup, KeyValueLookup>(pageInfo.ViewKey);
            return listView.Export(vm, pageInfo.DisplayCriteria);
        }
        private async Task<ListViewModel<KeyValueLookup, KeyValueLookup>> getListViewModel(PagingInfo pageInfo)
        {
            ListView<KeyValueLookup, KeyValueLookup> listView = listViewFactory.GetListView<KeyValueLookup, KeyValueLookup>(pageInfo.ViewKey);
            Expression<Func<KeyValueLookup, object>> sortColumn = o => o.DisplayValue;
            if (!String.IsNullOrEmpty(pageInfo.SortColumn))
            {
                sortColumn = listView.GetSortExpression(pageInfo.SortColumn);
            }
            else
            {
                pageInfo.SortColumn = "DisplayValue";
            }
            if (pageInfo.ActiveFilterFlag == null || pageInfo.ActiveFilterFlag == ActiveFilterFlag.Active)
            {
                listView.DefaultCriteria = k => k.Active;
            }
            else if (pageInfo.ActiveFilterFlag == ActiveFilterFlag.Inactive)
            {
                listView.DefaultCriteria = k => !k.Active;
            }
            var keyValues = await keyValueService.GetPagedListAsync(pageInfo, listView, sortColumn);
            pageInfo.TotalRecords = keyValues.TotalCount;
            ListViewModel<KeyValueLookup, KeyValueLookup> vm = new ListViewModel<KeyValueLookup, KeyValueLookup>(ControllerContext)
            {
                ListView = listView,
                PagedList = keyValues,
                PagingInfo = pageInfo
            };
            AppUtils.SetObjectAsJson(HttpContext.Session, pageInfo.ViewKey, pageInfo);
            return vm;
        }
        // GET: KeyValue/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var keyword = await keyValueService.GetModelAsync(id, true);

            ViewBag.ModalTitle = keyword.KeyValueType.Name;
            ViewBag.Disable = false;
            return PartialView("_edit", keyword);
        }
        public async Task<ActionResult> New(int key)
        {
            var keywordType = await keyValueService.GetKeyValueTypeAsync(key);
            if (keywordType != null)
            {
                var keyword = new KeyValueLookup() { Active = true, KeyValueTypeId = key };
                keyword.KeyValueType = keywordType;
                ViewBag.ModalTitle = keywordType.Name + " - New Keyword";
                ViewBag.Disable = false;
                return PartialView("_edit", keyword);
            }
            return BadRequest();
        }
        public async Task<ActionResult> Edit(int id)
        {
            var keyword = await keyValueService.GetModelAsync(id, true);
            if (keyword != null)
            {
                ViewBag.ModalTitle = keyword.KeyValueType.Name;
                ViewBag.Disable = false;
                return PartialView("_edit", keyword);
            }
            return BadRequest();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(KeyValueLookup data)
        {
            ViewStatusInfo status = new ViewStatusInfo() { AlertType = AlertTypes.Failed };
            ViewBag.StatusInfo = status;
            ViewBag.Disable = false;
            ViewBag.ModalTitle = data.DisplayValue;
            var userId = User.GetJAGCProfileUUID();
            var keywordType = await keyValueService.GetKeyValueTypeAsync(data.KeyValueTypeId);
            data.KeyValueType = keywordType;
            if (!ModelState.IsValid)
            {
                status.Message = "Validation Failed. ";
                HttpContext.Response.Headers.Add("HasError", "1");
                return PartialView("_edit", data);
            }

            if (keywordType.HasCode && string.IsNullOrWhiteSpace(data.Code))
            {
                ModelState.AddModelError("Code", "A Code is required");
                HttpContext.Response.Headers.Add("HasError", "1");
                status.Message = "Please enter a code";
                return PartialView("_edit", data);
            }

            if (await keyValueService.IsDuplicateAsync(data.Id, data.KeyValueTypeId, data.DisplayValue))
            {
                ModelState.AddModelError("DisplayValue", "This keyword already exist in the system");
                HttpContext.Response.Headers.Add("HasError", "1");
                status.Message = "Duplicate keyword";
                return PartialView("_edit", data);
            }

            var modelId = data.Id;
            if (data.IsNewRecord)
            {
                modelId = await keyValueService.AddAsync(data, userId);
            }
            else
            {
                await keyValueService.UpdateAsync(data, userId);
            }
            var keyword = await keyValueService.GetModelAsync(modelId);
            if (keyword != null)
            {

                ViewBag.Disable = false;
                status.AlertType = AlertTypes.Okay;
                status.Message = "Keyword saved successfully!";
                return PartialView("_edit", keyword);
            }
            HttpContext.Response.Headers.Add("HasError", "1");
            status.Message = "Save Failed. Please check the error and save again.";
            return PartialView("_edit", data);
        }




    }
}
