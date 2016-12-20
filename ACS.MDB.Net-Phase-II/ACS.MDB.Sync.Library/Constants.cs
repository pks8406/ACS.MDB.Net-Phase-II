using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACS.MDB.Sync.Library
{
    public class Constants
    {

        /// <summary>
        /// Get company data from Open account company table
        /// </summary>
        public const string GetCompanyOA = "SELECT kco, name, del  from PUB.mncompany";

        /// <summary>
        /// Get customer data from open account database
        /// </summary>
        public const string GetCustomerOA = "SELECT customer, name, company,\"vat-code\",currency, del, credate, abbname from PUB.oa_customer order by customer";

        /// <summary>
        /// Get cost centre data from open account
        /// </summary>
        public const string GetCostCentreOA = "SELECT costcentre, name, company, del from PUB.oa_costcentres";

        /// <summary>
        /// Get OA activity codes
        /// </summary>
        public const string GetActivityCodesOA = "SELECT panlcode, name, expensecode, company, del from PUB.oa_pcanalcode";

        /// <summary>
        /// Get OA Account codes
        /// </summary>
        public const string GetAccountCodesOA = "SELECT expensecode, name, company, del from PUB.oa_expensecodes";

        /// <summary>
        /// Get OA job codes
        /// </summary>
        public const string GetJobCodesOA = "SELECT project, name, company, del, customer from PUB.oa_pcproject";

        /// <summary>
        /// Get OA periods
        /// </summary>
        public const string GetPeriodsOA = "SELECT company,del,\"notes-id\",pyear, pdates,yrdsc,enddat,nowks,nodays,maxper,pername   from PUB.oa_periods";

        /// <summary>
        /// Get customer details from ARBS Database
        /// </summary>
        public const string GetCustomerMDB = "SELECT CustomerID, CustomerName, companyID,VatCode, currencyID, IsDeleted, CreationDate, ShortName from OACustomer order by CustomerID";

        /// <summary>
        /// Get company data from MDB
        /// </summary>
        public const string GetCompanyMDB = "SELECT ID, CompanyName, IsDeleted from OACompany";

        /// <summary>
        /// Get job codes from MDB
        /// </summary>
        public const string GetJobCodesMDB = "SELECT JobCodeID, JobCodeName, CompanyID, IsDeleted, Customer from OAJobCode";

        /// <summary>
        /// Get Activity codes from MDB
        /// </summary>
        public const string GetActivityCodesMDB = "SELECT ActivityID, ActivityName, AccountCode, CompanyID, IsDeleted from OAActivityCode";

        /// <summary>
        /// Get Account codes from MDB
        /// </summary>
        public const string GetAccountCodesMDB = "SELECT AccountID, AccountName, CompanyID, IsDeleted from OAAccountCode";

        /// <summary>
        /// Get periods from MDB
        /// </summary>
        public const string GetPeriodMDB = "SELECT CompanyID,IsDeleted,NotesID,Pyear, PDates,YearDescription,EndDate,NoOfWeeks,NoOfDays,MaxPeriod,PeriodName from OAPeriod";

        /// <summary>
        /// Get Cost centre from MDB
        /// </summary>
        public const string GetCostCentreMDB = "SELECT CostCentreID, CostCentreName, CompanyID, IsDeleted from OACostCentre";
    }
}
