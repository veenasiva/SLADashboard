using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLADashboard.Core;
using SLADashboard.Core.Interfaces;

namespace SLADashboard.Infrastructure
{

        public class SystemConfigRepository : ISystemConfigRepository
        {
            private SLADashboardDBContext context = new SLADashboardDBContext();

            public IEnumerable<Settings> Settings
            {
                get { return context.Settings.ToList(); }
            }

            public IEnumerable<SystemConfig> SystemConfig
            {
                get { return context.SystemConfig.ToList(); }
            }

            public void Insert(string UserName, Mode mode)
            {
                context.SystemConfig.Add(new SystemConfig() { UserName = UserName, Mode = mode });
                context.SaveChanges();
            }

            public void Update(string UserName, Mode mode)
            {

                context.SystemConfig.Add(new SystemConfig() { UserName = UserName, Mode = mode });
                context.SaveChanges();
            }

            public void Update(int ID, string username, Mode mode)
            {
                var existingsystemConfig = context.SystemConfig.Find(ID);
                if (existingsystemConfig != null)
                {
                    existingsystemConfig.UserName = username;
                    existingsystemConfig.Mode = mode;
                    context.SaveChanges();
                }
            }
        }
}
