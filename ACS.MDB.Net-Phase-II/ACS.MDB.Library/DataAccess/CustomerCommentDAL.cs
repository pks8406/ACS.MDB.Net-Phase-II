using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class CustomerCommentDAL : BaseDAL
    {
        /// <summary>
        /// Gets the customer comment list
        /// </summary>
        /// <param name="companyVOList">companyVO list object</param>
        /// <returns>Customer comment list</returns>
        public List<CustomerCommentVO> GetCustomerCommentList(List<CompanyVO> companyVOList)
        {
            //List<CustomerComment> customerCommentList = mdbDataContext.CustomerComments.Where(c => c.IsDeleted == false).ToList();

            List<CustomerCommentVO> customerCommentVOList = new List<CustomerCommentVO>();

            foreach (var i in companyVOList)
            {
                List<CustomerComment> customerCommentList = mdbDataContext.CustomerComments.Where(c => c.CompanyID == i.CompanyID && c.IsDeleted == false).ToList();

                foreach (var item in customerCommentList)
                {
                    customerCommentVOList.Add(new CustomerCommentVO(item));
                }
            }

            return customerCommentVOList;
        }

        /// <summary>
        /// Gets customer comment details by Id
        /// </summary>
        /// <param name="customerCommentId">customerCommentId</param>
        /// <returns>Customer comment details</returns>
        public CustomerCommentVO GetCustomerCommentById(int customerCommentId)
        {
            CustomerComment customerComment = mdbDataContext.CustomerComments.SingleOrDefault(c => c.ID == customerCommentId);

            CustomerCommentVO customerCommentVO = null;
            if (customerComment != null)
            {
                customerCommentVO = new CustomerCommentVO(customerComment);
            }

            return customerCommentVO;
        }

        /// <summary>
        /// Save the customer comment
        /// </summary>
        /// <param name="customerCommentVO">Value object of customer comment</param>
        public void SaveCustomerComment(CustomerCommentVO customerCommentVO)
        {
            if (customerCommentVO.CustomerCommentId == 0)
            {
                //Insert New Record
                CustomerComment newCustomerComment = new CustomerComment();
                newCustomerComment.CustomerID = customerCommentVO.InvoiceCustomerId;
                newCustomerComment.OACustomer = mdbDataContext.OACustomers.Where(c => c.ID == customerCommentVO.InvoiceCustomerId).SingleOrDefault();
                newCustomerComment.CustomerName = newCustomerComment.OACustomer.CustomerName;

                //newCustomerComment.CustomerName = customerCommentVO.InvoiceCustomerName;
                newCustomerComment.CompanyID = customerCommentVO.CompanyId;
                newCustomerComment.Comment = customerCommentVO.CustomerComment;
                newCustomerComment.Group = customerCommentVO.Group;
                newCustomerComment.CreationDate = DateTime.Now;
                newCustomerComment.CreatedBy = customerCommentVO.CreatedByUserId;
                mdbDataContext.CustomerComments.InsertOnSubmit(newCustomerComment);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                CustomerComment selectedCustomerComment = mdbDataContext.CustomerComments.SingleOrDefault(c => c.ID == customerCommentVO.CustomerCommentId);
                selectedCustomerComment.CustomerID = customerCommentVO.InvoiceCustomerId;
                selectedCustomerComment.OACustomer = mdbDataContext.OACustomers.Where(c => c.ID == customerCommentVO.InvoiceCustomerId).SingleOrDefault();
                selectedCustomerComment.CustomerName = selectedCustomerComment.OACustomer.CustomerName;
                //selectedCustomerComment.CustomerName = customerCommentVO.InvoiceCustomerName;
                selectedCustomerComment.CompanyID = customerCommentVO.CompanyId;
                selectedCustomerComment.Comment = customerCommentVO.CustomerComment;
                selectedCustomerComment.Group = customerCommentVO.Group;
                selectedCustomerComment.LastUpdatedDate = DateTime.Now;
                selectedCustomerComment.LastUpdatedBy = customerCommentVO.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Delete the Customer comment(s)
        /// </summary>
        /// <param name="Ids">Ids of customercomments to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteCustomerComment(List<int> Ids, int? userId)
        {
            CustomerComment deleteCustomerComment = new CustomerComment();

            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    deleteCustomerComment = mdbDataContext.CustomerComments.Where(c => c.ID == id).SingleOrDefault();

                    //To check weather Invoice Customer is associated with contrat or not
                    Contract contract = mdbDataContext.Contracts.Where(c => c.InvoiceCustomerID == deleteCustomerComment.CustomerID && c.CompanyID == deleteCustomerComment.CompanyID && !c.IsDeleted).FirstOrDefault();

                    if (contract == null)
                    {
                        deleteCustomerComment.IsDeleted = true;
                        deleteCustomerComment.LastUpdatedBy = userId;
                        deleteCustomerComment.LastUpdatedDate = DateTime.Now;
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Gets the customer comment
        /// </summary>
        /// <param name="comment">comment</param>
        /// <param name="companyId">company Id</param>
        /// <param name="customerId">customer Id</param>
        /// <returns>customer comment</returns>
        public CustomerCommentVO GetCustomerCommentByName(string comment, int companyId, int customerId)
        {
            CustomerComment customercomment = null;

            if (!string.IsNullOrEmpty(comment))
            {
                customercomment = mdbDataContext.CustomerComments.Where(c => c.Comment.Equals(comment) && c.CompanyID == companyId && c.CustomerID == customerId && c.IsDeleted == false).SingleOrDefault();
            }
            else
            {
                customercomment = mdbDataContext.CustomerComments.Where(c => c.CompanyID == companyId && c.CustomerID == customerId && c.IsDeleted == false).SingleOrDefault();
            }

            CustomerCommentVO customerCommentVO = null;
            if (customercomment != null)
            {
                customerCommentVO = new CustomerCommentVO(customercomment);
            }

            return customerCommentVO;
        }

        /// <summary>
        /// Get check whether customer is grouped or not
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="customerId">The customer id</param>
        /// <returns>value object of customer comment</returns>
        public CustomerCommentVO GetCustomerCommentByCompanyIdAndCustomerId(int companyId, int customerId)
        {
            CustomerComment customerComment = mdbDataContext.CustomerComments.FirstOrDefault(c => c.CompanyID == companyId
                                                                                                   && c.CustomerID == customerId
                                                                                                   && !c.IsDeleted);
            CustomerCommentVO customerCommentVO = customerComment != null ? new CustomerCommentVO(customerComment) : new CustomerCommentVO();
            return customerCommentVO;
        }
    }
}