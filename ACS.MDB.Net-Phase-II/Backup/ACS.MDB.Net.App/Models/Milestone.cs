using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;


namespace ACS.MDB.Net.App.Models
{
    public class Milestone : BaseModel
    {
        public Nullable<int> OldMilestoneID { get; set; }
        public int ContractID { get; set; }
        public int ContractLineID { get; set; }
        public int ContractMaintenanceID { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }

        //Added for Approve Maintenance
        public string CompanyName { get; set; }
        public string InvoiceCustomerName { get; set; }
        public string ContractNumber { get; set; }

        [Display(Name = "Milestone Status")]
        public string MilestoneStatus { get; set; }

        [Display(Name = "Milestone Status")]
        public string MilestoneStatusDescription { get; set; }

        [Display(Name = "MilestoneStatus ID")]
        [Required(ErrorMessage = "Please select Milestone status")]
        public int MilestoneStatusID { get; set; }

        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Please enter Amount")]
        [RegularExpression(@"^[+-]?\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Please enter valid Amount")]        
        public decimal Amount { get; set; }

        [Display(Name = "Renewal End Date")]
        public Nullable<System.DateTime> RenewalEndDate { get; set; }

        [Display(Name = "Renewal Start Date")]
        public Nullable<System.DateTime> RenewalStartDate { get; set; }

        /// <summary>
        /// Milestone status Approved or Unapproved
        /// </summary>
        [Display(Name= "Approved?")]
        public bool IsApproved { get; set; }

        [Display(Name = "Comments")]
        [StringLength(300, ErrorMessage = "Comment must be with a maximum length of 300")]
        public string Comments { get; set; }

