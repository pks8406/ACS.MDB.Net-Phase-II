using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Services
{
    public class SubProductService : BaseService
    {
        SubProductDAL subProdutDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SubProductService()
        {
            subProdutDAL = new SubProductDAL();
        }

        /// <summary>
        /// Gets the list of Sub Products based on Product Id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>List of Subproduct</returns>
        public List<SubProductVO> GetSubProductListById(int productId)
        {
            return subProdutDAL.GetSubProductListById(productId);
        }

        /// <summary>
        /// Gets the subProduct Details by Id
        /// </summary>
        /// <param name="subProductId">subProductId</param>
        /// <returns></returns>
        public SubProductVO GetSubProductById(int subProductId = 0)
        {
            return subProdutDAL.GetSubProductById(subProductId);
        }

        /// <summary>
        /// Save Sub Product
        /// </summary>
        /// <param name="subProductVO"></param>
        public void SaveSubProduct(SubProductVO subProductVO)
        {
            if (!string.IsNullOrEmpty(subProductVO.Version))
            {
                SubProductVO subProductExist = subProdutDAL.GetSubProductByName(subProductVO.Version, subProductVO.ProductId);

                if (subProductExist != null && subProductVO.SubProductId != subProductExist.SubProductId)
                {
                    throw new ApplicationException(Constants.SUBPRODUCT_ALREADY_EXIST);
                }
                else
                {
                    subProdutDAL.SaveSubProduct(subProductVO);
                }
            }
        }

        /// <summary>
        /// Delete Sub Product(s)
        /// </summary>
        /// <param name="Ids">Ids of sub products to be deleted</param>
        /// <param name="userId"></param>
        public void DeleteSubProduct(List<int> Ids, int? userId)
        {
            subProdutDAL.DeleteSubProduct(Ids, userId);
        }
    }
}