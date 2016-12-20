using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class Currency : BaseModel
    {
        /// <summary>
        /// Get or set currency name
        /// </summary>        
        [Required(ErrorMessage = "Please enter currency name")]
        [Display(Name = "Currency Name")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "Please enter valid currency")]
        [StringLength(4, ErrorMessage = "Currency name must be with a maximum length of 4")]
        public string CurrencyName { get; set; }

        /// <summary>
        /// Get or set currency object
        /// </summary>
        [Required(ErrorMessage = "Please enter description")]
        [StringLength(25, ErrorMessage = "Description must be with a maximum length of 25")]
        [RegularExpression("^([a-zA-Z0-9 &'\r\n]*)$", ErrorMessage = "Please enter valid description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Get or set currency exchange rates
        /// </summary>
        [Required(ErrorMessage = "Please enter exchange rate")]
        [RegularExpression(@"^\d{0,14}(\.\d{0,4})?$", ErrorMessage = "Please enter valid exchange rate")]
        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [Required()]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Default construcor
        /// </summary>
        public Currency()
        {            
        }

        /// <summary>
        /// Transpose currency value object to model object
        /// </summary>
        /// <param name="currency">Currency value objet</param>
        public Currency(CurrencyVO currency)
        {
            ID = currency.CurrencyID;
            CurrencyId = currency.CurrencyID;
            CurrencyName = currency.CurrencyName;
            Description = currency.Description;
            ExchangeRate = currency.ExchangeRate;
            IsActive = currency.IsActive;
        }

        /// <summary>
        /// Transposer the model object to value object
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public CurrencyVO Transpose(int? userId)
        {
            CurrencyVO currencyVO = new CurrencyVO();

            currencyVO.CurrencyID = this.ID;
            currencyVO.CurrencyName = this.CurrencyName;
            currencyVO.Description = this.Description.Trim().Replace("\r\n", "\n");
            currencyVO.ExchangeRate = this.ExchangeRate;
            currencyVO.IsActive = this.IsActive;
            currencyVO.CreatedByUserId = userId;
            currencyVO.LastUpdatedByUserId = userId;

            return currencyVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            String status = IsActive ? "Active" : "Inactive";

            return (CurrencyName != null && CurrencyName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (Description != null && Description.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
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
                CurrencyName, 
                Description, 
                String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE_UPTO_FOUR_DECIMAL,ExchangeRate), 
                IsActive ? "Active" : "Inactive" };
            return result;
        }
    }
}