using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    public class ForgotPasswordController : Controller
    {
        #region Public Methods

        /// <summary>
        /// Returns forgot password view
        /// </summary>
        /// <returns>Forgot Password View</returns>
        public ActionResult ForgotPasswordIndex(String email)
        {
            ViewBag.Email = email;
            return View();
        }

        /// <summary>
        /// Validates the user, Resets temporary password and sends mail to user Specified in email
        /// </summary>
        /// <param name="email">The email address</param>
        [HttpPost]
        public ActionResult ResetPassword(String email)
        {
            UserModel userModel = null;
            try
            {
                if (ModelState.IsValid)
                {
                    UserService userService = new UserService();
                    UserVO userVO = new UserVO();
                    userVO = userService.GetUserByEmailIdForForgotPassword(email);

                    //Validate the User Model
                    if (userVO != null)
                    {
                        userModel = new UserModel(userVO);
                        string temporaryPassword = GenerateRandomPassword();
                        userService.SetTemporaryPassword(userVO, temporaryPassword);
                        SendResetPasswordMail(email, userVO.UserName, temporaryPassword);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The email address entered does not exist.");
                    }

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
        /// Generate Randam Password
        /// </summary>
        /// <returns>Temporary Generated Random Password</returns>
        private string GenerateRandomPassword()
        {
            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyzABCDEFGHJKLMNPQRSTWXYZ0123456789";
            var chars = Enumerable.Range(0, 8)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        /// <summary>
        /// Sends Temporary Password to clients Email address
        /// </summary>
        /// <param name="email">User Email address</param>
        /// <param name="username">Username</param>
        /// <param name="password">Temporary Password</param>
        private void SendResetPasswordMail(string email, string username, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                string Body = "Dear " + username + ",\n\n";
                Body += "Your ARBS Password has been reset. Please login with the below mentioned temporary password.\n\n";
                Body += "Email: " + email + "\n\n";
                Body += "Password: " + password + "\n\n";
                Body += "Kindly note that this is a one time system generated temporary password which will allow you to login and change the password to your preferred choice. If you have received this password-assistance email you didn't request, Kindly ignore it.\n\n";
                mail.Subject = "ARBS Password has been Reset";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(ConfigurationManager.AppSettings["From"]);
                mail.Priority = MailPriority.High;

                //Set sender email id
                mail.To.Add(email);

                SmtpClient emailClient = new SmtpClient(ConfigurationManager.AppSettings["SMTPServerAddress"]);

                //Set credentials if provided
                string smptuserName = ConfigurationManager.AppSettings["SmtpUserName"];
                string smtpuserPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                if (!String.IsNullOrEmpty(smptuserName) && !String.IsNullOrEmpty(smtpuserPassword))
                {
                    emailClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUserName"],
                                                  ConfigurationManager.AppSettings["SmtpPassword"]);
                }

                emailClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                emailClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpIsSSL"]);
                emailClient.Send(mail.From.ToString(), mail.To.ToString(), mail.Subject.ToString(), mail.Body);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Public Methods
    }
}