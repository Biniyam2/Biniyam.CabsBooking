using CommonUtil.ListViews;
using JAGCNet.Library.Models;
using JAGCNet.Library.Services.Interfaces;
using JOIN_Data.Extensions;
using JOIN_Data.Models.CoreModels;
using JOIN_Data.Persistence;
using JOIN_Data.Services.Interfaces;
using JOIN_Data.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;


namespace JOINBoard.Controllers
{
    public partial class HomeController : Controller
    {
        private IJAGCProfilesService profileService;
        private IApplicationMembershipsService membershipService;
        private IUserService userSettingService;
        private IListViewService listViewService;
        private IListViewFactory viewFactory;

        private IJAGCUtilityService jagcUtilityService;
        private IConfiguration config;
        private IWebHostEnvironment env;

        private IApplicantService applicantService;
        private IApplicationService applicationService;

        public HomeController(
                  IWebHostEnvironment env,
                  IJAGCProfilesService profileService,
                  IApplicationMembershipsService membershipService,
                  IListViewService listViewService,
                  IListViewFactory viewFactory,
                  IJAGCUtilityService jagcUtilityService,
                  IUserService userSettingService,
                  IConfiguration config,
                  IApplicationService applicationService,
                  IApplicantService applicatorService
            )
        {
            this.profileService = profileService;
            this.membershipService = membershipService;
            this.userSettingService = userSettingService;
            this.jagcUtilityService = jagcUtilityService;
            this.listViewService = listViewService;
            this.viewFactory = viewFactory;
            this.env = env;
            this.config = config;
            this.applicationService = applicationService;
            this.applicantService = applicatorService;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LeftNavKey = LeftNavConstants.HOME;
        }

