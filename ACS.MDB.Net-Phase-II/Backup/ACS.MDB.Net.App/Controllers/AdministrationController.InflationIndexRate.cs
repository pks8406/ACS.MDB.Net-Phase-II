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
        /// Gets inflation index rates list by inflation index Id
        /// </summary>
        /// <param name="param">The params</param>
        /// <param name="inflationIndexId">Inflation Index Id</param>
        /// <returns>inflation index rate list</returns>
        public ActionResult GetInflationIndexRateListById(MODEL.jQueryDataTableParamModel param, int inflationIndexId)
        {
            try
            {
                InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();
                List<InflationIndexRateVO> inflationIndexRateVOList = inflationIndexRateService.GetInflationIndexRateListById(inflationIndexId);

                List<MODEL.InflationIndexRate> inflationIndexRateList = new List<MODEL.InflationIndexRate>();
                if (inflationIndexRateVOList != null)
                {
                    foreach (InflationIndexRateVO inflationIndexRateVO in inflationIndexRateVOList)
                    {
                        inflationIndexRateList.Add(new MODEL.InflationIndexRate(inflationIndexRateVO));
                    }
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetInflationIndexRateOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, inflationIndexRateList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        ///Create new Inflation index rate
        /// </summary>
        /// <returns>The inflation index rate details view</returns>
        // GET: /Administration/InflationIndexRateCreate
        public ActionResult InflationIndexRateCreate(int inflationIndexId, string inflationIndexName)
        {
            try
            {
                MODEL.InflationIndexRate inflationIndexRate = new MODEL.InflationIndexRate();
                inflationIndexRate.InflationIndexId = inflationIndexId;
                inflationIndexRate.IndexName = inflationIndexName;
                return PartialView("InflationIndexRateDetails", inflationIndexRate);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Get Details of inflation index rate by Id
        /// </summary>
        /// <param name="id">Index Rate Id</param>
        /// <returns>The inflation index rate details view</returns>
        public ActionResult InflationIndexRateEdit(int id)
        {
            MODEL.InflationIndexRate inflationIndexRate = null;
            try
            {
                InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();

                //Get inflation index rate
                InflationIndexRateVO inflationIndexRateVO = inflationIndexRateService.GetInflationIndexRateById(id);
                if (inflationIndexRateVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.INDEXRATE));
                }
                else
                {
                    inflationIndexRate = new InflationIndexRate(inflationIndexRateVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("InflationIndexRateDetails", inflationIndexRate);
        }

        /// <summary>
        /// Save the Inflation index rate
        /// </summary>
        /// <param name="model">The InflationIndexRate model</param>
        public ActionResult InflationIndexRateSave(MODEL.InflationIndexRate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();

                    InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();
                    //InflationIndexRateVO inflationIndexRateVO = new InflationIndexRateVO(model, userId);

                    InflationIndexRateVO inflationIndexRateVO = model.Transpose(userId);

                    inflationIndexRateService.SaveInflationIndexRate(inflationIndexRateVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    foreach (var item in ModelState)
                    {
                        if (item.Key == "chargingUpliftDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, Constants.INVALID_DATE);
                        }
                    }
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.INDEXRATE));
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete inflation index rate(s)
        /// </summary>
        /// <param name="Ids">Ids of inflation index rate to be deleted</param>
        public ActionResult InflationIndexRateDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();
                inflationIndexRateService.DeleteInflationIndexRate(Ids, userId);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns></returns>
        public Func<BaseModel, object> GetInflationIndexRateOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((InflationIndexRate)obj).chargingUpliftDate;
                    break;

                case 3:
                    sortFunction = obj => ((InflationIndexRate)obj).IndexRate;
                    break;

                case 4:
                    sortFunction = obj => ((InflationIndexRate)obj).IndexRatePerAnnum;
                    break;

                default:
                    sortFunction = obj => ((InflationIndexRate)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}