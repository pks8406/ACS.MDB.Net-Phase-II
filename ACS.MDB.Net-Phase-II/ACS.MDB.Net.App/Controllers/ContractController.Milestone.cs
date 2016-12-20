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
    public partial class ContractController
    {
        /// <summary>
        /// Returns milestone index view
        /// </summary>
        /// <returns>milestone view</returns>
        // GET: /ContractController.Milestone/
        public ActionResult MilestoneIndex()
        {
            return View();
        }

        /// <summary>
        /// Gets the list of contract milestone
        /// </summary>
        /// <param name="param"></param>
        /// <param name="contractMaintenanceId">ContractMaintenance Id</param>
        /// <returns>List of Milestones</returns>
        public ActionResult GetContractMilestoneList(MODEL.jQueryDataTableParamModel param, int contractMaintenanceId)
        {
            try
            {
                MilestoneService milestoneService = new MilestoneService();
                List<MilestoneVO> milestoneVOList = milestoneService.GetMilestoneList(contractMaintenanceId);

                List<MODEL.Milestone> milestoneList = new List<Milestone>();

                foreach (var item in milestoneVOList)
                {
                    milestoneList.Add(new MODEL.Milestone(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetMilestoneOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, milestoneList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit Milestone
        /// </summary>
        /// <param name="id">Milestone Id</param>
        /// <returns>Milestone details view</returns>
        public ActionResult MilestoneEdit(int id)
        {
            MODEL.Milestone milestone = new MODEL.Milestone();

            try
            {
                //Get milestone details
                MilestoneService milestoneService = new MilestoneService();
                MilestoneVO milestoneVO = milestoneService.GetMilestoneById(id);

                if (milestoneVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.MILESTONE));
                }
                else
                {
                    milestone = new MODEL.Milestone(milestoneVO);
                    milestone.MilestoneStatusList = GetMilestoneStatusList();
                    FillMilestoneBillingLines(milestone, milestone.MilestoneBillingLines);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("MilestoneDetails", milestone);
        }

        /// <summary>
        /// Save the milestone
        /// </summary>
        /// <param name="model">The Milestone model</param>
        [ValidateInput(false)]
        public ActionResult MilestoneSave(MODEL.Milestone model)
        {
            try
            {
                MilestoneService milestoneService = new MilestoneService();

                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    //MilestoneVO milestoneVO = new MilestoneVO(model, userId);

                    MilestoneVO milestoneVO = model.Transpose(userId);

                    milestoneService.SaveMilestone(milestoneVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.MILESTONE));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete milestone(s)
        /// </summary>
        /// <param name="Ids">Ids of milestones to be deleted</param>
        public ActionResult MilestoneDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                MilestoneService milestoneService = new MilestoneService();
                milestoneService.DeleteMilestone(Ids, userId);
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
        /// <returns>returns the field to sorted</returns>
        public Func<MODEL.BaseModel, object> GetMilestoneOrderingFunction(int sortCol)
        {
            Func<MODEL.BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.Milestone) obj).InvoiceDate;
                    break;

                case 3:
                    sortFunction = obj => ((MODEL.Milestone) obj).RenewalStartDate;
                    break;

                case 4:
                    sortFunction = obj => ((MODEL.Milestone) obj).RenewalEndDate;
                    break;

                case 5:
                    sortFunction = obj => ((MODEL.Milestone)obj).ActualBillDate;
                    break;

                case 6:
                    sortFunction = obj => ((MODEL.Milestone) obj).Amount;
                    break;

                case 7:
                    sortFunction = obj => ((MODEL.Milestone) obj).UpliftForMilestone;
                    break;

                case 8:
                    sortFunction = obj => ((MODEL.Milestone) obj).MilestoneStatusDescription;
                    break;

                default:
                    //sortFunction = obj => ((MODEL.Milestone)obj).RenewalStartDate;
                    break;
            }

            return sortFunction;
        }

        /// <summary>
        /// Gets the Milestone status List
        /// </summary>
        /// <returns>Milestonestatus List</returns>
        private List<MODEL.MilestoneStatus> GetMilestoneStatusList()
        {
            //MODEL.MilestoneStatus milestoneStatus = new MODEL.MilestoneStatus();
            MODEL.Milestone milestone = new MODEL.Milestone();
            MilestoneStatusService milestoneStatusService = new MilestoneStatusService();
            List<MilestoneStatusVO> milestoneStatusVOList = milestoneStatusService.GetMilestoneStatusList();

            foreach (var item in milestoneStatusVOList)
            {
                milestone.MilestoneStatusList.Add(new MODEL.MilestoneStatus(item));
            }

            return (milestone.MilestoneStatusList);
        }

        /// <summary>
        /// Fill milestone billing lines
        /// </summary>
        /// <param name="milestone">The milestone model object</param>
        /// <param name="milestoneBillingLines">The milestone billing lines</param>
        private void FillMilestoneBillingLines(Milestone milestone, List<MilestoneBillingLine> milestoneBillingLines)
        {
            foreach (var item in milestoneBillingLines)
            {
                switch (item.LineSequance)
                {
                    case 0:
                        milestone.billingText1 = item.LineText;
                        milestone.billingTextID1 = item.BillingLineID;
                        break;

                    case 1:
                        milestone.billingText2 = item.LineText;
                        milestone.billingTextID2 = item.BillingLineID;
                        break;

                    case 2:
                        milestone.billingText3 = item.LineText;
                        milestone.billingTextID3 = item.BillingLineID;
                        break;

                    case 3:
                        milestone.billingText4 = item.LineText;
                        milestone.billingTextID4 = item.BillingLineID;
                        break;

                    case 4:
                        milestone.billingText5 = item.LineText;
                        milestone.billingTextID5 = item.BillingLineID;
                        break;

                    case 5:
                        milestone.billingText6 = item.LineText;
                        milestone.billingTextID6 = item.BillingLineID;
                        break;

                    case 6:
                        milestone.billingText7 = item.LineText;
                        milestone.billingTextID7 = item.BillingLineID;
                        break;

                    case 7:
                        milestone.billingText8 = item.LineText;
                        milestone.billingTextID8 = item.BillingLineID;
                        break;

                    case 8:
                        milestone.billingText9 = item.LineText;
                        milestone.billingTextID9 = item.BillingLineID;
                        break;

                    case 9:
                        milestone.billingText10 = item.LineText;
                        milestone.billingTextID10 = item.BillingLineID;
                        break;

                    case 10:
                        milestone.billingText11 = item.LineText;
                        milestone.billingTextID11 = item.BillingLineID;
                        break;

                    case 11:
                        milestone.billingText12 = item.LineText;
                        milestone.billingTextID12 = item.BillingLineID;
                        break;

                    case 12:
                        milestone.billingText13 = item.LineText;
                        milestone.billingTextID13 = item.BillingLineID;
                        break;

                    case 13:
                        milestone.billingText14 = item.LineText;
                        milestone.billingTextID14 = item.BillingLineID;
                        break;

                    case 14:
                        milestone.billingText15 = item.LineText;
                        milestone.billingTextID15 = item.BillingLineID;
                        break;
                }
            }
        }
    }
}
