using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class InflationIndexRateDAL : BaseDAL
    {
        /// <summary>
        /// Gets inflation index rates list by inflation index Id
        /// </summary>
        /// <param name="param">The params</param>
        /// <param name="inflationindexId">Inflation Index Id</param>
        /// <returns>inflation index rate list</returns>
        public List<InflationIndexRateVO> GetInflationIndexRateListById(int inflationIndexId)
        {
            List<InflationIndexRateVO> inflationIndexRateVOList = new List<InflationIndexRateVO>();
            if (inflationIndexId != 0)
            {
                List<ChargingUplift> chargingUpliftList = mdbDataContext.ChargingUplifts
                                                            .Where(c => c.IndexId == inflationIndexId
                                                            && c.IsDeleted == false).ToList();

                foreach (var item in chargingUpliftList)
                {
                    inflationIndexRateVOList.Add(new InflationIndexRateVO(item));
                }
            }
            return inflationIndexRateVOList;
        }

        /// <summary>
        /// Get inflation index rate by Id
        /// </summary>
        /// <param name="indexRateId">indexRateId</param>
        /// <returns>Inflation Index Rate Details</returns>
        public InflationIndexRateVO GetInflationIndexRateById(int? indexRateId, DateTime? upliftDateFrom = null, DateTime? upliftDateTo = null)
        {
            ChargingUplift chargingUplift = null;
            InflationIndexRateVO inflationIndexRateVO = null;

            if (upliftDateFrom.HasValue && upliftDateTo.HasValue)
            {
                chargingUplift = mdbDataContext.ChargingUplifts.FirstOrDefault(c => c.IndexId == indexRateId && !c.IsDeleted &&
                    c.ChargingUpliftDate >= upliftDateFrom &&
                    c.ChargingUpliftDate <= upliftDateTo);
            }
            else
            {
                chargingUplift = mdbDataContext.ChargingUplifts.FirstOrDefault(c => c.ID == indexRateId && !c.IsDeleted);
            }

            if (chargingUplift != null)
            {
                inflationIndexRateVO = new InflationIndexRateVO(chargingUplift);
            }

            return inflationIndexRateVO;
        }

        /// <summary>
        /// Get Charging uplift details by index id & uplift date
        /// </summary>
        /// <param name="indexId">Index id</param>
        /// <param name="chargingUpliftDate">Charging uplift date</param>
        /// <returns></returns>
        public InflationIndexRateVO GetChargingUpliftByIdAndDate(int? indexId, DateTime? chargingUpliftDate)
        {
            ChargingUplift chargingUplift =
                mdbDataContext.ChargingUplifts.FirstOrDefault(c => c.IndexId == indexId &&  !c.IsDeleted &&
                                                                (c.ChargingUpliftDate.Value.Month == chargingUpliftDate.Value.Month
                                                                && c.ChargingUpliftDate.Value.Year == chargingUpliftDate.Value.Year));

            InflationIndexRateVO inflationIndexRateVO = null;
            if (chargingUplift != null)
            {
                inflationIndexRateVO = new InflationIndexRateVO(chargingUplift);
            }

            return inflationIndexRateVO;
        }

        /// <summary>
        /// Get Inflation index rate details date
        /// </summary>
        /// <param name="date">date to look for</param>
        /// <returns>Index rate details</returns>
        public InflationIndexRateVO GetInflationIndexRateByDate(InflationIndexRateVO inflationIndexRateVO)
        {
            ChargingUplift chargingIndex = mdbDataContext.ChargingUplifts.FirstOrDefault(c => c.ChargingUpliftDate != null &&
                                                                                              c.ChargingUpliftDate.Value.Year.Equals(inflationIndexRateVO.chargingUpliftDate.Value.Year) &&
                                                                                              c.ChargingUpliftDate.Value.Month.Equals(inflationIndexRateVO.chargingUpliftDate.Value.Month) &&
                                                                                              c.UpliftIndex == inflationIndexRateVO.IndexName &&
                                                                                              c.IsDeleted == false);

            if (chargingIndex != null)
            {
                inflationIndexRateVO = new InflationIndexRateVO(chargingIndex);
            }
            return inflationIndexRateVO;
        }

        /// <summary>
        /// Save the Inflation index rate
        /// </summary>
        /// <param name="inflationIndexRateVO">Value Object InflationIndexRate</param>
        public void SaveInflationIndexRate(InflationIndexRateVO inflationIndexRateVO)
        {
            if (inflationIndexRateVO.InflationIndexRateId == 0)
            {
                //Insert New Record
                ChargingUplift chargingUplift = new ChargingUplift();
                chargingUplift.IndexId = inflationIndexRateVO.InflationIndexId;
                chargingUplift.UpliftIndex = inflationIndexRateVO.IndexName;
                chargingUplift.ChargingUpliftDate = inflationIndexRateVO.chargingUpliftDate;
                chargingUplift.ActualRate = inflationIndexRateVO.IndexRatePerAnnum;
                chargingUplift.IndexRate = inflationIndexRateVO.IndexRate;
                chargingUplift.CreationDate = DateTime.Now;
                chargingUplift.CreatedBy = inflationIndexRateVO.CreatedByUserId;
                mdbDataContext.ChargingUplifts.InsertOnSubmit(chargingUplift);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                ChargingUplift chargingUplift = mdbDataContext.ChargingUplifts.SingleOrDefault(c => c.ID == inflationIndexRateVO.InflationIndexRateId);
                chargingUplift.IndexId = inflationIndexRateVO.InflationIndexId;
                chargingUplift.UpliftIndex = inflationIndexRateVO.IndexName;
                chargingUplift.ChargingUpliftDate = inflationIndexRateVO.chargingUpliftDate;
                chargingUplift.ActualRate = inflationIndexRateVO.IndexRatePerAnnum;
                chargingUplift.IndexRate = inflationIndexRateVO.IndexRate;
                chargingUplift.LastUpdatedDate = DateTime.Now;
                chargingUplift.LastUpdatedBy = inflationIndexRateVO.LastUpdatedByUserId;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Delete inflation index reate(s)
        /// </summary>
        /// <param name="Ids">Ids of inflation index rate to be deleted</param>
        public void DeleteInflationIndexRate(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    ChargingUplift chargingUplift = new ChargingUplift();
                    chargingUplift = mdbDataContext.ChargingUplifts.SingleOrDefault(c => c.ID == id);
                    chargingUplift.IsDeleted = true;
                    chargingUplift.LastUpdatedBy = userId;
                    chargingUplift.LastUpdatedDate = DateTime.Now;
                }
            }
            mdbDataContext.SubmitChanges();
        }
    }
}