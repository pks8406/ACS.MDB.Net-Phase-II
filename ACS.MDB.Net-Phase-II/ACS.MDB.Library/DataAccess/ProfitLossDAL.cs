using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ProfitLossDAL : BaseDAL
    {
        /// <summary>
        /// Gets P&L list filtered on Company
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>Return the list of ProfitLossVO</returns>
        public List<ProfitLossVO> GetPandLListByCompany(int? companyId)
        {
            List<ProfitLossVO> plVOList = new List<ProfitLossVO>();
            if (companyId != 0)
            {
                List<P_LMapping> plList = mdbDataContext.P_LMappings.Where(c => c.CompanyID == companyId && c.IsDeleted == false).ToList();

                foreach (P_LMapping profitloss in plList)
                {
                    plVOList.Add(new ProfitLossVO(profitloss));
                }

                //foreach (P_LMapping profitloss in plList)
                //{
                //    ProfitLossVO plVO = new ProfitLossVO(profitloss);
                //    OACostCentre costCenterName = mdbDataContext.OACostCentres
                //                                                .Where(oac => oac.ID == profitloss.CostCentreID && oac.CompanyID == companyId).SingleOrDefault();
                //    plVO.CostCenterName = costCenterName.CostCentreName;
                //    plVOList.Add(plVO);
                //}
            }
            return plVOList;
        }

        /// <summary>
        /// Save the P&L
        /// </summary>
        /// <param name="profitlossVO">Value object of P&L</param>
        public void SaveProfitLoss(ProfitLossVO profitlossVO)
        {
            if (profitlossVO.ProfitLossId == 0)
            {
                //Insert New Record
                P_LMapping newProfitLoss = new P_LMapping();
                newProfitLoss.CostCentreID = profitlossVO.CostCentreId;
                newProfitLoss.CompanyID = profitlossVO.CompanyId;
                newProfitLoss.P_L = profitlossVO.ProfitLossName;
                newProfitLoss.ID = profitlossVO.ProfitLossId;
                newProfitLoss.CreationDate = DateTime.Now;
                newProfitLoss.CreatedBy = profitlossVO.CreatedByUserId;
                mdbDataContext.P_LMappings.InsertOnSubmit(newProfitLoss);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                P_LMapping selectedProfitLoss = mdbDataContext.P_LMappings.SingleOrDefault(c => c.ID == profitlossVO.ProfitLossId);
                selectedProfitLoss.P_L = profitlossVO.ProfitLossName;
                selectedProfitLoss.CompanyID = profitlossVO.CompanyId;
                selectedProfitLoss.CostCentreID = profitlossVO.CostCentreId;
                selectedProfitLoss.LastUpdatedDate = DateTime.Now;
                selectedProfitLoss.LastUpdatedBy = profitlossVO.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Gets the Profitloss details by profitloss id.
        /// </summary>
        /// <param name="profitlossId">profitlossid</param>
        /// <returns>Profitloss details</returns>
        public ProfitLossVO GetPandLbyId(int profitlossId = 0)
        {
            P_LMapping profitloss = mdbDataContext.P_LMappings.SingleOrDefault(c => c.ID == profitlossId);
            ProfitLossVO profitlossVO = null;

            if (profitloss != null)
            {
                profitlossVO = new ProfitLossVO(profitloss);

                //Set the Costcenter Name
                OACostCentre costcenter = mdbDataContext.OACostCentres.FirstOrDefault(oac => oac.ID == profitloss.CostCentreID);
                profitlossVO.CostCenterName = costcenter.CostCentreName;

                //set the company name
                OACompany company = mdbDataContext.OACompanies.SingleOrDefault(oac => oac.ID == profitloss.CompanyID);
                profitlossVO.CompanyName = company.CompanyName;
            }
            return profitlossVO;
        }

        /// <summary>
        /// Delete Selected P&L
        /// </summary>
        /// <param name="Ids">List of ProfitLoss Ids</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteProfitLoss(List<int> Ids,int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    P_LMapping deleteProfitLoss = new P_LMapping();
                    deleteProfitLoss = mdbDataContext.P_LMappings.SingleOrDefault(c => c.ID == id);
                    deleteProfitLoss.IsDeleted = true;
                    deleteProfitLoss.LastUpdatedDate = DateTime.Now;
                    deleteProfitLoss.LastUpdatedBy = userId;
                    mdbDataContext.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Gets the CostCenter by CostCenterId
        /// </summary>
        /// <param name="costCentreId">CostCenter Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Return value object of P&L</returns>
        public ProfitLossVO GetCostCenterById(int costCentreId, int companyId)
        {
            P_LMapping profitLoss = mdbDataContext.P_LMappings.Where(c => c.CostCentreID == costCentreId && c.CompanyID == companyId && c.IsDeleted == false).SingleOrDefault();

            ProfitLossVO selectedProfitLoss = null;

            if (profitLoss != null)
            {
                selectedProfitLoss = new ProfitLossVO(profitLoss);

                //Set the Costcenter Name
                OACostCentre costcenter = mdbDataContext.OACostCentres.FirstOrDefault(oac => oac.ID == costCentreId);
                selectedProfitLoss.CostCenterName = costcenter.CostCentreName;

                //set the company name
                OACompany company = mdbDataContext.OACompanies.SingleOrDefault(oac => oac.ID == companyId);
                selectedProfitLoss.CompanyName = company.CompanyName;
            }
            return selectedProfitLoss;
        }
    }
}