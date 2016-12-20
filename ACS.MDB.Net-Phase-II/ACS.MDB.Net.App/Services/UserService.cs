using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    /// <summary>
    /// User service class handles user management
    /// </summary>
    public class UserService : BaseService
    {
        UserDAL userDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserService()
        {
            userDAL = new UserDAL();
        }

        /// <summary>
        /// Athenticate user credentials
        /// </summary>
        /// <param name="userVO">user value object</param>
        public void Authenticate(UserVO userVO)
        {
            if (!string.IsNullOrEmpty(userVO.EmailID) && !string.IsNullOrEmpty(userVO.Password))
            {
                //Encrypt the user password
                userVO.Password = MD5Hash(userVO.Password);

                //Get user details
                UserVO user = userDAL.GetUser(userVO);

                //If valid user
                if (user != null)
                {
                    //if user is valid but not active user
                    if (user.IsActive != true)
                    {
                        throw new ApplicationException(Constants.PLEASE_CONTACT_SYSTEM_ADMIN);
                    }
                    else
                    {
                        //set user details
                        userVO.ID = user.ID;
                        userVO.UserName = user.UserName;
                        userVO.UserType = user.UserType;
                        userVO.IsActive = user.IsActive;
                        userVO.CanExecuteReport = user.CanExecuteReport;
                        userVO.AssociatedCompanyList = user.AssociatedCompanyList;
                        userVO.IsPasswordTemporary = user.IsPasswordTemporary;
                        userVO.DefaultCompanyId = user.DefaultCompanyId;
                        //to save last login time of user
                        userDAL.SaveLastLoginTime(userVO);
                    }
                }
                else
                {
                    throw new ApplicationException(Constants.INVALID_USERNAME_OR_PASSWORD);
                }
            }
        }

        /// <summary>
        /// Gets user details by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user details</returns>
        public UserVO GetUserById(int id)
        {
            UserVO userDetail = userDAL.GetUserById(id);
            if (userDetail == null)
            {
                throw new ApplicationException(Constants.USER_NOT_EXIST);
            }

            return userDetail;
        }

        /// <summary>
        /// Get user details by email id
        /// </summary>
        /// <param name="emailId">user email id</param>
        /// <returns>user details</returns>
        public UserVO GetUserByEmailId(string emailId)
        {
            UserVO userDetail = userDAL.GetUserByEmailId(emailId);
            if (userDetail == null)
            {
                throw new ApplicationException(Constants.USER_NOT_EXIST);
            }
            return userDetail;
        }

        /// <summary>
        /// Gets list of users
        /// </summary>
        /// <returns></returns>
        public List<UserVO> GetUserList(string userType)
        {
            return userDAL.GetUserList(userType);
        }

        /// <summary>
        /// Save user details (new or edited user)
        /// </summary>
        /// <param name="userVO">user value object</param>
        public void SaveUser(UserVO userVO)
        {
            if (userVO != null && !string.IsNullOrEmpty(userVO.EmailID))
            {
                UserVO userExist = userDAL.GetUserByEmailId(userVO.EmailID);

                //Check whether user already exist or not
                if (userExist != null && userVO.ID != userExist.ID)
                {
                    throw new ApplicationException(Constants.EMAIL_ID_EXIST);
                }
                else
                {
                    //Check if at least one company is associated with user
                    if (!userVO.AssociatedCompanyList.Exists(x => x.IsSelected == true))
                    {
                        throw new ApplicationException(Constants.USER_MUST_BE_ASSOCIATED_WITH_ATLEAST_ONE_COMPANY);
                    }

                    //Check if default company is assciated with selected companies 
                    bool isDefaultCompanyExistInAssociatedCompanyList = userVO.AssociatedCompanyList.Exists(x => x.IsSelected == true && 
                        x.CompanyID == userVO.DefaultCompanyId);

                    if (isDefaultCompanyExistInAssociatedCompanyList == false)
                    {
                        throw new ApplicationException(Constants.DEFAULT_COMPANY_MUST_BE_FROM_SELECTED_COMPANY_LIST);
                    }

                    userVO.Password = MD5Hash(userVO.Password);
                    userDAL.SaveUser(userVO);
                }
            }
        }

        /// <summary>
        /// Gets the user types
        /// </summary>
        /// <returns>returns list of user types</returns>
        public List<string> GetUserTypes()
        {
            return userDAL.GetUserType();
        }

        /// <summary>
        /// Gets super user type
        /// </summary>
        /// <returns>returns list of user types</returns>
        public List<string> GetSuperUserType()
        {
            return userDAL.GetSuperUserType();
        }

        /// <summary>
        /// Get list of users based on search criteria match.
        /// </summary>
        /// <param name="searchText">The text to search for</param>
        /// <returns>List of userVO</returns>
        public List<UserVO> SearchUser(string searchText)
        {
            return userDAL.SearchUser(searchText);
        }

        /// <summary>
        /// Encrypt the user password
        /// </summary>
        /// <param name="password">user password</param>
        /// <returns>return encrypted password</returns>
        private string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Sets users TemporaryPassword 
        /// </summary>
        /// <param name="email">users email id</param>
        /// <param name="password">users password</param>
        public void SetTemporaryPassword(UserVO userVO, string password)
        {
            if (password != null && userVO != null && !string.IsNullOrEmpty(userVO.EmailID))
            {
                UserVO userExist = userDAL.GetUserByEmailId(userVO.EmailID);
                //Check whether user already exist or not
                if (userExist != null && userVO.ID != userExist.ID)
                {
                    throw new ApplicationException(Constants.EMAIL_ID_EXIST);
                }
                else
                {
                    userVO.Password = MD5Hash(password);
                    userDAL.SetTemporaryPassword(userVO);
                }
            }
        }

        /// <summary>
        /// Changes Users Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void ChangePassword(UserVO userVO, string oldPassword, string newPassword)
        {
            if (newPassword != null && userVO != null && !string.IsNullOrEmpty(userVO.EmailID))
            {
                UserVO userExist = userDAL.GetUserByEmailId(userVO.EmailID);
                //Check whether user already exist or not
                if (userExist != null && userVO.ID != userExist.ID)
                {
                    throw new ApplicationException(Constants.EMAIL_ID_EXIST);
                }
                else
                {
                    if (userVO.Password == MD5Hash(oldPassword))
                    {
                        userVO.Password = MD5Hash(newPassword);
                        userDAL.ChangePassword(userVO);
                    }
                    else
                    {
                        throw new ApplicationException(Constants.PASSWORD_NOT_MATCHED);
                    }
                }
            }
        }

      
        /// <summary>
        /// Get user details by email id for forgot password 
        /// </summary>
        /// <param name="emailId">user email id</param>
        /// <returns>user details</returns>
        public UserVO GetUserByEmailIdForForgotPassword(string emailId)
        {
            UserVO userDetail = userDAL.GetUserByEmailId(emailId);
            if (userDetail == null)
            {
                throw new ApplicationException(Constants.EMAIL_ID_NOT_EXIST);
            }
            return userDetail;
        }

        /// <summary>
        /// Get username based on id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetUserEmailIdById(int? Id)
        {
            return userDAL.GetUserEmailIdById(Id);
        }
    }
}