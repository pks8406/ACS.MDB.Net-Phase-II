using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class ActivityCode : BaseModel
    {
        /// <summary>
        /// Gets or set OAActivityCode Id
        /// </summary>
        public string OAActivityCodeId { get; set; }

        /// <summary>
        /// Gets or set ActivityCodeName
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set concatenated OAActivitycodeName and OAActivityCodeId
        /// </summary>
        public string  ActivityCodeName { get; set; }

        /// <summary>
        /// Gets or Set OAAccount Id
        /// </summary>
        public string  AccountCode { get; set; }

        /// <summary>
        /// Gets or Set Account Id
        /// </summary>
        public int? AccountId { get; set; }
        /// <summary>
        /// Gets or set Account Code
        /// </summary>
        public string  OAAccountId { get; set; }

        /// <summary>
        /// Gets or set Company Id
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityCode()
        { 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityCode"></param>
        public ActivityCode(ActivityCodeVO activityCode)
        {
            ID = activityCode.Id;
            OAActivityCodeId = activityCode.OAActivityCodeId;
            Name = activityCode.Name;
            ActivityCodeName = activityCode.Name + '-' + activityCode.OAActivityCodeId;
            OAAccountId = activityCode.OAAccountId;
            AccountId = activityCode.AccountId;
            AccountCode = activityCode.AccountCode;
            CompanyId = activityCode.CompanyId;
        }
    }
}