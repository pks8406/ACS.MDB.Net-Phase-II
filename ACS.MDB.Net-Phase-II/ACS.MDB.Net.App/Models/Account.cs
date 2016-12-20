using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class Account : BaseModel
    {
        /// <summary>
        /// Gets or set Account Code
        /// </summary>
        public string OAAccountId { get; set; }

        /// <summary>
        /// Gets or set Account Code
        /// </summary>
        public string  AccountName { get; set; }

        /// <summary>
        /// Gets or set Company Id
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public Account()
        { 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountVO"></param>
        public Account(AccountVO accountVO)
        {
            ID = accountVO.Id;
            OAAccountId = accountVO.OAAccountId;
            AccountName = accountVO.AccountName;
            CompanyId = accountVO.CompanyId;
        }

        /// <summary>
        /// Transpose model object to value object
        /// </summary>
        /// <param name="account"></param>
        public AccountVO Transpose ()
        {
            AccountVO accountVO = new AccountVO();

            accountVO.Id = this.ID;
            accountVO.OAAccountId = this.OAAccountId;
            accountVO.AccountName = this.AccountName;
            accountVO.CompanyId = this.CompanyId;

            return accountVO;
        }
    }
}