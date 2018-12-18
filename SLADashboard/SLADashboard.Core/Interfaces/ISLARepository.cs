using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core.Interfaces
{
    public interface ISLARepository
    {
        IEnumerable<SLA> SLAConfigurations { get; }
        SLA GetById(int id);
        SLA Insert(SLA SLAConfig);
        int Update(SLA SLAConfig);
        int Delete(int ID, string deletedUserName);
    }
}
