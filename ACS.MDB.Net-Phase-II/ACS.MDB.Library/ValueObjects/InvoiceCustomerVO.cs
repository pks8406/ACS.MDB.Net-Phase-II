

using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class InvoiceCustomerVO : BaseVO
    {
        /// <summary>
        /// Gets or set invoice customer id
        /// </summary>
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// Gets or set old customer id(Account code for customer)
        /// </summary>
        public string OACustomerId { get; set; }

        /// <summary>
        /// Gets or set associated company id with customer
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set customer name + customer id(Account code)
        /// </summary>
        public string CustomerandOACustomerId { get; set; }

        /// <summary>
        /// Gets or set associated currency id with customer
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        /// Gets or set Customer short name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or set customer name with customer name + code + short name
        /// </summary>
        public string CustomerNameCodeAndShortName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public InvoiceCustomerVO()
        {
        }

        /// <summary>
        /// Transpose Model object to value object
        /// </summary>
        /// <param name="endUser"></param>
        //public InvoiceCustomerVO(MODEL.InvoiceCustomer invoiceCustomer)
        //{
        //    InvoiceCustomerId = invoiceCustomer.ID;
        //    OACustomerId = invoiceCustomer.OACustomerId;
        //    CompanyId = invoiceCustomer.CompanyId;
        //    CompanyName = invoiceCustomer.CompanyName;
        //    Name = invoiceCustomer.Name;
        //    CurrencyId = invoiceCustomer.CurrencyId;
        //    ShortName = invoiceCustomer.ShortName;
        //    CustomerandOACustomerId = Name + " - " + OACustomerId;
        //    CustomerNameCodeAndShortName = Name + " - " + OACustomerId + " - " + ShortName;
        //}

        /// <summary>
        /// Transpose LINQ object to value object
        /// </summary>
        /// <param name="endUser"></param>
        public InvoiceCustomerVO(OACustomer invoiceCustomer)
        {
            InvoiceCustomerId = invoiceCustomer.ID;
            OACustomerId = invoiceCustomer.CustomerID;
            CompanyId = invoiceCustomer.CompanyID;
            CompanyName = invoiceCustomer.OACompany.CompanyName;
            Name = invoiceCustomer.CustomerName;
            CurrencyId = invoiceCustomer.CurrencyID;
            CustomerandOACustomerId = Name + " - " + OACustomerId;
            ShortName = invoiceCustomer.ShortName;
            CustomerNameCodeAndShortName = Name + " - " + OACustomerId + " - " + ShortName;
        }
    }
}