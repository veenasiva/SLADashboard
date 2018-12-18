using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SLADashboard.Filters
{
    public class SessionExpireAuthorise : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            if (filterContext.HttpContext.Session["mode"] != null)
            {
                return; //Session is still active so return;
            }
            else
            {
                var username = filterContext.HttpContext.User.Identity.Name;
                filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new
                               {
                                   controller = "Operator",
                                   action = "SystemTimeOut",
                                   userId = username,
                                   routeController = filterContext.RequestContext.RouteData.Values["controller"],
                                   routeAction = filterContext.RequestContext.RouteData.Values["action"]
                               }));
            }
        }

    }
}