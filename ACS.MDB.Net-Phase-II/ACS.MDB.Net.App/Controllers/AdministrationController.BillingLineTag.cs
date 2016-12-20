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
    /// The controller class to handle the billing line tag actions
    /// </summary>
    public partial class AdministrationController
    {
        /// <summary>
        /// Returns billing line tag index view
        /// </summary>
        /// <returns>billing line tag index view</returns>
        // GET: /Administration/BillingLineTagIndex
        public ActionResult BillingLineTagIndex()
        {
            return IndexViewForAuthorizeUser();
        }

        /// <summary>
        ///  Returns list of billing tags
        /// </summary>
        /// <param name="param">The filter an other parameters</param>
        /// <returns>List of billing tags</returns>
        // GET: /Administration/BillingTagList
        public ActionResult BillingTagList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                BillingLineTagService BillingLineTagService = new BillingLineTagService();
                List<MODEL.BillingLineTag> BillingLineTags = new List<MODEL.BillingLineTag>();

                //Get the list of billings tag
                List<BillingLineTagVO> BillingLineTagVOList = BillingLineTagService.GetBillingLineTags();

                foreach (var BillingLineTagVO in BillingLineTagVOList)
                {
                    MODEL.BillingLineTag BillingTag = new MODEL.BillingLineTag(BillingLineTagVO);
                    BillingLineTags.Add(BillingTag);
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetBillingLineTagOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, BillingLineTags, orderingFunction);
                return result;
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
        public Func<BaseModel, object> GetBillingLineTagOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 1:
                    sortFunction = obj => ((BillingLineTag)obj).Tag;
                    break;

                case 2:
                    sortFunction = obj => ((BillingLineTag)obj).Description;
                    break;

                default:
                    sortFunction = obj => ((BillingLineTag)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}