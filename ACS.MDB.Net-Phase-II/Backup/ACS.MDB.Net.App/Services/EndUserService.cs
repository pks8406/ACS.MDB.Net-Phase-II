using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class EndUserService
    {
        EndUserDAL endUserDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndUserService()
        {
            endUserDAL = new EndUserDAL();
        }

        /// <summary>
        /// Gets the list of End User based on company Id and customer Id
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="customerId">customer Id</param>
        /// <returns>List of Endusers</returns>
        public List<EndUserVO> GetEndUserListByCompanyIdandCustomerId(int companyId, int customerId)
        {
            return endUserDAL.GetEndUserListByCompanyIdandCustomerId(companyId, customerId);
        }


        /// <summary>
        /// Gets a list of end users filtered based on selected company and invoice customer
        /// </summary>
        /// <param name="companyId">The company Id</param>
        /// <param name="invoiceCustomerId">The invoice customer Id</param>
        /// <returns>The List of End Users</returns>
        public List<EndUserVO> GetEndUserList(int companyId, int invoiceCustomerId = 0)
        {
            return endUserDAL.GetEndUserList(companyId, invoiceCustomerId);
        }

        /// <summary>
        /// Save the enduser
        /// </summary>
        /// <param name="endUser">Valueobject of enduser</param>
        public void SaveEndUser(EndUserVO endUser)
        {
            if (!string.IsNullOrEmpty(endUser.Name))
            {
                EndUserVO isEnduserExist = endUserDAL.GetEndUserByName(endUser.Name, endUser.CompanyId, endUser.InvoiceCustomerId);

                //Check whether enduser already exist or not
                if (isEnduserExist != null && endUser.EndUserId != isEnduserExist.EndUserId)
                {
                    throw new ApplicationException(Constants.ENDUSER_ALREADY_EXIST);
                }
                else
                {
                    endUserDAL.SaveEndUser(endUser);
                }
            }
        }

        /// <summary>
        /// Get Enduser details by Id
        /// </summary>
        /// <param name="endUserId">EndUser Id</param>
        /// <returns>EndUser details</returns>
        public EndUserVO GetEndUserById(int endUserId)
        {
            return endUserDAL.GetEndUserById(endUserId);
        }

        /// <summary>
        /// Delete the EndUser(s)
        /// </summary>
        /// <param name="Ids">Ids of endusers to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteEndUser(List<int> Ids, int? userId)
        {
            endUserDAL.DeleteEndUser(Ids, userId);
        }

        /// <summary>
        /// to get EndUser name
        /// </summary>
        /// <param name="endUserId"></param>
        /// <returns></returns>
        public string GetEndUserName(string endUserId)
        {
            return endUserDAL.GetEndUserName(endUserId);
        }

        /// <summary>
        /// Get end username by end user id
        /// </summary>
        /// <returns>The end user name</returns>
        //public string GetEndUserNameByID(string id, int companyId)
        //{
        //    return endUserDAL.GetEndUserNameByID(id, companyId);
        //}
    
    }
}