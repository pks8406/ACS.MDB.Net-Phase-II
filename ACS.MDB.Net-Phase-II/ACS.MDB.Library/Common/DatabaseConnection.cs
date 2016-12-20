using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.Common
{
    public static class DatabaseConnection
    {
        /// <summary>
        /// MDB Database context object
        /// </summary>
        public static string DatabaseConnectionString = string.Empty;

        public static int DataContextCommandTimeout = 0;


    }
}
