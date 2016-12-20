using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class ActivityCategoryService : BaseService
    {
        ActivityCategoryDAL activityCategoryDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityCategoryService()
        {
            activityCategoryDAL = new ActivityCategoryDAL();
        }

        /// <summary>
        /// Get List of ActivityCategory
        /// </summary>
        /// <returns>List of ActivityCategory</returns>
        public List<ActivityCategoryVO> GetActivityCategoryList()
        {
            return activityCategoryDAL.GetActivityCategoryList();
        }
    }
}