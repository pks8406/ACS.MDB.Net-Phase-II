using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    public class ChangePasswordController : BaseController
    {
        /// <summary>
        /// Returns chage password view
        /// </summary>
        /// <returns>Change Password View</returns>
        public ActionResult ChangePasswordIndex()
        {
            return View();
        }

        /// <summary>
        /// This method is used to change the user password on first login and redirect to Home Page
        /// </summary>
        /// <param name="email">users email</param>
        /// <param name="oldPassword">users old password</param>
        /// <param name="newPassword">users new password</param>
        /// <param name="confirmPassword">users confirm password</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult FirstLogin(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ChangePassword(email, oldPassword, newPassword);
                    UserService userService = new UserService();
                    UserVO userVO = new UserVO(email, newPassword);
                    //Athenticate user credentials
                    userService.Authenticate(userVO);

                    //Set user authentication cookie
                    FormsAuthentication.SetAuthCookie(userVO.UserName, false);

                    //Clear the session
                    Session.Clear();

                    //Set Cache for logged in user
                    Session.SetUserId(userVO.ID);
                    //Session.SetUser(userVO);
                    Session.SetUserEmailId(userVO.EmailID);
                    Session.SetUserType(userVO.UserType);
                    Session.SetExecuteReportPermission(userVO.CanExecuteReport);
                    Session.SetUserAssociatedCompanyList(GetUserAssociatedCompanyList(userVO));
                    return new HttpStatusCodeResult(200);
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
            return View();
        }

        /// <summary>
        /// This method is used to change the password once the user is logged in
        /// </summary>
        /// <param name="oldPassword">user's oldpassword</param>
        /// <param name="newPassword">user's new password</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePasswordAfterLogin(string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session.GetUserId().HasValue)
                    {
                        string emailId = Session.GetUserEmailId();
                        ChangePassword(emailId, oldPassword, newPassword);
                        return new HttpStatusCodeResult(200);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden, Constants.SESSION_TIMEOUT);
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
            return View();
        }

        /// <summary>
        /// This method is used to change the user password
        /// </summary>
        /// <param name="email">users email address</param>
        /// <param name="oldPassword">users old password</param>
        /// <param name="newPassword">users new password</param>
        private void ChangePassword(string email, string oldPassword, string newPassword)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                UserService userService = new UserService();
                UserVO userVO = new UserVO();

                userVO = userService.GetUserByEmailId(email);

                //Set LasUpdatedUserId if user is in session
                if (userVO.IsPasswordTemporary != true)
                {
                    userVO.LastUpdatedByUserId = userId;
                }
                userService.ChangePassword(userVO, oldPassword, newPassword);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Gets list of company associated with logged in user
        /// </summary>
        /// <returns>List of company</returns>
        private List<Company> GetUserAssociatedCompanyList(UserVO user)
        {
            List<Company> userAssociatedCompanyList = new List<Company>();

            if (user != null)
            {
                List<Company> companyList = GetCompanyList();
                foreach (Company companyVO in companyList)
                {
                    if (user.AssociatedCompanyList.Exists(x => x.CompanyID == companyVO.ID))
                    {
                        userAssociatedCompanyList.Add(new Company(companyVO));
                    }
                }
            }

            return userAssociatedCompanyList;
        }
    }
}