using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ActivityCategoryDAL : BaseDAL
    {
        /// <summary>
        /// Get List of ActivityCategory
        /// </summary>
        /// <returns>List of ActivityCategoryVO</returns>
        public List<ActivityCategoryVO> GetActivityCategoryList()
        {
            List<ActivityCategoryVO> activityCategoryVOList = new List<ActivityCategoryVO>();

            List<ActivityRestriction> activityCategoryList = mdbDataContext.ActivityRestrictions.Where(c => c.StartCode != null && c.EndCode != null).OrderBy(c => c.ActivityCategory).ToList();

            foreach (ActivityRestriction item in activityCategoryList)
            {
                activityCategoryVOList.Add(new ActivityCategoryVO(item));
            }

            return activityCategoryVOList;
        }
    }
}