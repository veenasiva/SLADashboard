using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{

    public class BarCodeConfigSiteRepository : IBarCodeConfigSiteRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();

        public IEnumerable<BarCodeConfigSite> BarCodeConfigurationSites
        {
            get { return context.BarCodeConfigSite.ToList(); }
        }

        public void RemoveAll(int barCodeConfigID)
        {
            var recordToRemove = context.BarCodeConfigSite.Where(_ => _.BarCodeConfigID == barCodeConfigID).ToList();
            context.BarCodeConfigSite.RemoveRange(recordToRemove);
            context.SaveChanges();
        }

        public int Insert(BarCodeConfigSite barCodeConfigSite)
        {
            context.BarCodeConfigSite.Add(barCodeConfigSite);
            context.SaveChanges();
            return 0;
        }
    }
}
