using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Models
{
    public class ApproveMaintenance : BaseModel
    {
        [Required(ErrorMessage = "Please select Company")]
        public int? CompanyId { get; set; }

       // [Required(ErrorMessage = "Please select Company")]
        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        //[Required(ErrorMessage = "Please select invoice customer")]
        public int? InvoiceCustomerId { get; set; }

        [Display(Name = "Invoice Customer")]
        public string InvoiceCustomer { get; set; }

        //[Required(ErrorMessage = "Please select Division")]
        [Display(Name = "Division")]
        public int? DivisionId { get; set; }

        [Display(Name = "Division")]
        public string DivisionName { get; set; }

        //[Required(ErrorMessage = "Please select Milestone Status")]
        [Display(Name = "Milestone Status")]
        public int? MilestoneStatusId { get; set; }

        [Display(Name = "Milestone Status")]
        public string MilestoneStatusName { get; set; }

        [Display(Name = "Milestone Status")]
        public string MilestoneDescription { get; set; }


        /// <summary>
        /// To date to filter milestone
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Date Should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select To Date")]
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        /// <summary>
        /// Gets and set changing uplift date
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Date Should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select From Date")]
        [Display(Name = "From Date")]
        public string FromDate { get; set; }

        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Renewal Start Date")]
        public DateTime RenewalStartDate { get; set; }

        [Display(Name = "Renewal End Date")]
        public DateTime RenewalEndDate { get; set; }

        [Display(Name = "Approval Required?")]
        public bool? ApprovalRequired { get; set; }

        [Display(Name = "Approved?")]
        public bool IsApproved { get; set; }

        [Display(Name = "Approved?")]
        public string ApprovedStatus { get; set; }

        [Display(Name = "Milestone")]
        public string ActivityName { get; set; }

        public decimal? Percentage { get; set; }

        public decimal Amount { get; set; }

        public String BillingLines { get; set; }

        /// <summary>
        /// List of division
        /// </summary>
        public List<Division> DivisionList { get; set; }

        /// <summary>
        /// List of Invoice customer
        /// </summary>
        public List<InvoiceCustomer> InvoiceCustomerList { get; set; }

        /// <summary>
        /// List of milestone status
        /// </summary>
        public List<MilestoneStatus> MilestoneStatusList { get; set; }

        /// <summary>
        /// Gets or set Milestone billing lines
        /// </summary>
        public List<MilestoneBillingLine> MilestoneBillingLines { get; set; }

        public Milestone milestone { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApproveMaintenance()
        {
            DivisionList = new List<Division>();
            InvoiceCustomerList = new List<InvoiceCustomer>();
            MilestoneStatusList = new List<MilestoneStatus>();
            milestone = new Milestone();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="milestone">The milestone model object</param>
        public ApproveMaintenance(Milestone milestone)
        {
            ID = milestone.ID;
            CompanyId = milestone.CompanyID;
            CompanyName = milestone.CompanyName;
            InvoiceCustomer = milestone.ContractNumber;
            InvoiceDate = milestone.InvoiceDate;
            ApprovalRequired = milestone.IsApprovalRequired;
            MilestoneStatusId = milestone.MilestoneStatusID;
            MilestoneStatusName = milestone.MilestoneStatus;
            MilestoneDescription = milestone.MilestoneStatusDescription;
            ActivityName = milestone.ActivityName;
            Percentage = milestone.Percentage;
            Amount = milestone.Amount;
            ApprovedStatus = milestone.ApprovedStatus;
            IsApproved = milestone.IsApproved;
        }

        /// <summary>
        /// Convert milestone value object to Approve maintenance model
        /// </summary>
        /// <param name="milestoneVO">The milestoneVO object</param>
        public ApproveMaintenance(MilestoneVO milestoneVO)
        {
            ID = milestoneVO.ID;
            CompanyId = milestoneVO.CompanyID;
            CompanyName = milestoneVO.CompanyName;
            DivisionName = milestoneVO.DivisionName;
            InvoiceCustomer = milestoneVO.InvoiceCustomerName;
            ContractNumber = milestoneVO.ContractNumber;
            InvoiceDate = milestoneVO.InvoiceDate;
            ApprovalRequired = milestoneVO.ApprovedByCustomer;
            ApprovalRequired = milestoneVO.IsApprovalRequired;
            MilestoneStatusId = milestoneVO.MilestoneStatusID;
            MilestoneStatusName = milestoneVO.MilestoneStatusName;
            ActivityName = milestoneVO.ActivityName;
            Percentage = milestoneVO.Percentage;
            Amount = milestoneVO.Amount;
            ApprovedStatus = milestoneVO.ApprovedStatus;
            IsApproved = milestoneVO.IsApproved;
            MilestoneDescription = milestoneVO.MilestoneStatusDescription;
            BillingLines = milestoneVO.BillingLines;
            //MilestoneBillingLines = milestoneVO.MilestoneBillingLines;

            foreach (var milestoneBillingLineVo in milestoneVO.MilestoneBillingLineVos)
            {
                MilestoneBillingLines.Add(new MilestoneBillingLine(milestoneBillingLineVo));
            }

            milestone = new Milestone();

        }

        /// <summary>
        /// Function called to search the model for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (ContractNumber != null && ContractNumber.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (InvoiceCustomer != null && InvoiceCustomer.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (CompanyName != null && CompanyName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (DivisionName != null && DivisionName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (ActivityName != null && ActivityName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (!string.IsNullOrEmpty(Convert.ToString(InvoiceDate)) && InvoiceDate.ToString(Constants.DATE_FORMAT).Contains(str)) ||
                (!string.IsNullOrEmpty(Convert.ToString(Amount)) && Convert.ToString(Amount).StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained in the model as an array of strings (object). 
        /// Typically used to fill up the datatable grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { "<input type='checkbox' name='check5' value='" + ID + "'>", 
                                            ID, 
                                            DivisionName, 
                                            InvoiceCustomer, 
                                            ContractNumber, 
                                            InvoiceDate.Date.ToString(Constants.DATE_FORMAT), 
                                            String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, Amount), 
                                            IsApproved ? "Approved" : "Unapproved", 
                                            MilestoneDescription
                                            //null
                                            //String.IsNullOrEmpty(BillingLines) ? "No Billing Line Text exist" : BillingLines
            };
            return result;
        }
    }
}