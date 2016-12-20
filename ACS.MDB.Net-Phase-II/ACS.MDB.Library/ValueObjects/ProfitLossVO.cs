
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ProfitLossVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets profitloss id
        /// </summary>
        public int ProfitLossId { get; set; }

        /// <summary>
        /// Gets or Sets company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or Sets company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or Sets cost center id
        /// </summary>
        public int CostCentreId { get; set; }

        /// <summary>
        /// Gets or Sets cost center name
        /// </summary>
        public string CostCenterName { get; set; }

        public string OACostcenterId { get; set; }

        /// <summary>
        /// Gets or Sets profit loss name
        /// </summary>
        public string ProfitLossName { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProfitLossVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="profitloss">LINQ profitloss object</param>
        public ProfitLossVO(P_LMapping profitloss)
        {
            ProfitLossId = profitloss.ID;
            CompanyId = profitloss.CompanyID;
            CostCentreId = profitloss.CostCentreID;
            OACostcenterId = profitloss.OACostCentre.CostCentreID;
            CostCenterName = profitloss.OACostCentre.CostCentreName;
            ProfitLossName = profitloss.P_L;
            CreatedByUserId = profitloss.CreatedBy;
            LastUpdatedByUserId = profitloss.LastUpdatedBy;
        }

        /// <summary>
        /// Transpose model object to P&L value object
        /// </summary>
        /// <param name="profitloss">model objet</param>
        //public ProfitLossVO(ProfitLoss profitloss, int? userId)
        //{
        //    ProfitLossId = profitloss.ID;
        //    CompanyId = profitloss.CompanyId;
        //    CostCentreId = profitloss.CostCenterId;
        //    OACostcenterId = profitloss.OACostcenterId;
        //    CostCenterName = OACostcenterId + '-' + profitloss.CostCenterName;
        //    ProfitLossName = profitloss.ProfitLossName;
        //    CompanyName = profitloss.CompanyName;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}