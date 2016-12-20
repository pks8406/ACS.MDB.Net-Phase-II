using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class InflationIndex : BaseModel
    {
        /// <summary>
        /// Gets or set index name
        /// </summary>
        [Required(ErrorMessage = "Please enter index name")]
        [RegularExpression("^([a-zA-Z0-9 &'-]+)$", ErrorMessage = "Please enter valid index name")]
        [Display(Name = "Index Name")]
        public string IndexName { get; set; }

        /// <summary>
        /// Gets or set index description
        /// </summary>
        [Required(ErrorMessage = "Please enter description")]
        [Display(Name = "Description")]
        [RegularExpression(@"^([a-zA-Z0-9 ""&'-./%\r\n]+)$", ErrorMessage = "Please enter valid description")]
        [StringLength(30, ErrorMessage = "Description must be with a maximum length of 30")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets use index.
        /// If the index should be calculated by rate or percentage. If True then calculate by percentage else by rate.
        /// </summary>
        [Display(Name = "Use Index")]
        public bool UseIndex { get; set; }

        /// <summary>
        /// Gets or Sets inflation index id
        /// </summary>
        [Required()]
        public int InflationIndexId { get; set; }

        /// <summary>
        /// Gets inflation index rate List
        /// </summary>
        public List<InflationIndexRate> inflationIndexRateList { get; set; }

        /// <summary>
        /// Gets or set concatenated InflationIndexName and Description
        /// </summary>
        public string InflationIndexNameDesc { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InflationIndex()
        {
            inflationIndexRateList = new List<InflationIndexRate>();
        }
      
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InflationIndexVO">InflationIndexVO model object</param>
        public InflationIndex(InflationIndexVO inflationIndexVO)
        {
            ID = inflationIndexVO.InflationIndexId;
            InflationIndexId = inflationIndexVO.InflationIndexId;
            IndexName = inflationIndexVO.InflationIndexName;
            Description = inflationIndexVO.Description;
            UseIndex = inflationIndexVO.UseIndex;
            InflationIndexNameDesc = inflationIndexVO.Description + "-" + inflationIndexVO.InflationIndexName;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public InflationIndexVO Transpose(int? userId)
        {
            InflationIndexVO inflationIndexVO = new InflationIndexVO();

            inflationIndexVO.InflationIndexId = this.ID;
            inflationIndexVO.InflationIndexName = this.IndexName;
            inflationIndexVO.Description = this.Description;
            inflationIndexVO.UseIndex = this.UseIndex;
            inflationIndexVO.InflationIndexNameDesc = this.Description + "-" + this.IndexName;
            inflationIndexVO.CreatedByUserId = userId;
            inflationIndexVO.LastUpdatedByUserId = userId;

            return inflationIndexVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (IndexName != null && IndexName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { "<input type='checkbox' name='check1' value='" + ID + "'>",
                ID, IndexName, Description,  UseIndex ? "Yes" : "No"};
            return result;
        }
    }
}