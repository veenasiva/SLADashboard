using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLADashboard.Core;
using SLADashboard.Core.Interfaces;

namespace SLADashboard.Infrastructure
{

    public class SiteRepository : ISiteRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();

        public IEnumerable<Site> Sites
        {
            get { return context.Site.ToList(); }
        }
    }
}
