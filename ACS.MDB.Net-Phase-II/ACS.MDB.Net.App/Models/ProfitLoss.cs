using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ProfitLoss : BaseModel
    {
        /// <summary>
        /// Gets or set Compnay ID
        /// </summary>
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set Company Name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set CostCenter ID
        /// </summary>
        [Required(ErrorMessage = "Please select cost centre")]
        public int CostCenterId { get; set; }

        /// <summary>
        /// Gets or set CostCenter Name
        /// </summary>
        [Display(Name = "Cost Centre")]
        public string CostCenterName { get; set; }

        public string OACostcenterId { get; set; }

        /// <summary>
        /// Gets or set P&L name
        /// </summary>
        [Required(ErrorMessage = "Please enter P & L")]
        [Display(Name = "P & L")]
        [RegularExpression("^([a-zA-Z0-9 &'-]+)$", ErrorMessage = "Please enter valid P & L")]
        [StringLength(50, ErrorMessage = "P & L must be maximum length of 50")]
        public string ProfitLossName { get; set; }


        /// <summary>
        /// Gets the list of CostCenter
        /// </summary>        
        public List<CostCentre> costcenterList { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProfitLoss()
        {
            costcenterList = new List<CostCentre>();

        }

        /// <summary>
        /// Transpose profitloss value object to model object
        /// </summary>
        public ProfitLoss(ProfitLossVO profitlossVO)
        {
            ID = profitlossVO.ProfitLossId;
            CompanyId = profitlossVO.CompanyId;
            CostCenterId = profitlossVO.CostCentreId;
            OACostcenterId = profitlossVO.OACostcenterId;
            CostCenterName = OACostcenterId + '-' + profitlossVO.CostCenterName;
            ProfitLossName = profitlossVO.ProfitLossName;
            CompanyName = profitlossVO.CompanyName;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public ProfitLossVO Transpose(int? userId)
        {
            ProfitLossVO profitLossVO = new ProfitLossVO();

            profitLossVO.ProfitLossId = this.ID;
            profitLossVO.CompanyId = this.CompanyId;
            profitLossVO.CostCentreId = this.CostCenterId;
            profitLossVO.OACostcenterId = this.OACostcenterId;
            profitLossVO.CostCenterName = this.OACostcenterId + '-' + this.CostCenterName;
            profitLossVO.ProfitLossName = this.ProfitLossName;
            profitLossVO.CompanyName = this.CompanyName;
            profitLossVO.CreatedByUserId = userId;
            profitLossVO.LastUpdatedByUserId = userId;

            return profitLossVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (ProfitLossName != null && ProfitLossName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                    (CostCenterName != null && CostCenterName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { "<input type='checkbox' name='check5' value='" + ID + "'>",
                ID, CostCenterName, ProfitLossName};
            return result;
        }
    }
}