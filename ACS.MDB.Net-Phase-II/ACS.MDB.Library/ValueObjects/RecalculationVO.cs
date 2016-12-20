using System;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;


namespace ACS.MDB.Library.ValueObjects
{
    public class RecalculationVO : BaseVO
    {
        public int ID { get; set; }

        /// <summary>
        /// Gets or set uplift index ids
        /// </summary>
        public string IndexIds { get; set; }

        /// <summary>
        /// Gets or set recalculation status - while storing value in database
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or set recalculation status for view use only
        /// </summary>
        public string RecalculationStatus { get; set; }

        /// <summary>
        /// Gets or set Is For Uplift required
        /// </summary>
        public bool IsForUpliftRequired { get; set; }

        /// <summary>
        /// Gets or set Recalculation date
        /// </summary>
        public DateTime? RecalculationDate { get; set; }

        /// <summary>
        /// Get or set File Path
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecalculationVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="profitloss">LINQ profitloss object</param>
        public RecalculationVO(Recalculation recalculation)
        {
            ID = recalculation.ID;
            RecalculationStatus = GetRecalculationStatus(recalculation.Status);
            Status = recalculation.Status;
            IsForUpliftRequired = recalculation.IsForUpliftRequired;
            RecalculationDate = recalculation.Date;
            IndexIds = recalculation.IndexIds;
            LogFilePath = recalculation.LogFilePath;
            CreatedByUserId = recalculation.UserId;
            LastUpdatedByUserId = recalculation.UserId;
        }

        /// <summary>
        /// Transpose model object to  value object
        /// </summary>
        /// <param name="profitloss">model objet</param>
        //public RecalculationVO(Recalculation recalculation, int? userId)
        //{
        //    ID = recalculation.ID;
        //    Status = recalculation.Status;
        //    IsForUpliftRequired = recalculation.IsForUpliftRequired;
        //    RecalculationDate = recalculation.RecalculationDate;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}

        /// <summary>
        /// Get Recalculation Status to Display
        /// </summary>
        /// <param name="status">status value</param>
        /// <returns>Recalculation status</returns>
        private string GetRecalculationStatus(int status)
        { 
            string recalStatus = string.Empty;

            if(status == Convert.ToInt32(Constants.RecalculationStatus.IN_PROGRESS))
            {
                recalStatus = Constants.IN_PROGRESS;
            }
            else if(status == Convert.ToInt32(Constants.RecalculationStatus.PENDING))
            {
                recalStatus = Constants.PENDING;
            }
            else if(status == Convert.ToInt32(Constants.RecalculationStatus.COMPLETED))
            {
                recalStatus = Constants.COMPLETED;
            }
            else if(status == Convert.ToInt32(Constants.RecalculationStatus.COMPLETED_WITH_ERRORS))
            {
                recalStatus = Constants.COMPLETED_WITH_ERRORS;
            }

            return recalStatus;
        }
    }
}