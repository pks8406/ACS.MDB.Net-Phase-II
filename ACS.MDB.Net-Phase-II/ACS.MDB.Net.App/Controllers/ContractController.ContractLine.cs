using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ContractController
    {
        /// <summary>
        /// Get contract line index view
        /// </summary>
        /// <returns></returns>
        /// GET: /ContractController.ContractLine/
        public ActionResult ContractLineIndex()
        {
            return PartialView("ContractLineIndex");
            //return View();
        }

        /// <summary>
        /// Get the list of Contract Lines based on Contract Id
        /// </summary>
        /// <param name="param"></param>
        /// <param name="contractId">Contract Id</param>
        /// <returns>ContractLine List</returns>
        public ActionResult GetContractLineByContractId(MODEL.jQueryDataTableParamModel param, int? contractId)
        {
            try
            {
                ContractLineService contractLineService = new ContractLineService();
                List<ContractLineVO> contractLineVOList = contractLineService.GetContractLineByContractId(contractId.Value);

                List<MODEL.ContractLine> contractLines = new List<MODEL.ContractLine>();
                foreach (var item in contractLineVOList)
                {
                    contractLines.Add(new MODEL.ContractLine(item));
                }

                ////get the field on with sorting needs to happen and set the
                ////ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetContractListOrderingFunction(sortColumnIndex);
                var result = GetFilteredObjects(param, contractLines, orderingFunction);

                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create New Contract Line
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="contractId">contract id</param>
        /// <returns>ContractLine details view </returns>
        public ActionResult ContractLineCreate(int companyId, int customerId, int contractId)
        {
            try
            {
                MODEL.ContractLine contractLine = new MODEL.ContractLine();
                contractLine.CostCentreList = GetCostCenterList(companyId);
                contractLine.ActivityCategoryList = GetActivityCategoryList();
                contractLine.ActivityCategoryId = 21;
                contractLine.JobCodeList = GetJobCodeList(companyId, customerId);
                contractLine.ContractID = contractId;
                contractLine.ActivityCodeList = GetActivityCodeList(companyId);

                return PartialView("ContractLineDetails", contractLine);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit ContractLine by id
        /// </summary>
        /// <param name="id">ContractLine Id</param>
        /// <returns>ContractLine details view</returns>
        public ActionResult ContractLineEdit(int id)
        {
            MODEL.ContractLine contractLine = new MODEL.ContractLine(); ;

            try
            {
                ContractLineService contractLineService = new ContractLineService();

                //Get contractLine details
                ContractLineVO contractLineVO = contractLineService.GetContractLineById(id);

                if (contractLineVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.CONTRACT_LINE));
                }
                else
                {
                    contractLine = new MODEL.ContractLine(contractLineVO);
                    contractLine.CostCentreList = GetCostCenterList(contractLineVO.Contract.CompanyID);
                    contractLine.ActivityCategoryList = GetActivityCategoryList();
                    contractLine.JobCodeList = GetJobCodeList(contractLineVO.Contract.CompanyID, contractLineVO.Contract.InvoiceCustomerID);
                    contractLine.IsJobCodeExist = IsJobCodeExist(contractLine.JobCodeList, contractLine.JobCodeId);
                    contractLine.ActivityCodeList = GetActivityCodeList(contractLineVO.Contract.CompanyID);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("ContractLineDetails", contractLine);
        }

        /// <summary>
        /// Save the Contract Line
        /// </summary>
        /// <param name="model">model object of Contract Line</param>
        /// <returns></returns>
        public ActionResult ContractLineSave(MODEL.ContractLine model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    ContractLineService contractLineService = new ContractLineService();
                    //ContractLineVO contractLineVO = new ContractLineVO(model, userId);

                    ContractLineVO contractLineVO = model.Transpose(userId);

                    contractLineService.ContractLineSave(contractLineVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.CONTRACT_LINE));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete contractLines
        /// </summary>
        /// <param name="Ids">Ids of contactLine to be deleted</param>
        public ActionResult ContractLineDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractLineService contractLineService = new ContractLineService();
                contractLineService.ContractLineDelete(Ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns></returns>
        public Func<MODEL.BaseModel, object> GetContractListOrderingFunction(int sortCol)
        {
            Func<MODEL.BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.ContractLine)obj).ActivityCategory;
                    break;

                case 3:
                    sortFunction = obj => ((MODEL.ContractLine)obj).ActivityCode;
                    break;

                case 4:
                    sortFunction = obj => ((MODEL.ContractLine)obj).Account;
                    break;

                case 5:
                    sortFunction = obj => ((MODEL.ContractLine)obj).JobCode;
                    break;

                case 6:
                    sortFunction = obj => ((MODEL.ContractLine)obj).CostCenter;
                    break;

                //case 7:
                //    sortFunction = obj => ((MODEL.ContractLine)obj).QTY;
                //    break;

                default:
                    sortFunction = obj => ((MODEL.ContractLine)obj).ID;
                    break;
            }

            return sortFunction;
        }

        #region Methods

        /// <summary>
        /// Get Account Code based on ActivityCategory Id
        /// </summary>
        /// <param name="ActivityCodeId"></param>
        /// <returns>Json object having account code related information</returns>
        public JsonResult GetAccountCode(int ActivityCodeId)
        {
            List<string> accountCodeItem = new List<string>();

            ActivityCodeService activityCodeService = new ActivityCodeService();
            ActivityCodeVO activityCodeVO = activityCodeService.GetAccountByActivityCode(ActivityCodeId);

            return Json(activityCodeVO);
        }

        /// <summary>
        /// Gets the list of Costcenter based on Company Id
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>CostCentrer List</returns>
        private List<MODEL.CostCentre> GetCostCenterList(int? companyId)
        {
            MODEL.ContractLine contractLine = new MODEL.ContractLine();
            CostCenterService costcenterService = new CostCenterService();
            List<CostCentreVO> costcenterVOList = costcenterService.GetCostCenterList(companyId);

            foreach (CostCentreVO costcenter in costcenterVOList)
            {
                contractLine.CostCentreList.Add(new MODEL.CostCentre(costcenter));
            }

            return (contractLine.CostCentreList);
        }

        /// <summary>
        /// Gets the list of ActivityCategory
        /// </summary>
        /// <returns>ActivityCategory List</returns>
        private List<MODEL.ActivityCategory> GetActivityCategoryList()
        {
            MODEL.ContractLine contractLine = new MODEL.ContractLine();
            ActivityCategoryService activityCategoryService = new ActivityCategoryService();
            List<ActivityCategoryVO> activityCategoryVOList = activityCategoryService.GetActivityCategoryList();
            foreach (ActivityCategoryVO activityCategory in activityCategoryVOList)
            {
                contractLine.ActivityCategoryList.Add(new MODEL.ActivityCategory(activityCategory));
            }

            return (contractLine.ActivityCategoryList);
        }

        /// <summary>
        /// Gets the JobCode List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="CustomerId">customer Id</param>
        /// <returns>Jobcode list</returns>
        private List<MODEL.JobCode> GetJobCodeList(int? companyId, int customerId)
        {
            MODEL.ContractLine contractLine = new MODEL.ContractLine();
            JobCodeService jobCodeService = new JobCodeService();
            List<JobCodeVO> jobCodeVOList = jobCodeService.GetJobCodeList(companyId, customerId);

            foreach (JobCodeVO item in jobCodeVOList)
            {
                contractLine.JobCodeList.Add(new MODEL.JobCode(item));
            }

            return contractLine.JobCodeList;
        }

        /// <summary>
        /// Get the ActivityCode List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>ActivityCode List</returns>
        private List<MODEL.ActivityCode> GetActivityCodeList(int companyId)
        {
            MODEL.ContractLine contractLine = new MODEL.ContractLine();
            ActivityCodeService activityCodeService = new ActivityCodeService();
            List<ActivityCodeVO> activityCodeVOList = activityCodeService.GetActivityCodeList(companyId);

            foreach (ActivityCodeVO item in activityCodeVOList.FindAll(x => x.OAActivityCodeId.StartsWith("1")))
            {
                contractLine.ActivityCodeList.Add(new MODEL.ActivityCode(item));
            }

            return contractLine.ActivityCodeList;
        }

        /// <summary>
        /// To check job code is exist in Jobcode list or not
        /// </summary>
        /// <param name="JobCodeList">Job code list</param>
        /// <param name="jobCodeId">Jobcode Id</param>
        /// <returns>boolean value for jobcode exist</returns>
        private bool IsJobCodeExist(List<JobCode> JobCodeList, int jobCodeId)
        {
            bool isJobCodeExist = false;
            isJobCodeExist = JobCodeList.Any(x => x.ID == jobCodeId);
            return isJobCodeExist;
        }

        #endregion Methods
    }
}