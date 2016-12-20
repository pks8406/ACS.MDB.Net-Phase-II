using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class InvoiceCustomerDAL : BaseDAL
    {
        /// <summary>
        /// Get the invoice customer name from OAcustomer table
        /// </summary>
        /// <param name="id">invoice customer id</param>
        /// <returns>invoice customer name</returns>
        public string GetInvoiceCustomerNameByID(string id, int? companyId)
        {
            //TO DO:- Need to get end invoice customer name by company id
            OACustomer invoiceCustomer = mdbDataContext.OACustomers.Where(cust => cust.ID.Equals(id) && cust.CompanyID == companyId && cust.IsDeleted == false).SingleOrDefault();

            if (invoiceCustomer == null)
            {
                return string.Empty;
            }

            return invoiceCustomer.CustomerName;
        }

        /// <summary>
        /// Get the invoice customers by company id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>Hashtable having invoice customer id and name</returns>
        public List<InvoiceCustomerVO> GetInvoiceCustomersByCompanyIds(List<int> companyId)
        {
            List<InvoiceCustomerVO> invoiceCustomers = new List<InvoiceCustomerVO>();

            foreach (var id in companyId)
            {
                List<OACustomer> invoiceCustomerList = (mdbDataContext.OACustomers.Where(a => a.CompanyID == id && a.IsDeleted == false)).ToList();

                if (invoiceCustomerList != null)
                {
                    foreach (var item in invoiceCustomerList)
                    {
                        if (item.ID > 0)
                        {
                            invoiceCustomers.Add(new InvoiceCustomerVO(item));
                        }
                    }
                }
            }

            return invoiceCustomers.OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Get list of all invoice customer
        /// </summary>
        /// <returns>list of invoice customer</returns>
        public List<InvoiceCustomerVO> GetInvoiceCustomerList(int companyId)
        {
            List<OACustomer> invoiceCustomerList = mdbDataContext.OACustomers.Where(inv => inv.IsDeleted == false && inv.CompanyID == companyId).OrderBy(c => c.CustomerName).ToList();
            List<InvoiceCustomerVO> invoiceCustomerVOList = new List<InvoiceCustomerVO>();

            foreach (var item in invoiceCustomerList)
            {
                invoiceCustomerVOList.Add(new InvoiceCustomerVO(item));
            }

            return invoiceCustomerVOList;
        }

        /// <summary>
        /// Get currecny  based on invoice customer id
        /// </summary>
        /// <param name="invoiceCustomerId">invoice customer Id</param>
        /// <returns>invoice customer VO object</returns>
        public InvoiceCustomerVO GetCurrencyByCustomer(int invoiceCustomerId)
        {
            InvoiceCustomerVO invoiceCustomerVO = null;
            OACustomer invoiceCustomer = mdbDataContext.OACustomers.Where(x => x.ID == invoiceCustomerId).SingleOrDefault();

            if (invoiceCustomer != null)
            {
                invoiceCustomerVO = new InvoiceCustomerVO();
                invoiceCustomerVO.CurrencyId = invoiceCustomer.CurrencyID;
            }

            return invoiceCustomerVO;
        }       
    }
}