namespace ACS.MDB.Library.DataAccess
{
    public class AccountDAL : BaseDAL
    {
        ///// <summary>
        ///// Gets the AccountCode based on Account Id & Company Id
        ///// </summary>
        ///// <param name="accountId">Account Id</param>
        ///// <param name="companyId">Company Id</param>
        ///// <returns></returns>

        ////TODO:- Here we have to pass AccountId(int) insted of AccountId(string). Due to fact that we are in the migration process.
        //public AccountVO GetAccountCode(int accountId)
        //{
        //    AccountVO accountVO = new AccountVO();

        //    OAAccountCode accountCode = mdbDataContext.OAAccountCodes.Where(x=>x.ID == accountId && x.IsDeleted == false).SingleOrDefault();

        //    accountVO = new AccountVO(accountCode);

        //    return accountVO;
        //}
    }
}