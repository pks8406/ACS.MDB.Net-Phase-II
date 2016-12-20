
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class AuditReasonVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets Reason code
        /// </summary>
        public int ReasonCode { get; set; }

        /// <summary>
        /// Gets or sets Audit reason description
        /// </summary>
        public string ReasonDescription { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public AuditReasonVO()
        {
                
        }

        /// <summary> 
        /// Constructor
        /// </summary>
        /// <param name="auditReason"></param>
        public AuditReasonVO(AuditReason auditReason)
        {
            ReasonCode = auditReason.ReasonCode;
            ReasonDescription = auditReason.ReasonDescription;
            CreatedByUserId = auditReason.CreatedBy;
            LastUpdatedByUserId = auditReason.LastUpdatedBy;
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="auditReason"></param>
        //public AuditReasonVO(Models.AuditReason auditReason, int? userId)
        //{
        //    ReasonCode = auditReason.ReasonCode;
        //    ReasonDescription = auditReason.ReasonDescription;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}