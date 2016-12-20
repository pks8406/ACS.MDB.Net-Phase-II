using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ActivityCodeDAL : BaseDAL
    {
        /// <summary>
        /// Get the ActivityCode List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>Value object of activitycode List</returns>
        public List<ActivityCodeVO> GetActivityCodeList(int companyId)
        {
            List<ActivityCodeVO> activityCodeVOList = new List<ActivityCodeVO>();
            List<OAActivityCode> activityCodeList = mdbDataContext.OAActivityCodes.
                Where(c => c.CompanyID == companyId && c.IsDeleted == false).
                OrderBy(c => c.ActivityName).ToList();

            foreach (OAActivityCode item in activityCodeList)
            {
                activityCodeVOList.Add(new ActivityCodeVO(item));
            }

            return activityCodeVOList;
        }

        /// <summary>
        /// Get account code informaiton based on activity code id
        /// </summary>
        /// <param name="activityCodeId">activity code Id</param>
        /// <returns>The ActivityCodeVO object</returns>
        public ActivityCodeVO GetAccountByActivityCode(int activityCodeId)
        {
            ActivityCodeVO activityCodeVO = null;
            OAActivityCode activityCode = mdbDataContext.OAActivityCodes.Where(x => x.ID == activityCodeId).SingleOrDefault();

            if (activityCode != null)
            {
                activityCodeVO = new ActivityCodeVO();
                activityCodeVO.AccountId = activityCode.AccountCodeID;
                activityCodeVO.CompanyId = activityCode.CompanyID;
                activityCodeVO.AccountCode = activityCode.OAAccountCode.AccountName;
                activityCodeVO.OAAccountId = activityCode.OAAccountCode.AccountID;
            }

            return activityCodeVO;
        }
    }
}