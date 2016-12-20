
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class InflationIndexVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets inflation index id
        /// </summary>
        public int InflationIndexId { get; set; }

        /// <summary>
        /// Gets or Sets inflation index name
        /// </summary>
        public string InflationIndexName { get; set; }

        /// <summary>
        /// Gets or Sets inflation index description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets inflation use index
        /// </summary>
        public bool UseIndex { get; set; }

        /// <summary>
        /// Gets or set concatenated InflationIndexName and Description
        /// </summary>
        public string InflationIndexNameDesc { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InflationIndexVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="ChargingIndex">LINQ inflation index object</param>
        public InflationIndexVO(ChargingIndex inflationIndex)
        {
            InflationIndexId = inflationIndex.ID;
            InflationIndexName = inflationIndex.ChargingIndex1;
            Description = inflationIndex.Description;
            UseIndex = inflationIndex.IndexUsed;
            CreatedByUserId = inflationIndex.CreatedBy;
            LastUpdatedByUserId = inflationIndex.LastUpdatedBy;
            InflationIndexNameDesc = inflationIndex.Description + "-" + inflationIndex.ChargingIndex1;  
        }

        /// <summary>
        /// Transpose model object to Product value object
        /// </summary>
        /// <param name="InflationIndex"> model object</param>
        //public InflationIndexVO(InflationIndex inflationIndex, int? userId)
        //{
        //    InflationIndexId = inflationIndex.ID;
        //    InflationIndexName = inflationIndex.IndexName;
        //    Description = inflationIndex.Description;
        //    UseIndex = inflationIndex.UseIndex;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //    InflationIndexNameDesc = inflationIndex.Description + "-" + inflationIndex.IndexName;
        //}
    }
}