
using System;
namespace ACS.MDB.Library.ValueObjects
{
    /// <summary>
    /// This is base class for all VO objects
    /// </summary>
    public class BaseVO
    {
        /// <summary>
        /// Gets or Sets created by user id
        /// </summary>
        public int? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or Sets user id
        /// </summary>
        public int? LastUpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or Sets creation date
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets updation date
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }
    }
}