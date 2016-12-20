using System.Collections.Generic;

namespace ACS.MDB.Net.App.Models
{
    public class BaseModel
    {
        /// <summary>
        /// Gets or Sets the item id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or Sets the company list
        /// </summary>
        public List<Company> OAcompanyList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseModel()
        {
            
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// Needs to be overridden in the derived classes
        /// so implementation is class specific
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public virtual bool Contains(string str)
        {
            return true;
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// This needs to be overridden in the derived
        /// classes. Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        /// <returns></returns>
        public virtual object[] GetModelValue()
        {
            return new object[] { "one", "two", "three" };
        }
    }
}