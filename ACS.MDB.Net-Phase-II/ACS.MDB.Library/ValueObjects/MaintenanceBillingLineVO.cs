using System;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class MaintenanceBillingLineVO
    {
        /// <summary>
        /// Gets or sets billing line id
        /// </summary>
        public int BillingLineID { get; set; }

        /// <summary>
        /// Gets or sets maintenance id
        /// </summary>
        public Nullable<long> MaintenanceID { get; set; }

        /// <summary>
        /// Gets or sets billing line description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets billing line text
        /// </summary>
        public string LineText { get; set; }

        /// <summary>
        /// Gets or sets billing line sequence
        /// </summary>
        public Nullable<int> LineSequance { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public MaintenanceBillingLineVO()
        {
                
        }

         /// <summary>
        /// Transpose LINQ object to value object
        /// </summary>
        /// <param name="billingLine"></param>
        public MaintenanceBillingLineVO(MaintenanceBillingLine billingLine)
        {
            BillingLineID = billingLine.ID;
            MaintenanceID = billingLine.MaintenanceID;
            LineText = billingLine.LineText;
            Description = billingLine.Description;
            LineSequance = billingLine.LineSequance;
        }
    }
}