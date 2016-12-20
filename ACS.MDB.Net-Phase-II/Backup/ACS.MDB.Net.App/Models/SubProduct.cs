using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class SubProduct : BaseModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SubProduct()
        {
            ContractMaintenances = new List<ContractMaintenance>();
        }

        /// <summary>
        /// Gets or set Product Id
        /// </summary>
        [Required()]
        public int ProductId { get; set; }
        /// <summary>
        /// Gets or set Product Version
        /// </summary>
        [Required(ErrorMessage = "Please enter sub product name")]
        [Display(Name = " Sub Product Name")]
        [RegularExpression("^([a-zA-Z0-9 &'-./]+)$", ErrorMessage = "Please enter valid sub product name")]        
        [StringLength(25, ErrorMessage = "SubProduct name must be maximum length of 25")]
        public string Version { get; set; }
        /// <summary>
        /// Gets or set Activity
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// Gets or Set Product Name
        /// </summary>
        [Display(Name = " Product Name")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets ContractMaintenance List
        /// </summary>
        public List<ContractMaintenance> ContractMaintenances { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subProduct">The SubProductVO</param>
        public SubProduct(SubProductVO subProduct)
        {
            ID = subProduct.SubProductId;
            ProductId = subProduct.ProductId;
            ProductName = subProduct.ProductName;
            Version = subProduct.Version;
            Activity = subProduct.Activity;
        }

         /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public SubProductVO Transpose(int? userId)
        {
            SubProductVO subProductVO = new SubProductVO();

            subProductVO.SubProductId = this.ID;
            subProductVO.ProductId = this.ProductId;
            subProductVO.Version = this.Version;
            subProductVO.Activity = this.Activity;
            subProductVO.ProductName = this.ProductName;
            subProductVO.CreatedByUserId = userId;
            subProductVO.LastUpdatedByUserId = userId;

            return subProductVO;
        }


        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (Version != null && Version.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
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
                ID, Version };
            return result;
        }
    }
}