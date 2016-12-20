using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class DivisionDAL : BaseDAL
    {
        /// <summary>
        /// Gets division list filtered on Company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<DivisionVO> GetDivisionListByCompany(int? companyId)
        {
            List<DivisionVO> divisionVOList = new List<DivisionVO>();
            if (companyId != 0)
            {
                List<Division> divisionList = mdbDataContext.Divisions
                                            .Where(c => c.CompanyID == companyId).ToList();

                foreach (Division division in divisionList)
                {
                    divisionVOList.Add(new DivisionVO(division));
                }
            }

            return divisionVOList;
        }

        /// <summary>
        /// Gets list divisions id and name by company id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>Hashtable having division id and name</returns>
        public List<DivisionVO> GetDivisionsByCompanyId(List<int> companyIds)
        {
            List<DivisionVO> divisions = new List<DivisionVO>();

            foreach (var id in companyIds)
            {
                List<Division> divisionList = (mdbDataContext.Divisions.Where(d => d.CompanyID == id && d.IsActive == true)).OrderBy(n => n.DivisionName).ToList();

                if (divisionList != null)
                {
                    foreach (var item in divisionList)
                    {
                        divisions.Add(new DivisionVO(item));
                    }
                }
            }
            return divisions;
        }

        /// <summary>
        /// Save the division
        /// </summary>
        /// <param name="division">Value Object division</param>
        public void SaveDivision(DivisionVO division)
        {
            if (division.DivisionId == 0)
            {
                //Insert New Record

                Division newDivision = new Division();
                newDivision.DivisionName = division.DivisionName;
                newDivision.CompanyID = division.CompanyId;
                newDivision.IsActive = division.IsActive;
                newDivision.DocumentTypeCR = division.DocumentTypeCR;
                newDivision.DocumentTypeIN = division.DocumentTypeIN;
                newDivision.DepositDocumentTypeIN = division.DocumentTypeDepositInvoices;
                newDivision.DepositDocumentTypeCR = division.DocumentTypeDepositCredits;
                newDivision.DefaultInvoiceInAdvanced = division.DefaultInvoiceInAdvance;
                //newDivision.ID = division.DivisionId;
                newDivision.CreationDate = DateTime.Now;
                newDivision.CreatedBy = division.CreatedByUserId;
                mdbDataContext.Divisions.InsertOnSubmit(newDivision);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                Division selectedDivision = mdbDataContext.Divisions.SingleOrDefault(c => c.ID == division.DivisionId);
                selectedDivision.DivisionName = division.DivisionName;
                selectedDivision.CompanyID = division.CompanyId;
                selectedDivision.IsActive = division.IsActive;
                selectedDivision.DocumentTypeCR = division.DocumentTypeCR;
                selectedDivision.DocumentTypeIN = division.DocumentTypeIN;
                selectedDivision.DepositDocumentTypeIN = division.DocumentTypeDepositInvoices;
                selectedDivision.DepositDocumentTypeCR = division.DocumentTypeDepositCredits;
                selectedDivision.DefaultInvoiceInAdvanced = division.DefaultInvoiceInAdvance;
                selectedDivision.LastUpdatedDate = DateTime.Now;
                selectedDivision.LastUpdatedBy = division.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Gets Divisiondetails by DivisionId
        /// </summary>
        /// <param name="DivisionId">Division Id</param>
        /// <returns>Division details</returns>
        public DivisionVO GetDivisionById(int DivisionId = 0)
        {
            Division division = mdbDataContext.Divisions.SingleOrDefault(c => c.ID == DivisionId);
            DivisionVO divisionVO = null;

            if (division != null)
            {
                divisionVO = new DivisionVO(division);

                //set the company name
                OACompany company = mdbDataContext.OACompanies
                                                  .SingleOrDefault(oac => oac.ID == division.CompanyID);
                divisionVO.CompanyName = company.CompanyName;
            }
            return divisionVO;
        }

        /// <summary>
        /// Gets division by name
        /// </summary>
        /// <param name="divisionName"> division name</param>
        /// <returns></returns>
        public DivisionVO GetDivisionByName(string divisionName, int companyId)
        {
            Division division = mdbDataContext.Divisions
                                .Where(div => div.DivisionName.Equals(divisionName) && div.CompanyID == companyId).SingleOrDefault();

            DivisionVO selectedDivision = null;

            if (division != null)
            {
                selectedDivision = new DivisionVO(division);
            }
            return selectedDivision;
        }

        /// <summary>
        /// Check whether division is associated with contract
        /// </summary>
        /// <param name="division">division value object</param>
        /// <returns></returns>
        public int IsDivisionAssociatedWithContract(DivisionVO division)
        {
            int count = mdbDataContext.Contracts.Where(c => c.DivisionID == division.DivisionId && !c.IsDeleted).Count();
            return count;
        }

        /// <summary>
        /// Check whether division is associated with company
        /// </summary>
        /// <param name="divisionVO">division Value Object</param>
        /// <returns></returns>
        public DivisionVO IsDivisionActivatedInCompany(DivisionVO divisionVO)
        {
            Division division = mdbDataContext.Divisions.Where(div => div.DivisionName.Equals(divisionVO.DivisionName) && (div.IsActive == true) && div.CompanyID != divisionVO.CompanyId).SingleOrDefault();

            DivisionVO selectedDivision = null;

            if (division != null)
            {
                selectedDivision = new DivisionVO(division);

                //set the company name
                OACompany company = mdbDataContext.OACompanies.SingleOrDefault(oac => oac.ID == selectedDivision.CompanyId);
                selectedDivision.CompanyName = company.CompanyName;
            }
            return selectedDivision;
        }


    }
}