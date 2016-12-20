using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class MilestoneStatusService
    {
        MilestoneStatusDAL milestoneStatusDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MilestoneStatusService()
        {
            milestoneStatusDAL = new MilestoneStatusDAL();
        }

        /// <summary>
        /// Gets the list of Milestone status
        /// </summary>
        /// <returns>List of Milestone status</returns>
        public List<MilestoneStatusVO> GetMilestoneStatusList()
        {
            return milestoneStatusDAL.GetMilestoneStatusList();
        }
    }
}