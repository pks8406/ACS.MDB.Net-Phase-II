using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class MilestoneDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of milestone
        /// </summary>
        /// <param name="contractMaintenanceId">contractMaintenanceId</param>
        /// <param name="getAllRecords">Flag to identify to get all records or records that are not deleted</param>
        /// <returns>The list of milestones</returns>
        public List<MilestoneVO> GetMilestoneList(int contractMaintenanceId, bool getAllRecords = false)
        {
            List<Milestone> milestoneList = null;
            List<MilestoneVO> milestoneVOList = new List<MilestoneVO>();

            if (getAllRecords)
            {
                milestoneList = mdbDataContext.Milestones.Where(c => c.MaintenanceID == contractMaintenanceId).OrderBy(c => c.EstimatedDate).ToList();
            }
            else
            {
                milestoneList = mdbDataContext.Milestones.Where(c => c.MaintenanceID == contractMaintenanceId && c.IsDeleted == false).OrderBy(c => c.EstimatedDate).ToList();
            }
            foreach (var item in milestoneList)
            {
                milestoneVOList.Add(new MilestoneVO(item));
            }

            return milestoneVOList;
        }

        /// <summary>
        /// Get milestone details by Id
        /// </summary>
        /// <param name="milestoneId">milestone id</param>
        /// <returns>Milestone details</returns>
        public MilestoneVO GetMilestoneById(int milestoneId)
        {
            Milestone milestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == milestoneId);
            MilestoneVO milestoneVO = null;

            if (milestone != null)
            {
                milestoneVO = new MilestoneVO(milestone);
            }

            return milestoneVO;
        }

        /// <summary>
        /// Save recalculated milestones
        /// </summary>
        /// <param name="milestoneVos">list of milestone vos to save</param>
        public void SaveRecalculatedMilestone(List<MilestoneVO> milestoneVos)
        {
            foreach (var milestoneVo in milestoneVos)
            {
                Milestone selectedMilestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == milestoneVo.ID) ??
                                              new Milestone();

                selectedMilestone.ContractID = milestoneVo.ContractID;
                selectedMilestone.ContractLineID = milestoneVo.ContractLineID;
                selectedMilestone.MaintenanceID = milestoneVo.ContractMaintenanceID;
                selectedMilestone.EstimatedDate = milestoneVo.InvoiceDate;
                selectedMilestone.RenewalStartDate = milestoneVo.RenewalStartDate;
                selectedMilestone.RenewalEndDate = milestoneVo.RenewalEndDate;
                selectedMilestone.Amount = Math.Round(milestoneVo.Amount, 2);
                selectedMilestone.PreviousValue = milestoneVo.PreviousValue.Value;
                selectedMilestone.IsApproved = milestoneVo.IsApproved;
                selectedMilestone.MilestoneStatusID = milestoneVo.MilestoneStatusID;

                selectedMilestone.Uplift = milestoneVo.Uplift;
                selectedMilestone.UpliftFixedRate = milestoneVo.UpliftFixedRate;
                selectedMilestone.UpliftPercentage = milestoneVo.UpliftPercentage;
                selectedMilestone.UpliftRate = milestoneVo.UpliftRate;
                selectedMilestone.ChargingUpliftID = milestoneVo.ChargingUpliftID;
                selectedMilestone.IndexRate = milestoneVo.IndexRate;

                selectedMilestone.IsDeleted = milestoneVo.IsDeleted;
                selectedMilestone.LastUpdatedBy = milestoneVo.LastUpdatedByUserId;
                selectedMilestone.LastUpdatedDate = DateTime.Now;

                //Save contract maintenace lines
                foreach (var item in milestoneVo.MilestoneBillingLineVos)
                {
                    if (item.ID != 0)
                    {
                        MilestoneBillingLine milestoneBillingLine =
                           selectedMilestone.MilestoneBillingLines.ToList().Find(x => x.ID == item.ID && !x.IsDeleted);

                        if (milestoneBillingLine != null)
                        {
                            milestoneBillingLine.LineText = item.LineText;
                            milestoneBillingLine.LineSequance = item.LineSequance;
                            milestoneBillingLine.ContractID = selectedMilestone.ContractID;
                            milestoneBillingLine.MilestoneID = selectedMilestone.ID;
                            milestoneBillingLine.IsDeleted = item.IsDeleted;
                            milestoneBillingLine.LastUpdatedBy = selectedMilestone.LastUpdatedBy;
                            milestoneBillingLine.LastUpdatedDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        selectedMilestone.MilestoneBillingLines.Add(new MilestoneBillingLine()
                        {
                            LineText = item.LineText,
                            LineSequance = item.LineSequance,
                            ContractID = selectedMilestone.ContractID,
                            MilestoneID = selectedMilestone.ID,
                            IsDeleted = item.IsDeleted,
                            CreatedBy = selectedMilestone.CreatedBy,
                            CreationDate = DateTime.Now
                        });
                    }
                }

                if (selectedMilestone.ID == 0)
                {
                    selectedMilestone.CreatedBy = milestoneVo.CreatedByUserId;
                    selectedMilestone.CreationDate = DateTime.Now;

                    mdbDataContext.Milestones.InsertOnSubmit(selectedMilestone);
                }
            }
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Update milestone details from Approve Maintenance screen
        /// </summary>
        /// <param name="milestoneVO">Value object of Milestone</param>
        public void UpdateMilestone(MilestoneVO milestoneVO)
        {
            if (milestoneVO.ID != 0)
            {
                //Update Existing Record
                Milestone selectedMilestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == milestoneVO.ID);
                if (selectedMilestone != null)
                {
                    selectedMilestone.Amount = milestoneVO.Amount;
                    selectedMilestone.IsApproved = milestoneVO.IsApproved;
                    selectedMilestone.MilestoneStatusID = milestoneVO.MilestoneStatusID;
                    selectedMilestone.LastUpdatedBy = milestoneVO.LastUpdatedByUserId;
                    selectedMilestone.LastUpdatedDate = DateTime.Now;                    
                    mdbDataContext.SubmitChanges();
                }

            }
        }

        /// <summary>
        /// Save the milestone
        /// </summary>
        /// <param name="milestoneVO">Value object of milestone</param>
        public void SaveMilestone(MilestoneVO milestoneVO)
        {
            if (milestoneVO.ID != 0)
            {
                //Update Existing Record
                Milestone selectedMilestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == milestoneVO.ID);
                selectedMilestone.ContractID = milestoneVO.ContractID;
                selectedMilestone.ContractLineID = milestoneVO.ContractLineID;
                selectedMilestone.MaintenanceID = milestoneVO.ContractMaintenanceID;
                selectedMilestone.EstimatedDate = milestoneVO.InvoiceDate;
                selectedMilestone.RenewalStartDate = milestoneVO.RenewalStartDate;
                selectedMilestone.RenewalEndDate = milestoneVO.RenewalEndDate;
                selectedMilestone.Amount = milestoneVO.Amount;
                selectedMilestone.IsApproved = milestoneVO.IsApproved;
                selectedMilestone.MilestoneStatusID = milestoneVO.MilestoneStatusID;
                selectedMilestone.MilestoneStatus = milestoneVO.MilestoneStatusName;
                selectedMilestone.LastUpdatedBy = milestoneVO.LastUpdatedByUserId;
                selectedMilestone.LastUpdatedDate = DateTime.Now;

                //ContractMaintenance maintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(cm => cm.ID == milestoneVO.ContractMaintenanceID);
                //maintenance.Comment = milestoneVO.Comments;

                //Save contract maintenace lines
                foreach (var item in milestoneVO.MilestoneBillingLineVos)
                {
                    MilestoneBillingLine milestoneBillingLine =
                        selectedMilestone.MilestoneBillingLines.ToList().Find(x => x.ID == item.ID && x.ID != 0);

                    if (milestoneBillingLine != null)
                    {
                        milestoneBillingLine.LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText;
                        milestoneBillingLine.LineSequance = item.LineSequance;
                        milestoneBillingLine.ContractID = milestoneVO.ContractID;
                        milestoneBillingLine.MilestoneID = milestoneVO.ID;
                        milestoneBillingLine.LastUpdatedBy = milestoneVO.LastUpdatedByUserId;
                        milestoneBillingLine.LastUpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        selectedMilestone.MilestoneBillingLines.Add(new MilestoneBillingLine()
                                                                        {
                                                                            LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText,
                                                                            LineSequance = item.LineSequance,
                                                                            ContractID = milestoneVO.ContractID,
                                                                            MilestoneID = milestoneVO.ID,
                                                                            CreatedBy = milestoneVO.CreatedByUserId,
                                                                            CreationDate = DateTime.Now
                                                                        });
                    }
                }

                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Delete Milestones and associated details
        /// </summary>
        /// <param name="Ids">Ids of milestones to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteMilestone(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    //Delete milestones
                    Milestone milestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == id);
                    if (milestone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                    {
                        milestone.IsDeleted = true;
                        milestone.LastUpdatedDate = DateTime.Now;
                        milestone.LastUpdatedBy = userId;
                        //milestone.MilestoneStatusID = 9;
                        //milestone.MilestoneStatus = "RC";

                        //Delete milestone billing line tags
                        List<MilestoneBillingLine> milestoneBillingLines = mdbDataContext.MilestoneBillingLines.Where(x => x.MilestoneID == id).ToList();
                        foreach (var billingLine in milestoneBillingLines)
                        {
                            billingLine.IsDeleted = true;
                            billingLine.LastUpdatedDate = DateTime.Now;
                            billingLine.LastUpdatedBy = userId;
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Return total amount of contracts
        /// </summary>
        /// <param name="id">contract id</param>
        /// <param name="fromDate">from date</param>
        /// <param name="todate">to date</param>
        /// <returns></returns>
        public decimal GetTotalAmount(int id, DateTime fromDate, DateTime todate)
        {
            List<Milestone> milestones = mdbDataContext.Milestones.Where(m => m.ContractID == id && !m.IsDeleted && 
                                                    m.EstimatedDate >= fromDate && m.EstimatedDate <= todate).ToList();

            return milestones.Sum(milestone => milestone.Amount);
        }

        /// <summary>
        /// Gets the Milestone Billing Line based on Milestone
        /// </summary>
        /// <param name="milestoneVO">Value object of milestone</param>
        /// <returns></returns>
        public List<MilestoneBillingLineVO> GetMilestoneBillingLines(MilestoneVO milestoneVO)
        {
            List<MilestoneBillingLine> milestoneBillingLines =
                mdbDataContext.MilestoneBillingLines.Where(
                    b => b.MilestoneID == milestoneVO.ID && b.ContractID == milestoneVO.ContractID && !b.IsDeleted).ToList();

            List<MilestoneBillingLineVO> milestoneBillingLineVos = milestoneBillingLines.Select(milestoneBillingLine => new MilestoneBillingLineVO(milestoneBillingLine)).ToList();

            return milestoneBillingLineVos;
        }

        /// <summary>
        /// Saves Billing Line
        /// </summary>
        /// <param name="milestoneVO">MilestoneVo to save</param>
        public void SaveBillingLine(MilestoneVO milestoneVO)
        {
            if (milestoneVO != null)
            {
                ContractMaintenance maintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(cm => cm.ID == milestoneVO.ContractMaintenanceID);
                //maintenance.Comment = milestoneVO.Comments;
                maintenance.Comment = !string.IsNullOrEmpty(milestoneVO.Comments) ? milestoneVO.Comments.Trim() : milestoneVO.Comments;

                //Save milestone billing details
                foreach (var item in milestoneVO.MilestoneBillingLineVos)
                {
                    MilestoneBillingLine milestoneBillingLine =
                        mdbDataContext.MilestoneBillingLines.SingleOrDefault(b => b.ID == item.ID && !b.IsDeleted);

                    if (milestoneBillingLine != null)
                    {
                        milestoneBillingLine.LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText;
                        milestoneBillingLine.LineSequance = item.LineSequance;
                        milestoneBillingLine.ContractID = milestoneVO.ContractID;
                        milestoneBillingLine.MilestoneID = milestoneVO.ID;
                        milestoneBillingLine.LastUpdatedBy = milestoneVO.LastUpdatedByUserId;
                        milestoneBillingLine.LastUpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        MilestoneBillingLine newMilestoneBillingLine = new MilestoneBillingLine
                                                                           {
                                                                               LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText,
                                                                               LineSequance = item.LineSequance,
                                                                               ContractID = milestoneVO.ContractID,
                                                                               MilestoneID = milestoneVO.ID,
                                                                               CreatedBy = milestoneVO.CreatedByUserId,
                                                                               CreationDate = DateTime.Now
                                                                           };
                        mdbDataContext.MilestoneBillingLines.InsertOnSubmit(newMilestoneBillingLine);
                    }
                }

                mdbDataContext.SubmitChanges();
            }
        }
    }
}