using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.MDB.Net.App.Models
{
    public class ContractDetails : BaseModel
    {
        public Contract contract { get; set; }
        public List<Currency> currencyList { get; set; }
        public List<Company> companyList { get; set; }
        public List<Division> divisionList { get; set; }
        public List<EndUser> endUserList { get; set; }
        public List<ContractLine> contractLine { get; set; }
        public List<ContractMaintenance> contractMaintenance { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractDetails()
        {
            contract = new Contract();
            currencyList = new List<Currency>();
            companyList = new List<Company>();
            divisionList = new List<Division>();
            endUserList = new List<EndUser>();
            contractLine = new List<ContractLine>();
            contractMaintenance = new List<ContractMaintenance>();
        }
    }
}