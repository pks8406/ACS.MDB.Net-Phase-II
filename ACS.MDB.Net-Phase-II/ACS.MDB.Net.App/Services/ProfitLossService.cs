using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Services
{
    public class ProfitLossService : BaseService
    {
        ProfitLossDAL profitlossDAL = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProfitLossService()
        {
            profitlossDAL = new ProfitLossDAL();
        }

        /// <summary>
        /// Get P&L list filtered on Company
        /// </summary>
        /// <param name="companyId">Company Id</param>         
        /// <returns>List of P&L</returns>        

        public List<ProfitLossVO> GetPandLListByCompany(int? companyId)
        {
            return profitlossDAL.GetPandLListByCompany(companyId);
        }


        /// <summary>
        /// Save the P&L
        /// </summary>
        /// <param name="profitlossVO">Value object of ProfitLoss</param>
        public void SaveProfitLoss(ProfitLossVO profitlossVO)
        {
            if (profitlossVO.CostCentreId > 0)
            {
                ProfitLossVO profitLossExist = profitlossDAL.GetCostCenterById(profitlossVO.CostCentreId, profitlossVO.CompanyId);

                if (profitLossExist != null && profitlossVO.CostCentreId == profitLossExist.CostCentreId && profitlossVO.ProfitLossId == 0)
                {
                    throw new ApplicationException(String.Format(Constants.COST_CENTRE_ALREADY_MAPPED_WITH_COMPANY, profitLossExist.CostCenterName , profitLossExist.CompanyName));
                }
                else
                {
                    profitlossDAL.SaveProfitLoss(profitlossVO);
                }
            }
        }

        /// <summary>
        /// Gets the Profitloss details by profitloss id.
        /// </summary>
        /// <param name="profitlossId">profitlossid</param>
        /// <returns>Profitloss details</returns>
        public ProfitLossVO GetPandLbyId(int profitlossId = 0)
        {
            return profitlossDAL.GetPandLbyId(profitlossId);
        }

        /// <summary>
        /// Delete Selected P&L
        /// </summary>
        /// <param name="Ids">List of ProfitLoss Ids</param>
        public void DeleteProfitLoss(List<int> Ids, int? userId)
        {
            profitlossDAL.DeleteProfitLoss(Ids, userId);
        }
    }
}