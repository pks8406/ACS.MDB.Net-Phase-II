
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class CostCentreVO
    {
        /// <summary>
        /// Gets or Sets cost center id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Set Cost Center Id
        /// </summary>
        public string OACostCenterId { get; set; }

        /// <summary>
        /// Gets or Sets cost center name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set Concatenated CostCenterName and OACostCenterName
        /// </summary>
        public string CostCenterName { get; set; }

        /// <summary>
        /// Gets or Sets company id
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CostCentreVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="costcentre">LINQ costcenter object</param>
        public CostCentreVO(OACostCentre costcentre)
        {
            Id = costcentre.ID;
            OACostCenterId = costcentre.CostCentreID;
            Name = costcentre.CostCentreName;
            CostCenterName = costcentre.CostCentreID + '-' + costcentre.CostCentreName;
            CompanyId = costcentre.CompanyID;
        }

        /// <summary>
        /// Transpose model object to CostCentre value object
        /// </summary>
        /// <param name="costcentre">model object</param>
        //public CostCentreVO(CostCentre costcentre)
        //{
        //    Id = costcentre.ID;
        //    OACostCenterId = costcentre.OACostCenterId;
        //    Name = costcentre.Name;
        //    CostCenterName = costcentre.OACostCenterId + '-' + costcentre.Name;
        //    CompanyId = costcentre.CompanyId;
        //}
    }
}