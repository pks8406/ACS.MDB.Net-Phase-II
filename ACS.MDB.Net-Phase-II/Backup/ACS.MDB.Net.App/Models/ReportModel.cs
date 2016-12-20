using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ReportModel : BaseModel
    {
        //public string Title { get; set; }

        /// <summary>
        /// Gets or Set start date
        /// </summary>
        [Required(ErrorMessage = "Please enter Start date")] 
        [DataType(DataType.Date,ErrorMessage="Date Should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date (dd/mm/yyyy)")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets end date
        /// </summary>
        [Required(ErrorMessage = "Please enter End date")] 
        [DataType(DataType.Date, ErrorMessage="Date should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date (dd/mm/yyyy)")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets Revenue Start Date
        /// </summary>
        [Display(Name = "Revenue Start Date (dd/mm/yyyy)")]
        public string RevenueStartDate { get; set; }

        /// <summary>
        /// Gets or Sets Revenue End Date
        /// </summary>
        [Display(Name = "Revenue End Date (dd/mm/yyyy)")]
        public string RevenueEndDate { get; set; }

        /// <summary>
        /// Gets or Sets Company Name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or Sets Company ID
        /// </summary>       
        public int? CompanyID { get; set; }

        /// <summary>
        /// Gets or Sets User ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or Sets Division Name
        /// </summary>
        [Display(Name = "Division Name")]
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or Sets Division ID
        /// </summary>
        public int? DivisionID { get; set; }

        /// <summary>
        /// Gets or Sets DivisionList
        /// </summary>
        public List<DivisionVO> DivisionList { get; set; }

        /// <summary>
        /// Gets or Sets Invoice Customer Name
        /// </summary>
        [Display(Name = "Invoice Customer")]
        public string InvoiceCustomerName { get; set; }

        /// <summary>
        /// Gets of Sets Invoice Customer ID
        /// </summary>
        public int? InvoiceCustomerID { get; set; }

        /// <summary>
        /// Gets or Sets Invoice Customer List
        /// </summary>
        public List<InvoiceCustomerVO> InvoiceCustomerList { get; set; }

        /// <summary>
        /// Gets or Sets report date
        /// </summary>
        [Required(ErrorMessage = "Please enter Report date")]
        [DataType(DataType.Date, ErrorMessage = "Date should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Report Date (dd/mm/yyyy)")]
        public DateTime? ReportDate { get; set; }

        ///// <summary>
        ///// Gets or Sets End User Name
        ///// </summary>
        //[Display(Name = "End User")]
        //public string EndUserName { get; set; }

        ///// <summary>
        ///// Gets of Sets End User ID
        ///// </summary>
        //public string EndUserID { get; set; }

        ///// <summary>
        ///// Gets or Sets End User List
        ///// </summary>
        //public List<EndUserVO> EndUserList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportModel()
        {          
            DivisionList = new List<DivisionVO>();
            InvoiceCustomerList = new List<InvoiceCustomerVO>();
           // EndUserList = new List<EndUserVO>();
            OAcompanyList = new List<Company>();
        }
    }
}