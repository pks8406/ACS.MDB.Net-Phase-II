using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class Product : BaseModel
    {
        /// <summary>
        /// Gets or Set Product Name
        /// </summary>
        [Required(ErrorMessage = "Please enter product name")]
        [Display(Name = " Product Name")]
        [RegularExpression("^([a-zA-Z0-9 &'-.]+)$", ErrorMessage = "Please enter valid product name")]
        [StringLength(50, ErrorMessage = "Product name must be maximum length of 50")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or set Product Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets ContractMaintenance List
        /// </summary>
        public List<ContractMaintenance> ContractMaintenances { get; set; }

        /// <summary>
        /// Gets Subproduct List
        /// </summary>
        public List<SubProduct> SubProductList { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public Product()
        {
            ContractMaintenances = new List<ContractMaintenance>();
            SubProductList = new List<SubProduct>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Product(ProductVO product)
        {
            ID = product.ProductId;
            ProductId = product.ProductId;
            ProductName = product.ProductName;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public ProductVO Transpose(int? userId)
        {
            ProductVO productVO = new ProductVO();

            productVO.ProductId = this.ID;
            productVO.ProductName = this.ProductName;
            productVO.CreatedByUserId = userId;
            productVO.LastUpdatedByUserId = userId;

            return productVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (ProductName != null && ProductName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
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
                ID, ProductName};
            return result;
        }
    }
}