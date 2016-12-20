using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.MDB.Sync.Library
{
    public class CompareData
    {
        private static OdbcConnection OAConnection = null;
        private static SqlConnection MDBConnection = null;
        
        //Insert the table row
        private const string format = "yyyy/MM/dd";

        static List<string> UpdatedRows = new List<string>();
        static List<string> NewAddedRows = new List<string>();
        static List<string> NotMigratedRows = new List<string>();

        private int? userId = 0;

        static int diffCount = 0;
        static string LogFilePath = string.Empty;
        public bool IsError = false;
            
        /// <summary>
        /// Constructore
        /// </summary>
        /// <param name="oaConnection"></param>
        /// <param name="mdbConnection"></param>
        /// <param name="logFilePath"></param>
        public CompareData(OdbcConnection oaConnection, SqlConnection mdbConnection, string logFilePath, int? userId)
        {
            OAConnection = oaConnection;
            MDBConnection = mdbConnection;

            this.userId = userId;
            LogFilePath = logFilePath;
        }

        /// <summary>
        /// Run synchronisation
        /// </summary>
        public void Run()
        {
            SyncCompanyTable();
            SyncCustomerTable();
            SyncCostCentreTable();
            SyncAccountCodeTable();
            SyncActivityCodeTable();
            SyncJobCodeTable();
            SyncPeriodTable();
        }


        /// <summary>
        /// 1. Load Company data from OA & MDB 
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update Company data to MDB database
        /// </summary>
        private void SyncCompanyTable()
        {
            // Load data from OA Database
            DataSet OACompanyDataset = GetCompanyFromOA();

            // Load data from MDB Database
            DataSet MDBCompanyDataset = GetCompanyFromMDB();

            // Compare OA & MDB Database
            // Insert/Update data in MDB Database
            CompareOACompany(OACompanyDataset, MDBCompanyDataset);
        }

        /// <summary>
        /// 1. Load Customer data from OA & MDB 
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update Customer data to MDB database
        /// </summary>
        private void SyncCustomerTable()
        {
            // Load data from OA Database
            DataSet OACustomerDataset = GetCustomerFromOA();

            // Load Customer data from MDB Database
            DataSet MDBCustomerDataset = GetCustomerFromMDB();

            // Compare OA & MDB Database
            // Insert/Update Customer data in MDB Database
            CompareCustomerData(OACustomerDataset, MDBCustomerDataset);
        }

        /// <summary>
        /// 1. Load Cost Centre Code data from OA & MDB 
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update Cost Centre Code data to MDB database
        /// </summary>
        private void SyncCostCentreTable()
        {
            // Load data from OA Database
            DataSet OACostCentreDataset = GetCostCenterFromOA();

            // Load data from MDB Database
            DataSet MDBCostCentreDataset = GetCostCenterFromMDB();

            // Compare OA & MDB Database
            // Insert/Update Cost Centre data in MDB Database
            CompareCostCentreData(OACostCentreDataset, MDBCostCentreDataset);
        }

        /// <summary>
        /// 1. Load Account Code data from OA & MDB 
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update Account Code data to MDB database
        /// </summary>
        private void SyncAccountCodeTable()
        {
            // Get Account code data from OA database
            DataSet OAAccountCodeDataset = GetAccountCodeFromOA();

            // Get Account code data from MDB database
            DataSet MDBAccountCodeDataset = GetAccountCodeFromMDB();

            // Compare OA & MDB data 
            // Insert/Update data in MDB Database
            CompareAccountCodeData(OAAccountCodeDataset, MDBAccountCodeDataset);
        }

        /// <summary>
        /// 1. Load Activity Code data from OA & MDB Database
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update Activity data to MDB Database
        /// </summary>
        private void SyncActivityCodeTable()
        {
            //Get Activity Code from OA Database
            DataSet OAActivityCodeDataset = GetActivityCodeFromOA();

            //Get Activity Code from MDB Database
            DataSet MDBActivityCodeDataset = GetActivityCodeFromMDB();

            //Compare OA & MDB database 
            //Insert/Update Activity data in MDB Database
            CompareActivityCodeData(OAActivityCodeDataset, MDBActivityCodeDataset);
        }

        /// <summary>
        /// 1. Load JobCode data from OA & MDB 
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update JobCode data to MDB database
        /// </summary>
        private void SyncJobCodeTable()
        {
            //Get Job Code from OA Database
            DataSet OAJobCodeDataset = GetJobCodeFromOA();

            //Get JobCode from MDB Database
            DataSet MDBJobCodeDataset = GetJobCodeFromMDB();

            //Compare & Insert/Update data to MDB Database
            CompareJobCodeData(OAJobCodeDataset, MDBJobCodeDataset);
        }

        /// <summary>
        /// 1. Load Period data from OA & MDB Database
        /// 2. Compare OA & MDB data
        /// 3. Insert/Update Period data to MDB Database
        /// </summary>
        private void SyncPeriodTable()
        {
            //Get Period from OA Database
            DataSet OAPeriodDataset = GetPeriodFromOA();

            //Get Period from MDB Database
            DataSet MDBPeriodDataset = GetPeriodFromMDB();

            //Compare & Insert/Update data to MDB Database
            ComparePeriodData(OAPeriodDataset, MDBPeriodDataset);
        }


        #region Compare dataset

        /// <summary>
        /// Compare company of OA & MDB Database
        /// </summary>
        private void CompareOACompany(DataSet OACompanyDataSet, DataSet MDBCompanyDataSet)
        {
            // Compare Company OA & MDB data
            IEnumerable<DataRow> companyDifference = CompareDataset(OACompanyDataSet, MDBCompanyDataSet);

            diffCount = companyDifference.Count();
            if (diffCount > 0)
            {
                //Insert or update company data in MDB Database
                InsertOrUpdateCompanyDifference(companyDifference);
            }
        }

        /// <summary>
        /// Compare customer of OA & MDB
        /// </summary>
        private void CompareCustomerData(DataSet OACustomerDataSet, DataSet MDBCustomerDataSet)
        {
            // Compare Customer OA & MDB data
            IEnumerable<DataRow> customerDifference = CompareDataset(OACustomerDataSet, MDBCustomerDataSet);

            diffCount = customerDifference.Count();
            if (diffCount > 0)
            {
                //Insert or update Customer data in MDB Database
                InsertOrUpdateCustomerDifference(customerDifference);
            }
        }

        /// <summary>
        /// Compare Cost Centre of OA & MDB Database
        /// </summary>
        private void CompareCostCentreData(DataSet OACostCenterDataSet, DataSet MDBCostCenterDataSet)
        {
            IEnumerable<DataRow> costCenterDifference = CompareDataset(OACostCenterDataSet, MDBCostCenterDataSet);

            diffCount = costCenterDifference.Count();//Get the difference count
            if (diffCount > 0)
            {
                //Insert or update CostCenter in MDB Database
                InsertOrUpdateCostCenterDifference(costCenterDifference);
            }
        }

        /// <summary>
        /// Compare Account code of OA & MDB Database
        /// </summary>
        private void CompareAccountCodeData(DataSet OAAccountCodeDataSet, DataSet MDBAccountCodeDataSet)
        {
            IEnumerable<DataRow> accountCodeDifference = CompareDataset(OAAccountCodeDataSet, MDBAccountCodeDataSet);//Compare Result
            diffCount = accountCodeDifference.Count();//Get the difference count

            if (diffCount > 0)
            {
                InsertOrUpdateAccountCodeDifference(accountCodeDifference);//Call the insert or update AccountCode difference method
            }
        }

        /// <summary>
        /// Compare Activity code of OA & MDB database
        /// </summary>
        private void CompareActivityCodeData(DataSet OAActivityCodeDataSet, DataSet MDBActivityCodeDataSet)
        {
            IEnumerable<DataRow> activityCodeDifference = CompareDataset(OAActivityCodeDataSet, MDBActivityCodeDataSet);//Compare Result

            diffCount = activityCodeDifference.Count();//Get the difference count

            if (diffCount > 0)
            {
                //Insert or update Activity Code in MDB database
                InsertOrUpdateActivityCodeDifference(activityCodeDifference);
            }
        }

        /// <summary>
        /// Compare Jobcode of OA & MDB database
        /// </summary>
        private void CompareJobCodeData(DataSet OAJobCodeDataSet, DataSet MDBJobCodeDataSet)
        {
            IEnumerable<DataRow> jobCodeDifference = CompareDataset(OAJobCodeDataSet, MDBJobCodeDataSet);//Compare Result
            diffCount = jobCodeDifference.Count();//Get the difference count

            if (diffCount > 0)
            {
                //Insert or update jobcode in MDB Database
                InsertOrUpdateJobCodeDifference(jobCodeDifference);
            }
        }

        private void ComparePeriodData(DataSet OAPeriodDataSet, DataSet MDBPeriodDataSet)
        {
            IEnumerable<DataRow> periodDifference = CompareDataset(OAPeriodDataSet, MDBPeriodDataSet);//Compare Result
            diffCount = periodDifference.Count();//Get the difference count

            if (diffCount > 0)
            {
                //Insert or update period in MDB Database
                InsertOrUpdatePeriodDifference(periodDifference);
            }
        }

        #endregion

        #region Get Data From OA Tables

        /// <summary>
        /// Get Jobcode details from OA 
        /// </summary>
        private DataSet GetJobCodeFromOA()
        {
            DataSet OAJobCodeDataSet = new DataSet();

            //Fetch JobCode data from OpenAccount Database
            using (OdbcCommand command = new OdbcCommand(Constants.GetJobCodesOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OAJobCodeDataSet);
                }

                transaction.Commit();
                transaction.Dispose();
            }
            return OAJobCodeDataSet;
        }

        /// <summary>
        /// Get Account Code details from OA 
        /// </summary>
        private DataSet GetAccountCodeFromOA()
        {
            DataSet OAAccountCodeDataSet = new DataSet();

            //Fetch AccountCode data from OpenAccount Database
            using (OdbcCommand command = new OdbcCommand(Constants.GetAccountCodesOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OAAccountCodeDataSet);
                }

               transaction.Commit();
               transaction.Dispose();
            }
            return OAAccountCodeDataSet;
        }

        /// <summary>
        /// Get Activity Code details from OA 
        /// </summary>
        private DataSet GetActivityCodeFromOA()
        {
            DataSet OAActivityCodeDataSet = new DataSet();

            //Fetch ActivityCode data from OpenAccount Database
            using (OdbcCommand command = new OdbcCommand(Constants.GetActivityCodesOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OAActivityCodeDataSet);
                }
                transaction.Commit();
                transaction.Dispose();
            }
            return OAActivityCodeDataSet;
        }

        /// <summary>
        /// Get Cost Center details from OA 
        /// </summary>
        private DataSet GetCostCenterFromOA()
        {
            DataSet OACostCenterDataSet = new DataSet();

            //Fetch ActivityCode data from OpenAccount Database
            using (OdbcCommand command = new OdbcCommand(Constants.GetCostCentreOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OACostCenterDataSet);
                }
                transaction.Commit();
                transaction.Dispose();
            }
            return OACostCenterDataSet;
        }

        /// <summary>
        /// Returns list of OACustomers from OA
        /// </summary>
        /// <returns></returns>
        private DataSet GetCustomerFromOA()
        {
            DataSet OACustomerDataSet = new DataSet();

            using (OdbcCommand command = new OdbcCommand(Constants.GetCustomerOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OACustomerDataSet);
                }
                transaction.Commit();
                transaction.Dispose();
            }
            return OACustomerDataSet;
        }

        /// <summary>
        /// Returns list of OACompanies from OA
        /// </summary>
        /// <returns></returns>
        private DataSet GetCompanyFromOA()
        {
            DataSet OACompanyDataSet = new DataSet();

            using (OdbcCommand command = new OdbcCommand(Constants.GetCompanyOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OACompanyDataSet);
                }
                transaction.Commit();
                transaction.Dispose();
            }

            return OACompanyDataSet;
        }
        
        /// <summary>
        /// Returns the List of Period from OA
        /// </summary>
        /// <returns></returns>        
        private DataSet GetPeriodFromOA()
        {
            DataSet OAPeriodDataSet = new DataSet();

            using (OdbcCommand command = new OdbcCommand(Constants.GetPeriodsOA, OAConnection))
            {
                // Fix for ARBS-24
                OdbcTransaction transaction = null;
                transaction = OAConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = transaction;

                using (OdbcDataAdapter adapter = new OdbcDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(OAPeriodDataSet);
                }

                transaction.Commit();
                transaction.Dispose();
            }

            return OAPeriodDataSet;
        }

        #endregion

        #region Get Data From MDB Tables

        /// <summary>
        /// Get Data From Customer Table of MDB
        /// </summary>
        private DataSet GetCustomerFromMDB()
        {
            DataSet MDBCustomerDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetCustomerMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBCustomerDataSet);
                }
            }
            return MDBCustomerDataSet;
        }

        /// <summary>
        /// Get Data From Company Table of MDB
        /// </summary>
        private DataSet GetCompanyFromMDB()
        {
            DataSet MDBCompanyDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetCompanyMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBCompanyDataSet);
                }
            }

            return MDBCompanyDataSet;
        }

        /// <summary>
        /// Get Data From JobCode Table of MDB
        /// </summary>
        private DataSet GetJobCodeFromMDB()
        {
            DataSet MDBJobCodeDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetJobCodesMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBJobCodeDataSet);
                }
            }
            return MDBJobCodeDataSet;
        }

        /// <summary>
        /// Get Data From OAAccountCode Table of MDB
        /// </summary>
        private DataSet GetAccountCodeFromMDB()
        {
            DataSet MDBAccountCodeDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetAccountCodesMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBAccountCodeDataSet);
                }
            }
            return MDBAccountCodeDataSet;
        }

        /// <summary>
        /// Get Data From OAActivityCode Table of MDB
        /// </summary>
        private DataSet GetActivityCodeFromMDB()
        {
            DataSet MDBActivityCodeDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetActivityCodesMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBActivityCodeDataSet);
                }
            }
            return MDBActivityCodeDataSet;
        }

        /// <summary>
        /// Get Data From OACostCenter Table of MDB
        /// </summary>
        private DataSet GetCostCenterFromMDB()
        {
            DataSet MDBCostCenterDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetCostCentreMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBCostCenterDataSet);
                }
            }
            return MDBCostCenterDataSet;
        }

        /// <summary>
        /// Returns the Period data from the MDB database
        /// </summary>
        /// <returns></returns>
        private DataSet GetPeriodFromMDB()
        {
            DataSet MDBPeriodDataSet = new DataSet();

            using (SqlCommand command = new SqlCommand(Constants.GetPeriodMDB, MDBConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(MDBPeriodDataSet);
                }
            }
            return MDBPeriodDataSet;
        }

        #endregion

        /// <summary>
        /// Clear log
        /// </summary>
        private void ClearLog()
        {
            UpdatedRows.Clear();
            NewAddedRows.Clear();
            NotMigratedRows.Clear();
        }

        /// <summary>
        /// Compares the two datasets and returns the difference
        /// </summary>
        /// <param name="dataset1">OADataset</param>
        /// <param name="dataset2">MDBDataset</param>
        /// <returns></returns>
        private IEnumerable<DataRow> CompareDataset(DataSet dataset1, DataSet dataset2)
        {
            DataTable dataTable1 = dataset1.Tables[0];
            DataTable dataTable2 = dataset2.Tables[0];

            // Find the OA data which is not available in MDB
            IEnumerable<DataRow> CustomerDiff = dataTable1.AsEnumerable().Except(dataTable2.AsEnumerable(), DataRowComparer.Default);
            return CustomerDiff;
        }

        #region Insert Or Update Differences Methods

        /// <summary>
        /// Inserts or Updates Company Difference
        /// </summary>
        /// <param name="companyDifference"></param>
        private void InsertOrUpdateCompanyDifference(IEnumerable<DataRow> companyDifference)
        {
            ClearLog();
            string error = string.Empty;
            string log = string.Empty;
            try
            {
                var difference = companyDifference;
                foreach (DataRow row in difference)
                {
                    string companyID = row["kco"].ToString();
                    string selectQuery = "SELECT COUNT(*) FROM OACompany WHERE ID=" + companyID;

                    SqlCommand command = new SqlCommand(selectQuery, MDBConnection);
                    int count = (int)command.ExecuteScalar();
                    
                    string companyName = Convert.ToString(row["name"]).Replace("'", "''");
                    string sql = string.Empty;

                    // log the record
                    log = @"CompanyID - " + companyID + " | Company Name - " + companyName;


                    if (count > 0)
                    {
                        //Update the company details
                        sql = @"Update OACompany SET 
                                             CompanyName='" + companyName +                         // Company name
                                                 "',IsDeleted='" + row["del"] +                     // Is company deleted
                                                 "',CreatedBy=" + 0 +                               // Created by user
                                                 ", CreationDate='" + DateTime.Now.ToString(format) +      // Creation date & time
                                                 "',LastUpdatedBy=" + userId +                           // Last update by user
                                                 ",LastUpdatedDate='" + DateTime.Now.ToString(format) +    // Last updated date & time
                                                 "' WHERE ID=" + companyID + "";
                        
                        command = new SqlCommand(sql, MDBConnection);
                        int result =command.ExecuteNonQuery();
                        if (result == 1)
                        {
                            UpdatedRows.Add("Updated row : " + log);
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        //Insert the table row
                        sql = @"INSERT INTO OACompany 
                                        (ID, CompanyName, IsDeleted, CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate) 
                                        Values (" + companyID + ",'" +        // Company id 
                                                        companyName + "','" +     // Company name 
                                                        row["del"] + "'," +       // IsDeleted company flag
                                                        0 + ",'" +                // Created by username
                                                        DateTime.Now.ToString(format) + "'," +     // Creation date & time
                                                        userId + ",'" +                // Last updated by user
                                                        DateTime.Now.ToString(format) + "')";      // Last updated date & time

                        command = new SqlCommand(sql, MDBConnection);
                        int result =command.ExecuteNonQuery();
                        
                        if (result == 1)
                        {
                            NewAddedRows.Add("New added row :  " + log);
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                       
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Company : " + log  + e.Message;
                IsError = true;
            }
            finally {
                //Write log for company
                WriteLog(1, error);
            }
            
        }

        /// <summary>
        /// Inserts or Updates Customer Difference
        /// </summary>
        /// <param name="customerDifference"></param>
        private void InsertOrUpdateCustomerDifference(IEnumerable<DataRow> customerDifference)
        {
            ClearLog();
            string error = string.Empty;
            string log = string.Empty;

            try
            {
                var difference = customerDifference;

                foreach (DataRow row in difference)
                {
                    string customerID = Convert.ToString(row["customer"]);
                    string companyID = Convert.ToString(row["company"]);
                    string customerName = Convert.ToString(row["name"]).Replace("'", "''");
                    string customerShortName = Convert.ToString(row["abbname"]).Replace("'", "''");

                    log = @"Customer ID - " + customerID +
                                    " | Customer Name - " + customerName +
                                    " | Compnay ID - " + companyID + "";

                    if (!string.IsNullOrEmpty(customerID) || !string.IsNullOrEmpty(companyID))
                    {
                        //Validate company - check whether company id exist in company table or not
                        if (IsCompanyExist(companyID))
                        {
                            string sql = string.Empty;
                            string selectQuery = "SELECT COUNT(*) FROM OACustomer WHERE CustomerID='" + customerID + "' AND CompanyID='" + companyID + "'";
                            SqlCommand command = new SqlCommand(selectQuery, MDBConnection);

                            int count = (int)command.ExecuteScalar();

                            if (count > 0)
                            {
                                //Update the table row
                                sql = @"Update OACustomer SET 
                                           CustomerName='" + customerName +             // Customer name
                                               "', VatCode ='" + (row["vat-code"]) +    //VatCode
                                               "', CurrencyID = '" + (row["currency"]) + // Currency id
                                               "', IsDeleted ='" + (row["del"]) +       // IsDeleted customer flag
                                               "', ShortName = '" + customerShortName +   // Customer Shortname
                                               "' ,LastUpdatedDate='" + DateTime.Now.ToString(format) +  // Update last updated date
                                               "' ,LastUpdatedBy='" + userId +               // Update last update user
                                               "' WHERE CustomerID='" + customerID +
                                                  "' AND CompanyID=" + companyID + "";

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();
                                if (result == 1)
                                {
                                    UpdatedRows.Add("Updated row : " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                            else
                            {

                                DateTime creationDate = Convert.ToDateTime(row["credate"]);
                                sql = @"INSERT INTO OACustomer 
                                                (CustomerID, CustomerName, CompanyID,VatCode, CurrencyID,ShortName, IsDeleted, 
                                                CreationDate, CreatedBy, LastUpdatedBy,LastUpdatedDate) 
                                                Values ('" + customerID + "','" +           // Customer id
                                                                 customerName + "'," +      // Customer name
                                                                 companyID + ",'" +         // Company id
                                                                 row["vat-code"] + "','" +
                                                                 (row["currency"]) + "','" + // Currency id
                                                                 customerShortName + "','" +  // Customer shortname
                                                                 row["del"] + "','" +       // IsDeleted customer flag
                                                                 creationDate.ToString(format) + "'," +    // Customer creation date & time
                                                                 0 + "," +                  // Created by user
                                                                 userId + ",'" +                 // Last updated by user 
                                                                 DateTime.Now.ToString(format) + "' )";      // Last update date & time

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();

                                if (result == 1)
                                {
                                    NewAddedRows.Add("New added row :  " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        NotMigratedRows.Add("Not migrated row : " + log);
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Customer : " + log + " -- " + e.Message.ToString();
                IsError = true;
            }
            finally
            {
                //Write log for customer
                WriteLog(2, error);
            }
        }

        /// <summary>
        /// Inserts or updates Account Code difference
        /// </summary>
        /// <param name="accountCodeDifference"></param>
        private void InsertOrUpdateAccountCodeDifference(IEnumerable<DataRow> accountCodeDifference)
        {
            ClearLog();
            string error = string.Empty;
            string log = string.Empty;

            try
            {
                var difference = accountCodeDifference;
                foreach (DataRow row in difference)
                {
                    string accountCodeId = Convert.ToString(row["expensecode"]);
                    string companyID = Convert.ToString(row["company"]);
                    string accountName = Convert.ToString(row["name"]).Replace("'", "''");

                    log = @"AccountCode ID - " + accountCodeId +
                                  " | Account Name - " + accountName +
                                  " | Compnay ID - " + companyID + "";

                    if (!string.IsNullOrEmpty(accountCodeId) && !string.IsNullOrEmpty(companyID))
                    {
                        if (IsCompanyExist(companyID))
                        {
                            string sql = string.Empty;
                            string selectQuery = "SELECT COUNT(*) FROM OAAccountCode WHERE AccountID ='" + accountCodeId + "' AND CompanyID=" + companyID;
                            SqlCommand command = new SqlCommand(selectQuery, MDBConnection);

                            int count = (int)command.ExecuteScalar();

                            if (count > 0)
                            {
                                //Update the table row
                                sql = @"Update OAAccountCode SET 
                                                   AccountName= '" + accountName                // Update account name
                                                       + "', IsDeleted = '" + row["del"]
                                                       + "', CompanyID=" + companyID                // company id
                                                       + ", LastUpdatedDate='" + DateTime.Now.ToString(format)// last updated date & time
                                                       + "', LastUpdatedBy=" + userId +                   // last updated by user
                                                         " WHERE AccountID= '" + accountCodeId +
                                                            "' AND CompanyID=" + companyID;

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();
                                if (result == 1)
                                {
                                    UpdatedRows.Add("Updated row : " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                            else
                            {
                                //Insert the table row
                                sql = @"INSERT INTO OAAccountCode 
                                                (AccountID, AccountName, CompanyID, IsDeleted, CreationDate, 
                                                CreatedBy, LastUpdatedDate, LastUpdatedBy) 
                                                Values ('" + accountCodeId + "','" +          // Account code id
                                                                 accountName + "'," +             // Account name
                                                                 companyID + ",'" +               // associated company id
                                                                 row["del"] + "','" +             // isDeleted flag
                                                                 DateTime.Now.ToString(format) + "'," +            // Creation date & time
                                                                 0 + ",'" +                       // Created by user
                                                                 DateTime.Now.ToString(format) + "'," +            // Last updated date & time
                                                                 userId + ")";                         // Last updated by user

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();

                                if (result == 1)
                                {
                                    NewAddedRows.Add("New added row :  " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        NotMigratedRows.Add("Not migrated row : " + log);
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Account Code : " + log + " -- " + e.Message;
                IsError = true;
            }
            finally {
                //Write log for Account Code
                WriteLog(4, error);

            }
        }

        /// <summary>
        /// Inserts or Updates CostCenter difference 
        /// </summary>
        /// <param name="costCenterDifference"></param>
        private void InsertOrUpdateCostCenterDifference(IEnumerable<DataRow> costCenterDifference)
        {
            ClearLog();
            string error = string.Empty;
            string log = string.Empty;

            try
            {
                var difference = costCenterDifference;
                foreach (DataRow row in difference)
                {
                    string costCenterID = Convert.ToString(row["costcentre"]);
                    string companyID = Convert.ToString(row["company"]);
                    string costCentreName = Convert.ToString(row["name"]).Replace("'", "''");

                    log = @"Cost Centre ID - " + costCenterID +
                                    " | Cost Centre Name - " + costCentreName +
                                    " | Compnay ID - " + companyID;

                    if (!string.IsNullOrEmpty(costCenterID) && !string.IsNullOrEmpty(companyID))
                    {
                        if (IsCompanyExist(companyID))
                        {
                            string sql = string.Empty;
                            string selectQuery = "SELECT COUNT(*) FROM OACostCentre WHERE CostCentreID ='" + costCenterID + "' AND CompanyID=" + companyID + "";

                            SqlCommand command = new SqlCommand(selectQuery, MDBConnection);
                            int count = (int)command.ExecuteScalar();

                            if (count > 0)
                            {
                                //Update the table row
                                sql = @"Update OACostCentre SET 
                                                  CostCentreName='" + costCentreName +              //Cost centre name
                                                      "', IsDeleted ='" + row["del"] +                  // IsDeleted cost centre flag
                                                      "', CompanyID ='" + companyID +                   // associted company id
                                                      "', LastUpdatedDate='" + DateTime.Now.ToString(format) +           // Last updated date & time
                                                      "', LastUpdatedBy=" + userId +                         // Last updated by user 
                                                          " WHERE CostCentreID='" + costCenterID +
                                                             "' AND CompanyID=" + companyID;

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();
                                if (result == 1)
                                {
                                    UpdatedRows.Add("Updated row : " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                            else
                            {
                                //Insert the table row
                                sql = @"INSERT INTO OACostCentre 
                                                            (CostCentreID, CostCentreName, CompanyID, IsDeleted, 
                                                             CreatedBy, CreationDate, LastUpdatedBy,LastUpdatedDate) 
                                                             Values ('" + costCenterID + "','" +    // Cost Centre id 
                                                                         costCentreName + "'," +        // Cost Centre name
                                                                         companyID + ",'" +             // Associated company id
                                                                         row["del"] + "'," +            // IsDeleted cost centre flag
                                                                         0 + ",'" +                     // Created by user
                                                                         DateTime.Now.ToString(format) + "'," +          // Created by date & time
                                                                         userId + ",'" +                     // Last updated by user
                                                                         DateTime.Now.ToString(format) + "')";           // Last updated date & time

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();

                                if (result == 1)
                                {
                                    NewAddedRows.Add("New added row :  " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        NotMigratedRows.Add("Not migrated row : " + log);
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Cost Centre : " + log + " -- " + e.Message.ToString();
                IsError = true;
            }
            finally
            {
                //Write log for Cost Centre
                WriteLog(3, error);
            }
        }

        /// <summary>
        /// Inserts or Updates Activity Code Difference
        /// </summary>
        /// <param name="activityCodeDifference"></param>
        private void InsertOrUpdateActivityCodeDifference(IEnumerable<DataRow> activityCodeDifference)
        {
            ClearLog();
            string error = string.Empty;
            string log = string.Empty;

            try
            {
                var difference = activityCodeDifference;
                foreach (DataRow row in difference)
                {
                    string activityID = Convert.ToString(row["panlcode"]);
                    string activityName = Convert.ToString(row["name"]).Replace("'", "''");
                    string companyID = Convert.ToString(row["company"]);
                    string accountCode = Convert.ToString(row["expensecode"]);

                    log = @"Activity code ID - " + activityID +
                                    " | Activity name - " + activityName +
                                    " | Compnay ID - " + companyID +
                                    " | Account code - " + accountCode;


                    if (!string.IsNullOrEmpty(activityID) && !string.IsNullOrEmpty(companyID) && !string.IsNullOrEmpty(accountCode))
                    {
                        if (IsCompanyExist(companyID))
                        {
                            // Get surrogate key of account code 
                            int newAccountID = GetNewAccountCodeID(accountCode, companyID);
                            if (newAccountID > 0)
                            {
                                string sql = string.Empty;
                                string selectQuery = "SELECT COUNT(*) FROM OAActivityCode WHERE ActivityID='" + activityID + "' AND CompanyID=" + companyID;

                                SqlCommand command = new SqlCommand(selectQuery, MDBConnection);
                                int count = (int)command.ExecuteScalar();

                                if (count > 0)
                                {
                                    //Update the table row
                                    sql = @"Update OAActivityCode SET 
                                                        ActivityName='" + activityName              // Activity name
                                                            + "', IsDeleted ='" + row["del"]            // IsDeleted flag
                                                            + "', AccountCode = '" + accountCode        // Account code
                                                            + "', AccountCodeID = '" + newAccountID     // New account code id, surrogate key for account code
                                                            + "', LastUpdatedDate='" + DateTime.Now.ToString(format)      // Last updated date & time
                                                            + "', LastUpdatedBy=" + userId                    // Last updated by user
                                                            + " WHERE ActivityID='" + activityID
                                                            + "' AND CompanyID=" + companyID;

                                    command = new SqlCommand(sql, MDBConnection);
                                    int result = command.ExecuteNonQuery();
                                    if (result == 1)
                                    {
                                        UpdatedRows.Add("Updated row : " + log);
                                    }
                                    else
                                    {
                                        NotMigratedRows.Add("Not migrated row : " + log);
                                    }
                                }
                                else
                                {
                                    //Insert new activity code
                                    sql = @"INSERT INTO OAActivityCode 
                                                    (ActivityID, ActivityName, AccountCodeID, AccountCode, CompanyID, IsDeleted, 
                                                    CreatedBy, CreationDate, LastUpdatedBy,LastUpdatedDate) 
                                                    Values ('" + activityID + "','"         // Old Activity ID
                                                                   + activityName + "','"       // ActivityName
                                                                   + newAccountID + "','"       // New account id surrogate key
                                                                   + accountCode + "',"         // Old Account code
                                                                   + companyID + ",'"           // CompanyID
                                                                   + row["del"] + "',"          // IsDeleted
                                                                   + 0 + ",'"                   // Created by Userid
                                                                   + DateTime.Now.ToString(format) + "',"        // Creation datetime
                                                                   + userId + ",'"                   // Updated by user id
                                                                   + DateTime.Now.ToString(format) + "')";       // Updated datetime

                                    command = new SqlCommand(sql, MDBConnection);
                                    int result = command.ExecuteNonQuery();

                                    if (result == 1)
                                    {
                                        NewAddedRows.Add("New added row :  " + log);
                                    }
                                    else
                                    {
                                        NotMigratedRows.Add("Not migrated row : " + log);
                                    }
                                }
                            }
                            else
                            {
                                NotMigratedRows.Add("Not migrated row : " + log);
                            }
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        NotMigratedRows.Add("Not migrated row : " + log);
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Activity Code : " + log + " -- " + e.Message.ToString();
                IsError = true;
            }
            finally
            {
                //Write log for Cost Centre
                WriteLog(5, error);
            }
        }

        /// <summary>
        /// Inserts or updates Job Code difference 
        /// </summary>
        /// <param name="jobCodeDifference"></param>
        private void InsertOrUpdateJobCodeDifference(IEnumerable<DataRow> jobCodeDifference)
        {
            ClearLog();

            string error = string.Empty;
            string log = string.Empty;

            try
            {
                var difference = jobCodeDifference;
                foreach (DataRow row in difference)
                {
                    string jobCodeID = Convert.ToString(row["project"]);
                    string jobCodeName = Convert.ToString(row["name"]).Replace("'", "''");
                    string companyID = Convert.ToString(row["company"]);
                    string customerID = Convert.ToString(row["customer"]);

                    log = @"Job Code ID - " + jobCodeID +
                            " | Job Code Name - " + jobCodeName +
                            " | Compnay ID - " + companyID +
                            " | Customer ID - " + customerID;

                    if (!string.IsNullOrEmpty(jobCodeID) && !string.IsNullOrEmpty(companyID) && !string.IsNullOrEmpty(customerID))
                    {
                        if (IsCompanyExist(companyID))
                        {
                            // Get new customer id - surrogate key of customer
                            int newCustomerID = GetNewCustomerID(customerID, companyID);
                            if (newCustomerID > 0)
                            {
                                string sql = string.Empty;
                                string selectQuery = "SELECT COUNT(*) FROM OAJobCode WHERE JobCodeID='" + jobCodeID + "' AND CompanyID=" + companyID + "";
                                SqlCommand command = new SqlCommand(selectQuery, MDBConnection);

                                int count = (int)command.ExecuteScalar();
                                if (count > 0)
                                {
                                    //Update the table row
                                    sql = @"Update OAJobCode SET 
                                               JobCodeName='" + jobCodeName + // Job code name
                                                   "', Customer='" + customerID +  // Old customer name
                                                   "', CustomerID='" + newCustomerID + // new surrogate key for customer
                                                   "', CompanyID='" + companyID + // company id
                                                   "', IsDeleted='" + row["del"] + // deleted flag
                                                   "', LastUpdatedBy='" + userId + // last update by user id
                                                   "', LastUpdatedDate='" + DateTime.Now.ToString(format) + // last updated date
                                                       "' WHERE JobCodeID='" + jobCodeID +
                                                       "' AND CompanyID=" + companyID + "";

                                    command = new SqlCommand(sql, MDBConnection);
                                    int result = command.ExecuteNonQuery();
                                    if (result == 1)
                                    {
                                        UpdatedRows.Add("Updated row : " + log);
                                    }
                                    else
                                    {
                                        NotMigratedRows.Add("Not migrated row : " + log);
                                    }
                                }
                                else
                                {
                                    //Insert the table row
                                    sql = @"INSERT INTO OAJobCode 
                                                    (JobCodeID, JobCodeName, CompanyID, IsDeleted, Customer, CustomerID, 
                                                    CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate) 
                                                    Values ('" + jobCodeID + "','" +        // Job Code id
                                                                     jobCodeName + "'," +       // Job Code name
                                                                     companyID + ",'" +         // Company id
                                                                     row["del"] + "','" +       // IsDeleted flag
                                                                     customerID + "','" +       // Old customer id
                                                                     newCustomerID + "'," +     // New customer id - surrogate key of customer
                                                                     0 + ", '" +                // Created by user 
                                                                     DateTime.Now.ToString(format) + "'," +      // Creation date & time
                                                                     userId + ",'" +                 // Last updated by user
                                                                     DateTime.Now.ToString(format) + "')";       // Last updated date & time

                                    command = new SqlCommand(sql, MDBConnection);
                                    int result = command.ExecuteNonQuery();

                                    if (result == 1)
                                    {
                                        NewAddedRows.Add("New added row :  " + log);
                                    }
                                    else
                                    {
                                        NotMigratedRows.Add("Not migrated row : " + log);
                                    }
                                }
                            }
                            else
                            {
                                NotMigratedRows.Add("Not migrated row : " + log);
                            }
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        NotMigratedRows.Add("Not migrated row : " + log);
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Job Code : " + log + " -- " + e.Message.ToString();
                IsError = true;
            }
            finally
            {
                //Write log for Job Code
                WriteLog(6, error);
            }
        }

        /// <summary>
        /// Inserts or updates Period difference
        /// </summary>
        /// <param name="periodDifference"></param>
        private void InsertOrUpdatePeriodDifference(IEnumerable<DataRow> periodDifference)
        {
            ClearLog();
            string error = string.Empty;
            string log = string.Empty;
            try
            {
                var difference = periodDifference;
                foreach (DataRow row in difference)
                {
                    string companyID = row["company"].ToString();
                    int pYear= Convert.ToInt32(row["pyear"]);

                    // log the record
                    log = @"CompanyID - " + companyID + " | PYear - " + pYear;

                    if (!string.IsNullOrEmpty(companyID))
                    {
                        //Validate company - check whether company id exist in company table or not
                        if (IsCompanyExist(companyID))
                        {

                            // Check for null value in end date column
                            string endDate = (row["enddat"] is DBNull)
                                                 ? "Null"
                                                 : "'" + Convert.ToDateTime(row["enddat"]).ToString(format) + "'";

                            string selectQuery = "SELECT COUNT(*) FROM OAPeriod WHERE CompanyID=" + companyID +
                                                 " AND PYear=" +
                                                 pYear;

                            SqlCommand command = new SqlCommand(selectQuery, MDBConnection);
                            int count = (int) command.ExecuteScalar();

                            string sql = string.Empty;

                        


                            if (count > 0)
                            {

                                //Update the period details
                                sql = @"Update OAPeriod SET IsDeleted='" + row["del"] + // Is company deleted
                                      "',NotesID='" + row["notes-id"] +
                                      "',PYear =" + pYear +
                                      ",PDates='" + row["pdates"] +
                                      "',YearDescription='" + row["yrdsc"] +
                                      "',EndDate=" + endDate +
                                      ",NoOfWeeks='" + row["nowks"] +
                                      "',NoOfDays='" + row["nodays"] +
                                      "',MaxPeriod=" + row["maxper"] +
                                      ",PeriodName='" + row["pername"] +
                                      "' WHERE CompanyID=" + companyID + " AND PYear=" + pYear;

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();
                                if (result == 1)
                                {
                                    UpdatedRows.Add("Updated row : " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                            else
                            {
                                //Insert the table row
                                sql = @"INSERT INTO OAPeriod 
                                        (CompanyID,IsDeleted,NotesID,PYear,PDates,YearDescription,EndDate,NoOfWeeks,NoOfDays,MaxPeriod,PeriodName) 
                                        Values (" + companyID + ",'" + // Company id 
                                      row["del"] + "','" + // IsDeleted company flag
                                      row["notes-id"] + "'," +
                                      row["pyear"] + ",'" +
                                      row["pdates"] + "','" +
                                      row["yrdsc"] + "'," +
                                      endDate + ",'" +
                                      row["nowks"] + "','" +
                                      row["nodays"] + "','" +
                                      row["maxper"] + "','" +
                                      row["pername"] + "')";

                                command = new SqlCommand(sql, MDBConnection);
                                int result = command.ExecuteNonQuery();

                                if (result == 1)
                                {
                                    NewAddedRows.Add("New added row :  " + log);
                                }
                                else
                                {
                                    NotMigratedRows.Add("Not migrated row : " + log);
                                }
                            }
                        }
                        else
                        {
                            NotMigratedRows.Add("Not migrated row : " + log);
                        }
                    }
                    else
                    {
                        NotMigratedRows.Add("Not migrated row : " + log);
                    }
                }
            }
            catch (Exception e)
            {
                error = "Synchronisation Failed for Period : " + log + e.Message;
                IsError = true;
            }
            finally
            {
                //Write log for period
                WriteLog(7, error);
            }

        }

        #endregion

        /// <summary>
        /// Check whether companyid exist in company table or not
        /// </summary>
        /// <param name="companyID">company id</param>
        /// <returns>if exist return true; else false</returns>
        private bool IsCompanyExist(string companyID)
        {
            bool valid = true;
            string sql = "SELECT ID from OACompany WHERE ID = " + companyID;

            using (SqlCommand command = new SqlCommand(sql, MDBConnection))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    valid = false;
                }
                reader.Close();
            }
            return valid;
        }

        /// <summary>
        /// Get new surrogate key of customer 
        /// </summary>
        /// <param name="customerID">Old customer id</param>
        /// <param name="companyID">Company id</param>
        /// <returns>return surrogate key of customer else 0</returns>
        private int GetNewCustomerID(string customerID, string companyID)
        {
            int newCustomerID = 0;
            string sql = @"SELECT ID FROM OACustomer WHERE 
                                                     CustomerID ='" + customerID + "'"
                                                     + " AND CompanyID =" + companyID;

            using (SqlCommand command = new SqlCommand(sql, MDBConnection))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        newCustomerID = Convert.ToInt32(reader["ID"]);
                    }
                }
                reader.Close();
            }

            return newCustomerID;
        }

        /// <summary>
        /// Get surrogate key for account code
        /// </summary>
        /// <param name="accountCode">Old account code</param>
        /// <param name="companyID">Company id</param>
        /// <returns>return surrogate key of account code; else 0</returns>
        private int GetNewAccountCodeID(string accountCode, string companyID)
        {
            int accountID = 0;
            string sql = @"SELECT ID FROM OAAccountCode WHERE 
                                                     AccountID ='" + accountCode + "'"
                                                     + " AND CompanyID =" + companyID;

            using (SqlCommand command = new SqlCommand(sql, MDBConnection))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        accountID = Convert.ToInt32(reader["ID"]);
                    }
                }
                reader.Close();
            }

            return accountID;
        }

        /// <summary>
        /// Write log for respective table
        /// </summary>
        /// <param name="entityID">entity name/table name</param>
        private void WriteLog(int entityID, string error)
        {
            List<string> logMessage = new List<string>();

            UpdatedRows.Add("Total Updated Rows = " + UpdatedRows.Count + Environment.NewLine);
            NewAddedRows.Add("Total Added Rows = " + NewAddedRows.Count + Environment.NewLine);
            
            if (NotMigratedRows.Count > 0)
            {
                NotMigratedRows.Add("Total Not Migrated Rows = " + NotMigratedRows.Count + Environment.NewLine);
                NotMigratedRows.Add("Possible reason for not migrating data – May be they are orphaned data.");
            }

            logMessage.AddRange(UpdatedRows);
            logMessage.AddRange(NewAddedRows);
            logMessage.AddRange(NotMigratedRows);

            if (!string.IsNullOrEmpty(error))
            {
                logMessage.Add(error);
            }

            MDBFileLogger Logger = new MDBFileLogger(LogFilePath);
            Logger.Write(logMessage, entityID);
            
        }
    }
}
