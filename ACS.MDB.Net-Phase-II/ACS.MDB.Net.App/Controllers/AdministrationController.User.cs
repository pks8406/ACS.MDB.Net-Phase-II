using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    /// <summary>
    /// The controller actions for the user actions
    /// </summary>
    public partial class AdministrationController
    {
        #region User

        /// <summary>
        /// Returns user index view
        /// </summary>
        /// <returns>user index view</returns>
        // GET: /Administration/UserIndex
        public ActionResult UserIndex()
        {
            return IndexViewForAuthorizeUser();
        }

        /// <summary>
        /// Returns list of users
        /// </summary>
        /// <param name="param">The filter an other parameters</param>
        /// <returns>List of users</returns>
        // GET: /Administration/UserList
        public ActionResult UserList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                List<MODEL.UserModel> userList = new List<MODEL.UserModel>();
                UserService userService = new UserService();
                string UserType = SessionHelper.GetUserType(Session);

                List<UserVO> userVOList = userService.GetUserList(UserType);

                foreach (var item in userVOList)
                {
                    userList.Add(new MODEL.UserModel(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetUserOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, userList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <returns>The user details view</returns>
        public ActionResult UserCreate()
        {
            try
            {
                MODEL.UserModel user = new MODEL.UserModel();
                UserService userService = new UserService();

                List<MODEL.Company> companyList = GetCompanyList();
                user.UserTypeList = userService.GetUserTypes();

                //by default set the user to active
                user.IsActive = true;
                foreach (var item in companyList)
                {
                    user.CompanyList.Add(new MODEL.Company(item));
                }

                return PartialView("UserDetails", user);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit user details
        /// </summary>
        /// <param name="id">The selected user id</param>
        /// <returns>The user details view</returns>
        public ActionResult UserEdit(int id)
        {
            MODEL.UserModel userModel = new MODEL.UserModel();
            try
            {
                UserService userService = new UserService();
                DivisionService divisionService = new DivisionService();

                UserVO userVO = userService.GetUserById(id);
                //Add 'super user' as a user type for super user
                if (userVO.UserType == Constants.SUPER_USER)
                {
                    //gets Super User
                    userVO.UserTypes = userService.GetSuperUserType();
                }
                else
                {
                    userVO.UserTypes = userService.GetUserTypes();
                }
                
                List<MODEL.Company> companyList = GetCompanyList();

                //Transpose the user value object to model objecct
                userModel = new MODEL.UserModel(userVO);

                //TO DO - Need to implement better appproach to while editing password
                //While editing user details password should not be visible but model is getting binded
                //so always return invalid model
                // userModel.Password = "password";

                //Clear the company list
                userModel.CompanyList.Clear();

                //Iterate through each company check whether company is associated with user or not
                foreach (var item in companyList)
                {
                    var temp = userVO.AssociatedCompanyList.Find(c => c.CompanyID == item.ID);
                    if (temp != null)
                    {
                        item.IsSelected = temp.IsSelected;
                    }
                    userModel.CompanyList.Add(new MODEL.Company(item));
                }

                //if all company are associated with user set IsCheckAll = true
                if (userModel.CompanyList.TrueForAll(c => c.IsSelected == true))
                {
                    userModel.IsCheckAll = true;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("UserDetails", userModel);
        }

        /// <summary>
        /// Save user details
        /// </summary>
        /// <param name="model">user model</param>
        public ActionResult UserSave(MODEL.UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    //UserVO userVO = new UserVO(model, userId);

                    UserVO userVO = model.Transpose(userId);

                    UserService userService = new UserService();
                    userService.SaveUser(userVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.USER));
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns the field to sorted</returns>
        public Func<BaseModel, object> GetUserOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((UserModel)obj).UserName;
                    break;

                case 3:
                    sortFunction = obj => ((UserModel)obj).UserType;
                    break;

                case 4:
                    sortFunction = obj => ((UserModel)obj).EmailId;
                    break;

                case 5:
                    sortFunction = obj => ((UserModel)obj).IsActive;
                    break;

                default:
                    sortFunction = obj => ((UserModel)obj).ID;
                    break;
            }

            return sortFunction;
        }

        #endregion User
    }
}