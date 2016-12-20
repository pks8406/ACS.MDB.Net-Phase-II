using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class JobCodeService : BaseService
    {
        JobCodeDAL jobCodeDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public JobCodeService()
        {
            jobCodeDAL = new JobCodeDAL();
        }

        /// <summary>
        /// Gets the JobCode List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="CustomerId">customer Id</param>
        /// <returns>Jobcode List</returns>
        public List<JobCodeVO> GetJobCodeList(int? companyId, int CustomerId)
        { 
            return jobCodeDAL.GetJobCodeList(companyId, CustomerId);
        }
    }
}