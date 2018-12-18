using System.Collections.Generic;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{
    public class SLAValuesRepository:ISLAValuesRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();
        public  SLAValuesRepository(SLADashboardDBContext dbcontext)
        { context = dbcontext; }
        public IEnumerable<SLAValuesForSLA> GetSLAValues(int profileID,DateTime reportingDate)
        {
            var slaValues = from sla in context.SLA
                            join slavalues in context.SLAValues on new { p1=sla.ID, p2=reportingDate } equals new {p1= slavalues.SLAID, p2=slavalues.ReportingDate }  into temp
                            from slav in temp.DefaultIfEmpty()
                            where sla.ProfileID == profileID && !(sla.IsDeleted.HasValue && sla.IsDeleted.Value==true)
                            select new SLAValuesForSLA()
                            {
                                ID = slav.ReportingDate == reportingDate ? slav.ID : (int?)null,
                                ProfileID = sla.ProfileID,
                                SLAID = sla.ID,
                                ReportingDate = slav.ReportingDate == reportingDate ? slav.ReportingDate : (DateTime?)null,
                                QuantityProcessed = slav.ReportingDate == reportingDate ? slav.QuantityProcessed : (int?)null,
                                QuantityOutsideofSLA = slav.ReportingDate == reportingDate ? slav.QuantityOutsideofSLA : (int?)null,
                                SlaName = sla.Name,
                                SlaDescription = sla.Description
                            };

            
            return slaValues;
        }
        public IEnumerable<SLAValuesForSLA> GetSLAReport(int clientID, int profileID, DateTime reportingDate)
        {
            var slaValues = from sla in context.SLA
                            join slavalues in context.SLAValues on new { p1 = sla.ID, p2 = reportingDate } equals new { p1 = slavalues.SLAID, p2 = slavalues.ReportingDate } into temp
                            from slav in temp.DefaultIfEmpty()
                            join p in context.Profiles on sla.ProfileID equals p.ID
                            join c in context.Clients on p.ClientID equals c.ID
                            where c.ID==clientID && sla.ProfileID == (profileID>0?profileID:sla.ProfileID) && !(sla.IsDeleted.HasValue && sla.IsDeleted.Value == true) //&& ((slav.ReportingDate >= reportingStartDate && slav.ReportingDate <= reportingEndDate )|| slav.ReportingDate == null)
                           orderby sla.ProfileID
                            select new SLAValuesForSLA()
                            {
                                ID = slav.ReportingDate == reportingDate ? slav.ID : (int?)null,
                                ProfileID = sla.ProfileID,
                                SLAID = sla.ID,
                                ReportingDate = slav.ReportingDate == reportingDate ? slav.ReportingDate : (DateTime?)null,
                                QuantityProcessed = slav.ReportingDate == reportingDate ? slav.QuantityProcessed : (int?)null,
                                QuantityOutsideofSLA = slav.ReportingDate == reportingDate ? slav.QuantityOutsideofSLA : (int?)null,
                                SlaName = sla.Name,
                                SlaDescription = sla.Description,
                                SlaTarget=sla.Target
                            };

            return slaValues;
        }
        public SLAValues GetById(int? id)
        {
               return id.HasValue ?context.SLAValues.FirstOrDefault(s => s.ID == id):null;
         
        }
        public IEnumerable<SLAValues> GetBySLAId(int id)
        {
            return context.SLAValues.Where(s => s.SLAID == id).ToList();
        }
        public string Insert(SLAValues slaValues)
        {
            try
            {
                var SLAValuesObj = new SLAValues()
                {
                    SLAID = slaValues.SLAID,
                    ReportingDate=slaValues.ReportingDate,
                    QuantityProcessed = slaValues.QuantityProcessed,
                    QuantityOutsideofSLA = slaValues.QuantityOutsideofSLA,
                    UpdatedBy=slaValues.UpdatedBy,
                    UpdatedDate=slaValues.UpdatedDate
                };
                context.SLAValues.Add(SLAValuesObj);
                context.SaveChanges();
                return "1";
            }
            catch (DbEntityValidationException ex)
            {
                return ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage + ":" + ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().PropertyName;
            }

        }
        public string Update(SLAValues sla)
        {
            var existingSLAValue = context.SLAValues.Find(sla.ID);
            if (existingSLAValue != null)
            {
                try
                {
                    existingSLAValue.QuantityProcessed = sla.QuantityProcessed;
                    existingSLAValue.QuantityOutsideofSLA = sla.QuantityOutsideofSLA;
                    existingSLAValue.UpdatedBy = sla.UpdatedBy;
                    existingSLAValue.UpdatedDate = sla.UpdatedDate;
                    context.SaveChanges();
                    return existingSLAValue.ID.ToString();
                }catch(DbEntityValidationException ex)
                {
                    return ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage+":"+ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().PropertyName;
                }
            }
            return "Error-No matching record";
        }
    }
    
}
