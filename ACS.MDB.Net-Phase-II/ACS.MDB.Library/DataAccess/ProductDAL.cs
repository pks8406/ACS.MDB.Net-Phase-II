using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ProductDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of Products
        /// </summary>
        /// <returns>List of Product</returns>
        public List<ProductVO> GetProductList()
        {
            List<ProductVO> productVOList = new List<ProductVO>();
            List<Product> productList = mdbDataContext.Products.Where(c => c.IsDeleted == false).OrderBy(c => c.ProductName).ToList();

            foreach (var item in productList)
            {
                //Transpose LINQ currency object to value object
                productVOList.Add(new ProductVO(item));
            }
            return productVOList;
        }

        /// <summary>
        /// Get Product details by Id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>Product Details</returns>
        public ProductVO GetProductById(int productId)
        {
            Product product = mdbDataContext.Products.SingleOrDefault(c => c.ID == productId);
            ProductVO productVO = null;
            if (product != null)
            {
                productVO = new ProductVO(product);
            }
            return productVO;
        }

        /// <summary>
        /// Save the Product
        /// </summary>
        /// <param name="product">Value Object product</param>
        public void SaveProduct(ProductVO product)
        {
            if (product.ProductId == 0)
            {
                //Insert New Record
                Product newProduct = new Product();
                newProduct.ProductName = product.ProductName;
                //newProduct.ProductID = product.ProductId;
                newProduct.CreationDate = DateTime.Now;
                newProduct.CreatedBy = product.CreatedByUserId;
                mdbDataContext.Products.InsertOnSubmit(newProduct);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                Product selectedProduct = mdbDataContext.Products.SingleOrDefault(c => c.ID == product.ProductId);
                selectedProduct.ProductName = product.ProductName;
                selectedProduct.LastUpdatedDate = DateTime.Now;
                selectedProduct.LastUpdatedBy = product.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Get the Product by Name
        /// </summary>
        /// <param name="productName">The Product Name</param>
        /// <returns>Value Object product</returns>
        public ProductVO GetProductByName(string productName)
        {
            Product product = mdbDataContext.Products.Where(pro => pro.ProductName.Equals(productName) && pro.IsDeleted == false).SingleOrDefault();

            ProductVO productVO = null;

            if (product != null)
            {
                productVO = new ProductVO(product);
            }
            return productVO;
        }

        /// <summary>
        /// Delete the product(s) and sub product(s).
        /// </summary>
        /// <param name="Ids">Ids of products to be deleted</param>
        //public void DeleteProduct(List<int> Ids)
        //{
        //    Product deleteProduct = new Product();

        //    foreach (var id in Ids)
        //    {
        //        if (id != 0)
        //        {
        //            //@TODO : Allow delete of product and subproduct only if it is not associate with maintainance line
        //            deleteProduct = mdbDataContext.Products.Where(c => c.ID == id).SingleOrDefault();
        //            deleteProduct.IsDeleted = true;

        //            List<SubProduct> subProductList = mdbDataContext.SubProducts.Where(c => c.ProductID == deleteProduct.ID && c.IsDeleted == false).ToList();

        //            foreach (var subProduct in subProductList)
        //            {
        //                subProduct.IsDeleted = true;
        //            }
        //        }
        //    }
        //    mdbDataContext.SubmitChanges();
        //}

        /// <summary>
        /// Delete the product(s) and sub product(s).
        /// </summary>
        /// <param name="Ids">Ids of products to be deleted</param>
        public void DeleteProduct(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    List<ContractMaintenance> contractMaintenance =
                   mdbDataContext.ContractMaintenances.Where(c => c.ProductID == id && !c.IsDeleted).ToList();

                    if (contractMaintenance.Count <= 0)
                    {
                        Product deleteProduct = mdbDataContext.Products.FirstOrDefault(c => c.ID == id);

                        if (deleteProduct != null)
                        {
                            deleteProduct.IsDeleted = true;
                            deleteProduct.LastUpdatedBy = userId;
                            deleteProduct.LastUpdatedDate = DateTime.Now;

                            // Delete the associated sub products
                            List<SubProduct> subProductList =
                                mdbDataContext.SubProducts.Where(p => p.ProductID == id && !p.IsDeleted).ToList();

                            foreach (var subProduct in subProductList)
                            {
                                subProduct.IsDeleted = true;
                                subProduct.LastUpdatedBy = userId;
                                subProduct.LastUpdatedDate = DateTime.Now;
                            }
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ids"></param>
        public void ValidateProductIsAssociatedWithContrats(List<int> Ids)
        {
            bool canDelete = true;
            string productName = string.Empty;

            foreach (var id in Ids)
            {
                List<ContractMaintenance> contractMaintenance =
                    mdbDataContext.ContractMaintenances.Where(c => c.ProductID == id && !c.IsDeleted).ToList();

                if (contractMaintenance.Count > 0)
                {
                    canDelete &= false;
                }
            }
        }
    }
}