using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class CustomerCommentService
    {
        CustomerCommentDAL customerCommentDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerCommentService()
        {
            customerCommentDAL = new CustomerCommentDAL();
        }

        /// <summary>
        /// Gets the customer comment list
        /// </summary>
        /// <param name="companyVOList">companyVO list object</param>
        /// <returns>Customer comment list</returns>
        public List<CustomerCommentVO> GetCustomerCommentList(List<CompanyVO> companyVOList)
        {
            return customerCommentDAL.GetCustomerCommentList(companyVOList);
        }

        /// <summary>
        /// Save the customerComment
        /// </summary>
        /// <param name="customerCommentVO">Value object of customer comment</param>
        public void SaveCustomerComment(CustomerCommentVO customerCommentVO)
        {
            CustomerCommentVO isCustomerCommentExist = null;
            isCustomerCommentExist = customerCommentDAL.GetCustomerCommentByName(String.Empty, customerCommentVO.CompanyId, customerCommentVO.InvoiceCustomerId);
            if (isCustomerCommentExist != null && customerCommentVO.CustomerCommentId != isCustomerCommentExist.CustomerCommentId)
            {
                throw new ApplicationException(String.Format(Constants.INVOICECUSTOMER_ALREADY_HAVE_COMMENT, customerCommentVO.InvoiceCustomerName));
            }

            else
            {
                customerCommentDAL.SaveCustomerComment(customerCommentVO);
            }

            //if (!string.IsNullOrEmpty(customerCommentVO.CustomerComment))
            //{
            //     isCustomerCommentExist = customerCommentDAL.GetCustomerCommentByName(customerCommentVO.CustomerComment, customerCommentVO.CompanyId, customerCommentVO.InvoiceCustomerId);

            //    //Check whether customercomment already exist or not
            //    if (isCustomerCommentExist != null && customerCommentVO.CustomerCommentId != isCustomerCommentExist.CustomerCommentId)
            //    {
            //        throw new ApplicationException(Constants.CUSTOMERCOMMENT_ALREADY_EXIST);
            //    }
            //    else
            //    {
            //        customerCommentDAL.SaveCustomerComment(customerCommentVO);
            //    }
            //}
        }

        /// <summary>
        /// Gets customer comment details by Id
        /// </summary>
        /// <param name="customerCommentId">customerCommentId</param>
        /// <returns>Customer comment details</returns>
        public CustomerCommentVO GetCustomerCommentById(int customerCommentId)
        {
            return customerCommentDAL.GetCustomerCommentById(customerCommentId);
        }

        /// <summary>
        /// Gets customer comment details by companyId and cutomer id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="customerId">The customer id</param>
        /// <returns>Customer comment details</returns>
        public CustomerCommentVO GetCustomerCommentByCompanyAndCutomer(int companyId, int customerId)
        {
            return customerCommentDAL.GetCustomerCommentByName(string.Empty, companyId, customerId);
        }

        /// <summary>
        /// Delete the CustomerComment(s)
        /// </summary>
        /// <param name="Ids">Ids of customerComment to be deleted</param>
        public void DeleteCustomerComment(List<int> Ids, int? userId)
        {
            customerCommentDAL.DeleteCustomerComment(Ids, userId);
        }
    }
}