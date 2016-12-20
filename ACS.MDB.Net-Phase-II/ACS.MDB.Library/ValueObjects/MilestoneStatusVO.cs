using System;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class MilestoneStatusVO : BaseVO
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string MilestoneStatusName { get; set; }
        public string Description { get; set; }
        public Nullable<int> Order { get; set; }

        /// <summary>
        /// Default Consturctor
        /// </summary>
        public MilestoneStatusVO()
        { 
        }

        /// <summary>
        /// Transpose model object to MilestoneStatus value object
        /// </summary>
        /// <param name="milestoneStatus">milestoneStatus model object</param>
        //public MilestoneStatusVO(MODEL.MilestoneStatus milestoneStatus)
        //    : this()
        //{
        //    ID = milestoneStatus.ID;
        //    Status = milestoneStatus.Status;
        //    MilestoneStatusName = milestoneStatus.Status + " - " + milestoneStatus.Description;
        //    Description = milestoneStatus.Description;
        //    Order = milestoneStatus.Order;
        //}

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="milestoneStatus">LINQ object of milestoneStatus</param>
        public MilestoneStatusVO(MilestoneStatus milestoneStatus)
            : this()
        {
            ID = milestoneStatus.ID;
            Status = milestoneStatus.StatusName;
            MilestoneStatusName = milestoneStatus.StatusName + " - " + milestoneStatus.Description;
            Description = milestoneStatus.Description;
            Order = milestoneStatus.Order;
            
        }
    }
}