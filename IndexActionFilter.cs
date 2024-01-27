using CommonUtil.ListViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOINBoard.ActionFilters
{
    public class IndexActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                var queryString = context.HttpContext.Request.Query["pageInfo"];
                if (!string.IsNullOrEmpty(queryString))
                {
                    PagingInfo pageInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingInfo>(queryString);
                    if (pageInfo != null)
                    {
                        var controller = context.Controller as Controller;
                        controller.ViewBag.pageInfo = pageInfo;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
}
