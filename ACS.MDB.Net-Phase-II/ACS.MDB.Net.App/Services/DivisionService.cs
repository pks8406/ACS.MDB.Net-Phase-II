using System;
using System.Collections;
using System.Collections.Generic;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;


namespace ACS.MDB.Net.App.Services
{
    public class DivisionService : BaseService
    {
        DivisionDAL divisionDAL = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DivisionService()
        {
            divisionDAL = new DivisionDAL();
        }

        /// <summary>
        /// Get Division list filtered on Company
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="searchtext">Searchtext</param>
        /// <returns>List of Divisions</returns>
        public List<DivisionVO> GetDivisionListByCompany(int? companyId)
        {
            return divisionDAL.GetDivisionListByCompany(companyId);
        }

        /// <summary>
        /// Gets list divisions id and name by company id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>Division List</returns>
        public List<DivisionVO> GetDivisionsByCompanyIds(List<int> companyIds)
        {
            return divisionDAL.GetDivisionsByCompanyId(companyIds);
        }

        /// <summary>
        /// Save the division
        /// </summary>
        /// <param name="division">Value Object division</param>
        public void SaveDivision(DivisionVO division)
        {
            if (!string.IsNullOrEmpty(division.DivisionName))
            {
                if (division.IsActive == true)
                {
                    IsDivisionActivatedInCompany(division);
                }

                if (!division.IsActive)
                {
                    IsDivisionAssociatedWithContract(division);
                }

                DivisionVO divisionExist = divisionDAL.GetDivisionByName(division.DivisionName, division.CompanyId);
                //Check whether division already exist or not
                if (divisionExist != null && division.DivisionId != divisionExist.DivisionId)
                {
                    throw new ApplicationException(Constants.DIVISION_ALREADY_EXIST);
                }
                else
                {
                    divisionDAL.SaveDivision(division);
                }
            }
        }

        /// <summary>
        /// Get Divisiondetails by Id
        /// </summary>
        /// <param name="DivisionId">Division Id</param>
        /// <returns>Division details</returns>
        public DivisionVO GetDivisionById(int DivisionId)
        {
            return divisionDAL.GetDivisionById(DivisionId);
        }

        /// <summary>
        /// Check whether division is associated with contract
        /// </summary>
        /// <param name="division">division value object</param>
        /// <returns></returns>
        private void IsDivisionAssociatedWithContract(DivisionVO division)
        {
            int count = divisionDAL.IsDivisionAssociatedWithContract(division);
            if (count > 0)
            {
                throw new ApplicationException(Constants.DIVISION_CANNOT_BE_INACTIVE);
            }
        }

        /// <summary>
        /// Check whether division is associated with company
        /// </summary>
        /// <param name="divisionVO">division Value object</param>
        private void IsDivisionActivatedInCompany(DivisionVO divisionVO)
        {
            DivisionVO division = divisionDAL.IsDivisionActivatedInCompany(divisionVO);
            if (division != null)
            {
                throw new ApplicationException(String.Format(Constants.DIVISION_ALREADY_ACTIVE_IN_COMPANY, division.DivisionName, division.CompanyName));
            }
        }
    }
}