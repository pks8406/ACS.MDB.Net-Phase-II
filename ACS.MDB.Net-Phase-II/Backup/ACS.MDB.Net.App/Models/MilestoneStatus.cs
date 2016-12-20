using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class MilestoneStatus : BaseModel
    {

        public MilestoneStatus()
        {
        }

        public MilestoneStatus(MilestoneStatusVO milestoneStatusVO)
        {
            this.ID = milestoneStatusVO.ID;
            this.Status = milestoneStatusVO.MilestoneStatusName;
            this.MilestoneStatusName = milestoneStatusVO.MilestoneStatusName;
            this.Description = milestoneStatusVO.Description;
            this.Order = milestoneStatusVO.Order;
        }

        /// <summary>
        /// Gets or set Milestone status name
        /// </summary>
        public string MilestoneStatusName { get; set; }

        /// <summary>
        /// Gets or set Milestone description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or set Milestone order
        /// </summary>
        public Nullable<int> Order { get; set; }

        public string Status { get; set; }
    }
}