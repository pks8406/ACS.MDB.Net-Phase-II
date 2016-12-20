using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class CustomerComment : BaseModel
    {
        /// <summary>
        /// Gets or set customer Id
        /// </summary>
        [Required(ErrorMessage = "Please select Invoice customer")]
        [Range(1, double.MaxValue, ErrorMessage = "Please select Invoice customer")]
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// Gets or set old customer Id
        /// </summary>
        public string OldCustomerId { get; set; }

        /// <summary>
        /// Gets or set customer name
        /// </summary>
        [Display(Name = "Invoice Customer")]
        public string  InvoiceCustomerName { get; set; }

        /// <summary>
        /// Gets or set InvoiceCustomer
        /// </summary>
        [Display(Name = "Invoice Customer")]
        public string InvoiceCustomer { get; set; }

        /// <summary>
        /// Gets or set company Id
        /// </summary>
        [Required(ErrorMessage = "Please select Company name")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set company name
        /// </summary>
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please enter Company name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set company
        /// </summary>
        public string  Company { get; set; }

        /// <summary>
        /// Gets or set customer comment
        /// </summary>
        //[Required(ErrorMessage = "Please enter Comment")]
        [Display(Name = " Comment")]
        //[RegularExpression(@"^([a-zA-Z0-9 ""&'-.=#/%$£!\r\n]+)$", ErrorMessage = "Please enter valid customer comment")]
        [StringLength(220, ErrorMessage = "Customer comment must be maximum length of 220")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or set customer comment is grouped or not
        /// </summary>
        [Display(Name = "Group")]
        public bool Group { get; set; }

        public List<InvoiceCustomer> InvoiceCustomerList { get; set; }
        

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerComment()
        {
            InvoiceCustomerList = new List<InvoiceCustomer>();            
        }

        /// <summary>
        /// Transpose enduser value object to model object
        /// </summary>
        /// <param name="customerCommentVO">Value object of customerComment</param>
        public CustomerComment(CustomerCommentVO customerCommentVO)
        {
            ID = customerCommentVO.CustomerCommentId;
            InvoiceCustomerId = customerCommentVO.InvoiceCustomerId;
            OldCustomerId = customerCommentVO.OldCustomerId;
            InvoiceCustomerName = customerCommentVO.InvoiceCustomerName;
            InvoiceCustomer = InvoiceCustomerName + '-' + OldCustomerId;
            CompanyId = customerCommentVO.CompanyId;
            CompanyName = customerCommentVO.CompanyName;
            Company = customerCommentVO.CompanyName + '-' + customerCommentVO.CompanyId;
            //companyList = customerCommentVO.companyVOList
            Comment = customerCommentVO.CustomerComment;
            Group = customerCommentVO.Group;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public CustomerCommentVO Transpose(int? userId)
        {
            CustomerCommentVO customerCommentVO = new CustomerCommentVO();

            customerCommentVO.CustomerCommentId = this.ID;
            customerCommentVO.InvoiceCustomerId = this.InvoiceCustomerId;
            customerCommentVO.OldCustomerId = this.OldCustomerId;
            customerCommentVO.InvoiceCustomerName = this.InvoiceCustomerName;
            customerCommentVO.InvoiceCustomer = this.InvoiceCustomerName + '-' + this.OldCustomerId;
            customerCommentVO.CompanyId = this.CompanyId;
            customerCommentVO.CompanyName = this.CompanyName;
            customerCommentVO.Company = this.CompanyName + '-' + this.CompanyId;
            customerCommentVO.CustomerComment = this.Comment;
            customerCommentVO.Group = this.Group;
            customerCommentVO.CreatedByUserId = userId;
            customerCommentVO.LastUpdatedByUserId = userId;

            return customerCommentVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (InvoiceCustomer != null && InvoiceCustomer.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                 (Company != null && Company.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                 (Comment != null && Comment.StartsWith(str, StringComparison.CurrentCultureIgnoreCase))||
                 (InvoiceCustomerId != 0 && InvoiceCustomerId.ToString().StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
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
                ID, 
                InvoiceCustomer,
                Company,
                Comment,
                Group ? "True" : "False",};
            return result;
        }

    }
}