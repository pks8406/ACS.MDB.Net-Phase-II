using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.MDB.Net.App.Models
{
    public class ChargeFrequency : BaseModel
    {
        /// <summary>
        /// Gets or sets charge frequency type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets charge frequency name
        /// </summary>
        public string Frequency { get; set; }
    }
}