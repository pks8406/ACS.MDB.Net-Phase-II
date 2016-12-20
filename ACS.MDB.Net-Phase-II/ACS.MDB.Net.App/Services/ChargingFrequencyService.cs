using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class ChargingFrequencyService : BaseService
    {
        ChargingFrequencyDAL contractLineDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChargingFrequencyService()
        {
            contractLineDAL = new ChargingFrequencyDAL();
        }


        /// <summary>
        /// Gets the list of charging frequency
        /// </summary>
        /// <returns>List of Charging frequency</returns>
        public List<ChargingFrequencyVO> GetChargingFrequencyList()
        {
            return contractLineDAL.GetChargingFrequencyList();
        }
    }
}