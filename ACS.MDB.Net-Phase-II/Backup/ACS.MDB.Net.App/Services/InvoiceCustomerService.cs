using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class InvoiceCustomerService
    {
        InvoiceCustomerDAL invoiceCustomerDAL = null;

        public InvoiceCustomerService()
        {
            invoiceCustomerDAL = new InvoiceCustomerDAL();
        }
        /// <summary>
        /// Gets invoice customer name by customer id and associated company id
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <param name="companyId">The company id</param>
        /// <returns>The customer name</returns>
        public string GetInvoiceCustomerNameByID(string id, int? companyId)
        {            
            return invoiceCustomerDAL.GetInvoiceCustomerNameByID(id, companyId);
        }

        /// <summary>
        /// Get the invoice customers by company id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>Hashtable having invoice customer id and name</returns>
        public List<InvoiceCustomerVO> GetInvoiceCustomersByCompanyIds(List<int> companyIds)
        {            
            return invoiceCustomerDAL.GetInvoiceCustomersByCompanyIds(companyIds);
        }

        /// <summary>
        ///  Get list of all invoice customer 
        /// </summary>
        /// <param name="companyId">The company id for which to get all customers</param>
        /// <returns>List of invoice customers</returns>
        public List<InvoiceCustomerVO> GetInvoiceCustomerList(int companyId)
        {            
            return invoiceCustomerDAL.GetInvoiceCustomerList(companyId);
        }

        /// <summary>
        /// Get currecny  based on invoice customer id
        /// </summary>
        /// <param name="invoiceCustomerId">invoice customer Id</param>
        /// <returns>invoice customer VO object</returns>
        public InvoiceCustomerVO GetCurrencyByCustomer(int invoiceCustomerId)
        {
            return invoiceCustomerDAL.GetCurrencyByCustomer(invoiceCustomerId);
        }
    }
}