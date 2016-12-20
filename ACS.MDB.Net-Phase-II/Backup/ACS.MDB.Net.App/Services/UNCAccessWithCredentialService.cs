using System;
using System.Runtime.InteropServices;
using DWORD = System.UInt32;
using LPWSTR = System.String;
using NET_API_STATUS = System.UInt32;

namespace ACS.MDB.Net.App.Services
{
    public class UNCAccessWithCredentialService : IDisposable
    {
         #region NetApi32 Signatures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal LPWSTR ui2_local;
            internal LPWSTR ui2_remote;
            internal LPWSTR ui2_password;
            internal DWORD ui2_status;
            internal DWORD ui2_asg_type;
            internal DWORD ui2_refcount;
            internal DWORD ui2_usecount;
            internal LPWSTR ui2_username;
            internal LPWSTR ui2_domainname;
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseAdd(
            LPWSTR UncServerName,
            DWORD Level,
            ref USE_INFO_2 Buf,
            out DWORD ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseDel(
            LPWSTR UncServerName,
            LPWSTR UseName,
            DWORD ForceCond);


        #endregion

        /// <summary>
        /// Variables
        /// </summary>
        private bool disposed = false;
        private string uncPath;
        private string user;
        private string password;
        private string domain;
        private int lastError;

        /// <summary>
        /// A disposeable class that allows access to a UNC resource with credentials.
        /// </summary>
        public UNCAccessWithCredentialService()
        {
        }

        /// <summary>
        /// The last system error code returned from NetUseAdd or NetUseDel.  Success = 0
        /// </summary>
        public int LastError
        {
            get { return lastError; }
        }

        /// <summary>
        /// Dispose this obeject by force.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                NetUseDelete();
            }
            disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Connects to a UNC path using the credentials supplied.
        /// </summary>
        /// <param name="UNCPath">Fully qualified domain name UNC path</param>
        /// <param name="User">A user with sufficient rights to access the path.</param>
        /// <param name="Domain">Domain of User.</param>
        /// <param name="Password">Password of User</param>
        /// <returns>True if mapping succeeds.  Use LastError to get the system error code.</returns>
        public bool NetUseWithCredentials(string UNCPath, string User, string Domain, string Password)
        {
            uncPath = UNCPath;
            user = User;
            password = Password;
            domain = Domain;
            return NetUseWithCredentials();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool NetUseWithCredentials()
        {
            uint returncode;
            try
            {
                USE_INFO_2 useinfo = new USE_INFO_2();

                useinfo.ui2_remote = uncPath;
                useinfo.ui2_username = user;
                useinfo.ui2_domainname = domain;
                useinfo.ui2_password = password;
                useinfo.ui2_asg_type = 0;
                //useinfo.ui2_usecount = 1;
                uint paramErrorIndex;
                returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                lastError = (int)returncode;
                return returncode == 0;
            }
            catch
            {
                lastError = Marshal.GetLastWin32Error();
                //Exception ex = Marshal.GetExceptionForHR(lastError);

                return false;
            }
        }

        /// <summary>
        /// Ends the connection to the remote resource 
        /// </summary>
        /// <returns>True if it succeeds.  Use LastError to get the system error code</returns>
        public bool NetUseDelete()
        {
            uint returncode;
            try
            {
                returncode = NetUseDel(null, uncPath, 2);
                lastError = (int)returncode;
                return (returncode == 0);
            }
            catch
            {
                lastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~UNCAccessWithCredentialService()
        {
            Dispose();
        }
    }
}