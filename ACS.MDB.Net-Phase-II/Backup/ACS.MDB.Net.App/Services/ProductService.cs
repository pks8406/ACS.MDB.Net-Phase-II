using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;


namespace ACS.MDB.Net.App.Services
{
    public class ProductService : BaseService
    {
        ProductDAL productDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductService()
        {
            productDAL = new ProductDAL();
        }

        /// <summary>
        /// Gets the list of Products
        /// </summary>
        /// <returns>List of Product</returns>
        public List<ProductVO> GetProductList()
        {
            return productDAL.GetProductList();
        }

        /// <summary>
        /// Get the product details by product id
        /// </summary>
        /// <param name="productId">The prodcut id</param>
        /// <returns>The productVO</returns>
        public ProductVO GetProductById(int productId)
        {
            return productDAL.GetProductById(productId);
        }

        /// <summary>
        /// Save the Product
        /// </summary>
        /// <param name="product">Value Object of Product</param>
        public void SaveProduct(ProductVO product)
        {
            if (!string.IsNullOrEmpty(product.ProductName))
            {
                ProductVO productExist = productDAL.GetProductByName(product.ProductName);

                //Check whether product already exist or not
                if (productExist != null && product.ProductId != productExist.ProductId)
                {
                    throw new ApplicationException(Constants.PRODUCT_ALREADY_EXIST);
                }
                else
                {
                    productDAL.SaveProduct(product);
                }
            }
        }

        /// <summary>
        /// Delete the product(s)
        /// </summary>
        /// <param name="Ids">The product ids to delete</param>
        public void DeleteProduct(List<int> Ids, int? userId)
        {
            productDAL.DeleteProduct(Ids, userId);
        }
    }
}