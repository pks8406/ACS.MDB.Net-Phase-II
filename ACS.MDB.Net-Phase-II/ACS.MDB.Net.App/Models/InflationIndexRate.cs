using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class InflationIndexRate : BaseModel
    {
        /// <summary>
        /// Gets or set infaltion index rate id
        /// </summary>
        [Display(Name = "Index Id")]
        public int InflationIndexRateId { get; set; }

        /// <summary>
        /// Gets and set changing uplift date
        /// </summary>
        [Required(ErrorMessage = "Please enter date")]
        [Display(Name = "Date")]
        public DateTime? chargingUpliftDate { get; set; }


        /// <summary>
        /// Gets and set index rate per annum
        /// </summary>
        [Display(Name = "Index Rate")]        
        [RegularExpression(@"^\d{0,4}(\.\d{0,2})?$", ErrorMessage = "Please enter valid Index rate")]
        public Decimal? IndexRate { get; set; }

        /// <summary>
        /// Gets and set index rate per annum
        /// </summary>
        [Display(Name = "Rate(per annum)")]
        [RegularExpression(@"^[+-]?\d{0,2}(\.\d{0,2})?$", ErrorMessage = "Please enter valid rate per annum")]
        public Decimal? IndexRatePerAnnum { get; set; }

        /// <summary>
        /// Gets or Sets inflation index id
        /// </summary>
        public int InflationIndexId { get; set; }

        /// <summary>
        /// Gets or Sets inflation index name
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public InflationIndexRate()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inflationIndexRateVO"></param>
        public InflationIndexRate(InflationIndexRateVO inflationIndexRateVO)
        {
            ID = inflationIndexRateVO.InflationIndexRateId;
            InflationIndexId = inflationIndexRateVO.InflationIndexId;
            IndexName = inflationIndexRateVO.IndexName;
            chargingUpliftDate = inflationIndexRateVO.chargingUpliftDate.HasValue ? inflationIndexRateVO.chargingUpliftDate : null;
            IndexRate = inflationIndexRateVO.IndexRate;
            IndexRatePerAnnum = inflationIndexRateVO.IndexRatePerAnnum;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public InflationIndexRateVO Transpose(int? userId)
        {
            InflationIndexRateVO inflationIndexRateVO = new InflationIndexRateVO();

            inflationIndexRateVO.InflationIndexRateId = this.ID;
            inflationIndexRateVO.InflationIndexId = this.InflationIndexId;
            inflationIndexRateVO.IndexName = this.IndexName;
            inflationIndexRateVO.chargingUpliftDate = this.chargingUpliftDate;
            inflationIndexRateVO.IndexRate = this.IndexRate;
            inflationIndexRateVO.IndexRatePerAnnum = this.IndexRatePerAnnum.HasValue ? this.IndexRatePerAnnum / 100 : this.IndexRatePerAnnum;
            inflationIndexRateVO.CreatedByUserId = userId;
            inflationIndexRateVO.LastUpdatedByUserId = userId;

            return inflationIndexRateVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (chargingUpliftDate != null && chargingUpliftDate.Value.ToShortDateString().Contains(str));
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
                ID, 
                chargingUpliftDate.HasValue ? chargingUpliftDate.Value.Date.ToString(Constants.DATE_FORMAT) : null, 
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, IndexRate),   
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, IndexRatePerAnnum) };
            return result;
        }

    }
}