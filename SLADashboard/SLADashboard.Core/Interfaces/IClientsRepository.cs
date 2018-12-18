using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SLADashboard.Core.Interfaces
{
    public interface IClientsRepository
    {
        IEnumerable<Client> Clients { get; }
        void Insert(string description);
        IEnumerable<Client> FilteredClients(string IP_Prefix);
        IEnumerable<Client> ConfiguredClients();
    }
}