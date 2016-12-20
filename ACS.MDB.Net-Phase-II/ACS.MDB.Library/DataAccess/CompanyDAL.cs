using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class CompanyDAL : BaseDAL
    {
        /// <summary>
        /// Gets list of company
        /// </summary>
        /// <returns>Value object of company list</returns>
        public List<CompanyVO> GetCompanyList()
        {
            List<CompanyVO> companyVOList = new List<CompanyVO>();
            List<OACompany> companyList = mdbDataContext.OACompanies.Where(c => !c.IsDeleted).OrderBy(c => c.CompanyName).ToList();

            foreach (OACompany item in companyList)
            {
                companyVOList.Add(new CompanyVO(item));
            }

            return companyVOList;
        }

        /// <summary>
        /// Get company name by company id
        /// </summary>
        /// <param name="id">company id</param>
        /// <returns>company name</returns>
        public string GetCompanyNameByID(int id)
        {
            string companyName = string.Empty;

            OACompany company = mdbDataContext.OACompanies.Where(c => c.ID == id).SingleOrDefault();
            if (company != null)
            {
                companyName = company.CompanyName;
            }

            return companyName;
        }
    }
}