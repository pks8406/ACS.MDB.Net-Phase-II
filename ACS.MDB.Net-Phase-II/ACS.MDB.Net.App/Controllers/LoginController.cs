using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Filters;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class LoginController : BaseController
    {
        #region Manage User

        /// <summary>
        /// This method used to get login page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = null;
            return View(new UserModel());
        }

        /// <summary>
        /// This method used to do login.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model, string returnUrl)
        {
            try
            {
                UserService userService = new UserService();
                //UserVO userVO = new UserVO(model, 0);

                UserVO userVO = model.Transpose(0);

                //Authenticate user credentials
                userService.Authenticate(userVO);

                if (userVO.IsPasswordTemporary)
                {
                    model.IsPasswordTemporary = true;
                }

                if (userVO.IsPasswordTemporary == false)
                {
                    //Set user authentication cookie
                    FormsAuthentication.SetAuthCookie(userVO.UserName, false);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        //Set session timeout for non super user
                        if (userVO.UserType.ToLower() != "superuser")
                        {
                            //Set configurable session timeout (1 hour)
                            int sessionTimeout = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SessionTimeoutForNonSuperUser"]) ?
                                Convert.ToInt32(ConfigurationManager.AppSettings["SessionTimeoutForNonSuperUser"]) :
                                Session.Timeout;
                            Session.SetSessionTimeOut(sessionTimeout);
                        }

                        //Clear the session
                        Session.Clear();

                        //Set Cache for logged in user
                        Session.SetUserId(userVO.ID);
                        //Session.SetUser(userVO);
                        Session.SetUserType(userVO.UserType);
                        Session.SetUserEmailId(userVO.EmailID);
                        Session.SetExecuteReportPermission(userVO.CanExecuteReport);
                        Session.SetUserAssociatedCompanyList(GetUserAssociatedCompanyList(userVO));
                        Session.SetDefaultCompanyId(userVO.DefaultCompanyId);
                        
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }

            return View(model);
        }

        /// <summary>
        /// This method used to do logoff and redirect user to log in page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Abandon();
            Session.Clear();

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            //@TODO: write code to clear log in sesssion and then redirect to log in page
            return RedirectToAction("Login", "Login");
        }

        #endregion Manage User

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion Helpers
    }
}