using System.Collections.Generic;
using System.Linq;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;


namespace SLADashboard.Infrastructure
{
    public class SLARepository:ISLARepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();
        public IEnumerable<SLA> SLAConfigurations
        {
            get { return context.SLA.ToList(); }
        }
        public SLA GetById(int id)
        {
            return context.SLA.FirstOrDefault(s => s.ID == id);
        }
        public int Update(SLA sla)
        {
            var existingSLAConfig = context.SLA.Find(sla.ID);
            if (existingSLAConfig != null)
            {
                existingSLAConfig.Name = sla.Name;
                existingSLAConfig.Description = sla.Description;
                existingSLAConfig.Target = sla.Target;
                context.SaveChanges();
                return existingSLAConfig.ID;
            }
            return 0;
        }

        public SLA Insert(SLA sla)
        {
            var profile = context.Profiles.FirstOrDefault(_ => _.ID == sla.ProfileID);
            if (profile != null)
            {
                var SLAObj = new SLA()
                {
                    ProfileID=profile.ID,
                    Name=sla.Name,
                    Description=sla.Description,
                    Target=sla.Target
                };
                context.SLA.Add(SLAObj);
                context.SaveChanges();
                return SLAObj;
            }
            return null;
        }
        public int Delete(int ID, string deletedUserName)
        {
            var recordToDelete = context.SLA.Find(ID);
            if (recordToDelete != null)
            {
                recordToDelete.IsDeleted = true;
                recordToDelete.DeletedUser = deletedUserName;
                context.SaveChanges();
                return recordToDelete.ID;
            }
            return 0;
        }
    }
}
