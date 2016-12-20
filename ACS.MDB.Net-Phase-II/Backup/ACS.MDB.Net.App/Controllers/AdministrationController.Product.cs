using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;

//using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;
using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class AdministrationController
    {
        /// <summary>
        /// Returns product index view
        /// </summary>
        // GET: /Administration/ProductIndex
        public ActionResult ProductIndex()
        {
            return IndexViewForAuthorizeUser();
        }

        /// <summary>
        /// Returns list of products
        /// </summary>
        /// <param name="param">The filter an other parameters</param>
        /// <returns>List of Products</returns>
        //// GET: /Administration/ProductList
        public ActionResult ProductList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                ProductService productService = new ProductService();
                List<ProductVO> productVOList = productService.GetProductList();

                MODEL.Product product = new MODEL.Product();

                List<MODEL.Product> productList = new List<MODEL.Product>();
                foreach (var item in productVOList)
                {
                    productList.Add(new MODEL.Product(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetProductOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjectsOrderByAscending(param, productList, orderingFunction);

                return result;
            }
            catch (Exception e)
            {
                //ModelState.AddModelError("", e.Message);
                //return GetFilteredObjects(param, new List<MODEL.Product>(), null);
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        ///Create new Product
        /// </summary>
        /// <returns>The product details view</returns>
        // GET: /Administration/ProductCreate
        public ActionResult ProductCreate()
        {
            try
            {
                MODEL.Product product = new MODEL.Product();
                return PartialView("ProductDetails", product);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit product details
        /// </summary>
        /// <param name="id">The product Id</param>
        /// <returns>The product details view</returns>
        public ActionResult ProductEdit(int id)
        {
            MODEL.Product product = null;

            try
            {
                ProductService productService = new ProductService();

                //Get product details
                ProductVO productVO = productService.GetProductById(id);
                if (productVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.PRODUCT));
                }
                else
                {
                    product = new MODEL.Product(productVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("ProductDetails", product);
        }

        /// <summary>
        /// Save the Product
        /// </summary>
        /// <param name="model">model Object</param>
        /// <returns></returns>
        public ActionResult ProductSave(MODEL.Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    ProductService productService = new ProductService();
                    //ProductVO productVO = new ProductVO(model, userId);

                    ProductVO productVO = model.Transpose(userId);

                    productService.SaveProduct(productVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.PRODUCT));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete the Product and associated sub products
        /// </summary>
        /// <param name="Ids">The list of product id's</param>
        /// <returns></returns>
        public ActionResult ProductDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                ProductService productService = new ProductService();
                productService.DeleteProduct(Ids, userId);

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
        public Func<MODEL.BaseModel, object> GetProductOrderingFunction(int sortCol)
        {
            Func<MODEL.BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.Product)obj).ProductName;
                    break;

                default:
                    sortFunction = obj => ((MODEL.Product)obj).ProductName;
                    break;
            }

            return sortFunction;
        }
    }
}