using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{
    public class ProfilesRepository : IProfilesRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();

        public IEnumerable<Profile> Profiles
        {
            get { return context.Profiles.ToList(); }
        }

        public void Insert(int clientId, string name, string description, string idPrefix)
        {
            var client = context.Clients.FirstOrDefault(_ => _.ID == clientId);
            if (client != null)
            {
                var profile = new Profile()
                {
                    Name = name,
                    Description = description,
                    SLAIDPrefix = idPrefix,
                    ClientID = client.ID
                };
                client.Profiles.Add(profile);
                context.SaveChanges();
            };           
        }
    }
}

