using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Services
{
    public class ReleaseNoteService:BaseService
    {
        ReleaseNoteDAL releaseNoteDAL = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public ReleaseNoteService()
        {
            releaseNoteDAL = new ReleaseNoteDAL();
        }
        /// <summary>
        /// Get List of String
        /// </summary>
        /// <returns>List of String</returns>
        public List<String> GetReleaseNote(string filePath)
        {
            List<String> releaseNote;
            releaseNote=releaseNoteDAL.GetReleaseNote(filePath);

             if (releaseNote.Count != 0)
             {
                 return releaseNote;                  
             }
             else
             {
                 throw new ApplicationException(Constants.DATA_NOT_FOUND);
             }       
        }

    }
}