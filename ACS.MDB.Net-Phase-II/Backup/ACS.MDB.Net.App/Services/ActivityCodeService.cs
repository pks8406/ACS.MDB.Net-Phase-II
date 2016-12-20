using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
namespace ACS.MDB.Net.App.Services
{
    public class ActivityCodeService : BaseService
    {
        ActivityCodeDAL activityCodeDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityCodeService()
        {
            activityCodeDAL = new ActivityCodeDAL();
        }

        /// <summary>
        /// Get the ActivityCode List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>ActivityCode List</returns>
        public List<ActivityCodeVO> GetActivityCodeList(int companyId)
        {
            return activityCodeDAL.GetActivityCodeList(companyId);
        }

        /// <summary>
        /// GetAccount based on ActivityCode Id
        /// </summary>
        /// <param name="activityCodeId">ActivityCode Id</param>
        /// <returns></returns>
        public ActivityCodeVO GetAccountByActivityCode(int activityCodeId)
        {
            return activityCodeDAL.GetAccountByActivityCode(activityCodeId);
        }
    }
}