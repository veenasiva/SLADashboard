using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SLADashboard.Infrastructure
{
    public static class IPhelper
    {
        public static string GetIPAddress(this HttpRequestBase Request)
        {
            if (Request.Headers["CF-CONNECTING-IP"] != null) return Request.Headers["CF-CONNECTING-IP"].ToString();

            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) return Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();

            return Request.UserHostAddress;
        }
    }
}
