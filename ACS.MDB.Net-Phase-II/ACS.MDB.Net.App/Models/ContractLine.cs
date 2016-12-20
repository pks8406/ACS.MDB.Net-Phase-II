using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ContractLine : BaseModel
    {
        public int ContractLineID { get; set; }
        public int ContractID { get; set; }
        public string LineStatus { get; set; }
        public string LineDescription { get; set; }

        /// <summary>
        /// Gets or set ActivityCategory Id
        /// </summary>
        [Required(ErrorMessage = "Please select Activity category")]
        [Display(Name = "Activity Category Id")]
        public int ActivityCategoryId { get; set; }

        /// <summary>
        /// Gets or set ActivityCategory
        /// </summary>
        [Display(Name = "Activity Category")]
        public string ActivityCategory { get; set; }

        /// <summary>
        /// Gets or set ActivityCode Id
        /// </summary>
        [Required(ErrorMessage = "Please select Activity code")]
        [Display(Name = "Activity Code Id")]
        public int ActivityCodeId { get; set; }

        /// <summary>
        /// Gets or set OAActivityCode Id
        /// </summary>
        public string OAActivityCodeId { get; set; }

        /// <summary>
        /// Gets or set Activity Code
        /// </summary>
        [Display(Name = "Activity Code")]
        public string ActivityCode { get; set; }

        ///// <summary>
        ///// Gets or set QTY
        ///// </summary>
        //[Required(ErrorMessage = "Please enter Qty")]
        //[Range(1, 1000, ErrorMessage = "Please enter Qty between the range of 1 to 1000")]
        //[RegularExpression("^([0-9])*$", ErrorMessage = "Please enter valid Qty")]
        //[Display(Name = "Qty")]
        //public int? QTY { get; set; }

        /// <summary>
        /// Gets or set JobCode Id
        /// </summary>
        [Display(Name = "Job Code Id")]
        [Required(ErrorMessage = "Please select Job code")]
        public int JobCodeId { get; set; }

        /// <summary>
        /// Gets or set OAJobCodeId
        /// </summary>
        public string OAJobCodeId { get; set; }

        /// <summary>
        /// Gets or set JobCode
        /// </summary>
        [Display(Name = "Job Code")]
        public string JobCode { get; set; }

        /// <summary>
        /// Gets or Set Account Id
        /// </summary>        
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or set OAAccount Id
        /// </summary>
        public string OAAccountId { get; set; }

        /// <summary>
        /// Gets or sets OAAccount code
        /// </summary>
        public string OAAccountCode { get; set; }

        /// <summary>
        /// Gets or set Account 
        /// </summary>
        [Display(Name = "Account")]
        //[Required(ErrorMessage = "Please enter Account")]
        public string Account { get; set; }

        /// <summary>
        /// Gets or set ConstCenter Id
        /// </summary>
        [Display(Name = "Cost Centre Id")]
        [Required(ErrorMessage = "Please select Cost centre")]
        public int CostCenterId { get; set; }

        /// <summary>
        /// Gets or set OACostCenterId
        /// </summary>
        public string OACostCenterId { get; set; }

        /// <summary>
        /// Gets or set Cost Center
        /// </summary>
        [Display(Name = "Cost Centre")]
        public string CostCenter { get; set; }

        public string ContractLineDetails { get; set; }

        public Contract Contract { get; set; }
        public Milestone Milestone { get; set; }
        public List<ContractMaintenance> ContractMaintenances { get; set; }
        public List<ActivityCategory> ActivityCategoryList { get; set; }
        public List<ActivityCode> ActivityCodeList { get; set; }
        public List<JobCode> JobCodeList { get; set; }
        public List<CostCentre> CostCentreList { get; set; }

        public bool IsJobCodeExist { get; set; }
        public bool? IsDeleted { get; set; }
        public string Visible { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractLine()
        {
            ContractMaintenances = new List<ContractMaintenance>();
            ActivityCategoryList = new List<ActivityCategory>();
            ActivityCodeList = new List<ActivityCode>();
            JobCodeList = new List<JobCode>();
            CostCentreList = new List<CostCentre>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contractLineVO"></param>
        /// <param name="visibleOrHidden"></param>
        public ContractLine(ContractLineVO contractLineVO, string visibleOrHidden = "visible")
        {
            ID = contractLineVO.ContractLineID;
            ContractID = contractLineVO.ContractID;
            LineStatus = contractLineVO.LineStatus;
            LineDescription = contractLineVO.LineDescription;
            ActivityCategoryId = contractLineVO.ActivityCategoryId;
            ActivityCategory = contractLineVO.ActivityCategory;
            ActivityCodeId = contractLineVO.ActivityCodeId;
            OAActivityCodeId = contractLineVO.OAActivityCodeId;
            ActivityCode = contractLineVO.ActivityCode;
            AccountId = contractLineVO.AccountId;
            OAAccountId = contractLineVO.OAAccountId;
            OAAccountCode = contractLineVO.OAAccountCode;
            Account = contractLineVO.OAAccountCode + '-' + contractLineVO.OAAccountId;

            JobCodeId = contractLineVO.JobCodeId;
            OAJobCodeId = contractLineVO.OAJobCodeId;
            JobCode = contractLineVO.JobCode;
            CostCenterId = contractLineVO.CostCenterId;
            OACostCenterId = contractLineVO.OACostCenterId;
            CostCenter = contractLineVO.CostCenter;
            //QTY = contractLineVO.QTY;
            IsDeleted = contractLineVO.IsDeleted;
            ContractMaintenances = new List<ContractMaintenance>();
            ActivityCategoryList = new List<ActivityCategory>();
            ActivityCodeList = new List<ActivityCode>();
            JobCodeList = new List<JobCode>();
            CostCentreList = new List<CostCentre>();
            Visible = visibleOrHidden;

            ContractLineDetails = ActivityCode + "    |    " +
                OAJobCodeId + "    |    " +
                OAAccountCode + "    |    " +
                OACostCenterId + " - " +
                CostCenter;
        }

        /// <summary>
        /// Transpose model object to value object
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ContractLineVO Transpose(int? userId)
        {
            ContractLineVO contractLineVO = new ContractLineVO();

            contractLineVO.ContractLineID = this.ID;
            contractLineVO.ContractID = this.ContractID;
            contractLineVO.LineStatus = this.LineStatus;
            contractLineVO.LineDescription = this.LineDescription;
            contractLineVO.ActivityCategoryId = this.ActivityCategoryId;
            contractLineVO.ActivityCategory = this.ActivityCategory;
            contractLineVO.ActivityCategory = this.ActivityCategory;
            contractLineVO.ActivityCodeId = this.ActivityCodeId;
            contractLineVO.OAActivityCodeId = this.OAActivityCodeId;
            contractLineVO.ActivityCode = this.ActivityCode;
            contractLineVO.ActivityCodeName = this.ActivityCode + '-' + this.OAActivityCodeId;
            contractLineVO.AccountId = this.AccountId;
            contractLineVO.OAAccountId = this.OAAccountId;
            contractLineVO.OAAccountCode = this.OAAccountCode;
            contractLineVO.Account = this.OAAccountCode + '-' + this.OAAccountId;
            contractLineVO.JobCodeId = this.JobCodeId;
            contractLineVO.OAJobCodeId = this.OAJobCodeId;
            contractLineVO.JobCode = this.JobCode;
            contractLineVO.JobCodeName = this.JobCode + '-' + this.OAJobCodeId;
            contractLineVO.CostCenterId = this.CostCenterId;
            contractLineVO.OACostCenterId = this.OACostCenterId;
            contractLineVO.CostCenter = this.CostCenter;
            contractLineVO.CostCenterName = this.CostCenter + '-' + this.OACostCenterId;
            //QTY = contractLine.QTY;
            contractLineVO.IsDeleted = this.IsDeleted;

            contractLineVO.ContractLineDetails = this.ActivityCode + '|' + this.OAJobCodeId + '|' + this.Account + '|' + this.CostCenter;
            contractLineVO.CreatedByUserId = userId;
            contractLineVO.LastUpdatedByUserId = userId;

            return contractLineVO;

        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (ActivityCategory != null && ActivityCategory.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (ActivityCode != null && ActivityCode.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (OAActivityCodeId != null && OAActivityCodeId.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (Account != null && Account.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (JobCode != null && JobCode.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (OAJobCodeId != null && OAJobCodeId.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (CostCenter != null && CostCenter.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (OACostCenterId != null && OACostCenterId.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
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
                ID, ActivityCategory,
                ActivityCode + '-' + OAActivityCodeId, 
                Account,
                OAJobCodeId + '-' + JobCode,
                OACostCenterId + '-' + CostCenter ,
               // QTY,                
                ActivityCodeId,
                ActivityCode,
                OAJobCodeId};
            return result;
        }
    }
}