using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core.Interfaces
{
    public interface IBarCodeConfigSiteRepository
    {
        IEnumerable<BarCodeConfigSite> BarCodeConfigurationSites { get; }
        int Insert(BarCodeConfigSite BarCodeConfigSite);
        void RemoveAll(int barCodeConfigID);
    }
}
