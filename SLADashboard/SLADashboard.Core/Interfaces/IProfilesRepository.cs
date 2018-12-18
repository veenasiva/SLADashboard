using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLADashboard.Core.Interfaces
{
    public interface IProfilesRepository
    {
        IEnumerable<Profile> Profiles { get; }
        void Insert(int clientId, string name, string description, string idPrefix);
    }
}