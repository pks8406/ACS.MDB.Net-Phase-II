using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class UserModel : BaseModel
    {
        [Required(ErrorMessage = "Please enter Email id")]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter valid Email id")]
        [StringLength(50, ErrorMessage = "Email id must be maximum length of 50")]
        [Display(Name = "Email id")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [RegularExpression("^([a-zA-Z0-9!@#$%^&*()~]+)$", ErrorMessage = "Please enter valid Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password must be with a minimum length of 6 and a maximum length of 15")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Forgot Password")]
        public string ForgotPassword { get; set; }

        [Display(Name = " Is Active?")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        //Don't Remove Space from regex
        [RegularExpression("^([a-zA-Z0-9 ._]+)$", ErrorMessage = "Please enter valid Username")]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [StringLength(20)]
        [Display(Name = "User Type")]
        public string UserType { get; set; }

        [Display(Name = " Execute Reports?")]
        public bool CanExecuteReport { get; set; }

        /// <summary>
        /// Gets or set default company id
        /// </summary>
        [Display(Name = "Default Company")]
        [Required(ErrorMessage = "Please select Default Company")]
        public int? DefaultCompanyId { get; set; }
        /// <summary>
        /// Is all company associated with user
        /// </summary>
        [Display(Name = "Select all companies")]
        public bool IsCheckAll
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set New password
        /// </summary>
        [Required(ErrorMessage = "Please enter New password")]
        [RegularExpression("^([a-zA-Z0-9!@#$%^&*()~]+)$", ErrorMessage = "Please enter valid Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password must be with a minimum length of 6 and a maximum length of 15")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }


        /// <summary>
        /// Gets or sets Confirm password
        /// </summary>
        [Required(ErrorMessage = "Please enter Confirm password")]
        [RegularExpression("^([a-zA-Z0-9!@#$%^&*()~]+)$", ErrorMessage = "Please enter valid Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password must be with a minimum length of 6 and a maximum length of 15")]
        [Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or set users password is temporary or not
        /// </summary>
        public bool IsPasswordTemporary { get; set; }

        /// <summary>
        /// gets or sets login time
        /// </summary>
        public DateTime? LastLogintime { get; set; }
        /// <summary>
        /// List of user types
        /// </summary>
        public List<string> UserTypeList = new List<string>();

        /// <summary>
        /// List of company
        /// </summary>
        public List<Company> CompanyList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserModel()
        {
            CompanyList = new List<Company>();
            ConfirmPassword = Constants.TEMPORARY_PASSWORD;
            NewPassword = Constants.TEMPORARY_PASSWORD;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userVO">The UserVO</param>
        public UserModel(UserVO userVO)
            : this()
        {
            ID = userVO.ID;
            UserName = userVO.UserName;
            EmailId = userVO.EmailID;
            Password = userVO.Password;
            IsActive = userVO.IsActive;
            DefaultCompanyId = userVO.DefaultCompanyId;
            UserType = userVO.UserType;
            CanExecuteReport = userVO.CanExecuteReport;
            UserTypeList = userVO.UserTypes;

            foreach (var item in userVO.AssociatedCompanyList)
            {
                CompanyList.Add(new Company(item));
            }
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public UserVO Transpose(int? userId)
        {
            UserVO userVO = new UserVO();

            userVO.ID = this.ID;
            userVO.UserName = this.UserName;
            userVO.EmailID = this.EmailId;
            userVO.Password = this.Password;
            userVO.UserType = this.UserType;
            userVO.IsActive = this.IsActive;
            userVO.DefaultCompanyId = this.DefaultCompanyId;
            userVO.CanExecuteReport = this.CanExecuteReport;
            userVO.IsPasswordTemporary = this.IsPasswordTemporary;
            userVO.CreatedByUserId = userId;
            userVO.LastUpdatedByUserId = userId;
            userVO.LastLoginTime = System.DateTime.Now;

            foreach (var item in this.CompanyList)
            {
                userVO.AssociatedCompanyList.Add(item.Transpose());
            }

            //foreach (var item in this.CompanyList)
            //{
            //    userVO.AssociatedCompanyList.Add(new CompanyVO(item));
            //}

            return userVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            String status = IsActive ? "Active" : "Inactive";
            String reportExecution = CanExecuteReport ? "Yes" : "No";

            return (EmailId != null && EmailId.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (UserName != null && UserName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (UserType != null && UserType.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (status.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (reportExecution.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] {  "<input type='checkbox' name='check5' value='" + ID + "'>",
                ID, UserName, UserType, EmailId, IsActive ? "Active" : "Inactive", CanExecuteReport ? "Yes": "No"};
            return result;
        }
    }
}