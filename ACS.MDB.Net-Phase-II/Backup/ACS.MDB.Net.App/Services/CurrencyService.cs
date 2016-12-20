using System;
using System.Collections;
using System.Collections.Generic;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    /// <summary>
    /// Currency service class handle currency managment
    /// </summary>
    public class CurrencyService : BaseService
    {
        CurrencyDAL currencyDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CurrencyService()
        {
            currencyDAL = new CurrencyDAL();
        }

        /// <summary>
        /// Gets list of currency
        /// </summary>
        /// <returns>List of selectedCurrency</returns>
        public List<CurrencyVO> GetCurrencyList()
        {
            return currencyDAL.GetCurrencyList();
        }

        /// <summary>
        /// Gets list of currency id and name by company id
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>Hashtable having currency id and name</returns>
        public Hashtable GetCurrencyNames()
        {
            return currencyDAL.GetCurrencyNames();
        }
        /// <summary>
        /// Gets Currency Details by Id
        /// </summary>
        /// <param name="Id">Currency ID</param>
        /// <returns>Currency Details</returns>
        public CurrencyVO GetCurrencyById(int Id)
        {
            return currencyDAL.GetCurrencyById(Id);
        }

        /// <summary>
        /// Save currency details
        /// </summary>
        /// <param name="currency">The currency object (new or edited)</param>
        /// <returns></returns>
        public void SaveCurrency(CurrencyVO currency)
        {
            if (!string.IsNullOrEmpty(currency.CurrencyName))
            {
                if (!currency.IsActive)
                {
                    IsCurrencyAssociatedWithContract(currency);
                }

                CurrencyVO currencyExist = currencyDAL.GetCurrencyByName(currency.CurrencyName);

                //Check whether currency already exist or not
                if (currencyExist != null && currency.CurrencyID != currencyExist.CurrencyID)
                {
                    throw new ApplicationException(Constants.CURRENCY_ALREADY_EXIST);
                }
                else
                {
                    currencyDAL.SaveCurrency(currency);
                }
            }
        }


        /// <summary>
        /// Check whether currency is associated with contract
        /// </summary>
        /// <param name="currency">currency object</param>
        /// <returns></returns>
        private void IsCurrencyAssociatedWithContract(CurrencyVO currency)
        {
            int count = currencyDAL.IsCurrencyAssociatedWithContract(currency);
            if (count > 0)
            {
                throw new ApplicationException(Constants.CURRENCY_CANNOT_BE_INACTIVE);
            }
        }

        /// <summary>
        /// Delete selected currencies
        /// </summary>
        /// <param name="CurrencyId">The selected currency ids</param>
        /// <returns></returns>
        //public void DeleteCurrency(List<int> Ids)
        //{
        //    currencyDAL.DeleteCurrency(Ids);
        //}
    }
}