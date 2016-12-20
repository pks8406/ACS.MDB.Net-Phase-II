
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class MilestoneBillingLineVO
    {
        /// <summary>
        /// Gets or set milestone id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or set associated contract id
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or set milestine id
        /// </summary>
        public int MilestoneId { get; set; }

        /// <summary>
        /// Gets or set line sequance for milestone lines
        /// </summary>
        public int LineSequance { get; set; }

        /// <summary>
        /// Gets or set billing line 
        /// </summary>
        public string LineText { get; set; }

        /// <summary>
        /// Gets or set milestone billing line deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public MilestoneBillingLineVO()
        {
                
        }

        /// <summary>
        /// Constructor converts LINQ object to Value Object
        /// </summary>
        /// <param name="milestoneBillingLine"></param>
        public MilestoneBillingLineVO(MilestoneBillingLine milestoneBillingLine)
        {
            ID = milestoneBillingLine.ID;
            MilestoneId = milestoneBillingLine.MilestoneID;
            ContractId = milestoneBillingLine.ContractID;
            LineSequance = milestoneBillingLine.LineSequance.Value;
            LineText = milestoneBillingLine.LineText;
        }
    }
}