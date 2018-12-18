using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core.Interfaces
{
    public interface IBarCodeConfigRepository
    {
        IEnumerable<BarCodeConfig> BarCodeConfigurations { get; }
        int Insert(BarCodeConfig BarCodeConfig);
        void Update(BarCodeConfig BarCodeConfig);
    }
}
