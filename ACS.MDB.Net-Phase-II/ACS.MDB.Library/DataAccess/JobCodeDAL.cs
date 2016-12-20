using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class JobCodeDAL : BaseDAL
    {
        /// <summary>
        /// Gets the JobCode List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="CustomerId">customer Id</param>
        /// <returns>Value object of jobcode list</returns>
        public List<JobCodeVO> GetJobCodeList(int? companyId, int CustomerId)
        {
            List<JobCodeVO> jobCodeVOList = new List<JobCodeVO>();
            //ARBS-134-to display jobcode ID wise instead of Name
            List<OAJobCode> jobCodeList = mdbDataContext.OAJobCodes.Where(c => c.CompanyID == companyId && c.CustomerID == CustomerId && c.IsDeleted == false).OrderBy(c => c.JobCodeID).ToList();

            foreach (OAJobCode item in jobCodeList)
            {
                jobCodeVOList.Add(new JobCodeVO(item));
            }

            return jobCodeVOList;
        }
    }
}