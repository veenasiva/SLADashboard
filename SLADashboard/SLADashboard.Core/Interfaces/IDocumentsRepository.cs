using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLADashboard.Core.Interfaces
{
    public interface IDocumentsRepository
    {
        IEnumerable<Document> Documents { get; }
        void Insert(int profileID, string barCode, string SenderDetails, string addedUserName);
        void Delete(int ID, string deletedUserName);
    }
}