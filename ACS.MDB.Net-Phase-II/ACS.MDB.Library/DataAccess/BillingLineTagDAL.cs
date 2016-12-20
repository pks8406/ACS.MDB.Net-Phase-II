using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class BillingLineTagDAL : BaseDAL
    {
        /// <summary>
        /// This method will be used to get billing lines tags.
        /// </summary>
        /// <returns>list of billing lines tags</returns>
        public List<BillingLineTagVO> GetBillingLineTags()
        {
            List<BillingLineTagVO> BillingLineTagVOList = new List<BillingLineTagVO>();
            List<BillingLineTag> BillingLineTagList = mdbDataContext.BillingLineTags.Where(c => c.TagName != null && c.Description != null).ToList();

            foreach (BillingLineTag BillingLineTag in BillingLineTagList)
            {
                BillingLineTagVO BillingLineTagVO = new BillingLineTagVO(BillingLineTag);
                BillingLineTagVOList.Add(BillingLineTagVO);
            }

            return BillingLineTagVOList;
        }
    }
}