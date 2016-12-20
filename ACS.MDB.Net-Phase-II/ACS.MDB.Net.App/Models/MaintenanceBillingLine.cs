using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class MaintenanceBillingLine : BaseModel
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
        /// Transpose model object to value object
        /// </summary>
        /// <param name="billingLine">Model maintenance billing line</param>
        public MaintenanceBillingLineVO Transpose ()
        {
            MaintenanceBillingLineVO maintenanceBillingLineVO = new MaintenanceBillingLineVO();

            maintenanceBillingLineVO.BillingLineID = this.ID;
            maintenanceBillingLineVO.MaintenanceID = this.MaintenanceID;
            maintenanceBillingLineVO.LineText = this.LineText;
            maintenanceBillingLineVO.Description = this.Description;
            maintenanceBillingLineVO.LineSequance = this.LineSequance;

            return maintenanceBillingLineVO;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="billingLine"></param>
        public MaintenanceBillingLine(MaintenanceBillingLineVO billingLine)
        {
            BillingLineID = billingLine.BillingLineID;
            MaintenanceID = billingLine.MaintenanceID;
            LineText = billingLine.LineText;
            Description = billingLine.Description;
            LineSequance = billingLine.LineSequance;
        }
    }
}

