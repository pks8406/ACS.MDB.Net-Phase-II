using System;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class AuditReason : BaseModel
    {
        /// <summary>
        /// Gets or sets Audit reason code
        /// </summary>
        public int ReasonCode { get; set; }

        /// <summary>
        /// Gets or sets Delete reason code
        /// </summary>
        public int DeleteReason { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AuditReason()
        {
            
        }

        /// <summary>
        /// Gets or sets Audit reason description
        /// </summary>
        [Required(ErrorMessage = "Please enter Reason description")]
        //[RegularExpression("^([a-zA-Z0-9 &.,'-]+)$", ErrorMessage = "Please enter valid reason description")]
        [StringLength(50, ErrorMessage = "Reason description must be with a maximum length of 50")] 
        [Display(Name = "Reason Description")]
        public string ReasonDescription { get; set; }

        /// <summary>
        /// Constructor to transpose VO Object to model object
        /// </summary>
        /// <param name="auditReasonVO"></param>
        public AuditReason(AuditReasonVO auditReasonVO)
        {
            ReasonCode = auditReasonVO.ReasonCode;
            ReasonDescription = auditReasonVO.ReasonDescription;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public AuditReasonVO Transpose(int? userId)
        {
            AuditReasonVO auditReasonVO = new AuditReasonVO();

            auditReasonVO.ReasonCode = this.ID;
            auditReasonVO.ReasonDescription = this.ReasonDescription;
            auditReasonVO.CreatedByUserId = userId;
            auditReasonVO.LastUpdatedByUserId = userId;

            return auditReasonVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (ReasonDescription != null && ReasonDescription.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] {"<input type='checkbox' name='check5' value='" + ReasonCode + "'>", ReasonCode, ReasonDescription };
            return result;
        }
    }
}