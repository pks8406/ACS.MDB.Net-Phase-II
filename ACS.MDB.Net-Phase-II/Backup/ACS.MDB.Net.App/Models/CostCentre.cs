using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class CostCentre : BaseModel
    {
        /// <summary>
        /// Gets or set CostCenterId
        /// </summary>        
        [Display(Name = " CostCentre Id")]
        public string OACostCenterId { get; set; }

        /// <summary>
        /// Gets or set Costcenter Name
        /// </summary>
        [Required(ErrorMessage = "Please enter costcentre name")]
        [Display(Name = " CostCentre Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or set Concatenated CostCenterName and OACostCenterId
        /// </summary>
        public string CostCenterName { get; set; }
        /// <summary>
        /// Gets or set Compnay ID
        /// </summary>
        [Display(Name = "Company ID")]
        public int? CompanyId { get; set; }
        //public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CostCentre()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CostCentre(CostCentreVO costcenterVO)
        {
            ID = costcenterVO.Id;
            OACostCenterId = costcenterVO.OACostCenterId;
            CompanyId = costcenterVO.CompanyId;
            Name = costcenterVO.Name;
            CostCenterName = costcenterVO.OACostCenterId + '-' + costcenterVO.Name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CostCentre(CostCentre costcenter)
        {
            ID = costcenter.ID;
            OACostCenterId = costcenter.OACostCenterId;
            Name = costcenter.Name;
            CostCenterName = costcenter.OACostCenterId + '-' + costcenter.Name;
            CompanyId = costcenter.CompanyId;
        }
    }
}