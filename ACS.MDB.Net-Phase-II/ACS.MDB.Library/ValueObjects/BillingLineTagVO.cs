
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class BillingLineTagVO
    {  
        /// <summary>
        /// Gets or sets tag description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets line tag
        /// </summary>
        public string Tag { get; set; }                

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="BillingLineTag">The BillingLineTag object</param>
        public BillingLineTagVO(BillingLineTag BillingLineTag)
        {
            Tag = BillingLineTag.TagName;
            Description = BillingLineTag.Description;                       
        }
    }
}