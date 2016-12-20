using System;
using System.Collections.Generic;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
namespace ACS.MDB.Net.App.Services
{
    public class InflationIndexRateService : BaseService
    {
        InflationIndexRateDAL inflationIndexRateDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InflationIndexRateService()
        {
            inflationIndexRateDAL = new InflationIndexRateDAL();
        }

        /// <summary>
        /// Gets inflation index rates list by inflation index Id
        /// </summary>
        /// <param name="param">The params</param>
        /// <param name="inflationindexId">Inflation Index Id</param>
        /// <returns>inflation index rate list</returns>
        public List<InflationIndexRateVO> GetInflationIndexRateListById(int inflationIndexId)
        {
            return inflationIndexRateDAL.GetInflationIndexRateListById(inflationIndexId);
        }


        /// <summary>
        /// Get inflation index rate by Id
        /// </summary>
        /// <param name="indexRateId">indexRateId</param>
        /// <returns>Inflation Index Rate Details</returns>
        public InflationIndexRateVO GetInflationIndexRateById(int? indexRateId, DateTime? upliftDateFrom = null, DateTime? upliftDateTo = null)
        {
            return inflationIndexRateDAL.GetInflationIndexRateById(indexRateId, upliftDateFrom, upliftDateTo);
        }


        /// <summary>
        /// Save the Inflation index rate
        /// </summary>
        /// <param name="inflationIndexRateVO">Value Object InflationIndexRate</param>
        public void SaveInflationIndexRate(InflationIndexRateVO inflationIndexRateVO)
        {
            //Get and check whether index already exist with same name or not
            InflationIndexRateVO inflationIndexExist = inflationIndexRateDAL.GetInflationIndexRateByDate(inflationIndexRateVO);
            if (inflationIndexExist != null && inflationIndexRateVO.InflationIndexRateId != inflationIndexExist.InflationIndexRateId)
            {
                throw new ApplicationException(Constants.INDEX_RATE_ALREADY_EXIST);
            }
            else
            {
                inflationIndexRateDAL.SaveInflationIndexRate(inflationIndexRateVO);
            }
        }

        /// <summary>
        /// Delete inflation index reate(s)
        /// </summary>
        /// <param name="Ids">Ids of inflation index rate to be deleted</param>
        public void DeleteInflationIndexRate(List<int> Ids, int? userId)
        { 
            inflationIndexRateDAL.DeleteInflationIndexRate(Ids, userId); 
        }

        public InflationIndexRateVO GetChargingUpliftByIdAndDate(int? indexId, DateTime? chargingUpliftDate)
        {
            return inflationIndexRateDAL.GetChargingUpliftByIdAndDate(indexId, chargingUpliftDate);
        }
    }
}