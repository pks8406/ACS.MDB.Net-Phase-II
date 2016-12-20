using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ActivityCategoryVO
    {
        public int ActivityCategoryId { get; set; }
        public string ActivityCategoryName { get; set; }
        public string ActivityCategoryDescription { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityCategoryVO()
        { 
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="activityCategory">LINQ object</param>
        public ActivityCategoryVO(ActivityRestriction activityCategory)
        {
            ActivityCategoryId = activityCategory.ID;
            ActivityCategoryName = activityCategory.ActivityCategory;
            ActivityCategoryDescription = activityCategory.Description;
        }


        ///// <summary>
        ///// Transpose model object to ActivityCategory value object
        ///// </summary>
        ///// <param name="activityCategory">model object</param>
        //public ActivityCategoryVO(ActivityCategory activityCategory)
        //{
        //    ActivityCategoryId = activityCategory.ID;
        //    ActivityCategoryName = activityCategory.Name;
        //    ActivityCategoryDescription = activityCategory.Description;
        //}
    }
}