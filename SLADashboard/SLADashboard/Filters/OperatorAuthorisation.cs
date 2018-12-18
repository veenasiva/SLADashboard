using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SLADashboard.Infrastructure;

namespace SLADashboard.Filters
{

    public class OperatorAuthorisation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var username = filterContext.HttpContext.User.Identity.Name;
            if (((WindowsIdentity)filterContext.HttpContext.User.Identity).Groups.Where(_ => ((_.Translate(typeof(NTAccount)).ToString().Contains(GroupHelper.GetAdminGroup())) || 
                                                                                               _.Translate(typeof(NTAccount)).ToString().Contains(GroupHelper.GetOperatorGroup()))).Any())
            {
                return; //User is Operator so return;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new
                               {
                                   controller = "Operator",
                                   action = "AccessDenied",
                                   userId = username,
                                   routeController = filterContext.RequestContext.RouteData.Values["controller"],
                                   routeAction = filterContext.RequestContext.RouteData.Values["action"]
                               }));
            }
        }
    }
}