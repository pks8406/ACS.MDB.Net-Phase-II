
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class AccountVO
    {
        public int Id { get; set; }
        public string  OAAccountId { get; set; }
        public string  AccountName { get; set; }
        public int? CompanyId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountVO()
        { 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountCode"></param>
        public AccountVO(OAAccountCode accountCode)
        {
            Id = accountCode.ID;
            OAAccountId = accountCode.AccountID;
            AccountName = accountCode.AccountName;
            CompanyId = accountCode.CompanyID;            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountCode"></param>
        //public AccountVO(Account accountCode)
        //{
        //    Id = accountCode.ID;
        //    OAAccountId = accountCode.OAAccountId;
        //    AccountName = accountCode.AccountName;
        //    CompanyId = accountCode.CompanyId;
        //}
    }
}