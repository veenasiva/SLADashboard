using System;
using SLADashboard.Core.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{
    public class ClientsRepository : IClientsRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();

        public IEnumerable<Client> Clients
        {
            get { return context.Clients.ToList(); }
        }

        public IEnumerable<Client> ConfiguredClients()
        {
            var profileFilter = (from profile in context.Profiles
                                 select new { profile.ID, profile.ClientID, profile.Name, profile.Description }).Distinct().ToList()
                                .Select(_ => new Profile() { ID = _.ID, ClientID = _.ClientID, Name = _.Name, Description = _.Description });

            var clients = context.Clients.ToList();

            clients = (from client in clients
                       join profile in profileFilter
                           on client.ID equals profile.ClientID
                       select client).Distinct().ToList();

            clients = clients.Select(_ => new Client()
            {
                ID = _.ID,
                Description = _.Description,
                Profiles = profileFilter.Where(p => p.ClientID == _.ID).ToList()
            }).ToList();
            return clients;
        }

        public IEnumerable<Client> FilteredClients(string IP_Prefix)
        {
            var profileFilter = from profile in context.Profiles
                                select profile;// new Profile(){ ClientID = profile.ClientID, ID = profile.ID, Description = profile.Description };


            var clients = (from client in context.Clients
                           join profile in profileFilter
                           on client.ID equals profile.ClientID
                           select client).Distinct().ToList();

            clients = clients.Select(_ => new Client()
            {
                ID = _.ID,
                Description = _.Description,
                Profiles = profileFilter.Where(p => p.ClientID == _.ID).ToList()
            }).ToList();
            return clients;
        }

        public void Insert(string description)
        {
            var client = new Client()
            {
                Description = description
            };
            context.Clients.Add(client);
            context.SaveChanges();
        }
    }
}