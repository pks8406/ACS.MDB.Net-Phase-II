
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class CurrencyVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets currency id
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// Gets or Sets currency name
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// Gets or Sets currency description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets currency exchange rate
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Gets or Sets currency is acive or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CurrencyVO()
        {

        }

        /// <summary>
        /// Transpose LINQ object to currency value object
        /// </summary>
        /// <param name="currency">LINQ objetc</param>
        public CurrencyVO(Currency currency)
        {
            CurrencyID = currency.ID;
            CurrencyName = currency.CurrencyName;
            Description = currency.Description;
            ExchangeRate = currency.ExchangeRate;
            IsActive = currency.IsActive;
            CreatedByUserId = currency.CreatedBy;
            LastUpdatedByUserId = currency.LastUpdatedBy;
        }

        /// <summary>
        /// Transpose model object to currency value object
        /// </summary>
        /// <param name="currency">model objetc</param>
        //public CurrencyVO(MODEL.Currency currency, int? userId)
        //{
        //    CurrencyID = currency.ID;
        //    CurrencyName = currency.CurrencyName;
        //    Description = currency.Description.Trim().Replace("\r\n", "\n");
        //    ExchangeRate = currency.ExchangeRate;
        //    IsActive = currency.IsActive;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}