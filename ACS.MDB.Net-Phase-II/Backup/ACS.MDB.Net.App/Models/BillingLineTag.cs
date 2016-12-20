using System;
using ACS.MDB.Library.ValueObjects;


namespace ACS.MDB.Net.App.Models
{
    public class BillingLineTag : BaseModel
    {        
        /// <summary>
        /// Gets or sets tag name
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets Tag Description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="BillingLineTagVO"></param>
        public BillingLineTag(BillingLineTagVO BillingLineTagVO)
        {            
            Tag = BillingLineTagVO.Tag;
            Description = BillingLineTagVO.Description;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (Tag != null && Tag.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (Description != null && Description.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] {ID,Tag, Description };
            return result;
        }
    }

}