
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ProductVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets product id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or Sets product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductVO()
        { 
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="product">LINQ product object</param>
        public ProductVO(Product product)
        {
            ProductId = product.ID;
            ProductName = product.ProductName;
            CreatedByUserId = product.CreatedBy;
            LastUpdatedByUserId = product.LastUpdatedBy;
        }

        /// <summary>
        /// Transpose model object to Product value object
        /// </summary>
        /// <param name="product"> model object</param>
        //public ProductVO(Product product, int? userId)
        //{
        //    ProductId = product.ID;
        //    ProductName = product.ProductName;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}