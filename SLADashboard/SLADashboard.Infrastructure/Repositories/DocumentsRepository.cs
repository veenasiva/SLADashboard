using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SLADashboard.Core.Interfaces;
using SLADashboard.Core;

namespace SLADashboard.Infrastructure
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private SLADashboardDBContext context = new SLADashboardDBContext();

        public IEnumerable<Document> Documents
        {
            get { return Context.Documents.ToList(); }
        }

        public SLADashboardDBContext Context { get => context; set => context = value; }

        public void Insert(int profileID, string barCode, string senderDetails, string addedUserName)
        {
            var profile = Context.Profiles.FirstOrDefault(_ => _.ID == profileID);
            if (profile != null)
            {
                var document = new Document()
                {
                    Barcode = barCode,
                    Profile = profile,
                    ProfileID = profile.ID,
                    SenderDetails = senderDetails,
                    AddedUser = addedUserName,
                    DeletedUser = string.Empty,
                    IsDeleted = false,
                    TimeStamp = DateTime.Now
                };
                Context.Documents.Add(document);
                Context.SaveChanges();
            } 
        }

        public void Delete(int ID, string deletedUserName)
        {
            var recordToDelete = context.Documents.Find(ID);
            if (recordToDelete != null)
            {
                recordToDelete.IsDeleted = true;
                recordToDelete.DeletedUser = deletedUserName;
                context.SaveChanges();
            }
        }
    }
}