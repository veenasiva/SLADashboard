using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLADashboard.Models
{
    public class AccessDeniedViewModel
    {
        public string UserId { get; set; }
        public string RouteController { get; set; }
        public string RouteAction { get; set; }
    }
}