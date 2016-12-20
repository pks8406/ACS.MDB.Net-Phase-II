using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class InvoiceCustomer : BaseModel
    {
        //public int ID { get; set; }
        /// <summary>
        /// Gets or set old customer Id
        /// </summary>
        public string  OACustomerId { get; set; }
        /// <summary>
        /// Gets or set Company Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set Company Name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set Customer name
        /// </summary>
        [Display(Name = "Invoice Customer")]
        public string Name { get; set; }

        /// <summary>
        /// Gets and set concatenated Customer name and Old Customer Id
        /// </summary>
        public string  CustomerandOACustomerId { get; set; }

        /// <summary>
        /// Gets or set currency Id
        /// </summary>
        public string  CurrencyId { get; set; }

        /// <summary>
        /// Gets or set Customer short name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or set customer name with customer name + code + short name
        /// </summary>
        public string CustomerNameCodeAndShortName { get; set; }

        /// <summary>
        /// Gets or set combination of customer name and short name - for contract grid
        /// </summary>
        public string CustomerNameAndShortName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public InvoiceCustomer()
        { 
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invoiceCustomerVO"></param>
        public InvoiceCustomer(InvoiceCustomerVO invoiceCustomerVO)
        {
            ID = invoiceCustomerVO.InvoiceCustomerId;
            OACustomerId = invoiceCustomerVO.OACustomerId;
            CompanyId = invoiceCustomerVO.CompanyId;
            CompanyName = invoiceCustomerVO.CompanyName;
            Name = invoiceCustomerVO.Name;
            CurrencyId = invoiceCustomerVO.CurrencyId;
            CustomerandOACustomerId = invoiceCustomerVO.CustomerandOACustomerId;
            ShortName = invoiceCustomerVO.ShortName;
            CustomerNameAndShortName = Name + " - " + ShortName;
            CustomerNameCodeAndShortName = Name + " - " + OACustomerId + " - " + ShortName;
        }
    }
}