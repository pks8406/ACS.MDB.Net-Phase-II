using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class MilestoneService
    {
        MilestoneDAL milestoneDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MilestoneService()
        {
            milestoneDAL = new MilestoneDAL();
        }

        /// <summary>
        /// Gets the list of milestone
        /// </summary>
        /// <param name="contractMaintenanceId">contractMaintenanceId</param>
        /// <returns></returns>
        public List<MilestoneVO> GetMilestoneList(int contractMaintenanceId, bool getAllRecords = false)
        {
            return milestoneDAL.GetMilestoneList(contractMaintenanceId, getAllRecords);
        }


        /// <summary>
        /// Get milestone details by Id
        /// </summary>
        /// <param name="milestoneId">milestone id</param>
        /// <returns>Milestone details</returns>
        public MilestoneVO GetMilestoneById(int milestoneId)
        {
            return milestoneDAL.GetMilestoneById(milestoneId);        
        }

        /// <summary>
        /// Save the milestone
        /// </summary>
        /// <param name="milestoneVO">Value object of milestone</param>
        public void SaveMilestone(MilestoneVO milestoneVO)
        {
            milestoneDAL.SaveMilestone(milestoneVO);
        }

        /// <summary>
        /// Update milestone details from Approve Maintenance screen
        /// </summary>
        /// <param name="milestoneVO"></param>
        public void UpdateMilestone(MilestoneVO milestoneVO)
        {
            milestoneDAL.UpdateMilestone(milestoneVO);
        }

        public void SaveRecalculatedMilestone(List<MilestoneVO> milestoneVOs)
        {
            milestoneDAL.SaveRecalculatedMilestone(milestoneVOs);
        }

        /// <summary>
        /// Delete Milestones and associated details
        /// </summary>
        /// <param name="Ids">Ids of milestones to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteMilestone(List<int> Ids, int? userId)
        {
            milestoneDAL.DeleteMilestone(Ids, userId);
        }

        /// <summary>
        /// Get total amount based on contract id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public decimal GetTotalAmount(int contractId, DateTime fromDate, DateTime toDate)
        {
            return milestoneDAL.GetTotalAmount(contractId, fromDate, toDate);
        }

        /// <summary>
        /// Gets milestone billing lines associated with milestone
        /// </summary>
        /// <param name="milestoneVO"></param>
        /// <returns></returns>
        public List<MilestoneBillingLineVO> GetMilestoneBillingLines(MilestoneVO milestoneVO)
        {
            return milestoneDAL.GetMilestoneBillingLines(milestoneVO);
        }

        /// <summary>
        /// Save billing lines from Approve mainatenance screen.
        /// </summary>
        /// <param name="milestoneVO"></param>
        public void SaveBillingLines(MilestoneVO milestoneVO)
        {
            milestoneDAL.SaveBillingLine(milestoneVO);
        }
    }
}