using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{
    public class BarCodeConfigRepository : IBarCodeConfigRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();

        public IEnumerable<BarCodeConfig> BarCodeConfigurations
        {
            get { return context.BarCodeConfig.ToList(); }
        }

        public void Update(BarCodeConfig barCodeConfig)
        {
            var existingBarCodeConfig = context.BarCodeConfig.Find(barCodeConfig.ID);
            if (existingBarCodeConfig != null)
            {
                existingBarCodeConfig.ExportFileName = barCodeConfig.ExportFileName;
                existingBarCodeConfig.UseDefaultFileName = barCodeConfig.UseDefaultFileName;
                existingBarCodeConfig.UseTimeInFileName = barCodeConfig.UseTimeInFileName;
                existingBarCodeConfig.FileExportType = barCodeConfig.FileExportType;
                existingBarCodeConfig.DelimiterType = barCodeConfig.DelimiterType;
                existingBarCodeConfig.ExportSchedule = barCodeConfig.ExportSchedule;
                existingBarCodeConfig.ExportScheduleTime = barCodeConfig.ExportScheduleTime;
                existingBarCodeConfig.SenderDetails = barCodeConfig.SenderDetails;
                context.SaveChanges();
            }
        }

        public int Insert(BarCodeConfig barCodeConfig)
        {
            var profile = context.Profiles.FirstOrDefault(_ => _.ID == barCodeConfig.ProfileID);
            if (profile != null)
            {
                var barCodeConfigObj = new BarCodeConfig()
                {
                    ProfileID = profile.ID,
                    ExportFileName = barCodeConfig.ExportFileName,
                    DelimiterType = barCodeConfig.DelimiterType,
                    ExportSchedule = barCodeConfig.ExportSchedule,
                    ExportScheduleTime = barCodeConfig.ExportScheduleTime,
                    UseDefaultFileName = barCodeConfig.UseDefaultFileName,
                    UseTimeInFileName = barCodeConfig.UseTimeInFileName,
                    FileExportType = barCodeConfig.FileExportType,
                    SenderDetails = barCodeConfig.SenderDetails,
                    Sites = barCodeConfig.Sites
                };
                context.BarCodeConfig.Add(barCodeConfigObj);
                context.SaveChanges();
                return barCodeConfigObj.ID;
            }
            return 0;
        }
    }
}

