using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class MilestoneVO : BaseVO
    {
        public int ID { get; set; }
        public int ContractID { get; set; }
        public int ContractLineID { get; set; }
        public int ContractMaintenanceID { get; set; }
        public int CompanyID { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public string MilestoneStatusDescription { get; set; }
        public string MilestoneStatusName { get; set; }
        public int MilestoneStatusID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Description { get; set; }
        public string ApprovedStatus { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<bool> ApprovedByCustomer { get; set; }
        public string CustomerApprovedStatus { get; set; }
        public string ConditionType { get; set; }
        public string SLT_OUR_REF_2 { get; set; }
        public string CompanyName { get; set; }
        public string InvoiceCustomerName { get; set; }
        public int InvoiceCustomerID { get; set; }
        public string ContractNumber { get; set; }
        public string DivisionName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ActivityName { get; set; }
        public string BillingLines { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }

        public Nullable<int> OldMilestoneID { get; set; }
        public Nullable<System.DateTime> RenewalEndDate { get; set; }
        public Nullable<System.DateTime> RenewalStartDate { get; set; }
        public Nullable<int> DatabaseID { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> IndexRate { get; set; }
        public Nullable<decimal> PreviousValue { get; set; }
        public Nullable<int> PreviousLine { get; set; }
        public Nullable<bool> IsApprovalRequired { get; set; }
        public Nullable<decimal> UpliftForMilestone { get; set; }
        public Nullable<int> ChargingUpliftID;
        public Nullable<decimal> UpliftRate;
        public Nullable<decimal> UpliftFixedRate;
        public Nullable<decimal> Uplift;
        public Nullable<decimal> UpliftPercentage;
        /// <summary>
        /// Gets or set qty for milestone
        /// </summary>
        public int QTY { get; set; }

        /// <summary>
        /// Gets or set group id of associated contract maintenance
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Gets or set group name of the associated contract maintenance
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or set is associated contract maintenance grouped or not
        /// </summary>
        public bool? IsGrouped { get; set; }

        /// <summary>
        /// Gets or set is default line in group
        /// </summary>
        public bool? IsDefaultLineInGroup { get; set; }

        /// <summary>
        /// Gets or set period frequency for asspciated contract maintenance line
        /// </summary>
        public int PeriodFrequencyId { get; set; }

        /// <summary>
        /// Gets or set Actual bill date
        /// </summary>
        public DateTime? ActualBillDate { get; set; }

        //public List<MODEL.MilestoneBillingLine> MilestoneBillingLines { get; set; }

        /// <summary>
        /// Gets or set milestone billing line value object
        /// </summary>
        public List<MilestoneBillingLineVO> MilestoneBillingLineVos { get; set; }

        public List<MilestoneBillingLine> billingLinesToSave { get; set; }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public MilestoneVO()
        {
            //MilestoneBillingLines = new List<MODEL.MilestoneBillingLine>();
            MilestoneBillingLineVos = new List<MilestoneBillingLineVO>();

        }


        /// <summary>
        /// This method is specifically used to recalculate milestone 
        /// </summary>
        /// <param name="milestone"></param>
        /// <returns></returns>
        public MilestoneVO Transpose(Milestone milestone) 
        {
            ID = milestone.ID;
            OldMilestoneID = milestone.OldMilestoneID;
            ContractID = milestone.ContractID;
            ContractLineID = milestone.ContractLineID;
            ContractMaintenanceID = milestone.MaintenanceID;
            CompanyID = milestone.Contract.CompanyID;
            MilestoneStatusID = milestone.MilestoneStatusID;
            MilestoneStatusDescription = milestone.MilestoneStatus1.Description;
            MilestoneStatusName = milestone.MilestoneStatus1.StatusName;
            InvoiceDate = milestone.EstimatedDate;
            Amount = milestone.Amount;
            RenewalEndDate = milestone.RenewalEndDate;
            RenewalStartDate = milestone.RenewalStartDate;
            Description = milestone.Description;
            Percentage = milestone.Percentage;
            IndexRate = milestone.IndexRate;
            PreviousValue = milestone.PreviousValue;
            PreviousLine = milestone.PreviousLine;
            IsApprovalRequired = milestone.IsApprovalRequired;
            ApprovedStatus = milestone.ApprovedStatus;
            ApprovedBy = milestone.ApprovedBy;
            ApprovedByCustomer = milestone.ApprovedByCustomer;
            CustomerApprovedStatus = milestone.CustomerApprovedStatus;
            ConditionType = milestone.ConditionType;
            SLT_OUR_REF_2 = milestone.SLT_OUR_REF_2;
            ActualBillDate = milestone.BillDate;

            //CompanyID = milestone.Contract.CompanyID;
            CompanyName = milestone.Contract.OACompany.CompanyName;
            ContractNumber = milestone.Contract.ContractNumber;
            InvoiceCustomerID = milestone.Contract.InvoiceCustomerID;
            InvoiceCustomerName = milestone.Contract.OACustomer.CustomerName;
            DivisionName = milestone.Contract.Division.DivisionName;
            ActivityName = milestone.ContractLine.OAActivityCode.ActivityName;
            IsApproved = milestone.IsApproved;
            IsDeleted = milestone.IsDeleted;
            Comments = milestone.ContractMaintenance.Comment;
            Uplift = milestone.Uplift;
            UpliftForMilestone = milestone.Uplift;//milestone.Uplift.HasValue ? milestone.Uplift * 100 : milestone.Uplift;

            PeriodFrequencyId = milestone.ContractMaintenance.ChargeFrequencyID;
            GroupId = milestone.ContractMaintenance.GroupId;
            GroupName = milestone.ContractMaintenance.GroupName;
            IsGrouped = milestone.ContractMaintenance.IsGrouped;
            IsDefaultLineInGroup = milestone.ContractMaintenance.IsDefaultLineInGroup;
            
            return this;
        }

        /// <summary>
        /// Transpose LINQ milestone object to milestone value object
        /// </summary>
        /// <param name="milestone"></param>
        public MilestoneVO(Milestone milestone)
            : this()
        {
            ID = milestone.ID;
            OldMilestoneID = milestone.OldMilestoneID;
            ContractID = milestone.ContractID;
            ContractLineID = milestone.ContractLineID;
            ContractMaintenanceID = milestone.MaintenanceID;
            CompanyID = milestone.Contract.CompanyID;
            MilestoneStatusID = milestone.MilestoneStatusID;
            MilestoneStatusDescription = milestone.MilestoneStatus1.Description;
            MilestoneStatusName = milestone.MilestoneStatus1.StatusName;
            InvoiceDate = milestone.EstimatedDate;
            Amount = milestone.Amount;
            RenewalEndDate = milestone.RenewalEndDate;
            RenewalStartDate = milestone.RenewalStartDate;
            Description = milestone.Description;
            Percentage = milestone.Percentage;
            IndexRate = milestone.IndexRate;
            PreviousValue = milestone.PreviousValue;
            PreviousLine = milestone.PreviousLine;
            IsApprovalRequired = milestone.IsApprovalRequired;
            ApprovedStatus = milestone.ApprovedStatus;
            ApprovedBy = milestone.ApprovedBy;
            ApprovedByCustomer = milestone.ApprovedByCustomer;
            CustomerApprovedStatus = milestone.CustomerApprovedStatus;
            ConditionType = milestone.ConditionType;
            SLT_OUR_REF_2 = milestone.SLT_OUR_REF_2;
            ActualBillDate = milestone.BillDate;

            //CompanyID = milestone.Contract.CompanyID;
            CompanyName = milestone.Contract.OACompany.CompanyName;
            ContractNumber = milestone.Contract.ContractNumber;
            InvoiceCustomerID = milestone.Contract.InvoiceCustomerID;
            InvoiceCustomerName = milestone.Contract.OACustomer.CustomerName;
            DivisionName = milestone.Contract.Division.DivisionName;
            ActivityName = milestone.ContractLine.OAActivityCode.ActivityName;
            QTY = milestone.ContractMaintenance.QTY;
            IsApproved = milestone.IsApproved;
            IsDeleted = milestone.IsDeleted;
            Comments = milestone.ContractMaintenance.Comment;
            Uplift = milestone.Uplift;
            UpliftForMilestone = milestone.Uplift.HasValue ? milestone.Uplift * 100 : 0;

            PeriodFrequencyId = milestone.ContractMaintenance.ChargeFrequencyID;
            GroupId = milestone.ContractMaintenance.GroupId;
            GroupName = milestone.ContractMaintenance.GroupName;
            IsGrouped = milestone.ContractMaintenance.IsGrouped;
            IsDefaultLineInGroup = milestone.ContractMaintenance.IsDefaultLineInGroup;

            foreach (var item in milestone.MilestoneBillingLines)
            {
                if (item.IsDeleted == false)
                {
                    //MilestoneBillingLines.Add(new MODEL.MilestoneBillingLine(item));
                    MilestoneBillingLineVos.Add(new MilestoneBillingLineVO(item));
                }
            }
        }
    }
}