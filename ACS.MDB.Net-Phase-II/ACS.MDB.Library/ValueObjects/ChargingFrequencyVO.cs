
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ChargingFrequencyVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets the item id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Freqency Name
        /// </summary>
        public string FrequencyName { get; set; }

        /// <summary>
        /// Frequency
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ChargingFrequencyVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="chargeFrequency">LINQ charging frequency object</param>
        public ChargingFrequencyVO(ChargeFrequency chargeFrequency)
        {
            ID = chargeFrequency.ID;
            Frequency = chargeFrequency.Frequency;
            FrequencyName = chargeFrequency.FrequencyName;
        }
    }
}