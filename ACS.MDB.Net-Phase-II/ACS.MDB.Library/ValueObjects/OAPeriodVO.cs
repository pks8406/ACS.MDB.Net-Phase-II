using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class OAPeriodVO
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public OAPeriodVO()
        {

        }

        /// <summary>
        /// Transpose OA Period linq object to Value object
        /// </summary>
        /// <param name="oaPeriod"></param>
        public OAPeriodVO(OAPeriod oaPeriod)
        {
            ID = oaPeriod.ID;
            CompanyId = oaPeriod.CompanyID;
            IsDeleted = oaPeriod.IsDeleted;
            PostingYear = oaPeriod.PYear.ToString();
            PostingDates = oaPeriod.PDates;
            MaxPeriod = oaPeriod.MaxPeriod;
        }

        /// <summary>
        /// Gets or set posting period id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or set company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set posting period deleted?
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or set posting year for company
        /// </summary>
        public string PostingYear { get; set; }

        /// <summary>
        /// Gets or set posting dates for company
        /// </summary>
        public string PostingDates { get; set; }

        /// <summary>
        /// Gets or set Max Period for company id
        /// </summary>
        public int MaxPeriod { get; set; }
    }
}