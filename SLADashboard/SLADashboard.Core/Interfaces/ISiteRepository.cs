using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core.Interfaces
{

    public interface ISiteRepository
    {
        IEnumerable<Site> Sites { get; }
    }
}
