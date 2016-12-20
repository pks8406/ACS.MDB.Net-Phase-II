using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class BillingLineTagService : BaseService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BillingLineTagService()
        {

        }

        /// <summary>
        /// Gets billing lines tags.
        /// </summary>        
        /// <returns>list of billing lines tags</returns>
        public List<BillingLineTagVO> GetBillingLineTags()
        {
            BillingLineTagDAL BillingLineTagDAL = new BillingLineTagDAL();
            return BillingLineTagDAL.GetBillingLineTags();
        }
    }
}