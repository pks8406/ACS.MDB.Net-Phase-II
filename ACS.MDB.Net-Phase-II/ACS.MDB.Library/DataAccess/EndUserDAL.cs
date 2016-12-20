using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class EndUserDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of End User based on company Id and customer Id
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="customerId">customer Id</param>
        /// <returns>List of Endusers</returns>
        public List<EndUserVO> GetEndUserListByCompanyIdandCustomerId(int companyId, int customerId)
        {
            List<EndUser> endUserList = mdbDataContext.EndUsers.Where(c => c.BusinessPartner.CompanyID == companyId && c.BusinessPartner.InvoiceCustomerID == customerId && c.IsDeleted == false).ToList();

            List<EndUserVO> endUserVOList = new List<EndUserVO>();

            foreach (var item in endUserList)
            {
                endUserVOList.Add(new EndUserVO(item));
            }

            return endUserVOList;

            //foreach (var item in invoiceCustomerList)
            //{
            //    EndUserVO endUserVO = new EndUserVO
            //    {
            //        ID = item.ID,
            //        Name = item.CustomerName,
            //        CompanyID = item.CompanyID,
            //    };

            //    endUserVOList.Add(endUserVO);
            //}

            ////Search end user name in enduser table
            //List<EndUser> endUserList = mdbDataContext.EndUsers.Where(e => e.IsDeleted == false && e.BusinessPartner.CompanyID == companyId).ToList();
            //foreach (var item in endUserList)
            //{
            //    endUserVOList.Add(new EndUserVO(item));
            //}

            //return endUserVOList;
        }

        /// <summary>
        /// Get Enduser details by Id
        /// </summary>
        /// <param name="endUserId">EndUser Id</param>
        /// <returns>EndUser details</returns>
        public EndUserVO GetEndUserById(int endUserId)
        {
            EndUser enduser = mdbDataContext.EndUsers.SingleOrDefault(c => c.ID == endUserId);

            EndUserVO enduserVO = null;
            if (enduser != null)
            {
                enduserVO = new EndUserVO(enduser);
            }

            return enduserVO;
        }

        /// <summary>
        /// Gets a list of end users filtered based on selected company and invoice customer
        /// </summary>
        /// <param name="companyId">The company Id</param>
        /// <param name="invoiceCustomerId">The invoice customer Id</param>
        /// <returns>The List of End Users</returns>
        public List<EndUserVO> GetEndUserList(int companyId, int invoiceCustomerId = 0)
        {
            List<OACustomer> invoiceCustomerList = mdbDataContext.OACustomers.Where(c => c.IsDeleted == false && c.CompanyID == companyId).ToList();
            List<EndUserVO> endUserVOList = new List<EndUserVO>();

            //Add OACustomers
            foreach (var item in invoiceCustomerList)
            {
                endUserVOList.Add(new EndUserVO(item));
            }

            //Add End users based on invoice customer
            if (invoiceCustomerId != 0)
            {
                List<EndUser> endUserList = mdbDataContext.EndUsers.Where(e => e.IsDeleted == false && e.BusinessPartner.CompanyID == companyId && e.BusinessPartner.InvoiceCustomerID == invoiceCustomerId).ToList();
                foreach (var item in endUserList)
                {
                    endUserVOList.Add(new EndUserVO(item));
                }
            }

            return endUserVOList;
        }

        /// <summary>
        /// Save the enduser
        /// </summary>
        /// <param name="endUser">Value object of enduser</param>
        public void SaveEndUser(EndUserVO endUser)
        {
            if (endUser.ID == 0)
            {
                //Insert New Record
                EndUser newEndUser = new EndUser();

                //To check that Busiess partner for company and customer is exists or not
                BusinessPartner businessPartner = mdbDataContext.BusinessPartners.Where(c => c.CompanyID == endUser.CompanyId && c.InvoiceCustomerID == endUser.InvoiceCustomerId && c.IsDeleted == false).FirstOrDefault();
                if (businessPartner != null)
                {
                    newEndUser.BusinessPartnerID = businessPartner.ID;
                }
                else 
                {
                    newEndUser.BusinessPartner = new BusinessPartner();
                    newEndUser.BusinessPartner.CompanyID = endUser.CompanyId;
                    newEndUser.BusinessPartner.InvoiceCustomerID = endUser.InvoiceCustomerId;
                }

                newEndUser.EndUserName = endUser.Name;
                newEndUser.CreatedBy = endUser.CreatedByUserId;
                newEndUser.CreationDate = DateTime.Now;
                mdbDataContext.EndUsers.InsertOnSubmit(newEndUser);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                EndUser selectedEndUser = mdbDataContext.EndUsers.SingleOrDefault(c => c.ID == endUser.ID);
                selectedEndUser.BusinessPartner.CompanyID = endUser.CompanyId;
                selectedEndUser.BusinessPartner.InvoiceCustomerID = endUser.InvoiceCustomerId;
                selectedEndUser.EndUserName = endUser.Name;
                selectedEndUser.EndUserTextID = endUser.EndUserId;
                selectedEndUser.LastUpdatedBy = endUser.LastUpdatedByUserId;
                selectedEndUser.LastUpdatedDate = DateTime.Now;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Get the EndUser by name
        /// </summary>
        /// <param name="endUserName">EndUser name</param>
        /// <returns>Valueobject of enduser</returns>
        public EndUserVO GetEndUserByName(string endUserName, int companyId, int customerId)
        {
            EndUser enduser = mdbDataContext.EndUsers.Where(c => c.EndUserName.Equals(endUserName) && c.BusinessPartner.CompanyID == companyId && c.BusinessPartner.InvoiceCustomerID == customerId && c.IsDeleted == false).SingleOrDefault();

            EndUserVO endUserVO = null;
            if (enduser != null)
            {
                endUserVO = new EndUserVO(enduser);
            }

            return endUserVO;
        }

        /// <summary>
        /// Delete the EndUser(s)
        /// </summary>
        /// <param name="Ids">Ids of endusers to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteEndUser(List<int> Ids, int? userId)
        {
            EndUser deleteEndUser = new EndUser();

            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    deleteEndUser = mdbDataContext.EndUsers.Where(c => c.ID == id).SingleOrDefault();

                    //To check weather Enduser is associated with contrat or not
                    Contract contract = mdbDataContext.Contracts.Where(c => c.EndUserID == deleteEndUser.EndUserTextID && !c.IsDeleted).FirstOrDefault();
                    if (contract == null)
                    {
                        deleteEndUser.IsDeleted = true;
                        deleteEndUser.LastUpdatedBy = userId;
                        deleteEndUser.LastUpdatedDate = DateTime.Now;
                    }
                }
            }
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// to get end user name based on Id
        /// </summary>
        /// <param name="endUserId"></param>
        /// <returns></returns>
        public string GetEndUserName(string endUserId)
        {
            //to get end user details from EndUser table
            EndUser endUser = mdbDataContext.EndUsers.Where(c => c.EndUserTextID == endUserId && c.IsDeleted == false).SingleOrDefault();
            string endUserName = string.Empty;
            if (endUser == null)
            {
                //to get customer name from OACustomer table
                OACustomer oaCustomer = mdbDataContext.OACustomers.Where(c => c.ID == Convert.ToInt32(endUserId) && c.IsDeleted == false).SingleOrDefault();
                endUserName = oaCustomer.CustomerName;
            }
            else
            {
                endUserName = endUser.EndUserName;
            }

            return endUserName;
        }

        /// <summary>
        /// Get enduser name from EndUser table
        /// </summary>
        /// <param name="id">end user id</param>
        /// <returns>enduser name</returns>
        //public string GetEndUserNameByID(string id, int companyId)
        //{
        //    string endUserName = string.Empty;
        //    OACustomer invoiceCustomer = new OACustomer();
        //    invoiceCustomer = mdbDataContext.OACustomers.Where(cust => Convert.ToString(cust.ID).Equals(id) && cust.CompanyID == companyId).SingleOrDefault();

        //    if (invoiceCustomer != null && !string.IsNullOrEmpty(invoiceCustomer.CustomerName))
        //    {
        //        endUserName = invoiceCustomer.CustomerName;
        //    }

        //    //if end user name not found in OACustomer table
        //    // search end user name in end user table
        //    if (string.IsNullOrEmpty(endUserName))
        //    {
        //        //Search end user name in enduser table
        //        EndUser endUser = new EndUser();
        //        endUser = mdbDataContext.EndUsers.Where(e => e.EndUserTextID.Equals(id) && e.BusinessPartner.CompanyID == companyId).SingleOrDefault();
        //        if (endUser != null && !string.IsNullOrEmpty(endUser.EndUserName))
        //        {
        //            endUserName = endUser.EndUserName;
        //        }
        //    }

        //    return endUserName;
        //}
    }
}