using System;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class MilestoneBillingLine : BaseModel
    {
        private MilestoneBillingLineVO milestoneBillingLineVo;

        /// <summary>
        /// Gets or sets billing line id
        /// </summary>
        public int BillingLineID { get; set; }

        /// <summary>
        /// Gets or sets contract id
        /// </summary>
        public Nullable<int> ContractID { get; set; }

        /// <summary>
        /// Gets or sets milestone id
        /// </summary>
        public Nullable<int> MilestoneID { get; set; }

        /// <summary>
        /// Gets or sets description id
        /// </summary>
        public Nullable<int> DescriptionID { get; set; }

        /// <summary>
        /// Gets or sets line sequence id
        /// </summary>
        public Nullable<int> LineSequance { get; set; }

        /// <summary>
        /// Gets or sets line text
        /// </summary>
        public string LineText { get; set; }

        /// <summary>
        /// Gets or set is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        //public Nullable<int> DatabaseID { get; set; }

        ///// <summary>
        ///// Gets or sets contract
        ///// </summary>
        //public Contract Contract { get; set; }

        ///// <summary>
        ///// Gets or sets milestone
        ///// </summary>
        //public Milestone Milestone { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MilestoneBillingLine()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="billingLine">LINQ object</param>
        //public MilestoneBillingLine(ACS.MDB.Net.App.DataAccess.LINQ.MilestoneBillingLine billingLine)
        //{
        //    BillingLineID = billingLine.ID;
        //    ContractID = billingLine.ContractID;
        //    MilestoneID = billingLine.MilestoneID;
        //    LineText = billingLine.LineText;
        //    DescriptionID = billingLine.DescriptionID;
        //    LineSequance = billingLine.LineSequance;
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="billingLine">Model object</param>
        public MilestoneBillingLine(MilestoneBillingLine billingLine)
        {
            BillingLineID = billingLine.ID;
            ContractID = billingLine.ContractID;
            MilestoneID = billingLine.MilestoneID;
            LineText = billingLine.LineText;
            DescriptionID = billingLine.DescriptionID;
            LineSequance = billingLine.LineSequance;
        }

        /// <summary>
        /// Convert Value Object to MODEL object
        /// </summary>
        /// <param name="milestoneBillingLineVo"></param>
        public MilestoneBillingLine(MilestoneBillingLineVO milestoneBillingLineVo)
        {
            BillingLineID = milestoneBillingLineVo.ID;
            ContractID = milestoneBillingLineVo.ContractId;
            MilestoneID = milestoneBillingLineVo.MilestoneId;
            LineText = milestoneBillingLineVo.LineText;
            LineSequance = milestoneBillingLineVo.LineSequance;
        }
    }
}