        public bool IsDeleted { get; set; }
        public List<MilestoneStatus> MilestoneStatusList { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> IndexRate { get; set; }
        public Nullable<decimal> PreviousValue { get; set; }
        public Nullable<int> PreviousLine { get; set; }
        public Nullable<bool> IsApprovalRequired { get; set; }
        public string ApprovedStatus { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<bool> ApprovedByCustomer { get; set; }
        public string CustomerApprovedStatus { get; set; }
        public string ConditionType { get; set; }
        public string SLT_OUR_REF_2 { get; set; }

        /// <summary>
        /// Gets or set Actual bill date
        /// </summary>
        [Display(Name = "Actual Bill Date")]
        public DateTime? ActualBillDate { get; set; }

        public string ActivityName { get; set; }

        [Display(Name = "Uplift")]
        public Nullable<decimal> UpliftForMilestone { get; set; }

        public int billingTextID1 { get; set; }
        public int billingTextID2 { get; set; }
        public int billingTextID3 { get; set; }
        public int billingTextID4 { get; set; }
        public int billingTextID5 { get; set; }
        public int billingTextID6 { get; set; }
        public int billingTextID7 { get; set; }
        public int billingTextID8 { get; set; }
        public int billingTextID9 { get; set; }
        public int billingTextID10 { get; set; }
        public int billingTextID11 { get; set; }
        public int billingTextID12 { get; set; }
        public int billingTextID13 { get; set; }
        public int billingTextID14 { get; set; }
        public int billingTextID15 { get; set; }

        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText1 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText2 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText3 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText4 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText5 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText6 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText7 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText8 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText9 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText10 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText11 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText12 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText13 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText14 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=?@#/%\r\n\\[\\]]+)$")]
        public string billingText15 { get; set; }




        //public virtual ContractLine ContractLine { get; set; }
        //public virtual ContractMaintenance ContractMaintenance { get; set; }
        //public virtual Contract Contract { get; set; }
        //public virtual MilestoneStatus MilestoneStatu { get; set; }
        public List<MilestoneBillingLine> MilestoneBillingLines { get; set; }

        public List<MilestoneBillingLineVO>  MilestoneBillingLineVos { get; set; }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Milestone()
        {
            MilestoneBillingLines = new List<MilestoneBillingLine>();
            MilestoneStatusList = new List<MilestoneStatus>();
            MilestoneBillingLineVos = new List<MilestoneBillingLineVO>();
        }

        public Milestone(MilestoneVO milestoneVO)
            : this()
        {
            ID = milestoneVO.ID;
            OldMilestoneID = milestoneVO.OldMilestoneID;
            CompanyID = milestoneVO.CompanyID;
            CompanyName = milestoneVO.CompanyName;
            ContractNumber = milestoneVO.ContractNumber;
            ContractID = milestoneVO.ContractID;
            ContractLineID = milestoneVO.ContractLineID;
            ContractMaintenanceID = milestoneVO.ContractMaintenanceID;
            MilestoneStatusID = milestoneVO.MilestoneStatusID;
            MilestoneStatus = milestoneVO.MilestoneStatusName;
            MilestoneStatusDescription = milestoneVO.MilestoneStatusDescription;
            InvoiceDate = milestoneVO.InvoiceDate;
            Amount = milestoneVO.Amount;
            RenewalEndDate = milestoneVO.RenewalEndDate.HasValue ? milestoneVO.RenewalEndDate : null;
            RenewalStartDate = milestoneVO.RenewalStartDate.HasValue ? milestoneVO.RenewalStartDate : null;
            Description = milestoneVO.Description;
            Percentage = milestoneVO.Percentage;
            IndexRate = milestoneVO.IndexRate;
            PreviousValue = milestoneVO.PreviousValue;
            PreviousLine = milestoneVO.PreviousLine;
            IsApprovalRequired = milestoneVO.IsApprovalRequired;
            ApprovedStatus = milestoneVO.ApprovedStatus;
            ApprovedBy = milestoneVO.ApprovedBy;
            ApprovedByCustomer = milestoneVO.ApprovedByCustomer;
            CustomerApprovedStatus = milestoneVO.CustomerApprovedStatus;
            ConditionType = milestoneVO.ConditionType;
            SLT_OUR_REF_2 = milestoneVO.SLT_OUR_REF_2;
            CompanyName = milestoneVO.CompanyName;
            InvoiceCustomerName = milestoneVO.InvoiceCustomerName;
            ContractNumber = milestoneVO.ContractNumber;
            ActivityName = milestoneVO.ActivityName;
            IsApproved = milestoneVO.IsApproved;
            Comments = milestoneVO.Comments;
            UpliftForMilestone = milestoneVO.UpliftForMilestone;
            ActualBillDate = milestoneVO.ActualBillDate;

            foreach (var item in milestoneVO.MilestoneBillingLineVos)
            {
                MilestoneBillingLines.Add(new MilestoneBillingLine(item));
            }
        }

        /// <summary>
        /// Transpose model milestone object to milestone value object
        /// </summary>
        /// <param name="contractMaintenance"></param>
        public MilestoneVO Transpose(int? userId)
        {
            MilestoneVO milestoneVO = new MilestoneVO();

            milestoneVO.ID = this.ID;
            milestoneVO.OldMilestoneID = this.OldMilestoneID;
            milestoneVO.ContractID = this.ContractID;
            milestoneVO.ContractLineID = this.ContractLineID;
            milestoneVO.ContractMaintenanceID = this.ContractMaintenanceID;
            milestoneVO.MilestoneStatusID = this.MilestoneStatusID;
            milestoneVO.MilestoneStatusDescription = this.MilestoneStatusDescription;
            milestoneVO.MilestoneStatusName = this.MilestoneStatus;
            milestoneVO.InvoiceDate = this.InvoiceDate;
            milestoneVO.Amount = this.Amount;
            milestoneVO.RenewalEndDate = this.RenewalEndDate;
            milestoneVO.RenewalStartDate = this.RenewalStartDate;
            milestoneVO.CreatedByUserId = userId;
            milestoneVO.LastUpdatedByUserId = userId;
            milestoneVO.Description = this.Description;
            milestoneVO.Percentage = this.Percentage;
            milestoneVO.IndexRate = this.IndexRate;
            milestoneVO.PreviousValue = this.PreviousValue;
            milestoneVO.PreviousLine = this.PreviousLine;
            milestoneVO.IsApprovalRequired = this.IsApprovalRequired;
            milestoneVO.ApprovedStatus = this.ApprovedStatus;
            milestoneVO.ApprovedBy = this.ApprovedBy;
            milestoneVO.ApprovedByCustomer = this.ApprovedByCustomer;
            milestoneVO.CustomerApprovedStatus = this.CustomerApprovedStatus;
            milestoneVO.ConditionType = this.ConditionType;
            milestoneVO.SLT_OUR_REF_2 = this.SLT_OUR_REF_2;
            milestoneVO.IsApproved = this.IsApproved;
            milestoneVO.Comments = this.Comments;
            

            milestoneVO.UpliftForMilestone = this.UpliftForMilestone;

            FillBillingLines(this);

            milestoneVO.MilestoneBillingLineVos = MilestoneBillingLineVos;

            return milestoneVO;
        }

        /// <summary>
        /// Fill billing lines to store in database
        /// </summary>
        /// <param name="milestone">The milestone object</param>
        private void FillBillingLines(Milestone milestone)
        {
            if (MilestoneBillingLineVos == null)
            {
                MilestoneBillingLineVos = new List<MilestoneBillingLineVO>();
            }

            //if (!String.IsNullOrEmpty(contractMaintenance.billingText1))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID1, LineSequance = 0, LineText = milestone.billingText1 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText2))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID2, LineSequance = 1, LineText = milestone.billingText2 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText3))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID3, LineSequance = 2, LineText = milestone.billingText3 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText4))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID4, LineSequance = 3, LineText = milestone.billingText4 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText5))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID5, LineSequance = 4, LineText = milestone.billingText5 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText6))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID6, LineSequance = 5, LineText = milestone.billingText6 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText7))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID7, LineSequance = 6, LineText = milestone.billingText7 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText8))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID8, LineSequance = 7, LineText = milestone.billingText8 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText9))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID9, LineSequance = 8, LineText = milestone.billingText9 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText10))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID10, LineSequance = 9, LineText = milestone.billingText10 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText11))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID11, LineSequance = 10, LineText = milestone.billingText11 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText12))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID12, LineSequance = 11, LineText = milestone.billingText12 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText13))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID13, LineSequance = 12, LineText = milestone.billingText13 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText14))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID14, LineSequance = 13, LineText = milestone.billingText14 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText15))
            //{
            MilestoneBillingLineVos.Add(new MilestoneBillingLineVO() { ID = milestone.billingTextID15, LineSequance = 14, LineText = milestone.billingText15 });
            //}
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (InvoiceDate != null && InvoiceDate.ToShortDateString().Contains(str)) ||
                (RenewalStartDate != null && RenewalStartDate.Value.ToShortDateString().Contains(str)) ||
                (RenewalEndDate != null && RenewalEndDate.Value.ToShortDateString().Contains(str)) ||
                (ActualBillDate != null && ActualBillDate.Value.ToShortDateString().Contains(str)) ||
                (Amount != 0 && Amount.ToString().StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (UpliftForMilestone != 0 && UpliftForMilestone.ToString().StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (MilestoneStatusDescription != null && MilestoneStatusDescription.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
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
                InvoiceDate.Date.ToString(Constants.DATE_FORMAT),
                RenewalStartDate.HasValue ? RenewalStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                RenewalEndDate.HasValue ? RenewalEndDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                ActualBillDate.HasValue ? ActualBillDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, Amount), 
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE,UpliftForMilestone),
                MilestoneStatusDescription};
            return result;
        }
    }
}