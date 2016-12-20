using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;
using System.IO;

namespace ACS.MDB.Net.App.Models
{
    public class Recalculation : BaseModel
    {
        /// <summary>
        /// Gets or set status of recalculation to display
        /// </summary>
        [Display(Name = "Status")]
        public string RecalculationStatus { get; set; }

        /// <summary>
        /// Gets or set uplift index ids
        /// </summary>
        public string IndexIds { get; set; }

        /// <summary>
        /// Gets or set status of recalculation to save
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or set Is for uplift required 
        /// </summary>
        [Display(Name = "Recalculation")]
        public bool IsForUpliftRequired { get; set; }

        /// <summary>
        /// Gets or set Date on which Recalculation process runs
        /// </summary>
        [Display(Name = "Date")]
        public DateTime? RecalculationDate { get; set; }

        /// <summary>
        /// Get or set File Path
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Get or set Is File Available
        /// </summary>
        public bool IsFileAvailable { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Recalculation()
        {
        }

        /// <summary>
        /// Transpose recalculation value object to model object
        /// </summary>
        public Recalculation(RecalculationVO recalculationVO)
        {
            ID = recalculationVO.ID;
            RecalculationStatus = recalculationVO.RecalculationStatus;
            IsForUpliftRequired = recalculationVO.IsForUpliftRequired;
            IndexIds = recalculationVO.IndexIds;
            LogFilePath = recalculationVO.LogFilePath;
            RecalculationDate = recalculationVO.RecalculationDate.HasValue ? recalculationVO.RecalculationDate : null;
            if (LogFilePath != null)            
            {
                IsFileAvailable = IsFileExist(LogFilePath);
            }
        }

        /// <summary>
        /// Check if the file exist or not
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns></returns>
        public bool IsFileExist(string filePath)        
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { 
                "<input type='checkbox' name='check1' value='" + ID + "'>",
                ID, 
                IsForUpliftRequired ? "Recalculate For Uplift Required" + "("+ IndexIds + ")" : "Recalculate For Uplift Not Required" , 
                RecalculationDate.HasValue ? RecalculationDate.Value.ToString(Constants.DATE_TIME_FORMAT) : null,
                RecalculationStatus,
                LogFilePath,
                IsFileAvailable
            };
            return result;
        }
    }
 }