        public ActionResult Index() => View();
        [Route("login")]
        public async Task<IActionResult> login(string ReturnUrl)
        {

            ClaimsPrincipal principal;

            if (env.EnvironmentName == "Local")
            {               
                principal = await GetPrincipal(AppConstants.DEV_TEST_USER_ID, false);              
            }
            else
            {
                principal = await GetPrincipal(0, true);
            }

            //principal = await GetPrincipal(0, true);
            if (principal != null)
            {
                var identity = ((ClaimsIdentity)principal.Identity);
                String active = identity.GetClaimValue("Active");
                if (active == "True")
                {
                    var authProperties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(Int32.Parse(config.GetValue<string>("SessionTimeOut"))),
                        IsPersistent = false,
                    };
                    //var dict = new Dictionary<string, int>();
                    //dict.Add("APP_1", 20);
                    //dict.Add("APP_2", 30);
                    //dict.Add("APP_3", 50);
                    //dict.Add("APP_4", 80);
                    //dict.Add("APP_5", 120);
                    //ViewBag["appsUsersCount"] = dict;

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                    await membershipService.UpdateLastLoginAsync(identity.GetClaimValue(ClaimConstants.JAGCPROFILE_UUID).ToString(), AppConstants.APPLICATION_CODE);
                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Cases");
                    }
                }
            }
            return RedirectToAction(nameof(ErrorLogin));
        }


        private async Task<ClaimsPrincipal> GetPrincipal(long userId, bool isCacUser)
        {
            JAGCProfile jagcProfile = null;
            CacUser cacUser = null;
            if (isCacUser)
            {
                cacUser = new CacUser(HttpContext);
                jagcProfile = await profileService.GetJAGCProfileForCacUserAsync(cacUser, true);
            }
            else
            {
                jagcProfile = await profileService.GetJAGCProfileByDodIdAsync(userId, true);
            }

            if (jagcProfile != null && jagcProfile.Active)
            {
                var Identity = jagcProfile.GetClaimsIdentity();
                //Check if ITD User
                var isITDUser = jagcProfile.GlobalAppAdmin || jagcProfile.GlobalAppReader || jagcProfile.HasSpecialFlag;
                var allowCreate = jagcProfile.ITDServiceDesk;
                var isITDAdmin = jagcProfile.GlobalAppAdmin || jagcProfile.ITDLeadership || jagcProfile.ITDProjectOfficer;
                if (isITDUser)
                {
                    Identity.AddClaim(new Claim(ClaimConstants.IS_ITD_USER, "True"));
                }
                Identity.AddClaim(new Claim(ClaimConstants.ACTIVE_USER, "True"));
                Identity.AddClaim(new Claim(ClaimConstants.JAGCNET_MEMBER, "True"));
                var membership = await membershipService.GetMembershipByUUIDAsync(jagcProfile.UUID, AppConstants.APPLICATION_CODE);
                if (membership != null && membership.Active)
                {
                    if (membership.LastLogin != null)
                    {
                        Identity.AddClaim(new Claim(ClaimConstants.LAST_LOGIN, membership.LastLogin.Value.ToLocalTime().ToString()));
                    }
                    UserSetting permission = await userSettingService.GetSettingByJAGCProfileUUIDAsync(jagcProfile.UUID);
                    if (permission != null)
                    {
                        Identity.AddClaim(new Claim(ClaimConstants.APPLICATION_USER_ID, permission.Id.ToString()));
                        Identity.AddClaim(new Claim(ClaimConstants.MANAGE_KEYWORDS, permission.ManageKeywords.ToString()));
                        Identity.AddClaim(new Claim(ClaimConstants.MANAGE_USERS, permission.ManageUsers.ToString()));
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_PRESENTATION, permission.AllowCreatePresentations.ToString()));
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_SURVEY, permission.AllowCreateSurveys.ToString()));
                        AppUtils.SetObjectAsJson(HttpContext.Session, SessionKeyConstants.USER_SETTING, permission);
                    }
                    return new ClaimsPrincipal(Identity);
                }
                else if (isITDUser)
                {
                    if (allowCreate)
                    {
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_PRESENTATION, "True"));
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_SURVEY, "True"));
                    }
                    else
                    {
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_PRESENTATION, "False"));
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_SURVEY, "False"));
                    }


                    Identity.AddClaim(new Claim(ClaimConstants.GLOBAL_ADMIN, jagcProfile.GlobalAppAdmin.ToString()));
                    Identity.AddClaim(new Claim(ClaimConstants.GLOBAL_READER, jagcProfile.GlobalAppReader.ToString()));
                    if (jagcProfile.ITDServiceDesk || jagcProfile.GlobalAppAdmin)
                    {
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_PRESENTATION, "True"));
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_SURVEY, "True"));
                    }
                    else
                    {
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_PRESENTATION, "False"));
                        //Identity.AddClaim(new Claim(ClaimConstants.CREATE_SURVEY, "False"));
                    }
                    if (isITDAdmin || jagcProfile.ITDServiceDesk)
                    {
                        Identity.AddClaim(new Claim(ClaimConstants.MANAGE_KEYWORDS, "True"));
                        Identity.AddClaim(new Claim(ClaimConstants.MANAGE_USERS, "True"));
                    }
                    else
                    {
                        Identity.AddClaim(new Claim(ClaimConstants.MANAGE_KEYWORDS, "False"));
                        Identity.AddClaim(new Claim(ClaimConstants.MANAGE_USERS, "False"));
                    }


                    UserSetting permission = await userSettingService.GetSettingByJAGCProfileUUIDAsync(jagcProfile.UUID);
                    if (permission == null)
                    {
                        //Add a default permission record for the ITD User
                        UserSetting permission1 = new UserSetting()
                        {
                            JAGCProfileUUID = jagcProfile.UUID,
                            ManageKeywords = false,
                            ManageUsers = false
                        };
                        if (jagcProfile.GlobalAppAdmin || jagcProfile.ITDServiceDesk)
                        {
                            // permission1.AllowCreatePresentations = true;
                            // permission1.AllowCreateSurveys = true;
                        }
                        await userSettingService.AddAsync(permission1, User.GetJAGCProfileUUID());
                    }
                    permission = await userSettingService.GetSettingByJAGCProfileUUIDAsync(jagcProfile.UUID);
                    if (permission != null)
                    {
                        Identity.AddClaim(new Claim(ClaimConstants.APPLICATION_USER_ID, permission.Id.ToString()));
                        AppUtils.SetObjectAsJson(HttpContext.Session, SessionKeyConstants.USER_SETTING, permission);
                    }

                    return new ClaimsPrincipal(Identity);
                }
                else
                {
                    return new ClaimsPrincipal(Identity);
                }
            }
            else if (!cacUser.IsEmpty())
            {
                var Identity = new ClaimsIdentity(
                         new[]
                         {
                            new Claim(ClaimTypes.Name,cacUser.DoDEmail),
                            new Claim("FullNameFML", cacUser.DoDEmail),
                            new Claim("DODID", cacUser.DODID.ToString()),
                            new Claim("JAGCProfileUUID", cacUser.DODID.ToString()),
                            new Claim(ClaimConstants.ACTIVE_USER, "True"),
                            new Claim(ClaimConstants.JAGCNET_MEMBER, "False")
                         },
                         CookieAuthenticationDefaults.AuthenticationScheme
               );
            }
            return null;
        }
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("_JAGCNet.Cookie." + AppConstants.APPLICATION_CODE);
            return View("Logout");
        }
        [Route("keep-alive")]
        public ActionResult KeepAlive() => Json(new { success = true });

        [Route("403-forbidden-error")]
        public IActionResult AccessError()
        {
            return View("ErrorForbidden");
        }
        public IActionResult ErrorLogin() => View();
        public IActionResult ErrorForbidden() => View();


    }
}
