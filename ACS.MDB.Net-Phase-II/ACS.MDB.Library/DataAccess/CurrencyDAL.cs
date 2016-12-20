using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class CurrencyDAL : BaseDAL
    {

        /// <summary>
        /// Returns list of currency
        /// </summary>
        /// <returns>Currency List</returns>
        public List<CurrencyVO> GetCurrencyList()
        {
            List<CurrencyVO> currencyVOList = new List<CurrencyVO>();
            List<Currency> currencyList = mdbDataContext.Currencies.ToList();
            foreach (var item in currencyList)
            {
                //Transpose LINQ currency object to value object
                currencyVOList.Add(new CurrencyVO(item));
            }
            return currencyVOList;
        }

        /// <summary>
        /// Gets list of currency id and name by company id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>Hashtable having currency id and name</returns>
        public Hashtable GetCurrencyNames()
        {
            Hashtable currency = new Hashtable();

            var currencyList = (from a in mdbDataContext.Currencies
                                where a.IsActive == true
                                select new { a.ID, a.CurrencyName });

            foreach (var item in currencyList)
            {
                currency.Add(item.ID, item.CurrencyName);
            }
            return currency;
        }

        /// <summary>
        /// Get Currency Details by Id
        /// </summary>
        /// <param name="Id"> Currency ID</param>
        /// <returns>Currency Details</returns>
        public CurrencyVO GetCurrencyById(int Id)
        {
            Currency currency = new Currency();
            currency = mdbDataContext.Currencies.SingleOrDefault(c => c.ID == Id);

            CurrencyVO currencyVO = null;
            if (currency != null)
            {
                currencyVO = new CurrencyVO(currency);
            }
            return currencyVO;
        }
        /// <summary>
        /// Gets the cloned object
        /// </summary>
        /// <returns></returns>
        public Currency getClone()
        {
            return (Currency)this.MemberwiseClone();
        }

        /// <summary>
        /// Save Currency Details
        /// </summary>
        /// <param name="currencyVO">Value Object currencyVO</param>
        /// <returns></returns>
        public void SaveCurrency(CurrencyVO currencyVO)
        {
            Currency currency = null;

            if (currencyVO.CurrencyID == 0)
            {
                //create new currency
                currency = new Currency();
                currency.CreationDate = DateTime.Now;
                currency.CreatedBy = currencyVO.CreatedByUserId;
            }
            else
            {
                //get currency for update
                currency = mdbDataContext.Currencies.SingleOrDefault(c => c.ID == currencyVO.CurrencyID);                
                currency.LastUpdatedDate = DateTime.Now;
                currency.LastUpdatedBy = currencyVO.LastUpdatedByUserId;
            }

            //Create or update currency details
            currency.CurrencyName = currencyVO.CurrencyName;
            currency.Description = currencyVO.Description.Trim().Replace("\r\n", "\n");
            currency.ExchangeRate = currencyVO.ExchangeRate;
            currency.IsActive = currencyVO.IsActive;

            if (currencyVO.CurrencyID == 0)
            {
                //If new currency
                mdbDataContext.Currencies.InsertOnSubmit(currency);
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Delete selected currency
        /// </summary>
        /// <param name="CurrencyId">CurrencyId </param>
        /// <returns></returns>
        //public void DeleteCurrency(List<int> Ids)
        //{
        //    foreach (var id in Ids)
        //    {
        //        if (id != 0)
        //        {
        //            Currency deleteCurrency = new Currency();

        //            deleteCurrency = mdbDataContext.Currencies.SingleOrDefault(c => c.ID == id);
        //            deleteCurrency.IsActive = true;

        //            mdbDataContext.SubmitChanges();
        //        }
        //    }
        //}

        /// <summary>
        /// Get currency details by currency name
        /// </summary>
        /// <param name="currencyVO">currency value object</param>
        /// <returns>return currency details</returns>
        public CurrencyVO GetCurrencyByName(string currencyName)
        {
            Currency currency = mdbDataContext.Currencies.Where(cur =>
                                                            cur.CurrencyName.Equals(currencyName)).SingleOrDefault();

            CurrencyVO currencyVO = null;

            if (currency != null)
            {
                currencyVO = new CurrencyVO(currency);
            }
            return currencyVO;
        }

        /// <summary>
        /// Check whether currency is associated with contract
        /// </summary>
        /// <param name="currency">currency object</param>
        /// <returns></returns>
        public int IsCurrencyAssociatedWithContract(CurrencyVO currency)
        {
            int count = mdbDataContext.Contracts.Count(c => c.CurrencyID == currency.CurrencyID && !c.IsDeleted);
            return count;
        }
    }
}