using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ContractLineVO : BaseVO
    {
        public int ContractLineID { get; set; }
        public int ContractID { get; set; }
        public string LineStatus { get; set; }
        public string LineDescription { get; set; }


        public int ActivityCategoryId { get; set; }
        public string ActivityCategory { get; set; }

        public int ActivityCodeId { get; set; }
        public string OAActivityCodeId { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityCodeName { get; set; }

        //public int? QTY { get; set; }

        public int JobCodeId { get; set; }
        public string OAJobCodeId { get; set; }
        public string JobCode { get; set; }
        public string JobCodeName { get; set; }

        public int AccountId { get; set; }
        public string OAAccountCode { get; set; }
        public string OAAccountId { get; set; }
        public string Account { get; set; }

        public int CostCenterId { get; set; }
        public string OACostCenterId { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterName { get; set; }

        public bool? IsDeleted { get; set; }

        public Contract Contract { get; set; }
        public Nullable<int> DatabaseID { get; set; }
        public List<MilestoneVO> MilestoneVO { get; set; }

        public string ContractLineDetails { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractLineVO()
        {
            MilestoneVO = new List<MilestoneVO>();
        }


        /// <summary>
        /// Transpose model object to value object
        /// </summary>
        /// <param name="contractLine">The contractline Model</param>
        /// <param name="userId">The user id</param>
        //public ContractLineVO(MODEL.ContractLine contractLine, int? userId)
        //{
        //    ContractLineID = contractLine.ID;
        //    ContractID = contractLine.ContractID;
        //    LineStatus = contractLine.LineStatus;
        //    LineDescription = contractLine.LineDescription;
        //    ActivityCategoryId = contractLine.ActivityCategoryId;
        //    ActivityCategory = contractLine.ActivityCategory;
        //    ActivityCodeId = contractLine.ActivityCodeId;
        //    OAActivityCodeId = contractLine.OAActivityCodeId;
        //    ActivityCode = contractLine.ActivityCode;
        //    ActivityCodeName = contractLine.ActivityCode + '-' + contractLine.OAActivityCodeId;
        //    AccountId = contractLine.AccountId;
        //    OAAccountId = contractLine.OAAccountId;
        //    OAAccountCode = contractLine.OAAccountCode;
        //    Account = contractLine.OAAccountCode + '-' + contractLine.OAAccountId;
        //    JobCodeId = contractLine.JobCodeId;
        //    OAJobCodeId = contractLine.OAJobCodeId;
        //    JobCode = contractLine.JobCode;
        //    JobCodeName = contractLine.JobCode + '-' + contractLine.OAJobCodeId;
        //    CostCenterId = contractLine.CostCenterId;
        //    OACostCenterId = contractLine.OACostCenterId;
        //    CostCenter = contractLine.CostCenter;
        //    CostCenterName = contractLine.CostCenter + '-' + contractLine.OACostCenterId;
        //    //QTY = contractLine.QTY;
        //    IsDeleted = contractLine.IsDeleted;

        //    ContractLineDetails = ActivityCode + '|' + OAJobCodeId + '|' + Account + '|' + CostCenter;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contractLine">The contractLine LINQ object</param>
        public ContractLineVO(ContractLine contractLine)
        {
            ContractLineID = contractLine.ID;
            ContractID = contractLine.ContractID;
            LineStatus = contractLine.LineStatus;
            LineDescription = contractLine.LineDescription;
            ActivityCategoryId = contractLine.ActivityCategoryID;
            ActivityCategory = contractLine.ActivityRestriction.Description;
            ActivityCodeId = contractLine.ActivityCodeID;
            OAActivityCodeId = contractLine.OAActivityCode.ActivityID;
            ActivityCode = contractLine.OAActivityCode.ActivityName;
            ActivityCodeName = contractLine.OAActivityCode.ActivityName + '-' + contractLine.OAActivityCode.ActivityID;
            AccountId = contractLine.AccountCodeID;
            OAAccountCode = contractLine.OAAccountCode.AccountName;
            OAAccountId = contractLine.OAAccountCode.AccountID;
            Account = contractLine.OAAccountCode.AccountName + '-' + contractLine.OAAccountCode.AccountID;
            JobCodeId = contractLine.JobCodeID;
            OAJobCodeId = contractLine.OAJobCode.JobCodeID;
            JobCode = contractLine.OAJobCode.JobCodeName;
            JobCodeName = contractLine.OAJobCode.JobCodeName + '-' + contractLine.OAJobCode.JobCodeID;
            CostCenterId = contractLine.CostCentreID;
            OACostCenterId = contractLine.OACostCentre.CostCentreID;
            CostCenter = contractLine.OACostCentre.CostCentreName;
            CostCenterName = contractLine.OACostCentre.CostCentreName + '-' + contractLine.OACostCentre.CostCentreID;
            //QTY = contractLine.QTY;
            IsDeleted = contractLine.IsDeleted;
            Contract = contractLine.Contract;

            //ContractLineDetails = ActivityCode + '|' + OAJobCodeId + '|' + Account + '|' + CostCenter + '|' + QTY;
            CreatedByUserId = contractLine.CreatedBy;
            LastUpdatedByUserId = contractLine.LastUpdatedBy;
        }
    }

}