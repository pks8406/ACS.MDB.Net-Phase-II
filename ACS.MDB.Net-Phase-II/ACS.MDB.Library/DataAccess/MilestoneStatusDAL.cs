using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class MilestoneStatusDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of Milestone status
        /// </summary>
        /// <returns>List of Milestone status</returns>
        public List<MilestoneStatusVO> GetMilestoneStatusList()
        {
            List<MilestoneStatusVO> milestoneStatusVOList = new List<MilestoneStatusVO>();
            List<MilestoneStatus> milestoneStatusList = mdbDataContext.MilestoneStatus.Where(m => m.IsDeleted == false).ToList();

            foreach (var item in milestoneStatusList)
            {
                milestoneStatusVOList.Add(new MilestoneStatusVO(item));
            }
            return milestoneStatusVOList;
        }
    }
}