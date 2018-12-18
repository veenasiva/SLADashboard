using System.Web;
using System.Web.Mvc;

namespace SLADashboard
{
    public sealed class FilterConfig
    {
        private FilterConfig() { }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
