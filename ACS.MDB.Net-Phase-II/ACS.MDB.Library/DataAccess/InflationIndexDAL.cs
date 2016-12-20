using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class InflationIndexDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of Inflation Index
        /// </summary>
        /// <returns>List of Inflation Index</returns>
        public List<InflationIndexVO> GetInflationIndexList()
        {
            List<InflationIndexVO> inflationIndexVOList = new List<InflationIndexVO>();
            List<ChargingIndex> inflationIndexList = mdbDataContext.ChargingIndexes.Where(c => c.IsDeleted == false).OrderBy(c => c.ChargingIndex1).ToList();

            foreach (var item in inflationIndexList)
            {
                //Transpose LINQ currency object to value object
                inflationIndexVOList.Add(new InflationIndexVO(item));
            }

            return inflationIndexVOList;
        }

        /// <summary>
        /// Get Inflation index details by Id
        /// </summary>
        /// <param name="indexId">index Id</param>
        /// <returns>IndexId Details</returns>
        public InflationIndexVO GetInflationIndexById(int indexId = 0)
        {
            ChargingIndex inflationIndex = mdbDataContext.ChargingIndexes.SingleOrDefault(c => c.ID == indexId);
            InflationIndexVO inflationIndexVO = null;
            if (inflationIndex != null)
            {
                inflationIndexVO = new InflationIndexVO(inflationIndex);
            }
            return inflationIndexVO;
        }

        /// <summary>
        /// Get Inflation index details name
        /// </summary>
        /// <param name="indexName">index name</param>
        /// <returns>Index details</returns>
        public InflationIndexVO GetInflationIndexByName(string indexName)
        {
            ChargingIndex chargingIndex = mdbDataContext.ChargingIndexes.Where(x => x.ChargingIndex1.Equals(indexName) && x.IsDeleted == false).SingleOrDefault();

            InflationIndexVO inflationIndexVO = null;

            if (chargingIndex != null)
            {
                inflationIndexVO = new InflationIndexVO(chargingIndex);
            }
            return inflationIndexVO;
        }

        /// <summary>
        /// Save the Inflation index
        /// </summary>
        /// <param name="inflationIndexVO">Value Object InflationIndex</param>
        public void SaveInflationIndex(InflationIndexVO inflationIndexVO)
        {
            if (inflationIndexVO.InflationIndexId == 0)
            {
                //Insert New Record
                ChargingIndex chargingIndex = new ChargingIndex();
                chargingIndex.ChargingIndex1 = inflationIndexVO.InflationIndexName;
                chargingIndex.Description = inflationIndexVO.Description.Trim().Replace("\r\n", "\n");
                chargingIndex.IndexUsed = inflationIndexVO.UseIndex;
                chargingIndex.CreationDate = DateTime.Now;
                chargingIndex.CreatedBy = inflationIndexVO.CreatedByUserId;
                mdbDataContext.ChargingIndexes.InsertOnSubmit(chargingIndex);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                ChargingIndex chargingIndex = mdbDataContext.ChargingIndexes.SingleOrDefault(c => c.ID == inflationIndexVO.InflationIndexId);
                chargingIndex.ChargingIndex1 = inflationIndexVO.InflationIndexName;
                chargingIndex.Description = inflationIndexVO.Description.Trim().Replace("\r\n", "\n");
                chargingIndex.IndexUsed = inflationIndexVO.UseIndex;
                chargingIndex.LastUpdatedDate = DateTime.Now;
                chargingIndex.LastUpdatedBy = inflationIndexVO.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Delete the inflationIndex Record(s)
        /// </summary>
        /// <param name="Ids">The inflation index id list to remove</param>
        public void DeleteInflationIndex(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    //To check weather Charging index is associated with contratmaintaince or not
                    List<ContractMaintenance> contractMaintaince = mdbDataContext.ContractMaintenances.Where(c => c.InflationIndexID == id && !c.IsDeleted).ToList();

                    if (contractMaintaince.Count <= 0)
                    {
                        ChargingIndex deleteChargingIndex = new ChargingIndex();
                        deleteChargingIndex = mdbDataContext.ChargingIndexes.FirstOrDefault(c => c.ID == id);

                        if (deleteChargingIndex != null)
                        {
                            deleteChargingIndex.IsDeleted = true;
                            deleteChargingIndex.LastUpdatedBy = userId;
                            deleteChargingIndex.LastUpdatedDate = DateTime.Now;

                            List<ChargingUplift> chargingUpliftList = mdbDataContext.ChargingUplifts.Where
                                (c => c.IndexId == id && c.IsDeleted == false).ToList();

                            foreach (var chargingUplift in chargingUpliftList)
                            {
                                chargingUplift.IsDeleted = true;
                                chargingUplift.LastUpdatedBy = userId;
                                chargingUplift.LastUpdatedDate = DateTime.Now;
                            }
                        }
                    }
                }
            }
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Validate milestone 
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public List<ContractMaintenance> ValidateAdHocAndCredit(List<int> Ids)
        {
            List<ContractMaintenance> contractMaintenances = null;
            foreach (var id in Ids)
            {
                contractMaintenances = mdbDataContext.ContractMaintenances
                                                     .Where(cm => (cm.ChargeFrequencyID == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC) && (cm.FinalRenewalEndDate == null || cm.FinalRenewalStartDate == null))
                                                         && (cm.ChargeFrequencyID == Convert.ToInt32(Constants.ChargeFrequency.CREDIT) && (cm.FinalRenewalEndDate == null || cm.FinalRenewalStartDate == null))
                                                         && cm.InflationIndexID == id && !cm.IsDeleted).ToList();
            }
            return contractMaintenances;
        }

        /// <summary>
        /// Gets the List of Recalculation requests
        /// </summary>
        /// <returns>Recalculation Request List</returns>
        public List<RecalculationVO> GetRecalculationRequestList()
        {
            InflationIndexVO inflationIndexVO = new InflationIndexVO();
            List<RecalculationVO> recalculationVOList = new List<RecalculationVO>();
            List<Recalculation> recalculationList = mdbDataContext.Recalculations.Where(c => c.IsDeleted == false).OrderByDescending(c => c.Date).ToList();


            foreach (var item in recalculationList)
            {
                string indexNames = string.Empty;
                if (item.IndexIds != "0")
                {
                    int length = item.IndexIds.Split(';').Count();
                    for (int i = 0; i < length; i++)
                    {
                        var id = System.Convert.ToInt32(item.IndexIds.Split(';')[i]);
                        inflationIndexVO = GetInflationIndexById(id);
                        indexNames += inflationIndexVO.InflationIndexName + ",";
                        //item.IndexIds += inflationIndexVO.InflationIndexName + ",";
                    }
                    item.IndexIds = string.Empty;
                    item.IndexIds = indexNames.Remove(indexNames.Length - 1);
                }

                recalculationVOList.Add(new RecalculationVO(item));
            }

            //foreach (var item in recalculationList)
            //{
            //    recalculationVOList.Add(new RecalculationVO(item));
            //}


            return recalculationVOList;
        }

        /// <summary>
        /// Save Request for recalculation
        /// </summary>
        /// <param name="recalculationVO">Recalculation Value object</param>
        public void RequestForRecalculation(RecalculationVO recalculationVO)
        {
            //Insert New Record
            Recalculation recalculation = new Recalculation();
            recalculation.IndexIds = recalculationVO.IndexIds;
            recalculation.IsForUpliftRequired = recalculationVO.IsForUpliftRequired;
            recalculation.Status = recalculationVO.Status;
            recalculation.Date = DateTime.Now;
            recalculation.UserId = recalculationVO.CreatedByUserId.Value;

            mdbDataContext.Recalculations.InsertOnSubmit(recalculation);
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Check whether any pending request for recalculation process
        /// </summary>
        /// <param name="ids"></param>
        public RecalculationVO GetRecalculationRequest()
        {
            // Get 
            List<Recalculation> recalculations =
                mdbDataContext.Recalculations.Where(r => !r.IsDeleted &&
                                                         (r.Status == Convert.ToInt32(Constants.RecalculationStatus.PENDING) ||
                                                          r.Status == Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS))).ToList();
            // Check is recalculation already running 
            bool isRecalculationAlreadyRunning =
                recalculations.Any(r => r.Status == Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS));

            RecalculationVO recalculationVO = null;

            if (!isRecalculationAlreadyRunning)
            {

                recalculationVO = (from recalculation in recalculations
                                   where
                                         (recalculation.Status == Convert.ToInt32(Constants.RecalculationStatus.PENDING))
                                   select new RecalculationVO(recalculation)).ToList().Min();
            }

            return recalculationVO;
        }

        /// <summary>
        /// Set Recalculation status
        /// </summary>
        /// <param name="recalculationVO">recalculation VO object</param>
        /// <param name="status">new recalculation status</param>
        public void SetRecalculationStatus(RecalculationVO recalculationVO, int status)
        {
            //RecalculationVO recalculationVO = null;
            Recalculation recalculation = mdbDataContext.Recalculations.SingleOrDefault(r => r.ID == recalculationVO.ID);
            if (recalculation != null)
            {
                recalculation.Status = status;
                recalculation.LogFilePath = recalculationVO.LogFilePath;
                //recalculationVO = new RecalculationVO(recalculation);
                mdbDataContext.SubmitChanges();
            }
            //return recalculationVO;
        }

        /// <summary>
        /// Delete the recalculation Record(s)
        /// </summary>
        /// <param name="Ids">The recalculation id list to remove</param>
        /// <param name="userId">user id</param>
        public void DeleteRecalculation(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    Recalculation recalculation = mdbDataContext.Recalculations.SingleOrDefault(r => r.ID == id);
                    
                    if (recalculation.Status != Convert.ToInt32(Constants.RecalculationStatus.IN_PROGRESS))
                    {
                        recalculation.IsDeleted = true;
                        recalculation.LastUpdatedDate = DateTime.Now;
                        recalculation.LastUpdatedBy = userId;
                    }
                }
            }
            mdbDataContext.SubmitChanges();
        }
    }
}