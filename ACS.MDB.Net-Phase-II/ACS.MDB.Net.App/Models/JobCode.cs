using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class JobCode : BaseModel
    {

        /// <summary>
        /// Gets or set OAJobCode Id
        /// </summary>
        public string OAJobCodeId { get; set; }

        /// <summary>
        /// Gets or set JobCode Name
        /// </summary>
        [Display(Name = "JobCode Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or set concatenated JobCodeName and OAJobCodeId
        /// </summary>
        public string JobCodeName { get; set; }

        /// <summary>
        /// Gets or set Compnay ID
        /// </summary>
        [Display(Name = "Company ID")]
        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or Set Customer Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or set JobCode Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobCode()
        {
        }

        /// <summary>
        /// Transpose JobCode value object to model object
        /// </summary>
        /// <param name="jobCodeVO">value object</param>
        public JobCode(JobCodeVO jobCodeVO)
        {
            ID = jobCodeVO.Id;
            OAJobCodeId = jobCodeVO.OAJobCodeId;
            Name = jobCodeVO.Name;
            JobCodeName = jobCodeVO.OAJobCodeId + '-' + jobCodeVO.Name;
        }
    }
}