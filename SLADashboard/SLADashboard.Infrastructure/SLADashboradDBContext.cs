using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{
    public class SLADashboardDBContext : DbContext
    {
        public SLADashboardDBContext() : base("SLA")
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<SystemConfig> SystemConfig { get; set; }
        public DbSet<Settings> Settings { get; set; }


        public DbSet<SLA> SLA { get; set; }
        public DbSet<SLAValues> SLAValues { get; set; }



    }
}