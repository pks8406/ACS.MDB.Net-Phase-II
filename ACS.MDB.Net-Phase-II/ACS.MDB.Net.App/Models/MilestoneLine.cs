using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.MDB.Net.App.Models
{
    public class MilestoneLine : BaseModel
    {
        public Nullable<int> MilestoneID { get; set; }
        public string MilestoneType { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string QuoteNo { get; set; }
        public Nullable<decimal> QuoteCharge { get; set; }
        public Nullable<int> UpliftRate { get; set; }
        public Nullable<int> UpliftFixedRate { get; set; }
        public Nullable<int> UpliftID { get; set; }
        public Nullable<int> Uplift { get; set; }
        public Nullable<bool> QuoteRequired { get; set; }
        public Nullable<bool> QuoteApproval { get; set; }
        public Nullable<decimal> PreviousValue { get; set; }
        public Nullable<int> PreviousLine { get; set; }
        public Nullable<int> IndexRate { get; set; }
        public Nullable<int> CLPercentage { get; set; }

        public Milestone Milestone { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MilestoneLine()
        {

        }
    }
}