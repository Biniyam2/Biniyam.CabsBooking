
using CommonUtil.ListViews;
using JAGCNet.Library.Models;
using JAGCNet.Library.Services.Interfaces;
using JOINBoard.ActionFilters;
using JOIN_Data.Extensions;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Models.ViewModels;
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
    [Authorize(Policy = AuthConstants.POLICY_MANAGE_USERS)]
    public class UsersController : Controller
    {
        private IApplicationMembershipsService membershipService;
        private IJAGCProfilesService jagcProfilesService;
        private IUserService userService;
        private readonly IListViewFactory listViewFactory;
        private readonly IListViewService listViewService;

        private string viewKey = "Users";
        private string defaultSortColumn = "FullName";
        private readonly IAuthorizationService authorizationService;
        private bool isEditor = false;
        public UsersController(
                  IApplicationMembershipsService membershipService,
                  IJAGCProfilesService jagcProfilesService,
                  IUserService userService,
                  IListViewFactory listViewFactory,
                  IListViewService listViewService,
                  IAuthorizationService authorizationService)
        {
            this.membershipService = membershipService;
            this.jagcProfilesService = jagcProfilesService;
            this.userService = userService;
            this.listViewFactory = listViewFactory;
            this.listViewService = listViewService;
            this.authorizationService = authorizationService;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LeftNavKey = LeftNavConstants.USERS;
            ViewBag.LeftNav = "_left_Nav_Admin";
        }
        [HttpPost, HttpGet]
        [ServiceFilter(typeof(IndexActionFilter))]
        public async Task<IActionResult> Index(CommonUtil.ListViews.PagingInfo pageInfo)
        {
            if (HttpContext.Request.Method == "GET")
            {
                if (ViewBag.pageInfo != null)
                {
                    pageInfo = ViewBag.pageInfo;
                }
            }

            if (String.IsNullOrEmpty(pageInfo.CategoryKey))
            {
                pageInfo.ViewKey = viewKey;
            }
            else
            {
                pageInfo.ViewKey = pageInfo.CategoryKey;
            }

            ListView<UserListViewModel, UserListViewModel> listView = listViewFactory.GetListView<UserListViewModel, UserListViewModel>(pageInfo.ViewKey);
            ListViewModel<UserListViewModel, UserListViewModel> vm = await getListViewModel(pageInfo);
            isEditor = (await authorizationService.AuthorizeAsync(User, AuthConstants.POLICY_MANAGE_USERS)).Succeeded;
            if (!isEditor)
                vm.ListView.AllowCreate = false;
            vm.PartialURL = "users";

            var userSetting = AppUtils.GetObjectFromJson<UserSetting>(SessionKeyConstants.USER_SETTING, HttpContext.Session);
            if (userSetting.DefaultToVerticalSplitView)
            {
                pageInfo.DisplayType = DisplayMode.VerticalSplit;
                vm.ListView.AllowExtSearch = false;
            }
            if (Request.IsAjaxRequest())
            {
                if (pageInfo.DisplayType == DisplayMode.Table)
                {
                    vm.ListView.AllowExtSearch = true;
                    return PartialView("ListView/_listViewTableData", vm);
                }
                return PartialView("ListView/_listViewTableData_VerticalSplit", vm);
            }
            return View("Index", vm);
        }
        [HttpGet]
        [Route("[controller]/export")]
        public async Task<IActionResult> Export(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                key = viewKey;
            }
            CommonUtil.ListViews.PagingInfo pageInfo = AppUtils.GetObjectFromJson<CommonUtil.ListViews.PagingInfo>(key, HttpContext.Session);
            pageInfo.PageSize = -1;
            ListViewModel<UserListViewModel, UserListViewModel> vm = await getListViewModel(pageInfo);
            ListView<UserListViewModel, UserListViewModel> listView = listViewFactory.GetListView<UserListViewModel, UserListViewModel>(pageInfo.ViewKey);
            return listView.Export(vm, pageInfo.DisplayCriteria);
        }

        private async Task<ListViewModel<UserListViewModel, UserListViewModel>> getListViewModel(CommonUtil.ListViews.PagingInfo pageInfo)
        {
            ListView<UserListViewModel, UserListViewModel> listView = listViewFactory.GetListView<UserListViewModel, UserListViewModel>(pageInfo.ViewKey);
            Expression<Func<UserListViewModel, object>> sortColumn = o => o.FullName;
            if (!String.IsNullOrEmpty(pageInfo.SortColumn))
            {
                sortColumn = listView.GetSortExpression(pageInfo.SortColumn);
            }
            else
            {
                pageInfo.SortColumn = defaultSortColumn;
            }
            if (pageInfo.ActiveFilterFlag == null || pageInfo.ActiveFilterFlag == ActiveFilterFlag.Active)
            {
                listView.DefaultCriteria = k => k.ProfileActive == "Yes" && k.MembershipActive == "Yes";
            }
            else if (pageInfo.ActiveFilterFlag == ActiveFilterFlag.Inactive)
            {
                listView.DefaultCriteria = k => k.ProfileActive != "Yes" || k.MembershipActive != "Yes";
            }
            listView.UserListView = await listViewService.GetModelAsync(pageInfo.ViewKey, User.GetJAGCProfileUUID());
            var recordList = await userService.GetPagedListAsync(pageInfo, listView, sortColumn);
            pageInfo.TotalRecords = recordList.TotalCount;

            ListViewModel<UserListViewModel, UserListViewModel> vm = new ListViewModel<UserListViewModel, UserListViewModel>(ControllerContext)
            {
                ListView = listView,
                PagedList = recordList,
                PagingInfo = pageInfo
            };
            vm.RecordRoute = "users";
            vm.PartialURL = "users";
            AppUtils.SetObjectAsJson(HttpContext.Session, pageInfo.ViewKey, pageInfo);
            return vm;
        }

        [HttpPost]
        [Route("[controller]/filter-values")]
        public async Task<IActionResult> GetUniqueValues(CommonUtil.ListViews.PagingInfo pageInfo, string columnName)
        {
            ListView<UserListViewModel, UserListViewModel> listView = listViewFactory.GetListView<UserListViewModel, UserListViewModel>(pageInfo.ViewKey);
            if (pageInfo.ActiveFilterFlag == null || pageInfo.ActiveFilterFlag == ActiveFilterFlag.Active)
            {
                listView.DefaultCriteria = k => k.ProfileActive == "Yes" && k.MembershipActive == "Yes";
            }
            else if (pageInfo.ActiveFilterFlag == ActiveFilterFlag.Inactive)
            {
                listView.DefaultCriteria = k => k.ProfileActive != "Yes" || k.MembershipActive != "Yes";
            }
            var uniqueList = await userService.GetUniqueColumnValuesAsync(pageInfo, listView, columnName);
            ViewBag.FilterFieldName = columnName;
            ViewBag.FilterFieldTitle = listView.GetViewColumn(columnName).Title;
            return PartialView("ListView/_ListViewColumnUniqueData", uniqueList);
        }
        [HttpPost]
        [Route("[controller]/update-display")]
        public async Task<IActionResult> UpdateDisplay(CommonUtil.ListViews.PagingInfo pageInfo)
        {

            ListView<UserListViewModel, UserListViewModel> listView = listViewFactory.GetListView<UserListViewModel, UserListViewModel>(pageInfo.ViewKey);
            ListViewModel<UserListViewModel, UserListViewModel> vm = await getListViewModel(pageInfo);
            vm.RecordRoute = "users";
            AppUtils.SetObjectAsJson(HttpContext.Session, "DisplayType", pageInfo.DisplayType);
            await listView.UpdateDisplayType(HttpContext, userService);

            if (pageInfo.DisplayType == DisplayMode.Table)
            {
                vm.ListView.AllowExtSearch = true;
                return PartialView("ListView/_listView", vm);
            }
            vm.ListView.AllowExtSearch = false;
            return PartialView("ListView/_listView_VerticalSplit", vm);
        }

        [Route("[controller]/JAGCUserList")]
        public async Task<ActionResult> JAGCUserList(JAGCNet.Library.Models.PagingInfo pagingInfo)
        {
            var jagcProfiles = await jagcProfilesService.GetJAGCProfilesAsync(pagingInfo);
            return PartialView("_jagcProfileSelector", jagcProfiles);
        }
        [HttpGet]
        [Authorize(Policy = AuthConstants.POLICY_MANAGE_USERS)]
        [Route("[controller]/add-membership")]
        public async Task<ActionResult> AddMembership(string personKey)
        {
            var isDuplicate = await membershipService.IsMemberAsync(personKey, AppConstants.APPLICATION_CODE);
            if (isDuplicate)
            {
                return Json(new { success = false, message = "The user already exist in the system" });
            }
            var membership = await membershipService.AddMembershipAsync(personKey, AppConstants.APPLICATION_CODE, User.GetJAGCProfileUUID(), "Regular User");
            if (membership != null)
            {
                var jagcProfile = await jagcProfilesService.GetJAGCProfileByUUIDAsync(personKey);
                UserSetting permission = await userService.GetSettingByJAGCProfileUUIDAsync(personKey);
                if (permission == null)
                {
                    permission = await AddDefaultPermission(personKey);
                }
                if (permission != null)
                {
                    return Json(new { success = true, personKey = permission.JAGCProfileUUID, name = jagcProfile.DisplayName });
                }
                return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet]
        [Authorize(Policy = AuthConstants.POLICY_MANAGE_USERS)]
        [Route("[controller]/DeactivateMembership")]
        public async Task<ActionResult> DeactivateMembership(string personId)
        {
            var membership = await membershipService.GetMembershipByUUIDAsync(personId, AppConstants.APPLICATION_CODE);
            if (membership != null)
            {
                await membershipService.DeactivateMembershipAsync(personId, AppConstants.APPLICATION_CODE, User.GetJAGCProfileUUID());
                await userService.ToggleActive(personId, User.GetJAGCProfileUUID());
                return RedirectToAction("Details", "Users", new { id = personId });
            }
            return BadRequest();
        }
        [HttpGet]
        [Authorize(Policy = AuthConstants.POLICY_MANAGE_USERS)]
        [Route("[controller]/RestoreMembership")]
        public async Task<ActionResult> RestoreMembership(string personId)
        {
            var membership = await membershipService.GetMembershipByUUIDAsync(personId, AppConstants.APPLICATION_CODE);
            if (membership != null)
            {
                await membershipService.RestoreMembershipAsync(personId, AppConstants.APPLICATION_CODE, User.GetJAGCProfileUUID());
                await userService.ToggleActive(personId, User.GetJAGCProfileUUID());
                return RedirectToAction("Details", "Users", new { id = personId });
            }
            return BadRequest();
        }
        private async Task<UserSetting> AddDefaultPermission(string personId)
        {
            JAGCProfile profile = await jagcProfilesService.GetJAGCProfileByUUIDAsync(personId);
            if (profile != null)
            {
                UserSetting permission = new UserSetting()
                {
                    JAGCProfileUUID = personId,
                    ManageKeywords = false,
                    ManageUsers = false,
                    Active = true
                    //AllowCreatePresentations = false,
                    //AllowCreateSurveys = false,
                };

                await userService.AddAsync(permission, User.GetJAGCProfileUUID());
                return permission;
            }
            return null;

        }
        [HttpGet]
        [Route("[controller]/{key}")]
        [Route("[controller]/Details/{key}")]
        public async Task<IActionResult> Details(string key)
        {
            JAGCProfile profile = await jagcProfilesService.GetJAGCProfileByUUIDAsync(key, true);
            if (profile != null)
            {
                var vm = new ApplicationUserViewModel() { JAGCProfile = profile };
                var userSetting = await userService.GetSettingByJAGCProfileUUIDAsync(key);
                var createdBy = await jagcProfilesService.GetJAGCProfileByUUIDAsync(userSetting.CreatedById, false, true);
                userSetting.CreatedBy = createdBy.FullName;
                vm.UserSetting = userSetting;
                vm.Membership = profile.GetApplicationMembership(AppConstants.APPLICATION_CODE);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_details", vm);
                }
                return View(vm);
                //return PartialView("_details",vm);
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Policy = AuthConstants.POLICY_MANAGE_USERS)]
        [Route("[controller]/{id}/edit-permission")]
        public async Task<IActionResult> EditPermission(int id)
        {
            UserSetting permission = await userService.GetModelAsync(id);
            return PartialView("_editPermission", permission);
        }
        [HttpPost]
        [Authorize(Policy = AuthConstants.POLICY_MANAGE_USERS)]
        [Route("[controller]/SavePermission")]
        public async Task<IActionResult> SavePermission(UserSetting permission)
        {
            var membership = await membershipService.GetMembershipByUUIDAsync(permission.JAGCProfileUUID, AppConstants.APPLICATION_CODE);
            UserSetting permissionInDb = await userService.GetModelAsync(permission.Id);
            permissionInDb.ManageUsers = permission.ManageUsers;
            permissionInDb.ManageKeywords = permission.ManageKeywords;
            //permissionInDb.AllowCreatePresentations = permission.AllowCreatePresentations;
            //permissionInDb.AllowCreateSurveys = permission.AllowCreateSurveys;
            await userService.UpdateAsync(permissionInDb, User.GetJAGCProfileUUID());
            return RedirectToAction("Details", new { key = permission.JAGCProfileUUID });
        }


    }
}
