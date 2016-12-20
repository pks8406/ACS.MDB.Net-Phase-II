using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class UserVO : BaseVO
    {
        public int ID { get; set; }

        /// <summary>
        /// Gets or set logn in username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or set user email id
        /// </summary>
        public string EmailID { get; set; }

        /// <summary>
        /// Gets or set user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or set user is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or set user type
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Gets or set user can execute report or not
        /// </summary>
        public bool CanExecuteReport { get; set; }

        /// <summary>
        /// Gets or set default company id
        /// </summary>
        public int? DefaultCompanyId { get; set; }

        /// <summary>
        /// Gets or set users password is temporary or not
        /// </summary>
        public bool IsPasswordTemporary { get; set; }

        /// <summary>
        /// List of company
        /// </summary>
        public List<CompanyVO> AssociatedCompanyList = new List<CompanyVO>();

        /// <summary>
        /// Gets or set user types
        /// </summary>
        public List<string> UserTypes = new List<string>();

        /// <summary>
        /// gets or sets login time of user
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserVO()
        {

        }

        /// <summary>
        /// Constructor - Transpose LINQ User object to Value object
        /// </summary>
        /// <param name="user">LINQ User object</param>
        public UserVO(User user)
        {
            ID = user.ID;
            UserName = user.Username;
            EmailID = user.EmailID;
            Password = user.Password;
            IsActive = user.IsActive;
            UserType = user.UserType;
            DefaultCompanyId = user.DefaultCompany;
            CreatedByUserId = user.CreatedBy;
            LastUpdatedByUserId = user.LastUpdatedBy;
            CanExecuteReport = user.CanExecuteReport;
            IsPasswordTemporary = user.IsPasswordTemporary;
            LastLoginTime = user.LastLoginTime;
        }

        /// <summary>
        /// Transpose Model object to Value object
        /// </summary>
        /// <param name="model"></param>
        //public UserVO(UserModel model, int? userId)
        //{
        //    ID = model.ID;
        //    UserName = model.UserName;
        //    EmailID = model.EmailId;
        //    Password = model.Password;
        //    UserType = model.UserType;
        //    IsActive = model.IsActive;
        //    DefaultCompanyId = model.DefaultCompanyId;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //    CanExecuteReport = model.CanExecuteReport;
        //    IsPasswordTemporary = model.IsPasswordTemporary;

        //    foreach (var item in model.CompanyList)
        //    {
        //        AssociatedCompanyList.Add(item.Transpose());
        //    }
        //}

        /// <summary>
        /// Transpose email and password to value object
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserVO(string email, string password)
        {
            EmailID = email;
            Password = password;
        }
    }
}