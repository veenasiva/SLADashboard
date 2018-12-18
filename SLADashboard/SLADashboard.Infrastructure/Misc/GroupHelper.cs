using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Infrastructure
{
    public static class GroupHelper
    {
        private static SLADashboardDBContext context = new SLADashboardDBContext();
        public static string GetAdminGroup()
        {
            return context.Settings.Where(x => x.Name == "AdminGroup").FirstOrDefault().Value;
        }

        public static string GetOperatorGroup()
        {
            return context.Settings.Where(x => x.Name == "OperatorGroup").FirstOrDefault().Value;
        }
    }
}
