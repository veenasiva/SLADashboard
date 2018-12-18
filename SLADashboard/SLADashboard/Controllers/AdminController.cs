using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;
using SLADashboard.Models;
using Newtonsoft.Json;
using SLADashboard.Infrastructure;
using System.Security.Principal;
using SLADashboard.Filters;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace SLADashboard.Controllers
{
    
    public class AdminController : Controller
    {
        private IClientsRepository clientRepository;
        private IProfilesRepository profileRepository;
        private ISLARepository slaRepository;
        private ISLAValuesRepository slavaluesRepository;
        private ISystemConfigRepository systemConfigRepository;

        public IClientsRepository ClientRepository { get => clientRepository; set => clientRepository = value; }
        public IProfilesRepository ProfileRepository { get => profileRepository; set => profileRepository = value; }
        public ISLARepository SlaRepository { get => slaRepository; set => slaRepository = value; }
        public ISLAValuesRepository SlaValuesRepository { get => slavaluesRepository; set => slavaluesRepository = value; }
        public ISystemConfigRepository SystemConfigRepository { get => systemConfigRepository; set => systemConfigRepository = value; }

        public AdminController(IClientsRepository clientRepository, IProfilesRepository profileRepository,
            ISLARepository slaRepository,ISLAValuesRepository slavaluesRepository,
            ISystemConfigRepository systemConfigRepository)
        {
            ClientRepository = clientRepository;
            ProfileRepository = profileRepository;
            SlaRepository = slaRepository;
            SlaValuesRepository = slavaluesRepository;
            SystemConfigRepository = systemConfigRepository;

        }

        public ActionResult checkProgressSteps()
        {
            return PartialView("_ProgressRibbonPartial");
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeMode(string username, Mode mode)
        {
            var systemConfig = SystemConfigRepository.SystemConfig.FirstOrDefault(_ => _.UserName == username);

            if (systemConfig == null)
            {
                SystemConfigRepository.Insert(username, mode);
            }
            else
            {
                SystemConfigRepository.Update(systemConfig.ID, username, mode);
            }
            Session["mode"] = mode;
            return RedirectToAction("SelectConfiguredProfile", "Operator");
        }

        

        [AdminAuthorisation]
        [SessionExpireAuthorise]
        public ActionResult ConfigureClient()
        {
            SetMode();
            return View(new AddClientViewModel() { Clients = ClientRepository.Clients.ToList(), NewClientName = string.Empty });
        }

        [AdminAuthorisation]
        [SessionExpireAuthorise]
        public ActionResult ConfigureProfile(int clientId)
        {
            var client = ClientRepository.Clients.ToList().Where(_ => _.ID == clientId).FirstOrDefault();
            SetMode();
            return View(model: new AddProfileViewModel() { Client = new Client() { ID = client.ID, Description = client.Description }, Profiles = client.Profiles.ToList() });
        }

       
        public ActionResult AddClient(string clientData)
        {
            var addClientView = JsonConvert.DeserializeObject<AddClientViewModel>(clientData);
            var error = string.Empty;
            Regex rg = new Regex(@"^[a-zA-Z0-9\s-]*$");

            if (addClientView.NewClientName.Length == 0 || !rg.IsMatch(addClientView.NewClientName))
            {
                error = "Client Name invalid or blank";
            }
            else if (ClientRepository.Clients.Any(_ => _.Description.ToUpper() == addClientView.NewClientName.ToUpper()))
            {
                error = $"{addClientView.NewClientName} already exists";
            }
            else
            {
                ClientRepository.Insert(addClientView.NewClientName);
                var client = ClientRepository.Clients.FirstOrDefault(_ => _.Description == addClientView.NewClientName);
                return Json(new JsonResultReturn() { Success = true, SuccessMessage = JsonConvert.SerializeObject(client) });
            }
              
            ModelState.AddModelError("NewClientName", error);
            return Json(new JsonResultReturn()
                        {
                            Success = false,
                            ErrorMessage = ModelState.Values.Where(i => i.Errors.Count > 0).Select(_ => _.Errors[0].ErrorMessage).FirstOrDefault()
                        });       
        }


        public ActionResult AddProfile(string profileData)
        {
            var addProfileView = JsonConvert.DeserializeObject<AddProfileViewModel>(profileData);
            var client = ClientRepository.Clients.ToList().Where(_ => _.ID == addProfileView.Client.ID).FirstOrDefault();
            var error = string.Empty;
            Regex rgname = new Regex(@"^[a-zA-Z0-9\s-(),.]*$");
            Regex rgdesc = new Regex(@"^[^<>!@#%/?*]+$");
            if (addProfileView.NewProfileName.Length == 0 || !rgname.IsMatch(addProfileView.NewProfileName))
            {
                error = "Profile Name invalid or blank";
                ModelState.AddModelError("NewProfileName", error);
            }
            else if (addProfileView.NewProfileDescription.Length == 0 || !rgdesc.IsMatch(addProfileView.NewProfileDescription))
            {
                error = "Profile Description invalid or blank";
                ModelState.AddModelError("NewProfileDescription", error);
            }
            else if (addProfileView.NewProfileIDPrefix.Length == 0 || !rgname.IsMatch(addProfileView.NewProfileIDPrefix))
            {
                error = "Profile IDPrefix invalid or blank";
                ModelState.AddModelError("NewProfileIDPrefix", error);
            }
            else if (client.Profiles.Any(_ => _.Name.ToUpper() == addProfileView.NewProfileName.ToUpper()))
            {
                error = $"{addProfileView.NewProfileName} already exist for {client.Description}";
                ModelState.AddModelError("NewProfileName", error);
            }
            else
            {
                ProfileRepository.Insert(addProfileView.Client.ID, addProfileView.NewProfileName, addProfileView.NewProfileDescription, addProfileView.NewProfileIDPrefix);
                var profile = ProfileRepository.Profiles.FirstOrDefault(_ => _.Name == addProfileView.NewProfileName && _.ClientID == addProfileView.Client.ID);
                return Json(new JsonResultReturn() { Success = true, SuccessMessage = JsonConvert.SerializeObject(new Profile() { ID = profile.ID, Name = profile.Name, Description = profile.Description, SLAIDPrefix = profile.SLAIDPrefix })});
            }
            string key=string.Empty;
            foreach (var modelStateKey in ViewData.ModelState.Keys)
            {
                key = modelStateKey;
 
            }
            
            return Json(new JsonResultReturn()
            {
                Success = false,
                ErrorMessage = ModelState.Values.Where(i => i.Errors.Count > 0).Select(_ => _.Errors[0].ErrorMessage).FirstOrDefault(),
                ErrorKey=key
            });
        }

        private void SetMode()
        {
            var userName = (((WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name);
            var systemConfig = SystemConfigRepository.SystemConfig.Where(_ => _.UserName == userName).FirstOrDefault();
            if (systemConfig == null)
            {
                SystemConfigRepository.Insert(userName, Mode.Operator);
                systemConfig = SystemConfigRepository.SystemConfig.Where(_ => _.UserName == userName).First();
            }

            Session["mode"] = systemConfig.Mode;
        }

        [AdminAuthorisation]
        [SessionExpireAuthorise]
        public ActionResult ConfigureSLA(int clientId, int profileId)
        {
            IEnumerable<SLA> sla = new List<SLA>();
            var client = ClientRepository.Clients.FirstOrDefault(_ => _.ID == clientId);
            var profile = ProfileRepository.Profiles.FirstOrDefault(_ => _.ID == profileId);
            sla = SlaRepository.SLAConfigurations.Where(_ => _.ProfileID == profileId && !(_.IsDeleted.HasValue && _.IsDeleted == true)).ToList();
            //Remove circular reference
            sla = sla.Select(_ => new SLA() { ID = _.ID, Name = _.Name, Description = _.Description, Target = _.Target });
            SetMode();

            return View(new SLAListViewModel()
            {
                Client = new Client() { ID = client.ID, Description = client.Description },
                Profile = new Profile() { ID = profile.ID, Description = profile.Description },
                Sla =sla
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SLADefinition(int profileID)
        {
            try
            {
                var profile = ProfileRepository.Profiles.FirstOrDefault(_ => _.ID == profileID);
                var client = ClientRepository.Clients.FirstOrDefault(_ => _.ID == profile.ClientID);
                var slaDefinitions = new SLADefinitionListViewModel()
                {
                    ClientID=client.ID,
                    ProfileID=profile.ID,
                    SLAList = SlaRepository.SLAConfigurations.Where(_ => _.ProfileID == profileID && !(_.IsDeleted.HasValue && _.IsDeleted == true))
                    .Select(x => new SLAViewModel()
                    {
                        ID=x.ID,
                        ProfileID=x.ProfileID,
                        Name=x.Name,
                        Description=x.Description,
                        Target=x.Target
                    })
                   .ToList()
                };
                return PartialView("_SLADefinitionsView", slaDefinitions);
            }
            catch (InvalidCastException ex)
            {
                ModelState.AddModelError("SLADefinition", ex.Message);
                return PartialView("_SLADefinitionsView", new SLADefinitionListViewModel());
            }
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditSLAPartialView(string id)
        {
            ModelState.Clear();
            int selectedSlaId = Convert.ToInt32(id);
            SLA sla = slaRepository.GetById(selectedSlaId);
            var profile = profileRepository.Profiles.Where(p => p.ID == sla.ProfileID).FirstOrDefault();
            var client = ClientRepository.Clients.Where(c => c.ID == profile.ClientID).FirstOrDefault();
            return PartialView("_EditSLAPartialView", new SLAViewModel()
            {
                Profile=profile,
                Client=client,
                ID=sla.ID,
                Name=sla.Name,
                Description=sla.Description,
                Target=sla.Target

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSLA(SLA sla)
        {
            int index = 0;
            String result = String.Empty;
            //filter all profiles for current profile
            //sla names not to be duplicated within profile
            if (SlaRepository.SLAConfigurations.Where(_ => _.ProfileID==sla.Profile.ID && _.ID!=sla.ID).Any(_ => _.Name.ToUpper() == sla.Name.ToUpper()))
            {
                ModelState.AddModelError("", $"{sla.Name} already exists");
                return Json(new JsonResultReturn()
                {
                    Success = false,
                    ErrorMessage = ModelState.Values.Where(i => i.Errors.Count > 0).Select(_ => _.Errors[0].ErrorMessage).FirstOrDefault()
                });
            }
            else
            {
                index = SlaRepository.Update(sla);
                if (index > 0)
                {
                    result = "1";
                }
                else
                    result = "0";
            }
            return Json(new JsonResultReturn() { Success = true, SuccessMessage = "Successfully Updated" +result });

        }


        public JsonResult DeleteSLA(string id)
        {
            int index = 0;
            String result = String.Empty;
            int selectedSlaId = Convert.ToInt32(id);

            //check if any values for this sla has been added
            if (SlaValuesRepository.GetBySLAId(selectedSlaId).Any())
            {
                ModelState.AddModelError("", $"{SlaRepository.GetById(selectedSlaId).Name} has report values defined,Delete is unsuccessful");
                return Json(new JsonResultReturn()
                {
                    Success = false,
                    ErrorMessage = ModelState.Values.Where(i => i.Errors.Count > 0).Select(_ => _.Errors[0].ErrorMessage).FirstOrDefault()
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var userName = (((WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name);
                index = SlaRepository.Delete(selectedSlaId, userName);
                if (index > 0)
                {
                    result = "1";
                }
                else
                    result = "0";
                return Json(new JsonResultReturn() { Success = true, SuccessMessage = "Successfully Deleted" +result }, JsonRequestBehavior.AllowGet);
            }
        }
     
        public ActionResult AddSLA(int clientId, int profileId)
        {
            var client = ClientRepository.Clients.FirstOrDefault(_ => _.ID == clientId);
            var profile = ProfileRepository.Profiles.FirstOrDefault(_ => _.ID == profileId);
            return PartialView("_AddSLA",new SLAViewModel()
            {
                Client = new Client() { ID = client.ID, Description = client.Description },
                Profile = new Profile() { ID = profile.ID, Description = profile.Description }
            });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertSLA(int profileId,string name,string description,double target)
        {
            SLA sla = new SLA(){ ProfileID=profileId,Name=name,Description=description,Target=target};
            //Validate
            if (String.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("", "Name cant be blank");
            }
            else if (String.IsNullOrEmpty(description))
            {
                ModelState.AddModelError("", "Description cant be blank");
            }
            else if (String.IsNullOrEmpty(target.ToString()))
            {
                ModelState.AddModelError("", "Target cant be blank");
            }
            //sla not to be duplicated inside profile
            else if (SlaRepository.SLAConfigurations.Any(_ => _.Name.ToUpper() == name.ToUpper() && _.ProfileID == sla.ProfileID))
            {
                ModelState.AddModelError("",$"{name} already exists");
            }
            else
            {
                SlaRepository.Insert(sla);

                return Json(new JsonResultReturn() { Success = true});

            }
            return Json(new JsonResultReturn()
            {
                Success = false,
                ErrorMessage = ModelState.Values.Where(i => i.Errors.Count > 0).Select(_ => _.Errors[0].ErrorMessage).FirstOrDefault()
            });
        }
       

    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
        }
    }
}