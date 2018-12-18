using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLADashboard.Core.Interfaces
{
    public interface ISLAValuesRepository
    {
        IEnumerable<SLAValuesForSLA> GetSLAValues(int profileID,DateTime reportingDate);
        IEnumerable<SLAValuesForSLA> GetSLAReport(int clientID, int profileID, DateTime reportingDate);
        SLAValues GetById(int? id);
        IEnumerable<SLAValues> GetBySLAId(int id);
        string Insert(SLAValues slaValues);
        string Update(SLAValues slaValue);
        //int Delete(int ID, string deletedUserName);
    }
}
