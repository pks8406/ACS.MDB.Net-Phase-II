
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ActivityCodeVO
    {
        public int Id { get; set; }

        public string OAActivityCodeId { get; set; }
        
        public string Name { get; set; }

        public string ActivityCodeName { get; set; }

        public string  AccountCode { get; set; }

        public int? AccountId { get; set; }

        public string OAAccountId { get; set; }

        public int? CompanyId { get; set; }
        

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityCodeVO()
        { 
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="activityCode">LINQ object</param>
        public ActivityCodeVO(OAActivityCode activityCode)
        {
           Id = activityCode.ID;
           OAActivityCodeId = activityCode.ActivityID;
           Name = activityCode.ActivityName;
           ActivityCodeName = activityCode.ActivityName + '-' + activityCode.ActivityID;
            //this.AccountCode = activityCode.AccountCode;
           AccountId = activityCode.AccountCodeID;
           CompanyId = activityCode.CompanyID;
        }

        /// <summary>
        /// Transpose model object to ActivityCode value object
        /// </summary>
        /// <param name="activityCode">model object</param>
        //public ActivityCodeVO(ActivityCode activityCode)
        //{
        //   Id = activityCode.ID;
        //   OAActivityCodeId = activityCode.OAActivityCodeId;
        //   Name = activityCode.Name;
        //   ActivityCodeName = activityCode.Name + '-' + activityCode.OAActivityCodeId;
        //   OAAccountId = activityCode.OAAccountId;
        //   AccountId = activityCode.AccountId;
        //   AccountCode = activityCode.AccountCode;
        //   CompanyId = activityCode.CompanyId;
        //}
    }
}