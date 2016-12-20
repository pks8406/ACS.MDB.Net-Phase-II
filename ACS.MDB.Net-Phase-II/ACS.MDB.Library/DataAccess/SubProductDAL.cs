using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class SubProductDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of ProductsGets the list of SubProducts based on Product Id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>List of Subproduct</returns>
        public List<SubProductVO> GetSubProductListById(int productId)
        {
            List<SubProductVO> subProductVOList = new List<SubProductVO>();
            if (productId != 0)
            {
                List<SubProduct> subProductList = mdbDataContext.SubProducts.Where(c => c.ProductID == productId && c.IsDeleted == false).OrderBy(c => c.Version).ToList();

                foreach (var item in subProductList)
                {
                    subProductVOList.Add(new SubProductVO(item));
                }
            }
            return subProductVOList;
        }

        /// <summary>
        /// Save the SubProduct
        /// </summary>
        /// <param name="subProduct"></param>
        public void SaveSubProduct(SubProductVO subProduct)
        {
            if (subProduct.SubProductId == 0)
            {
                //Insert New Record
                SubProduct newSubProduct = new SubProduct();
                newSubProduct.Version = subProduct.Version;
                //newSubProduct.ID = subProduct.SubProductId;
                newSubProduct.ProductID = subProduct.ProductId;
                newSubProduct.CreationDate = DateTime.Now;
                newSubProduct.CreatedBy = subProduct.CreatedByUserId;
                mdbDataContext.SubProducts.InsertOnSubmit(newSubProduct);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                SubProduct selectedSubProduct = mdbDataContext.SubProducts.SingleOrDefault(c => c.ID == subProduct.SubProductId);
                selectedSubProduct.ProductID = subProduct.ProductId;
                selectedSubProduct.Version = subProduct.Version;
                selectedSubProduct.LastUpdatedDate = DateTime.Now;
                selectedSubProduct.LastUpdatedBy = subProduct.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Get SubproductDetails by Id
        /// </summary>
        /// <param name="subProductId">subProductId</param>
        /// <returns>SubproductDetails</returns>
        public SubProductVO GetSubProductById(int subProductId = 0)
        {
            SubProduct subProduct = mdbDataContext.SubProducts.SingleOrDefault(c => c.ID == subProductId);
            SubProductVO subProductVO = null;
            if (subProduct != null)
            {
                subProductVO = new SubProductVO(subProduct);
            }
            return subProductVO;
        }

        /// <summary>
        /// Gets the Subproduct by Name
        /// </summary>
        /// <param name="subProductName">subProductName</param>
        /// <returns></returns>
        public SubProductVO GetSubProductByName(string subProductName, int? productId)
        {
            SubProduct subProduct = mdbDataContext.SubProducts.Where(pro => pro.Version.Equals(subProductName) && pro.ProductID == productId && pro.IsDeleted == false).SingleOrDefault();

            SubProductVO selectedSubProduct = null;

            if (subProduct != null)
            {
                selectedSubProduct = new SubProductVO(subProduct);
            }
            return selectedSubProduct;
        }

        ///  <summary>
        /// Delete selected Sub Product(s)
        ///  </summary>
        ///  <param name="Ids">Ids of sub products to be deleted</param>
        /// <param name="userId">Logged in user id</param>
        public void DeleteSubProduct(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    //To check weather SubProduct is associated with contratmaintaince or not
                    List<ContractMaintenance> contractMaintaince = mdbDataContext.ContractMaintenances.Where(c => c.SubProductID == id && !c.IsDeleted).ToList();
                    if (contractMaintaince.Count <= 0 )
                    {
                        SubProduct deleteSubProduct =
                            mdbDataContext.SubProducts.FirstOrDefault(sp => sp.ID == id);

                        if (deleteSubProduct != null)
                        {
                            deleteSubProduct.IsDeleted = true;
                            deleteSubProduct.LastUpdatedBy = userId;
                            deleteSubProduct.LastUpdatedDate = DateTime.Now;
                        }
                    }
                }
            }
            mdbDataContext.SubmitChanges();
        }
    }
}