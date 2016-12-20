using System;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.DataAccess
{
    public class BaseDAL 
    {
        /// <summary>
        /// MDB Database context object
        /// </summary>
        public MDBDataContext mdbDataContext = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseDAL()
        {
            mdbDataContext = new MDBDataContext(DatabaseConnection.DatabaseConnectionString);

            //To avoid Error : Timeout expired. The timeout period elapsed prior to completion of the operation or the server is not responding.
            mdbDataContext.CommandTimeout = !String.IsNullOrEmpty(DatabaseConnection.DataContextCommandTimeout.ToString()) ?
                Convert.ToInt32(DatabaseConnection.DataContextCommandTimeout) :
                mdbDataContext.CommandTimeout;
            
        }

    }
}