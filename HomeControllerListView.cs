using CommonUtil.ListViews;
using JOIN_Data.Extensions;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JOINBoard.Controllers
{
    public partial class HomeController : Controller
    {


        //ListView Methods
        public async Task<IActionResult> SetPreviewContainerHeight(int height)
        {
            var userId = HttpContext.User.GetJAGCProfileUUID();
            if (!String.IsNullOrEmpty(userId))
            {
                UserSetting setting = await userSettingService.GetSettingByJAGCProfileUUIDAsync(userId, false);
                setting.PreviewBarPosition = height;
                await userSettingService.UpdateAsync(setting, userId);
                AppUtils.SetObjectAsJson(HttpContext.Session, SessionKeyConstants.USER_SETTING, setting);
            }
            return Ok();
        }
        [Route("list-view/{viewKey}/view-setting")]
        public async Task<IActionResult> GetViewSetting(string viewKey)
        {
            var userId = HttpContext.User.GetJAGCProfileUUID();
            var setting = await listViewService.GetModelAsync(viewKey, userId, true);
            setting.ListView = viewFactory.GetListView(viewKey);
            return PartialView("ListView/_listViewSetting", setting);
        }
        [Route("list-view/{viewKey}/view-display-setting")]
        public async Task<IActionResult> GetViewDisplaySetting(string viewKey)
        {
            var userId = HttpContext.User.GetJAGCProfileUUID();
            var setting = await listViewService.GetModelAsync(viewKey, userId);
            setting.ListView = viewFactory.GetListView(viewKey);
            return PartialView("ListView/_listViewDisplaySetting", setting);
        }
        [Route("list-view/{viewKey}/view-export-setting")]
        public async Task<IActionResult> GetViewExportSetting(string viewKey)
        {
            var userId = HttpContext.User.GetJAGCProfileUUID();
            var setting = await listViewService.GetModelAsync(viewKey, userId);
            setting.ListView = viewFactory.GetListView(viewKey);
            return PartialView("ListView/_listViewExportSetting", setting);
        }
        [HttpGet]
        [Route("list-view/{viewKey}/{type}/sort-title")]
        public async Task<IActionResult> SortTitle(string viewKey, string type, string sourceField, string targetField)
        {
            UserListView setting = null;
            if (type == "display")
            {
                setting = await listViewService.SortViewTitleAsync(viewKey, sourceField, targetField, User.GetJAGCProfileUUID());
            }
            else
            {
                setting = await listViewService.SortExportColumnAsync(viewKey, sourceField, targetField, User.GetJAGCProfileUUID());
            }
            setting.ListView = viewFactory.GetListView(viewKey);
            var view = type == "display" ? "ListView/_listViewDisplaySetting" : "ListView/_listViewExportSetting";
            return PartialView(view, setting);
        }

        [HttpGet]
        [Route("list-view/{viewKey}/{type}/add-field")]
        public async Task<IActionResult> AddListField(string viewKey, string type, string field)
        {
            var setting = await listViewService.AddFieldAsync(viewKey, field, type, User.GetJAGCProfileUUID());
            setting.ListView = viewFactory.GetListView(viewKey);
            var view = type == "display" ? "ListView/_listViewDisplaySetting" : "ListView/_listViewExportSetting";
            return PartialView(view, setting);
        }
        [HttpGet]
        [Route("list-view/{viewKey}/{type}/remove-field")]
        public async Task<IActionResult> RemoveListField(string viewKey, string type, string field)
        {
            var setting = await listViewService.RemoveFieldAsync(viewKey, field, type, User.GetJAGCProfileUUID());
            setting.ListView = viewFactory.GetListView(viewKey);
            var view = type == "display" ? "ListView/_listViewDisplaySetting" : "ListView/_listViewExportSetting";
            return PartialView(view, setting);
        }

        [HttpGet]
        [Route("list-view/{viewKey}/{type}/check-all")]
        public async Task<IActionResult> CheckAllFields(string viewKey, string type)
        {
            var setting = await listViewService.CheckAllAsync(viewKey, type, User.GetJAGCProfileUUID());
            setting.ListView = viewFactory.GetListView(viewKey);
            var view = type == "display" ? "ListView/_listViewDisplaySetting" : "ListView/_listViewExportSetting";
            return PartialView(view, setting);
        }
        [HttpGet]
        [Route("list-view/{viewKey}/{type}/uncheck-all")]
        public async Task<IActionResult> UnCheckAllFields(string viewKey, string type)
        {
            var setting = await listViewService.UncheckAllAsync(viewKey, type, User.GetJAGCProfileUUID());
            setting.ListView = viewFactory.GetListView(viewKey);
            var view = type == "display" ? "ListView/_listViewDisplaySetting" : "ListView/_listViewExportSetting";
            return PartialView(view, setting);
        }
        //End ListView Methods
    }
}
