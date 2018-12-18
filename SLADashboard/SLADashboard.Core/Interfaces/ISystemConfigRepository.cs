using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core.Interfaces
{
    public interface ISystemConfigRepository
    {
        IEnumerable<SystemConfig> SystemConfig { get; }
        IEnumerable<Settings> Settings { get; }
        void Insert(string UserName, Mode mode);
        void Update(int ID, string username, Mode mode);
    }
}
