using System;
using System.Collections.Generic;
using System.Web;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Common
{
    /// <summary>
    /// This class is responsible to do session management for whole application
    /// </summary>
    public static class SessionHelper
    {
        /// <summary>
        /// Set logged in user id
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetUserId(this HttpSessionStateBase session, int? UserId)
        {
            if (session[SessionKeys.UserId] == null)
            {
                session[SessionKeys.UserId] = UserId;
            }
        }

        /// <summary>
        /// Set logged in user information
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetUser(this HttpSessionStateBase session, UserVO userVO)
        {
            if (session[SessionKeys.User] == null)
            {
                session[SessionKeys.User] = userVO;
            }
        }

        /// <summary>
        /// Set logged in user type (User role information)
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetUserType(this HttpSessionStateBase session, string userType)
        {
            if (session[SessionKeys.UserType] == null)
            {
                session[SessionKeys.UserType] = userType;
            }
        }

        /// <summary>
        /// Set logged in user associated company list
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetUserAssociatedCompanyList(this HttpSessionStateBase session, List<Company> companyList)
        {
            if (session[SessionKeys.AssociatedCompanyList] == null)
            {
                session[SessionKeys.AssociatedCompanyList] = companyList;
            }
        }

        /// <summary>
        /// Set execute report permission for logged in user (User role information)
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetExecuteReportPermission(this HttpSessionStateBase session, bool canExecuteReport)
        {
            if (session[SessionKeys.CanExecuteReport] == null)
            {
                session[SessionKeys.CanExecuteReport] = canExecuteReport;
            }
        }

        /// <summary>
        /// Set logged in user email id
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetUserEmailId(this HttpSessionStateBase session, string userEmailId)
        {
            if (session[SessionKeys.UserEmailId] == null)
            {
                session[SessionKeys.UserEmailId] = userEmailId;
            }
        }

        /// <summary>
        /// Set Default company id for logged in user
        /// </summary>
        /// <param name="session">The session object</param>
        public static void SetDefaultCompanyId(this HttpSessionStateBase session, int? DefaultCompanyId)
        {
            if (session[SessionKeys.DefaultCompanyId] == null)
            {
                session[SessionKeys.DefaultCompanyId] = DefaultCompanyId;
            }
        }

        /// <summary>
        /// Gets session timeout in seconds.
        /// </summary>
        /// <param name="session">The session object</param>
        /// <returns>Session timeout in seconds</returns>
        public static void SetSessionTimeOut(this HttpSessionStateBase session, int timeout)
        {
            session.Timeout = timeout;
        }

        /// <summary>
        /// Get logged in user information
        /// </summary>
        /// <param name="session">The session object</param>
        public static int? GetUserId(this HttpSessionStateBase session)
        {
            return session[SessionKeys.UserId] == null ? null : (int?)session[SessionKeys.UserId];
        }

        /// <summary>
        /// Get logged in user information
        /// </summary>
        /// <param name="session">The session object</param>
        public static UserVO GetUser(this HttpSessionStateBase session)
        {
            return session[SessionKeys.User] == null ? null : (UserVO)session[SessionKeys.User];
        }

        /// <summary>
        /// Get logged in user type (User role information)
        /// </summary>
        /// <param name="session">The session object</param>
        public static List<Company> GetUserAssociatedCompanyList(this HttpSessionStateBase session)
        {
            return session[SessionKeys.AssociatedCompanyList] == null ? null : (List<Company>)session[SessionKeys.AssociatedCompanyList];
        }

        /// <summary>
        /// Get logged in user type (User role information)
        /// </summary>
        /// <param name="session">The session object</param>
        public static string GetUserType(this HttpSessionStateBase session)
        {
            return session[SessionKeys.UserType] == null ? String.Empty : (string)session[SessionKeys.UserType];
        }

        /// <summary>
        /// Get execute report permission for logged in user (User role information)
        /// </summary>
        /// <param name="session">The session object</param>
        public static bool GetExecuteReportPermission(this HttpSessionStateBase session)
        {
            return session[SessionKeys.CanExecuteReport] == null ? false : (bool)session[SessionKeys.CanExecuteReport];
        }

        /// <summary>
        /// Get logged in user email id
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetUserEmailId(this HttpSessionStateBase session)
        {
            return session[SessionKeys.UserEmailId] == null ? String.Empty : (string)session[SessionKeys.UserEmailId];
        }

        /// <summary>
        /// Get Default company id associate with logged in user
        /// </summary>
        /// <param name="session">The session object</param>
        public static int? GetDefaultCompanyId(this HttpSessionStateBase session)
        {
            return session[SessionKeys.DefaultCompanyId] == null ? 0 : (int?)session[SessionKeys.DefaultCompanyId];
        }

        /// <summary>
        /// Get visible/hidden string based on logged in user type.
        /// For viewer usertype only it should be hidden
        /// </summary>
        /// <param name="session">The session object</param>
        /// <returns>visible or hidden text based on logged in user type</returns>
        public static string AllowUserToEdit(this HttpSessionStateBase session)
        {
            string userType = session.GetUserType();
            string allowUserToEdit = "visible";
            if (userType.Equals(Constants.VIEWER_USER))
            {
                allowUserToEdit = "hidden";
            }
            return allowUserToEdit;
        }
    }

    /// <summary>
    /// Class having session keys
    /// </summary>
    public static class SessionKeys
    {
        public const string User = "User";
        public const string UserType = "UserType";
        public const string CanExecuteReport = "CanExecuteReport";
        public const string AssociatedCompanyList = "AssociatedCompanyList";
        public const string UserId = "UserId";
        public const string UserEmailId = "UserEmailId";
        public const string DefaultCompanyId = "DefaultCompanyId";
    }
}