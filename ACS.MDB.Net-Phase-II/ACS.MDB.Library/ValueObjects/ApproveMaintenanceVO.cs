using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ApproveMaintenanceVO : BaseVO
    {
        /// <summary>
        /// Milestone ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or set Company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set Company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set Invoice Customer ID
        /// </summary>
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// Get or set Invoice Customer Name
        /// </summary>
        public string InvoiceCustomer { get; set; }

        /// <summary>
        /// Gets or set Division id
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or set Division name
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or set Milestone status id
        /// </summary>
        public int? MilestoneStatusId { get; set; }

        /// <summary>
        /// Gets or set contract id
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// To date to filter milestone
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets and set changing uplift date
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or set contract number
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or set invoice date
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Gets or set renewal start date
        /// </summary>
        public DateTime RenewalStartDate { get; set; }

        /// <summary>
        /// Gets or set renewal end date
        /// </summary>
        public DateTime RenewalEndDate { get; set; }

        /// <summary>
        /// Gets or sets approval required?
        /// </summary>
        public bool ApprovalRequired { get; set; }

        /// <summary>
        /// Gets or set milestone approved?
        /// </summary>
        public bool IsApproved { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// List of division
        /// </summary>
        public List<DivisionVO> DivisionList { get; set; }

        /// <summary>
        /// List of Invoice customer
        /// </summary>
        public List<InvoiceCustomerVO> InvoiceCustomerList { get; set; }

        /// <summary>
        /// List of milestone status
        /// </summary>
        public List<MilestoneStatusVO> MilestoneStatusList { get; set; }

        //public List<MODEL.MilestoneBillingLine> MilestoneBillingLines { get; set; }
        public List<MilestoneBillingLine> billingLinesToSave { get; set; }

        //public ApproveMaintenanceVO(MODEL.ApproveMaintenance milestone, int? userId)
        //{
        //    ID = milestone.ID;
        //    InvoiceDate = milestone.InvoiceDate;
        //    MilestoneStatusId = milestone.MilestoneStatusId;
        //    Amount = milestone.Amount;
        //    LastUpdatedByUserId = userId;

        //    //FillBillingLines(milestone);
        //}

        /// <summary>
        /// Fill billing lines to store in database
        /// </summary>
        /// <param name="milestone">The milestone object</param>
        //private void FillBillingLines(MODEL.Milestone milestone)
        //{
        //    if (billingLinesToSave == null)
        //    {
        //        billingLinesToSave = new List<MilestoneBillingLine>();
        //    }

        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText1))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID1, LineSequance = 0, LineText = milestone.billingText1 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText2))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID2, LineSequance = 1, LineText = milestone.billingText2 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText3))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID3, LineSequance = 2, LineText = milestone.billingText3 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText4))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID4, LineSequance = 3, LineText = milestone.billingText4 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText5))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID5, LineSequance = 4, LineText = milestone.billingText5 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText6))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID6, LineSequance = 5, LineText = milestone.billingText6 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText7))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID7, LineSequance = 6, LineText = milestone.billingText7 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText8))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID8, LineSequance = 7, LineText = milestone.billingText8 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText9))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID9, LineSequance = 8, LineText = milestone.billingText9 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText10))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID10, LineSequance = 9, LineText = milestone.billingText10 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText11))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID11, LineSequance = 10, LineText = milestone.billingText11 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText12))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID12, LineSequance = 11, LineText = milestone.billingText12 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText13))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID13, LineSequance = 12, LineText = milestone.billingText13 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText14))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID14, LineSequance = 13, LineText = milestone.billingText14 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText15))
        //    //{
        //    billingLinesToSave.Add(new MilestoneBillingLine() { ID = milestone.billingTextID15, LineSequance = 14, LineText = milestone.billingText15 });
        //    //}
        //}
    }
}