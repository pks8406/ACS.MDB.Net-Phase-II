using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ContractMaintenance : BaseModel
    {
        public int ContractId { get; set; }

        [Display(Name = "Coding Line")]
        [Required(ErrorMessage = "Please select Coding Details")]
        public int ContractLineId { get; set; }
        public string ContractNumber { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        
        public int InvoiceInAdvance { get; set; }

        [Display(Name = "Inflation Index")]
        public int? InflationIndexId { get; set; }
        public int InvoiceAdvancedId { get; set; }

        [Display(Name = "Coding Line")]
        [Required(ErrorMessage = "Please select coding Details")]
        public int ActivityCodeId { get; set; }

        [Display(Name = "Activity Code")]
        public string ActivityCode { get; set; }

        public string  OAActivityId { get; set; }

        [Required(ErrorMessage = " Please select Charge frequency")]
        [Display(Name = "Charge Frequency")]
        public int PeriodFrequencyId { get; set; }

        [Display(Name = "Charge Frequency")]
        public string PeriodFrequency { get; set; }

        [Display(Name = "Product")]
        [Required(ErrorMessage = " Please select Product")]
        public int ProductId { get; set; }

        [Display(Name = "Sub Product")]
        //[Required(ErrorMessage = "Please select subproduct")]
        public int? SubProductId { get; set; }

        [Required(ErrorMessage = "Please select create reason")]
        [Display(Name = "Create Reason")]
        public int ReasonCode { get; set; }

        [Display(Name = "First Period Start Date")]
        public DateTime? FirstPeriodStartDate { get; set; }

        [Display(Name = "First Period Amount")]
        [RegularExpression(@"^[+-]?\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Please enter valid first period amount")]        
        public decimal FirstPeriodAmount { get; set; }

        [Display(Name = "First Renewal Date")]
        //[Required(ErrorMessage = "Please enter First Renewal date")]
        public DateTime? FirstRenewalDate { get; set; }

        [Display(Name = "Uplift Required?")]
        public bool UpliftRequired { get; set; }

        [Display(Name = "First Annual Uplift Date")]
        
        public Nullable<System.DateTime> FirstAnnualUpliftDate { get; set; }

        [Display(Name = "Base Annual Amount")]
        [Required(ErrorMessage = "Please enter base annual amount")]
        [RegularExpression(@"^[+-]?\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Please enter valid base annual amount")]
        [Range(double.MinValue, double.MaxValue)]
        public decimal BaseAnnualAmount { get; set; }

        [Display(Name = "Amount")]
        [RegularExpression(@"^[+-]?\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Please enter valid amount")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Amount cannot be zero")]
        public Nullable<decimal> EndAmount { get; set; }

        public string InflationIndexName { get; set; }

        public Nullable<bool> FixedUpliftRequired { get; set; }

        [Display(Name = "Additional Fixed %")]
        [RegularExpression(@"^[+-]?\d{0,2}(\.\d{0,2})?$", ErrorMessage = "Please enter valid additional fixed")]
        public Nullable<decimal> InflationFixedAdditional { get; set; }

        //[Display(Name = "Termination Reason")]
        //[StringLength(300, ErrorMessage = "Termination Reason must be with a maximum length of 300")]
        ////[RegularExpression(@"^([a-zA-Z0-9 ""&'-._=#/%$£!?\r\n]+)$", ErrorMessage = "Please enter valid termination reason")]
        //public string TerminationReason { get; set; }

        /// <summary>
        /// set and get termination description based on delete reason
        /// </summary>
        public string TerminationDescription { get; set; }

        [Display(Name = "Termination Reason")]
        public int? DeleteReason { get; set; }

        /// <summary>
        /// set and get Document Type
        /// </summary>
        [Display(Name = "Document Type")]
        public int? DocumentTypeId { get; set; }

        [Display(Name = "Comment")]
        [StringLength(300, ErrorMessage = "Comment must be with a maximum length of 300")]
        //[RegularExpression(@"^([a-zA-Z0-9 ""&'-._=#/%$£!?\r\n]+)$", ErrorMessage = "Please enter valid comment")]
        public string Comment { get; set; }

        [Display(Name = "End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> FinalRenewalStartDate { get; set; }

        [Display(Name = "End Date")]
        public Nullable<System.DateTime> FinalRenewalEndDate { get; set; }

        //[Display(Name = "Backlog?")]
        //public bool IncludeInForecast { get; set; }

        [Display(Name = "Backlog")]
        //public bool IncludeInForecast { get; set; }
        public int IncludeInForecast { get; set; }

        //public Nullable<int> LastMilestoneBilled { get; set; }

        public Nullable<int> InvoiceAdvancedArrears { get; set; }
        //public Nullable<System.DateTime> LatestVersionDate { get; set; }

        [Display(Name = "Activation Date")]        
        public Nullable<System.DateTime> ReasonDate { get; set; }

        [Display(Name = "Job Code")]        
        public string OAJobCodeId { get; set; }

        public string CostCenter { get; set; }

        public string  Account { get; set; }

        /// <summary>
        /// Gets or set QTY
        /// </summary>
        [Required(ErrorMessage = "Please enter Qty")]
        [Range(1, 1000, ErrorMessage = "Please enter Qty between the range of 1 to 1000")]
        [RegularExpression("^([0-9])*$", ErrorMessage = "Please enter valid Qty")]
        [Display(Name = "Qty")]
        public int QTY { get; set; }

        public string  ContractLineDetails { get; set; }

        public string BillingLines { get; set; }
        //public bool IncludeForecast { get; set; }

        public int MileStoneCount { get; set; }

        public int MilestoneStatusCount { get; set; }

        public bool MilestoneStatusCountForLinkLoaded { get; set; }

        [Display(Name = "Termination Date")]
        public Nullable<System.DateTime> DeleteDate { get; set; }

        [Display(Name = "Create Date")]
        [Required(ErrorMessage = "Please enter Create date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Customer Notes")]
        public string CustomerComment { get; set; }

        public int InvoiceCustomerId { get; set; }

        public int? GroupId { get; set; }

        public string GroupName { get; set; }

        public bool? IsGrouped { get; set; }

        public bool? IsDefaultLineInGroup { get; set; }

        [Display(Name = " On : ")]
        public DateTime? LastUpdatedDate { get; set; }

        [Display(Name = "Last changed by : ")]
        public int? LastUpdatedBy { get; set; }

        public string LastUpdatedByEmailId { get; set; }

        [Display(Name = "Forecast Billing Start Date")]
        [DataType(DataType.Date, ErrorMessage = "Date Should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[Required(ErrorMessage = " Please select Forecast Billing Start Date")]
        public DateTime? ForecastBillingStartDate { get; set; }

        public virtual ICollection<Milestone> Milestones { get; set; }

        public List<ContractLine> ContractLineList { get; set; }
        public List<ActivityCode> ActivityCodeList { get; set; }
        public List<ChargingFrequencyVO> ChargingFrequencyList { get; set; }
        //public List<IncludeForecast> IncludeForecastList { get; set; }
        public List<ProductVO> ProductList { get; set; }
        public List<SubProductVO> SubProductList { get; set; }
        public List<InflationIndexVO> InflationIndexList { get; set; }
        public List<InvoiceAdvanced> InvoiceAdvancedList { get; set; }
        public List<InvoiceAdvanced> InvoiceAdvancedValueList { get; set; }
        public List<AuditReason> AuditReasonList { get; set; }
        public List<MaintenanceBillingLineVO> billingLinesToSave { get; set; }

        public List<MaintenanceBillingLineVO> MaintenanceBillingLineVOs { get; set; }

        public List<MaintenanceBillingLine> MaintenanceBillingLines { get; set; }

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

        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText1 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText2 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText3 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText4 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText5 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText6 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText7 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText8 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText9 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText10 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText11 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText12 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText13 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText14 { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 &'-:;!$^_.=@#/%\r\n\\[\\]]+)$")]
        public string billingText15 { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractMaintenance()
        {
            ContractLineList = new List<ContractLine>();
            ActivityCodeList = new List<ActivityCode>();
            ChargingFrequencyList = new List<ChargingFrequencyVO>();
            //IncludeForecastList = new List<IncludeForecast>();
            ProductList = new List<ProductVO>();
            SubProductList = new List<SubProductVO>();
            InflationIndexList = new List<InflationIndexVO>();
            InvoiceAdvancedList = new List<InvoiceAdvanced>();
            AuditReasonList = new List<AuditReason>();
            MaintenanceBillingLineVOs = new List<MaintenanceBillingLineVO>();
            //this.Milestones = new List<Milestone>();
            billingLinesToSave = new List<MaintenanceBillingLineVO>();

            MaintenanceBillingLines = new List<MaintenanceBillingLine>();
        }

        /// <summary>
        /// Transpose value object to model object
        /// </summary>
        public ContractMaintenance(ContractMaintenanceVO contractMaintenanceVO) : this()
        {
            ID = contractMaintenanceVO.ID;
            PeriodFrequencyId = contractMaintenanceVO.PeriodFrequencyId;
            PeriodFrequency = contractMaintenanceVO.PeriodFrequency;
            BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount;
            FirstPeriodAmount = contractMaintenanceVO.FirstPeriodAmount;
            FirstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate.HasValue ? contractMaintenanceVO.FirstPeriodStartDate : null;
            FirstRenewalDate = contractMaintenanceVO.FirstRenewalDate.HasValue ? contractMaintenanceVO.FirstRenewalDate : null;
            FinalRenewalStartDate = contractMaintenanceVO.FinalRenewalStartDate.HasValue ? contractMaintenanceVO.FinalRenewalStartDate : null;
            FinalRenewalEndDate = contractMaintenanceVO.FinalRenewalEndDate.HasValue ? contractMaintenanceVO.FinalRenewalEndDate : null;
            FirstAnnualUpliftDate = contractMaintenanceVO.FirstAnnualUpliftDate.HasValue ? contractMaintenanceVO.FirstAnnualUpliftDate : null;
            
            EndAmount = contractMaintenanceVO.EndAmount;
            InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional;
            ActivityCodeId = contractMaintenanceVO.ActivityCodeId;
            ActivityCode = contractMaintenanceVO.ActivityCode;

            ContractId = contractMaintenanceVO.ContractId;
            ContractNumber = contractMaintenanceVO.ContractNumber;
            ContractLineId = contractMaintenanceVO.ContractLineId;
            CompanyId = contractMaintenanceVO.CompanyId;
            CompanyName = contractMaintenanceVO.CompanyName;
            InvoiceCustomerId = contractMaintenanceVO.InvoiceCustomerId;
 
            ProductId = contractMaintenanceVO.ProductId;
            SubProductId = contractMaintenanceVO.SubProductId;
            InflationIndexId = contractMaintenanceVO.InflationIndexId;
            InflationIndexName = contractMaintenanceVO.InflationIndexName;
            InvoiceAdvancedId = contractMaintenanceVO.InvoiceAdvancedId;
            InvoiceInAdvance = contractMaintenanceVO.InvoiceAdvancedId;
            IncludeInForecast = contractMaintenanceVO.IncludeInForecast;
            InvoiceAdvancedArrears = contractMaintenanceVO.InvoiceAdvancedArrears;

            CreationDate = contractMaintenanceVO.CreationDate;
            ReasonCode = contractMaintenanceVO.ReasonId;
            ReasonDate = contractMaintenanceVO.ReasonDate;
            Comment = contractMaintenanceVO.Comment;
            UpliftRequired = contractMaintenanceVO.UpliftRequired.HasValue ? contractMaintenanceVO.UpliftRequired.Value : false;
            TerminationDescription = contractMaintenanceVO.TerminationDescription;
            DeleteReason = contractMaintenanceVO.DeleteReason;
            ForecastBillingStartDate = contractMaintenanceVO.ForecastBillingStartDate;
            DeleteDate = contractMaintenanceVO.DeleteDate;

            OAActivityId = contractMaintenanceVO.OAActivityId;
            OAJobCodeId = contractMaintenanceVO.OAJobCodeId;
            CostCenter = contractMaintenanceVO.CostCenter;
            Account = contractMaintenanceVO.Account;
            QTY = contractMaintenanceVO.QTY;

            GroupId = contractMaintenanceVO.GroupId;
            GroupName = contractMaintenanceVO.GroupName;
            IsGrouped = contractMaintenanceVO.IsGrouped;
            IsDefaultLineInGroup = contractMaintenanceVO.IsDefaultLineInGroup;

            DocumentTypeId = contractMaintenanceVO.DocumentTypeId;

            ContractLineDetails = ActivityCode + "    |    " +
                OAJobCodeId + "    |    " +
                Account + "    |    " +
                CostCenter + "    |    " +
                QTY + "    ";

            BillingLines = contractMaintenanceVO.BillingLines;
            MileStoneCount = contractMaintenanceVO.MileStoneCount;
            MilestoneStatusCount = contractMaintenanceVO.MilestoneStatusCount;
            MilestoneStatusCountForLinkLoaded = contractMaintenanceVO.MilestoneStatusCountForLinkLoaded;
            LastUpdatedDate = contractMaintenanceVO.LastUpdatedDate;
            LastUpdatedBy = contractMaintenanceVO.LastUpdatedByUserId;

            foreach (var maintenanceBillingLineVo in contractMaintenanceVO.MaintenanceBillingLineVos)
            {
                MaintenanceBillingLines.Add(new MaintenanceBillingLine(maintenanceBillingLineVo));
            }

            //MaintenanceBillingLines = contractMaintenanceVO.MaintenanceBillingLines;
        }

        /// <summary>
        /// Transpose model object to value object
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ContractMaintenanceVO Transpose(int? userId)
        {
            ContractMaintenanceVO contractMaintenanceVO = new ContractMaintenanceVO();

            contractMaintenanceVO.ID = this.ID;
            contractMaintenanceVO.ContractId = this.ContractId;
            contractMaintenanceVO.ContractLineId = this.ContractLineId;
            contractMaintenanceVO.InvoiceCustomerId = this.InvoiceCustomerId;
            contractMaintenanceVO.PeriodFrequencyId = this.PeriodFrequencyId;
            contractMaintenanceVO.PeriodFrequency = this.PeriodFrequency;
            contractMaintenanceVO.ActivityCodeId = this.ActivityCodeId;

            contractMaintenanceVO.ProductId = this.ProductId;
            contractMaintenanceVO.SubProductId = this.SubProductId;
            if (contractMaintenanceVO.SubProductId == -1)
            {
                contractMaintenanceVO.SubProductId = null;
            }

            contractMaintenanceVO.InflationIndexId = this.InflationIndexId;
            contractMaintenanceVO.InflationIndexName = this.InflationIndexName;
            contractMaintenanceVO.InvoiceAdvancedId = this.InvoiceInAdvance;
            contractMaintenanceVO.ReasonId = this.ReasonCode;
            contractMaintenanceVO.BaseAnnualAmount = this.BaseAnnualAmount;
            contractMaintenanceVO.FirstPeriodAmount = this.FirstPeriodAmount;
            contractMaintenanceVO.FirstPeriodStartDate = this.FirstPeriodStartDate;
            contractMaintenanceVO.FirstRenewalDate = this.FirstRenewalDate;
            contractMaintenanceVO.FirstAnnualUpliftDate = this.FirstAnnualUpliftDate;
            contractMaintenanceVO.FinalRenewalStartDate = this.FinalRenewalStartDate;
            contractMaintenanceVO.FinalRenewalEndDate = this.FinalRenewalEndDate;
            contractMaintenanceVO.EndAmount = this.EndAmount;
            contractMaintenanceVO.ActivityCode = this.ActivityCode;

            contractMaintenanceVO.CreationDate = this.CreationDate;
            contractMaintenanceVO.ReasonDate = this.ReasonDate;
            contractMaintenanceVO.ForecastBillingStartDate = this.ForecastBillingStartDate;
            contractMaintenanceVO.InvoiceAdvancedArrears = this.InvoiceAdvancedArrears.Value;
            contractMaintenanceVO.IncludeInForecast = this.IncludeInForecast;
            contractMaintenanceVO.Comment = this.Comment;
            contractMaintenanceVO.UpliftRequired = this.UpliftRequired;
            contractMaintenanceVO.InflationFixedAdditional = this.InflationFixedAdditional.HasValue ? this.InflationFixedAdditional / 100 : this.InflationFixedAdditional;
            // TerminationReason = this.TerminationReason;
            contractMaintenanceVO.DeleteReason = this.DeleteReason;
            contractMaintenanceVO.TerminationDescription = this.TerminationDescription;
            contractMaintenanceVO.DeleteDate = this.DeleteDate;
            contractMaintenanceVO.MileStoneCount = this.MileStoneCount;
            contractMaintenanceVO.MilestoneStatusCount = this.MilestoneStatusCount;
            contractMaintenanceVO.MilestoneStatusCountForLinkLoaded = this.MilestoneStatusCountForLinkLoaded;

            contractMaintenanceVO.OAActivityId = this.OAActivityId;
            contractMaintenanceVO.OAJobCodeId = this.OAJobCodeId;
            contractMaintenanceVO.CostCenter = this.CostCenter;
            contractMaintenanceVO.Account = this.Account;
            contractMaintenanceVO.QTY = this.QTY;

            contractMaintenanceVO.GroupId = this.GroupId;
            contractMaintenanceVO.GroupName = this.GroupName;
            contractMaintenanceVO.IsGrouped = this.IsGrouped;
            contractMaintenanceVO.IsDefaultLineInGroup = this.IsDefaultLineInGroup;
            contractMaintenanceVO.DocumentTypeId = this.DocumentTypeId;

            contractMaintenanceVO.CreatedByUserId = userId;
            contractMaintenanceVO.LastUpdatedByUserId = userId;
            
            FillBillingLines(this);

            contractMaintenanceVO.MaintenanceBillingLineVos = this.billingLinesToSave;

            return contractMaintenanceVO;
        }

        /// <summary>
        /// Fill billing lines to store in database
        /// </summary>
        /// <param name="contractMaintenance">The contract maintenance object</param>
        private void FillBillingLines(ContractMaintenance contractMaintenance)
        {
            if (billingLinesToSave == null)
            {
                billingLinesToSave = new List<MaintenanceBillingLineVO>();
            }

            //if (!String.IsNullOrEmpty(contractMaintenance.billingText1))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID1, LineSequance = 1, LineText = contractMaintenance.billingText1 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText2))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID2, LineSequance = 2, LineText = contractMaintenance.billingText2 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText3))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID3, LineSequance = 3, LineText = contractMaintenance.billingText3 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText4))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID4, LineSequance = 4, LineText = contractMaintenance.billingText4 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText5))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID5, LineSequance = 5, LineText = contractMaintenance.billingText5 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText6))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID6, LineSequance = 6, LineText = contractMaintenance.billingText6 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText7))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID7, LineSequance = 7, LineText = contractMaintenance.billingText7 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText8))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID8, LineSequance = 8, LineText = contractMaintenance.billingText8 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText9))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID9, LineSequance = 9, LineText = contractMaintenance.billingText9 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText10))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID10, LineSequance = 10, LineText = contractMaintenance.billingText10 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText11))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID11, LineSequance = 11, LineText = contractMaintenance.billingText11 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText12))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID12, LineSequance = 12, LineText = contractMaintenance.billingText12 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText13))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID13, LineSequance = 13, LineText = contractMaintenance.billingText13 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText14))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID14, LineSequance = 14, LineText = contractMaintenance.billingText14 });
            //}
            //if (!String.IsNullOrEmpty(contractMaintenance.billingText15))
            //{
            billingLinesToSave.Add(new MaintenanceBillingLineVO() { BillingLineID = contractMaintenance.billingTextID15, LineSequance = 15, LineText = contractMaintenance.billingText15 });
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
            return (
                PeriodFrequency != null && PeriodFrequency.StartsWith(str, StringComparison.CurrentCultureIgnoreCase) ||
                 ActivityCode != null && ActivityCode.StartsWith(str, StringComparison.CurrentCultureIgnoreCase) ||
                 OAJobCodeId != null && OAJobCodeId.StartsWith(str, StringComparison.CurrentCultureIgnoreCase) ||
                 BaseAnnualAmount != 0 && BaseAnnualAmount.ToString().StartsWith(str, StringComparison.CurrentCultureIgnoreCase) ||
                 FirstPeriodStartDate != null && FirstPeriodStartDate.Value.ToShortDateString().Contains(str) ||
                 FirstPeriodAmount != 0 && FirstPeriodAmount.ToString().StartsWith(str, StringComparison.CurrentCultureIgnoreCase) ||
                 FirstRenewalDate != null && FirstRenewalDate.Value.ToShortDateString().Contains(str) ||
                 FinalRenewalStartDate != null && FinalRenewalStartDate.Value.ToShortDateString().Contains(str) ||
                 FinalRenewalEndDate != null && FinalRenewalEndDate.Value.ToShortDateString().Contains(str) ||
                 EndAmount != null && EndAmount.ToString().StartsWith(str, StringComparison.CurrentCultureIgnoreCase));

        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { 
                "<input type='checkbox' name='check5' value='" + ID + "'>",
                ID,                 
                ActivityCode + " - " + OAActivityId, 
                OAJobCodeId,
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, BaseAnnualAmount),
                PeriodFrequency, 
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, FirstPeriodAmount), 
                FirstPeriodStartDate.HasValue ? FirstPeriodStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : null, 
                FirstRenewalDate.HasValue ? FirstRenewalDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                FinalRenewalStartDate.HasValue ? FinalRenewalStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                FinalRenewalEndDate.HasValue ? FinalRenewalEndDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, EndAmount),
                ActivityCode,
                Account,
                CostCenter,
                QTY,
                IncludeInForecast,                
                null,
                String.IsNullOrEmpty(BillingLines) ? "No Billing Line Text exist" : BillingLines,
                GroupName,
                IsGrouped,
                IsDefaultLineInGroup,
                PeriodFrequencyId,
                DocumentTypeId,
                InvoiceAdvancedArrears,
                InvoiceInAdvance
                };
            return result;
        }
    }
}