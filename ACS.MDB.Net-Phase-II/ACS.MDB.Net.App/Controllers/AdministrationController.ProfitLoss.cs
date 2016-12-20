using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class AdministrationController
    {
        /// <summary>
        /// Returns profitloss index view
        /// </summary>
        /// <returns>profitloss index view</returns>
        /// GET: /AdministrationController/ProfitLossIndex
        public ActionResult ProfitLossIndex()
        {
            MODEL.ProfitLoss profitLossModel = new ProfitLoss();
            try
            {
                profitLossModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            }
            catch (Exception)
            {
            }
            return IndexViewForAuthorizeUser(profitLossModel);
        }

        /// <summary>
        /// Returns list of P&L for the specified company,costcenter and selection criteria.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="costcenterId">costcenter id</param>
        /// <param name="companyId">company id</param>
        /// <returns>ProfitLoss List</returns>
        public ActionResult ProfitLossList(MODEL.jQueryDataTableParamModel param, int? companyId = 0)
        {
            try
            {
                ProfitLossService profitlossService = new ProfitLossService();
                List<MODEL.ProfitLoss> profitlossList = new List<MODEL.ProfitLoss>();

                List<ProfitLossVO> profitlossVOlist = profitlossService.GetPandLListByCompany(companyId);

                foreach (ProfitLossVO profitlossVO in profitlossVOlist)
                {
                    profitlossList.Add(new MODEL.ProfitLoss(profitlossVO));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetProfitLossOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, profitlossList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create new p & l for specified company
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="companyName">The companay name</param>
        /// <returns>The profit loss details view</returns>
        public ActionResult ProfitLossCreate(int companyId, string companyName)
        {
            try
            {
                MODEL.ProfitLoss profitloss = new MODEL.ProfitLoss();

                profitloss.costcenterList = GetCostCenterList(companyId);
                profitloss.CompanyId = companyId;
                profitloss.CompanyName = companyName;
                return PartialView("ProfitLossDetails", profitloss);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit profitloss details by id.
        /// </summary>
        /// <param name="id">Profitloss Id</param>
        /// <returns>The profit loss details view</returns>
        public ActionResult ProfitLossEdit(int id)
        {
            MODEL.ProfitLoss profitloss = new ProfitLoss();
            try
            {
                ProfitLossService profitlossService = new ProfitLossService();

                //Get P&L details
                ProfitLossVO profitlossVO = profitlossService.GetPandLbyId(id);
                if (profitlossVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.PROFITANDLOSS));
                }
                else
                {
                    profitloss = new ProfitLoss(profitlossVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("ProfitLossDetails", profitloss);
        }

        /// <summary>
        /// Save the profitloss details.
        /// </summary>
        /// <param name="model">The ProfitLoss model</param>
        public ActionResult ProfitLossSave(MODEL.ProfitLoss model)
        {
            try
            {
                ProfitLossService profitlossService = new ProfitLossService();

                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();

                    //ProfitLossVO profitlossVO = new ProfitLossVO(model, userId);

                    ProfitLossVO profitlossVO = model.Transpose(userId);

                    profitlossService.SaveProfitLoss(profitlossVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.PROFITANDLOSS));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete p & l for selected ids
        /// </summary>
        /// <param name="ids">The selected profitloss ids</param>
        /// <returns>return the status of the deletion</returns>
        // POST: /Administration/ProfitLossDelete
        public ActionResult ProfitLossDelete(List<int> ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                ProfitLossService profitlossService = new ProfitLossService();
                profitlossService.DeleteProfitLoss(ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Gets the list of Costcenter based on Company Id
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>Costcenter List</returns>
        private List<MODEL.CostCentre> GetCostCenterList(int? companyId)
        {
            MODEL.ProfitLoss profitloss = new MODEL.ProfitLoss();
            CostCenterService costcenterService = new CostCenterService();
            List<CostCentreVO> costcenterVOList = costcenterService.GetCostCenterList(companyId);

            foreach (CostCentreVO costcenter in costcenterVOList)
            {
                profitloss.costcenterList.Add(new MODEL.CostCentre(costcenter));
            }

            return (profitloss.costcenterList);
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns field to be sorted</returns>
        public Func<BaseModel, object> GetProfitLossOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((ProfitLoss)obj).CostCenterName;
                    break;

                case 3:
                    sortFunction = obj => ((ProfitLoss)obj).ProfitLossName;
                    break;

                default:
                    sortFunction = obj => ((ProfitLoss)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}