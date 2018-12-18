using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SLADashboard.Core.Interfaces;
using SLADashboard.Models;
using SLADashboard.Infrastructure;
using System.IO;
using System.Collections.Generic;
using SLADashboard.Core;
using System.Security.Principal;
using SLADashboard.Filters;
using System.Globalization;
using System.Web.UI;


namespace SLADashboard.Controllers
{
    public class OperatorController : Controller
    {
        private IClientsRepository clientRepository;
        private IProfilesRepository profileRepository;
        private ISystemConfigRepository systemConfigRepository;
        private ISLARepository slaRepository;
        private ISLAValuesRepository slaValuesRepository;

        public IClientsRepository ClientRepository { get => clientRepository; set => clientRepository = value; }
        public IProfilesRepository ProfileRepository { get => profileRepository; set => profileRepository = value; }
        public ISystemConfigRepository SystemConfigRepository { get => systemConfigRepository; set => systemConfigRepository = value; }
        public ISLARepository SlaRepository { get => slaRepository; set => slaRepository = value; }
        public ISLAValuesRepository SlaValuesRepository { get => slaValuesRepository; set => slaValuesRepository = value; }

        public OperatorController(IClientsRepository clientsRepository,
            IProfilesRepository profileRepository, ISystemConfigRepository systemConfigRepository,ISLARepository slaRepository,ISLAValuesRepository slaValuesRepository)
        {
            ClientRepository = clientsRepository;
            ProfileRepository = profileRepository;
            SystemConfigRepository = systemConfigRepository;
            SlaRepository = slaRepository;
            SlaValuesRepository = slaValuesRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult FilterClientSearchList(string filter)
        {
                var clientViewModel = this.ClientRepository.ConfiguredClients().Where(_ => _.Description.ToUpper().StartsWith(filter.ToUpper())).ToList();
                return PartialView("_ClientSearchView", clientViewModel);
        }

        public ActionResult SystemTimeOut(string UserId, string routeController, string routeAction)
        {
            if ((UserId != null) && (UserId.Contains("\\")))
            {
                //Remove Domain Name
                UserId = UserId.Split('\\')[1];
            }
            var accessDeniedViewModel = new AccessDeniedViewModel { UserId = UserId, RouteController = routeController, RouteAction = routeAction };
            return View(accessDeniedViewModel);
        }

        public ActionResult AccessDenied(string UserId, string routeController, string routeAction)
        {
            if ((UserId != null) && (UserId.Contains("\\")))
            {
                //Remove Domain Name
                UserId = UserId.Split('\\')[1];
            }
            var accessDeniedViewModel = new AccessDeniedViewModel { UserId = UserId, RouteController = routeController, RouteAction = routeAction };
            return View(accessDeniedViewModel);
        }

        [OperatorAuthorisation]
        public ActionResult SelectConfiguredProfile()
        {
            // Set Inital Mode
            var userName = (((WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name);

            var requiredGroups = ((WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Groups
                .Where(_ => (_.Translate(typeof(NTAccount)).ToString().Contains(GroupHelper.GetAdminGroup()))
                         || (_.Translate(typeof(NTAccount)).ToString().Contains(GroupHelper.GetOperatorGroup())));
            if (requiredGroups.Count() == 0)
            {
                return RedirectToAction("AccessDenied", 
                    new { UserId = userName,
                          routeController = RouteData.Values["controller"],
                          routeAction = RouteData.Values["action"]
                });
            }


            var systemConfig = SystemConfigRepository.SystemConfig.Where(_ => _.UserName == userName).FirstOrDefault();
            if (systemConfig == null)
            {
                SystemConfigRepository.Insert(userName, Mode.Operator);
                systemConfig = SystemConfigRepository.SystemConfig.Where(_ => _.UserName == userName).First();
            }
            Session["mode"] = systemConfig.Mode;

            IEnumerable<Client> clientViewModel = new List<Client>();

            clientViewModel = ClientRepository.ConfiguredClients().ToList();
            return View(clientViewModel);
        }

       
        [OperatorAuthorisation]
        [SessionExpireAuthorise]
        public ActionResult ConfigureStatus()
        {
            var clients = ClientRepository.Clients.OrderBy(_ => _.Description).ToList();

            if (clients.Count() == 0)
            {
                return RedirectToAction("SelectConfiguredProfile"); // No clients exist so revert back to configure screen
            }

            var profiles = ProfileRepository.Profiles.Where(_ => _.ClientID == clients.First().ID).ToList();

            if (profiles.Count() == 0)
            {
                return RedirectToAction("SelectConfiguredProfile"); // No profiles exist for the default client, so revert back to configure screen
            }

            return View(new ConfigureStatusViewModel()
            {
                Clients = clients,
                Profiles = profiles
            });
        }


      
        [OperatorAuthorisation]
        [SessionExpireAuthorise]
        public ActionResult ConfigureReport(int selectedYear, int selectedMonth)
        {
            var clients = ClientRepository.Clients.OrderBy(_ => _.Description).ToList();

            if (clients.Count() == 0)
            {
                return RedirectToAction("SelectConfiguredProfile"); // No clients exist so revert back to configure screen
            }

            return View(new ConfigureReportViewModel()
            {
                Clients = clients,
                Years = GetYears(DateTime.Now.Year),
                Months=GetMonths(DateTime.Now.Year),
                SelectedYear = selectedYear,
                SelectedMonth = selectedMonth
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult LoadProfilesForClient(int clientID)
        {
            var profiles = ProfileRepository.Profiles.Where(_ => _.ClientID == clientID).ToList();
            return PartialView("_ReportProfilesView", profiles);
        }


        [OperatorAuthorisation]
        [SessionExpireAuthorise]
        [HttpGet]
        public ActionResult SLAValues( int profileId, int selectedYear, int selectedMonth)
        {
                        
            var profile = ProfileRepository.Profiles.FirstOrDefault(_ => _.ID == profileId);
            var client = ClientRepository.Clients.FirstOrDefault(_ => _.ID == profile.ClientID);
            ViewBag.clientname = client.Description;
            ViewBag.profilename = profile.Name;
            return View(new ConfigureSLAValuesViewModel()
            {
                ProfileID=profileId,
                Years = GetYears(selectedYear),
                Months = GetMonths(selectedYear),
                SelectedYear= selectedYear,
                SelectedMonth=selectedMonth
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SLAValuesTable(int profileId,  int selectedYear, int selectedMonth)
        {
            int? Year = DateTime.Now.Year;

            IEnumerable<SLAValuesForSLA> slaValues = new List<SLAValuesForSLA>();

            var d = new DateTime(selectedYear, selectedMonth, 1);
            slaValues = SlaValuesRepository.GetSLAValues(profileId, d);

            var result=new SLAValuesTableModel()
            {
                SlaValues = slaValues.ToList(),
                ProfileID=profileId,
                SelectedYear=selectedYear,
                SelectedMonth=selectedMonth
            };
            return PartialView("_SLAValuesView", result);
        }
        [OperatorAuthorisation]
        [SessionExpireAuthorise]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")] //We need to catch general exception if any and report it to the end user
        public JsonResult SLAValues(SLAValuesModel sla)
        {
            SLAValues slaValues = new SLAValues();
            var result=string.Empty;

            if (sla.SlaValues != null)
            {
                foreach (var slavalue in sla.SlaValues)
                {
                    if (slavalue.QuantityProcessed.HasValue && slavalue.QuantityOutsideofSLA.HasValue)
                    {
                        try
                        {
                            slaValues.UpdatedBy = (((WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name);
                            slaValues.UpdatedDate = DateTime.Now;
                            if (slavalue.ID > 0)//update
                            {
                                slaValues.ID = slavalue.ID.Value;
                                slaValues.QuantityProcessed = slavalue.QuantityProcessed.Value;
                                slaValues.QuantityOutsideofSLA = slavalue.QuantityOutsideofSLA.Value;
                                result = SlaValuesRepository.Update(slaValues);
                            }
                            else//insert
                            {
                                slaValues.SLAID = slavalue.SLAID;
                                slaValues.ReportingDate = new DateTime(sla.SelectedYear, sla.SelectedMonth, 1);
                                slaValues.QuantityProcessed = slavalue.QuantityProcessed.Value;
                                slaValues.QuantityOutsideofSLA = slavalue.QuantityOutsideofSLA.Value;
                                result = SlaValuesRepository.Insert(slaValues);
                            }
                        }
                        catch(InvalidOperationException oex)
                        {
                            Console.Write(oex.Message);
                            result = "Error-Please enter valid numbers";
                        }
                        catch(Exception ex)
                        {
                            result = "Error-" + ex.Message;
                        }
                    }
                }
                
            }
            if (result!=null && result.StartsWith("Error-"))
            {
                ModelState.AddModelError("ErrorDescription", result);
                return Json(new JsonResultReturn()
                {
                    Success = false,
                    ErrorMessage = ModelState.Values.Where(i => i.Errors.Count > 0).Select(_ => _.Errors[0].ErrorMessage).FirstOrDefault()
                });
            }
            return Json(new JsonResultReturn() { Success = true });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult LoadMonthsForYear(int year)
        {
            var months = GetMonths(year);
            return PartialView("_MonthsListView", months);
        }
        #region MonthYear dropdowns

        private List<Year> GetYears(int? iSelectedYear)
        {
            iSelectedYear = iSelectedYear??DateTime.Now.Year;
            List<Year> years = new List<Year>();
            for (int i = 2017; i <= iSelectedYear; i++)
            {
                years.Add(new Year
                {
                    key = i,
                    value = i.ToString()
                });
            }

            //Default It will Select Current Year  
            return years;
        }


        //DropDown : GetMonths() will fill Months as per selected Year and Return List.  
        private List<Month> GetMonths(int? iSelectedYear)
        {
            List<Month> monthsList = new List<Month>();
            var months = Enumerable.Range(1, 12).Select(i => new
            {
                A = i,
                B = DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
            });

            int CurrentMonth = 1; //January  

            if (iSelectedYear == DateTime.Now.Year)
            {
                CurrentMonth = DateTime.Now.Month;
                months = Enumerable.Range(1, CurrentMonth).Select(i => new
                {
                    A = i,
                    B = DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
                });
            }

            foreach (var item in months)
            {
                monthsList.Add(new Month { value = item.B.ToString(), key = item.A });
            }

            //Default It will Select Current Month  
            return monthsList;
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult TableReport(string reportData)
        {
            try
            {
                var reportConfig = JsonConvert.DeserializeObject<ReportConfig>(reportData);
                var clientName = ClientRepository.Clients.First(_ => _.ID == reportConfig.ClientID).Description;
                var d = new DateTime(reportConfig.SelectedYear, reportConfig.SelectedMonth, 1);

                var tableReportViewModel = new TableReport()
                {
                    ReportName = $"{clientName} SLA Report",
                    Period = $"Period: {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(reportConfig.SelectedMonth)},{reportConfig.SelectedYear}", //string.Format("Period: {0} ", d.ToShortDateString()),
                    
                    ReportInfo = SlaValuesRepository.GetSLAReport(reportConfig.ClientID, reportConfig.ProfileID,d)
                                .Select(x => new ReportInfo()
                                {
                                    ClientName=clientRepository.Clients.First(_ => _.ID== (profileRepository.Profiles.First(p => p.ID == x.ProfileID).ClientID)).Description,
                                    Profile = profileRepository.Profiles.First(_ => _.ID == x.ProfileID).Name,
                                    ProfileID=x.ProfileID,
                                    ProfileDescription=profileRepository.Profiles.First(_ => _.ID==x.ProfileID).Description,
                                    ProfilePrefix= profileRepository.Profiles.First(_ => _.ID == x.ProfileID).SLAIDPrefix,
                                    Date = x.ReportingDate.ToString(),
                                    SlaName=x.SlaName,
                                    SlaDescription=x.SlaDescription,
                                    Target=x.SlaTarget,
                                    TargetAchieved= x.QuantityProcessed.HasValue? Math.Round(((double)((double)(x.QuantityProcessed.Value-x.QuantityOutsideofSLA.Value)/x.QuantityProcessed)*100),2).ToString():"100",
                                    SLAIndicator= x.QuantityProcessed.HasValue ? Math.Round(((double)((double)(x.QuantityProcessed.Value - x.QuantityOutsideofSLA.Value) / x.QuantityProcessed) * 100), 2) >= x.SlaTarget?"PASS":"FAIL" : "PASS"
                                })
                                
                                .ToList()
                };

                return PartialView("_TableReportView", tableReportViewModel);
            }
            catch (InvalidCastException ex)
            {
                ModelState.AddModelError("TableReport", ex.Message);
                return PartialView("_TableReportView", new TableReport());
            }
        }
        [HttpPost]
        [OperatorAuthorisation]
        [ValidateAntiForgeryToken]
        public ActionResult ExportReport(string reportData)
        {
            try
            {
                var reportConfig = JsonConvert.DeserializeObject<TableReport>(reportData);

                var exportFileName = $"{reportConfig.ReportName}-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                var filePath = systemConfigRepository.Settings.Where(x => x.Name == "ReportPath").FirstOrDefault().Value;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var fullPath = $"{filePath}\\{exportFileName}";// string.Format("{0}\\{1}", filePath, exportFileName);
                ExportScannedData.ExportReport(fullPath, reportConfig);

                return Json(new JsonResultReturn() { Success = true, SuccessMessage = "File \"" + exportFileName + "\" was successfully exported to " + filePath });
            }
            catch (InvalidCastException ex)
            {
                return Json(new JsonResultReturn() { Success = false, ErrorMessage = ex.Message });
            }
        }

       
    }

}