using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;
using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class AdministrationController
    {
        /// <summary>
        /// Get Sub ProductList by Product Id
        /// </summary>
        /// <param name="param"></param>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        public ActionResult GetSubProductListById(MODEL.jQueryDataTableParamModel param, int productId)
        {
            try
            {
                List<MODEL.SubProduct> subProductList = new List<MODEL.SubProduct>();
                SubProductService subProductService = new SubProductService();

                List<SubProductVO> subProductVOList = subProductService.GetSubProductListById(productId);

                if (subProductList != null)
                {
                    foreach (SubProductVO subProductVO in subProductVOList)
                    {
                        subProductList.Add(new MODEL.SubProduct(subProductVO));
                    }
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetSubProductOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjectsOrderByAscending(param, subProductList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create New SubProduct
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productName">The product name</param>
        /// <returns>The subProductDetails view</returns>
        public ActionResult SubProductCreate(int productId, string productName)
        {
            try
            {
                MODEL.SubProduct subProduct = new MODEL.SubProduct();
                subProduct.ProductName = productName;
                return PartialView("SubProductDetails", subProduct);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Save the sub product
        /// </summary>
        /// <param name="model">The SubProduct model</param>
        public ActionResult SubProductSave(MODEL.SubProduct model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();

                    SubProductService subProductService = new SubProductService();
                    //SubProductVO subProductVO = new SubProductVO(model, userId);

                    SubProductVO subProductVO = model.Transpose(userId);

                    subProductService.SaveSubProduct(subProductVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.SUBPRODUCT));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit subproduct details
        /// </summary>
        /// <param name="id">SubProduct Id</param>
        /// <returns>The SubProduct Details view</returns>
        public ActionResult SubProductEdit(int id)
        {
            MODEL.SubProduct subProduct = null;
            try
            {
                SubProductService subProductService = new SubProductService();

                //Get sub product details
                SubProductVO subProductVO = subProductService.GetSubProductById(id);
                if (subProductVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.SUBPRODUCT));
                }
                else
                {
                    subProduct = new MODEL.SubProduct(subProductVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("SubProductDetails", subProduct);
        }

        /// <summary>
        /// Delete Selected SubProducts
        /// </summary>
        /// <param name="Ids">The selected sub products id's</param>
        /// <returns></returns>
        public ActionResult SubProductDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                SubProductService subProductService = new SubProductService();
                subProductService.DeleteSubProduct(Ids, userId);

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
        public Func<MODEL.BaseModel, object> GetSubProductOrderingFunction(int sortCol)
        {
            Func<MODEL.BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.SubProduct)obj).Version;
                    break;

                default:
                    sortFunction = obj => ((MODEL.SubProduct)obj).Version;
                    break;
            }

            return sortFunction;
        }
    }
}