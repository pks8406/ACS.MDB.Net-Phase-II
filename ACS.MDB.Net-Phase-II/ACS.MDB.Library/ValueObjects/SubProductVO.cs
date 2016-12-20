
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class SubProductVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets sub product id
        /// </summary>
        public int SubProductId { get; set; }

        /// <summary>
        /// Gets or Sets product id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or Sets version
        /// </summary>
        public string  Version { get; set; }

        /// <summary>
        /// Gets or Sets activity
        /// </summary>
        public string  Activity { get; set; }

        /// <summary>
        /// Gets or Sets product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SubProductVO()
        { 
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="subProduct">LINQ subProduct object</param>
        public SubProductVO(SubProduct subProduct)
        {
            SubProductId = subProduct.ID;
            ProductId = subProduct.ProductID;
            Version = subProduct.Version;
            Activity = subProduct.Activity;
            ProductName = subProduct.Product.ProductName;
            CreatedByUserId = subProduct.Product.CreatedBy;
            LastUpdatedByUserId = subProduct.Product.LastUpdatedBy;
        }

        /// <summary>
        /// Transpose model object to SubProduct value object
        /// </summary>
        /// <param name="subProduct">model object</param>
        //public SubProductVO(SubProduct subProduct, int? userId)
        //{
        //    SubProductId = subProduct.ID;            
        //    ProductId = subProduct.ProductId;
        //    Version = subProduct.Version;
        //    Activity = subProduct.Activity;
        //    ProductName = subProduct.ProductName;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}