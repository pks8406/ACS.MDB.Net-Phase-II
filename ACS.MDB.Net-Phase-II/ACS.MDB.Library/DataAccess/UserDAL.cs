using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class UserDAL : BaseDAL
    {
        /// <summary>
        /// Return specific user details
        /// </summary>
        /// <param name="userVO">user value object</param>
        /// <returns>returns user value object</returns>
        public UserVO GetUser(UserVO userVO)
        {
            UserVO userDetail = null;
            User user = mdbDataContext.Users.SingleOrDefault(u => u.EmailID == userVO.EmailID && u.Password == userVO.Password);

            //check user exist in database or not
            if (user != null)
            {
                userDetail = new UserVO(user);
                userDetail.AssociatedCompanyList = GetAssociatedCompanyByUserID(user.ID);
                return userDetail;
            }

            //return user details
            return userDetail;
        }

        /// <summary>
        /// Get list of users
        /// </summary>
        /// <returns>list of users</returns>
        public List<UserVO> GetUserList(string userType)
        {
            List<UserVO> userVOList = new List<UserVO>();
            List<User> users = new List<User>();

            if (userType == "SuperUser")
            {
                //Get all the users from database
                users = mdbDataContext.Users.ToList();
            }
            else
            {
                //Get users from database whoes userType is not SuperUser
                users = mdbDataContext.Users.Where(c => c.UserType != "SuperUser").ToList();
            }

            foreach (User item in users)
            {
                UserVO user = new UserVO(item);
                user.AssociatedCompanyList = GetAssociatedCompanyByUserID(user.ID);
                userVOList.Add(user);
            }

            return userVOList;
        }

        /// <summary>
        /// Get user details by  id
        /// </summary>
        /// <param name="emailId">user email id</param>
        /// <returns>user detail</returns>
        public UserVO GetUserById(int id)
        {
            UserVO userDetail = null;
            User user = mdbDataContext.Users.SingleOrDefault(u => u.ID == id);

            //check user exist in database or not
            if (user != null)
            {
                userDetail = new UserVO(user);
                userDetail.AssociatedCompanyList = GetAssociatedCompanyByUserID(userDetail.ID);
                return userDetail;
            }

            //return user details
            return userDetail;
        }

        /// <summary>
        /// Get user details by email id
        /// </summary>
        /// <param name="emailId">user email id</param>
        /// <returns>user detail</returns>
        public UserVO GetUserByEmailId(string emailId)
        {
            UserVO userDetail = null;
            User user = mdbDataContext.Users.SingleOrDefault(u => u.EmailID == emailId);

            //check user exist in database or not
            if (user != null)
            {
                userDetail = new UserVO(user);

                //get associated company with user
                userDetail.AssociatedCompanyList = GetAssociatedCompanyByUserID(userDetail.ID);

                //Get list of user types
                userDetail.UserTypes = GetUserType();
                return userDetail;
            }

            //return user details
            return userDetail;
        }

        /// <summary>
        /// Save user details.
        /// </summary>
        /// <param name="userVO">user value object</param>
        public void SaveUser(UserVO userVO)
        {
            //using (mdbDataContext.Transaction.Connection.Open())
            //{
            User userDetail = null;
            if (userVO.ID == 0)
            {
                userDetail = new User();
                //Create new user
                userDetail.EmailID = userVO.EmailID;
                userDetail.Password = userVO.Password;
                userDetail.Username = userVO.UserName;
                userDetail.UserType = userVO.UserType;
                userDetail.DefaultCompany = userVO.DefaultCompanyId;
                userDetail.IsActive = userVO.IsActive;
                userDetail.CanExecuteReport = userVO.CanExecuteReport;
                userDetail.CreationDate = DateTime.Now;
                userDetail.CreatedBy = userVO.CreatedByUserId;

                mdbDataContext.Users.InsertOnSubmit(userDetail);
            }
            else
            {
                if (userVO.UserType == Constants.SUPER_USER)
                {
                    //Update super user details
                    userDetail = mdbDataContext.Users.Where(u => u.UserType == userVO.UserType).SingleOrDefault();
                    userDetail.EmailID = userVO.EmailID;
                }
                else
                {
                    //Update user details
                    userDetail = mdbDataContext.Users.Where(u => u.EmailID == userVO.EmailID).SingleOrDefault();
                }

                userDetail.Username = userVO.UserName;
                userDetail.UserType = userVO.UserType;
                userDetail.IsActive = userVO.IsActive;
                userDetail.DefaultCompany = userVO.DefaultCompanyId;
                userDetail.CanExecuteReport = userVO.CanExecuteReport;
                userDetail.LastUpdatedDate = DateTime.Now;
                userDetail.LastUpdatedBy = userVO.LastUpdatedByUserId;
            }

            mdbDataContext.SubmitChanges();
            //Map user with selected comapny
            if (userVO.AssociatedCompanyList != null)
            {
                MapUserWithCompany(userVO);
            }
            //Commit the database transaction
            // mdbDataContext.Transaction.Commit();
            // }
        }

        /// <summary>
        /// Get list of comapanies associated with user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>list of associated company</returns>
        public List<CompanyVO> GetAssociatedCompanyByUserID(int userId)
        {
            List<CompanyVO> associatedCompanyList = new List<CompanyVO>();
            List<UserCompanyMapping> userCompanyMapping = mdbDataContext.UserCompanyMappings.Where(u => u.UserID == userId
                                                                            && u.IsActive == true).ToList();

            foreach (var item in userCompanyMapping)
            {
                CompanyVO company = new CompanyVO();
                company.CompanyID = item.CompanyID;
                company.IsSelected = true;
                associatedCompanyList.Add(company);
            }

            return associatedCompanyList;
        }

        /// <summary>
        /// Map user with company
        /// </summary>
        /// <param name="userVO">user value object</param>
        private void MapUserWithCompany(UserVO userVO)
        {
            int userID = userVO.ID;
            if (userVO.ID == 0)
            {
                //Get the user id of newly added user
                User user = mdbDataContext.Users.Where(u => u.EmailID == userVO.EmailID).SingleOrDefault();
                userID = user.ID;
            }

            foreach (var item in userVO.AssociatedCompanyList)
            {
                UserCompanyMapping updateUserMapping = mdbDataContext.UserCompanyMappings.Where(um => um.UserID == userID
                                                                               && um.CompanyID == item.CompanyID).SingleOrDefault();

                //If new user company mapping
                if (item.IsSelected && updateUserMapping == null)
                {
                    updateUserMapping = new UserCompanyMapping();
                    updateUserMapping.CompanyID = item.CompanyID;
                    updateUserMapping.UserID = userID;
                    updateUserMapping.IsActive = true;
                    updateUserMapping.CreationDate = DateTime.Now;
                    updateUserMapping.CreatedBy = userVO.CreatedByUserId;

                    mdbDataContext.UserCompanyMappings.InsertOnSubmit(updateUserMapping);
                }
                else if (updateUserMapping != null)
                {
                    updateUserMapping.IsActive = item.IsSelected;
                    updateUserMapping.LastUpdatedDate = DateTime.Now;
                    updateUserMapping.LastUpdatedBy = userVO.LastUpdatedByUserId;
                }
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Get list of user types
        /// </summary>
        /// <returns>returns list of user type</returns>
        public List<string> GetUserType()
        {
            List<string> userTypes = new List<string>();

            userTypes.Add("Admin");
            userTypes.Add("Normal");
            userTypes.Add("Viewer");

            return userTypes;
        }

        /// <summary>
        /// Get Super user types
        /// </summary>
        /// <returns>returns list of user type</returns>
        public List<string> GetSuperUserType()
        {
            List<string> userTypes = new List<string>();
            userTypes.Add("SuperUser");

            return userTypes;
        }

        /// <summary>
        /// Search the User on the bases of text passed
        /// </summary>
        /// <param name="searchText">Text to search user</param>
        /// <returns>Returns the list of users contain in search</returns>
        public List<UserVO> SearchUser(string searchText)
        {
            List<UserVO> userVOList = new List<UserVO>();
            List<User> users = null;

            users = mdbDataContext.Users.Where(u => u.EmailID.Contains(searchText)
                                                        || u.Username.Contains(searchText)
                                                        || u.UserType.Contains(searchText)).ToList();
            if (users != null)
            {
                foreach (User userItem in users)
                {
                    userVOList.Add(new UserVO(userItem));
                }
            }
            else
            {
                throw new ApplicationException("No data found.");
            }

            return userVOList;
        }

        /// <summary>
        /// Saves Users Temporary Password
        /// </summary>
        /// <param name="userVO">Value object of user</param>
        public void SetTemporaryPassword(UserVO userVO)
        {
            User userDetail = null;
            userDetail = mdbDataContext.Users.Where(u => u.ID == userVO.ID).SingleOrDefault();
            userDetail.Password = userVO.Password;
            userDetail.IsPasswordTemporary = true;
            userDetail.LastUpdatedDate = DateTime.Now;
            userDetail.LastUpdatedBy = userVO.ID;
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Changees User Password
        /// </summary>
        /// <param name="userVO">Value object of user</param>
        public void ChangePassword(UserVO userVO)
        {
            User userDetail = null;
            userDetail = mdbDataContext.Users.Where(u => u.ID == userVO.ID).SingleOrDefault();
            userDetail.Password = userVO.Password;
            userDetail.IsPasswordTemporary = false;
            userDetail.LastUpdatedDate = DateTime.Now;
            userDetail.LastUpdatedBy = userVO.LastUpdatedByUserId;
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// to get user emailId based on id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserEmailIdById(int? userId)
        {
            User user = mdbDataContext.Users.Where(u => u.ID == userId && u.IsActive == true).SingleOrDefault();
            string emailId=string.Empty;
            if (user != null)
            {
                emailId = user.EmailID;
            }
            return emailId;
        }

        /// <summary>
        /// to save login time of the user
        /// </summary>
        /// <param name="userVO"></param>
        public void SaveLastLoginTime(UserVO userVO)
        {
            User user = mdbDataContext.Users.Where(u => u.ID == userVO.ID && u.IsActive == true).SingleOrDefault();
            if (user != null)
            {
                user.LastLoginTime = userVO.LastLoginTime;
                mdbDataContext.SubmitChanges();
            }
        }

    }
}