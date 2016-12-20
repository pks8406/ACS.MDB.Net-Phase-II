using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class CompanyService : BaseService
    {
        CompanyDAL companyDAL = null;        
    
        /// <summary>
        /// Default Constructor
        /// </summary>
         public CompanyService()
        {
            companyDAL = new CompanyDAL();
        }

         /// <summary>
         /// Gets list of company
         /// </summary>
         /// <returns>Value object of company list</returns>
         public List<CompanyVO> GetCompanyList()
         {
             return companyDAL.GetCompanyList();
         }

        /// <summary>
        /// Get company name by company id
        /// </summary>
        /// <param name="id">company id</param>
        /// <returns>company name</returns>
        public string GetCompanyNameByID(int id)
        {
            return companyDAL.GetCompanyNameByID(id);
        }
    }
}