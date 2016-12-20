using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ActivityCategory : BaseModel
    {
        /// <summary>
        /// Gets or set ActivityCategoryName
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set ActivityCategoryDescription
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityCategory()
        {         
        }

        /// <summary>
        /// Transpose ActivityCategory value object to model object
        /// </summary>
        /// <param name="activityCategory">ActivityCategory value object</param>
        public ActivityCategory(ActivityCategoryVO activityCategory)
        {
            ID = activityCategory.ActivityCategoryId;
            Name = activityCategory.ActivityCategoryName;
            Description = activityCategory.ActivityCategoryDescription;
        }
    }
}