using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ContractMaintenanceGroup : BaseModel
    {
        public int ContractId { get; set; }

        [Display(Name = "Inflation Index")]
        public int? InflationIndexId { get; set; }        

        [Display(Name = "Coding Line")]
        [Required(ErrorMessage = "Please select coding Details")]
        public int ActivityCodeId { get; set; }

        [Display(Name = "Activity Code")]
        public string ActivityCode { get; set; }

        public string OAActivityId { get; set; }

        [Required(ErrorMessage = " Please select Charge frequency")]
        [Display(Name = "Charge Frequency")]
        public int PeriodFrequencyId { get; set; }

        [Display(Name = "Charge Frequency")]
        public string PeriodFrequency { get; set; }
        

        [Display(Name = "First Period Start Date")]
        public DateTime? FirstPeriodStartDate { get; set; }

        [Display(Name = "First Period Amount")]
        [RegularExpression(@"^[+-]?\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Please enter valid first period amount")]
        public decimal FirstPeriodAmount { get; set; }

        [Display(Name = "First Renewal Date")]
        //[Required(ErrorMessage = "Please enter First Renewal date")]
        public DateTime? FirstRenewalDate { get; set; }


        [Display(Name = "Base Annual Amount")]        
        [Range(double.MinValue, double.MaxValue)]
        public decimal BaseAnnualAmount { get; set; }

        [Display(Name = "Job Code")]
        public string OAJobCodeId { get; set; }

        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> FinalRenewalStartDate { get; set; }

        [Display(Name = "End Date")]
        public Nullable<System.DateTime> FinalRenewalEndDate { get; set; }

        [Display(Name = "Amount")]
        [RegularExpression(@"^[+-]?\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Please enter valid amount")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Amount cannot be zero")]
        public Nullable<decimal> EndAmount { get; set; }

        /// <summary>
        /// set and get Document Type
        /// </summary>
        [Display(Name = "Document Type")]
        public int? DocumentTypeId { get; set; }

        public Nullable<int> InvoiceAdvancedArrears { get; set; }

        public int InvoiceInAdvance { get; set; }
       
        /// <summary>
        /// Gets or set QTY
        /// </summary>
        [Required(ErrorMessage = "Please enter Qty")]
        [Range(1, 1000, ErrorMessage = "Please enter Qty between the range of 1 to 1000")]
        [RegularExpression("^([0-9])*$", ErrorMessage = "Please enter valid Qty")]
        [Display(Name = "Qty")]
        public int QTY { get; set; }

        public string ContractLineDetails { get; set; }        

        public int InvoiceCustomerId { get; set; }        

        public bool? IsDefaultLineInGroup { get; set; }

        public bool? IsGrouped { get; set; }

        public int? GroupId { get; set; }

        [Required(ErrorMessage = "Please select group name")]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        public bool? IsNewGroup { get; set; }

        public bool? IsExistingGroup { get; set; }

        public List<string> GroupNameList { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ContractMaintenanceGroup()
        {
            GroupNameList = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        public ContractMaintenanceGroup(ContractMaintenanceVO contractMaintenanceVO): this()
        {
            ID = contractMaintenanceVO.ID;
            PeriodFrequencyId = contractMaintenanceVO.PeriodFrequencyId;
            PeriodFrequency = contractMaintenanceVO.PeriodFrequency;
            BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount;
            FirstPeriodAmount = contractMaintenanceVO.FirstPeriodAmount;
            FirstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate.HasValue ? contractMaintenanceVO.FirstPeriodStartDate : null;
            FirstRenewalDate = contractMaintenanceVO.FirstRenewalDate.HasValue ? contractMaintenanceVO.FirstRenewalDate : null;            
            ActivityCodeId = contractMaintenanceVO.ActivityCodeId;
            ActivityCode = contractMaintenanceVO.ActivityCode;
            FinalRenewalStartDate = contractMaintenanceVO.FinalRenewalStartDate.HasValue ? contractMaintenanceVO.FinalRenewalStartDate : null;
            FinalRenewalEndDate = contractMaintenanceVO.FinalRenewalEndDate.HasValue ? contractMaintenanceVO.FinalRenewalEndDate : null;
            EndAmount = contractMaintenanceVO.EndAmount;

            ContractId = contractMaintenanceVO.ContractId;            
            InvoiceCustomerId = contractMaintenanceVO.InvoiceCustomerId;            
            InflationIndexId = contractMaintenanceVO.InflationIndexId;                        
            OAActivityId = contractMaintenanceVO.OAActivityId;
            OAJobCodeId = contractMaintenanceVO.OAJobCodeId;
            DocumentTypeId = contractMaintenanceVO.DocumentTypeId;
            InvoiceAdvancedArrears = contractMaintenanceVO.InvoiceAdvancedArrears;
            InvoiceInAdvance = contractMaintenanceVO.InvoiceAdvancedId;
            QTY = contractMaintenanceVO.QTY;

            IsGrouped = contractMaintenanceVO.IsGrouped;
            GroupId = contractMaintenanceVO.GroupId;
            GroupName = contractMaintenanceVO.GroupName;
            
            IsDefaultLineInGroup = contractMaintenanceVO.IsDefaultLineInGroup;


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
            contractMaintenanceVO.PeriodFrequencyId = this.PeriodFrequencyId;
            contractMaintenanceVO.PeriodFrequency = this.PeriodFrequency;
            contractMaintenanceVO.ActivityCodeId = this.ActivityCodeId;                        
            contractMaintenanceVO.InflationIndexId = this.InflationIndexId;                                    
            contractMaintenanceVO.BaseAnnualAmount = this.BaseAnnualAmount;
            contractMaintenanceVO.FirstPeriodAmount = this.FirstPeriodAmount;
            contractMaintenanceVO.FirstPeriodStartDate = this.FirstPeriodStartDate;
            contractMaintenanceVO.FirstRenewalDate = this.FirstRenewalDate;

            contractMaintenanceVO.FinalRenewalStartDate = this.FinalRenewalStartDate;
            contractMaintenanceVO.FinalRenewalEndDate = this.FinalRenewalEndDate;
            contractMaintenanceVO.EndAmount = this.EndAmount;
       
            contractMaintenanceVO.ActivityCode = this.ActivityCode;
            
            contractMaintenanceVO.OAActivityId = this.OAActivityId;
            contractMaintenanceVO.OAJobCodeId = this.OAJobCodeId;
            contractMaintenanceVO.DocumentTypeId = this.DocumentTypeId;
            contractMaintenanceVO.InvoiceAdvancedArrears = this.InvoiceAdvancedArrears.Value;
            contractMaintenanceVO.InvoiceAdvancedId = this.InvoiceInAdvance;
            
            contractMaintenanceVO.QTY = this.QTY;

            contractMaintenanceVO.IsGrouped = this.IsGrouped;
            contractMaintenanceVO.GroupId = this.GroupId;
            contractMaintenanceVO.GroupName = this.GroupName;
            contractMaintenanceVO.IsDefaultLineInGroup = this.IsDefaultLineInGroup;

            contractMaintenanceVO.CreatedByUserId = userId;
            contractMaintenanceVO.LastUpdatedByUserId = userId;                        

            return contractMaintenanceVO;
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            var isCheckboxSelected = "";
            var isRadioButtonSelected = "";
            if (IsGrouped != null)
            {
                isCheckboxSelected = "<input type='checkbox' name='check5' value='" + ID + "' checked='true' disabled='true'  >";
            }
            else 
            {
                isCheckboxSelected = "<input type='checkbox' name='check5' value='" + ID + "' >";
            }

            if (IsDefaultLineInGroup == true)
            {
                isRadioButtonSelected = "<input type='radio' name='rdbIsDefault' value='" + ID + "' checked='true' >";
            }
            else 
            {
                isRadioButtonSelected = "<input type='radio' name='rdbIsDefault' value='" + ID + "' >";
            }

            object[] result = new object[] {                    
                isCheckboxSelected,
                ID,         
                ActivityCode + " - " + OAActivityId,         
                OAJobCodeId,
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, BaseAnnualAmount),
                PeriodFrequency,                 
                FirstPeriodStartDate.HasValue ? FirstPeriodStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : null, 
                FinalRenewalStartDate.HasValue ? FinalRenewalStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                FinalRenewalEndDate.HasValue ? FinalRenewalEndDate.Value.Date.ToString(Constants.DATE_FORMAT) : null,
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, EndAmount),
                isRadioButtonSelected,
                GroupName,
                IsGrouped,
                GroupId,                
                PeriodFrequencyId,
                IsDefaultLineInGroup
                };
            return result;
        }

    }
}