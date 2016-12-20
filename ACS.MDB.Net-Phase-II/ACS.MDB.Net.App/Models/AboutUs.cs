using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACS.MDB.Net.App.Models
{
    public class AboutUs : BaseModel
    {
        /// <summary>
        /// Get or Set Release Notes
        /// </summary>
        [Display(Name=" Release Notes:")]
        public List<string> ReleaseNotes { get; set; }

        /// <summary>
        /// Get or Set Version Number
        /// </summary>
        [Display(Name = "ARBS Version:")]
        public string VersionNumber { get; set; }

        /// <summary>
        /// Get or Set Live Date
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Date Should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name="Live Date:")]
        public DateTime LiveDate { get; set; }
    }
}