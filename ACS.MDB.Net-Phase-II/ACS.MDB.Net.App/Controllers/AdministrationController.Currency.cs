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
    /// <summary>
    /// Controller class to handle the currency actions
    /// </summary>
    public partial class AdministrationController
    {
        /// <summary>
        /// Returns currency index view
        /// </summary>
        /// <returns>currency index view</returns>
        // GET: /Administration/CurrencyIndex
        public ActionResult CurrencyIndex()
        {
            return IndexViewForAuthorizeUser();
        }

        /// <summary>
        /// Returns a list of currency
        /// </summary>
        /// <param name="param">The filter an other parameters</param>
        /// <returns>List of currency</returns>
        // GET: /Administration/CurrencyList
        public ActionResult CurrencyList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                CurrencyService currencyService = new CurrencyService();
                List<CurrencyVO> currencyVOList = currencyService.GetCurrencyList();

                List<MODEL.Currency> currencies = new List<MODEL.Currency>();
                foreach (var item in currencyVOList)
                {
                    currencies.Add(new Models.Currency(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetCurrencyOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, currencies, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create new currency
        /// </summary>
        /// <returns></returns>
        // GET: /Administration/CurrencyCreate
        public ActionResult CurrencyCreate()
        {
            try
            {
                MODEL.Currency currency = new MODEL.Currency();

                //Set by default new currency as active 
                currency.IsActive = true;
                return PartialView("CurrencyDetails", currency);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit currency detail
        /// </summary>
        /// <param name="id">The selected currency id</param>
        /// <returns>The currency index details view</returns>
        // GET: /Administration/CurrencyEdit
        public ActionResult CurrencyEdit(int id)
        {
            MODEL.Currency currency = null;
            try
            {
                CurrencyService currencyService = new CurrencyService();

                //Get currency details
                CurrencyVO currencyVO = currencyService.GetCurrencyById(id);
                if (currencyVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.CURRENCY));
                }
                else
                {
                    currency = new MODEL.Currency(currencyVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("CurrencyDetails", currency);
        }

        /// <summary>
        /// Save the currency (new or edited)
        /// </summary>
        /// <param name="model">The currency details (model)</param>
        /// <returns></returns>
        // POST: /Administration/CurrencySave
        public ActionResult CurrencySave(MODEL.Currency model)
        {
            try
            {
                bool ismodelValid = ModelState.IsValid;
                if (!ismodelValid)
                {
                  ismodelValid = IsModelValidForMultilineTextbox("Description", model.Description, 25);
                  if (ismodelValid)
                  {
                      model.Description = model.Description.Replace("\r\n", "\n");
                  }
                }

                if (ismodelValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    CurrencyService currencyService = new CurrencyService();
                    //CurrencyVO currencyVO = new CurrencyVO(model, userId);
                    CurrencyVO currencyVO = model.Transpose(userId);

                    currencyService.SaveCurrency(currencyVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.CURRENCY));
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete selected list of currencies
        /// </summary>
        /// <param name="ids">The selected currency ids</param>
        /// <returns></returns>
        // POST: /Administration/CurrenciesDelete
        //public ActionResult CurrenciesDelete(List<int> ids)
        //{
        //    try
        //    {
        //        CurrencyService currencySvc = new CurrencyService();
        //        currencySvc.DeleteCurrency(ids);

        //        return new HttpStatusCodeResult(200);
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeAndErrorResult(500, e.Message);
        //    }
        //}

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns></returns>
        public Func<BaseModel, object> GetCurrencyOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((Currency)obj).CurrencyName;
                    break;

                case 3:
                    sortFunction = obj => ((Currency)obj).Description;
                    break;

                case 4:
                    sortFunction = obj => ((Currency)obj).ExchangeRate;
                    break;

                case 5:
                    sortFunction = obj => ((Currency)obj).IsActive;
                    break;

                default:
                    sortFunction = obj => ((Currency)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}