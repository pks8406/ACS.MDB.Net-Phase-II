using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class EndUser : BaseModel
    {
        /// <summary>
        /// Gets or set Company Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set InvoiceCustomerId
        /// </summary>
        [Display(Name = "Invoice Customer")]
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// Gets or set EndUserText Id
        /// </summary>
        public string EndUserId { get; set; }

        /// <summary>
        /// Gets or set EndUser Name
        /// </summary>
        [Required(ErrorMessage = "Please enter End User name")]
        [Display(Name = " End User Name")]
        [RegularExpression("^([a-zA-Z0-9 &'-.]+)$", ErrorMessage = "Please enter valid End User name")]
        [StringLength(50, ErrorMessage = "End User name must be maximum length of 50")]
        public string Name { get; set; }

        public List<InvoiceCustomer> InvoiceCustomerList { get; set; }

        public string EndUserNameWithCode { get; set; }

        /// <summary>
        /// gets or sets new/edit end user
        /// </summary>
        public bool IsNewEndUser { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndUser()
        {
            InvoiceCustomerList = new List<InvoiceCustomer>();
        }

        /// <summary>
        /// Transpose enduser value object to model object
        /// </summary>
        /// <param name="endUserVO">Value object of end user</param>
        public EndUser(EndUserVO endUserVO)
        {
            ID = endUserVO.ID;
            CompanyId = endUserVO.CompanyId;
            InvoiceCustomerId = endUserVO.InvoiceCustomerId;
            EndUserId = endUserVO.EndUserId;
            Name = endUserVO.Name;
            EndUserNameWithCode = endUserVO.EndUserNameWithCustomerCode;
            IsNewEndUser = endUserVO.IsNewEndUser;
        }

        // <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public EndUserVO Transpose(int? userId)
        {
            EndUserVO endUserVO = new EndUserVO();

            endUserVO.ID = this.ID;
            endUserVO.EndUserId = this.EndUserId;
            endUserVO.CompanyId = this.CompanyId;
            endUserVO.InvoiceCustomerId = this.InvoiceCustomerId;
            endUserVO.Name = this.Name;
            endUserVO.CreatedByUserId = userId;
            endUserVO.LastUpdatedByUserId = userId;
            return endUserVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (Name != null && Name.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { "<input type='checkbox' name='check5' value='" + ID + "'>",
                ID, Name};
            return result;
        }

    }
}