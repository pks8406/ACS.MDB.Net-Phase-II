using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ChargingFrequencyDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of charging frequency
        /// </summary>
        /// <returns>List of Charging frequency</returns>
        public List<ChargingFrequencyVO> GetChargingFrequencyList()
        {
            List<ChargeFrequency> chargingFrequencyList = mdbDataContext.ChargeFrequencies.ToList();
            List<ChargingFrequencyVO> chargingFrequencyVOList = new List<ChargingFrequencyVO>();
            foreach (var item in chargingFrequencyList)
            {
                chargingFrequencyVOList.Add(new ChargingFrequencyVO(item));
            }
            return chargingFrequencyVOList;
        }

        /// <summary>
        /// Get Period frequency name by id
        /// </summary>
        /// <param name="chargeFrequencyId">charge frequency id</param>
        /// <returns>Charge frequency name</returns>
        public string GetChargeFrequencyNameById(int chargeFrequencyId)
        {
            string chargeFrequencyName = string.Empty;

            ChargeFrequency chargeFrequency = mdbDataContext.ChargeFrequencies.Where(c => c.ID == chargeFrequencyId).SingleOrDefault();

            if (chargeFrequency != null)
            {
                chargeFrequencyName = chargeFrequency.FrequencyName;
            }

            return chargeFrequencyName;
        }

    }
}