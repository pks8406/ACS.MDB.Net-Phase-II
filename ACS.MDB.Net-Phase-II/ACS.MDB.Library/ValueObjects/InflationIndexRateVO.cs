using System;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class InflationIndexRateVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets inflation index rate id
        /// </summary>
        public int InflationIndexRateId { get; set; }

        /// <summary>
        /// Gets or Sets inflation index name
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Gets or Sets inflation index id
        /// </summary>
        public int InflationIndexId { get; set; }

        /// <summary>
        /// Gets or Sets charging uplift date
        /// </summary>
        public DateTime? chargingUpliftDate { get; set; }

        /// <summary>
        /// Gets or Sets inflation index rate 
        /// </summary>
        public decimal? IndexRate { get; set; }

        /// <summary>
        /// Gets or Sets inflation index rate (per annum)
        /// </summary>
        public decimal? IndexRatePerAnnum { get; set; }

        /// <summary>
        /// Get or set Proposed rate for charging uplift(% column)
        /// </summary>
        public decimal? ProposedRate { get; set; }

        public bool IndexUsed { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InflationIndexRateVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="chargingUplift">LINQ chargingUplift object</param>
        public InflationIndexRateVO(ChargingUplift chargingUplift)
        {
            IndexName = chargingUplift.UpliftIndex;
            InflationIndexId = chargingUplift.IndexId;
            InflationIndexRateId = chargingUplift.ID;
            chargingUpliftDate = chargingUplift.ChargingUpliftDate;
            IndexRate = chargingUplift.IndexRate;
            IndexRatePerAnnum = chargingUplift.ActualRate.HasValue ? chargingUplift.ActualRate * 100 : chargingUplift.ActualRate;
            CreatedByUserId = chargingUplift.CreatedBy;
            LastUpdatedByUserId = chargingUplift.LastUpdatedBy;
            IndexUsed = chargingUplift.ChargingIndex.IndexUsed;
        }

        /// <summary>
        /// Transpose model object to InflationIndexRate value object
        /// </summary>
        /// <param name="inflationIndexRate">InflationIndexRate model object</param>
        //public InflationIndexRateVO(InflationIndexRate inflationIndexRate, int? userId)
        //{
        //    IndexName = inflationIndexRate.IndexName;
        //    InflationIndexId = inflationIndexRate.InflationIndexId;
        //    InflationIndexRateId = inflationIndexRate.ID;
        //    chargingUpliftDate = inflationIndexRate.chargingUpliftDate;
        //    IndexRate = inflationIndexRate.IndexRate;
        //    IndexRatePerAnnum = inflationIndexRate.IndexRatePerAnnum.HasValue ? inflationIndexRate.IndexRatePerAnnum / 100 : inflationIndexRate.IndexRatePerAnnum;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}