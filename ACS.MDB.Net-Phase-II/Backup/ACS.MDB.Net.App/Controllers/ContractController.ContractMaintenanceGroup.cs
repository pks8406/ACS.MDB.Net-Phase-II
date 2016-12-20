using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ContractController
    {
        //
        // GET: /ContractController.ContractMaintenanceGroup/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodFrequencyId"></param>
        /// <returns></returns>
        public ActionResult ContractMaintenanceGroupIndex(int contractId, int periodFrequencyId, 
                                                          string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                          string finalBillingPeriodEndDate, int documentTypeId, 
                                                          int invoiceAdvancedArrears, int invoiceInAdvance, 
                                                          bool? isNewGroup, bool? isExistingGroup, string groupName)
        {

            MODEL.ContractMaintenanceGroup contractMaintenanceGroup = new MODEL.ContractMaintenanceGroup();

            //DateTime dtFirstPeriodStartDate = new DateTime();
            //DateTime dtFinalBillingPeriodEndDate = new DateTime();
            if (groupName == "null")
            {
                groupName = null;
            }

            contractMaintenanceGroup.ContractId = contractId;
            contractMaintenanceGroup.PeriodFrequencyId = periodFrequencyId;
            contractMaintenanceGroup.DocumentTypeId = documentTypeId;
            contractMaintenanceGroup.InvoiceAdvancedArrears = invoiceAdvancedArrears;
            contractMaintenanceGroup.InvoiceInAdvance = invoiceInAdvance;
            contractMaintenanceGroup.IsNewGroup = isNewGroup;
            contractMaintenanceGroup.IsExistingGroup = isExistingGroup;
            contractMaintenanceGroup.GroupName = groupName;

            DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
            DateTime? dtFirstRenewalDate = ConvertDate(firstRenewalDate);
            DateTime? dtFinalBillingPeriodStartDate = ConvertDate(finalBillingPeriodStartDate);
            DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);

            contractMaintenanceGroup.FirstPeriodStartDate = dtFirstPeriodStartDate;
            contractMaintenanceGroup.FirstRenewalDate = dtFirstRenewalDate;
            contractMaintenanceGroup.FinalRenewalStartDate = dtFinalBillingPeriodStartDate;
            contractMaintenanceGroup.FinalRenewalEndDate = dtFinalBillingPeriodEndDate;

            //Fill Groupname list if group is existing
            if (isExistingGroup == true)
            {
                contractMaintenanceGroup.GroupNameList = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, 
                                                                                             dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                             dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                             invoiceAdvancedArrears, invoiceInAdvance);
            }

            #region commented code
            //if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate != "null")
            //{
            //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //    contractMaintenanceGroup.FirstPeriodStartDate = dtFirstPeriodStartDate;
            //    contractMaintenanceGroup.FinalRenewalEndDate = dtFinalBillingPeriodEndDate;
            //}
            //else if (firstPeriodStartDate == "null" && finalBillingPeriodEndDate != "null")
            //{
            //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //    contractMaintenanceGroup.FirstPeriodStartDate = null;
            //    contractMaintenanceGroup.FinalRenewalEndDate = dtFinalBillingPeriodEndDate;
            //}
            //else if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate == "null")
            //{
            //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //    contractMaintenanceGroup.FirstPeriodStartDate = dtFirstPeriodStartDate;
            //    contractMaintenanceGroup.FinalRenewalEndDate = null;
            //}
            //else
            //{
            //    contractMaintenanceGroup.FirstPeriodStartDate = null;
            //    contractMaintenanceGroup.FinalRenewalEndDate = null;
            //}
            #endregion

            return PartialView("ContractMaintenanceGroupIndex", contractMaintenanceGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodFrequencyId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult GetContractMaintenanceGroupList(MODEL.jQueryDataTableParamModel param,
                                                            int contractId, int periodFrequencyId,
                                                            string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                            string finalBillingPeriodEndDate, int documentTypeId,
                                                            int invoiceAdvancedArrears, int invoiceInAdvance,
                                                            bool? isGroupNew, bool? isGroupExisting,
                                                            string groupName)
        {
            try
            {
                List<MODEL.ContractMaintenanceGroup> contractMaintenanceGroupList = new List<MODEL.ContractMaintenanceGroup>();
                List<ContractMaintenanceVO> contractMaintenanceVOList = new List<ContractMaintenanceVO>();
                ContractMaintenanceGroupService contractManitenanceGroupService = new ContractMaintenanceGroupService();

                if (groupName == "")
                {
                    groupName = null;
                }

                if (groupName != null && isGroupExisting != true)
                {
                    //When user click on upgroup and if that selected record id default billing line 
                    //return GetContractMaintenanceGroupListByGroupNameForDefaultLine(param, contractId, periodFrequencyId, groupName);
                    contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupNameForDefaultLine(contractId, periodFrequencyId, groupName);
                }
                else if (isGroupExisting == true)
                {
                    // When user want to add new records in existing group
                    //return GetContractMaintenanceGroupListByGroupName(param, contractId, periodFrequencyId, firstPeriodStartDate, finalBillingPeriodEndDate, groupName);
                    contractMaintenanceVOList = GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, 
                                                                                           firstPeriodStartDate,firstRenewalDate, finalBillingPeriodStartDate,
                                                                                           finalBillingPeriodEndDate,documentTypeId, 
                                                                                           invoiceAdvancedArrears, invoiceInAdvance, groupName);
                }
                else
                {
                    // To create new group get all billing lines based in criteria
                    contractMaintenanceVOList = GetContractMaintenanceList(contractId, periodFrequencyId, 
                                                                           firstPeriodStartDate,firstRenewalDate,finalBillingPeriodStartDate,
                                                                           finalBillingPeriodEndDate,documentTypeId, 
                                                                           invoiceAdvancedArrears, invoiceInAdvance, isGroupNew);
                }


                foreach (var item in contractMaintenanceVOList)
                {
                    contractMaintenanceGroupList.Add(new Models.ContractMaintenanceGroup(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                //int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //var orderingFunction = GetContractMaintenanceOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, contractMaintenanceGroupList);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }

        }

        /// <summary>
        /// Get Contract maintenance billing lines when user is creating new group based on matching criteria
        /// </summary>
        /// <param name="contractId">Contract id belongs to contract maintenance line</param>
        /// <param name="periodFrequencyId">Period frequency of selected contract maintenance line</param>
        /// <param name="firstPeriodStartDate">First period start date of selected contract maintenance line</param>
        /// <param name="finalBillingPeriodEndDate">Final billing period end date of selected contract maintenance line</param>
        /// <param name="isGroupNew">Is user creating new group</param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceList(int contractId, int periodFrequencyId, 
                                                                      string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                                      string finalBillingPeriodEndDate, int documentTypeId, 
                                                                      int invoiceAdvancedArrears, int invoiceInAdvance, bool? isGroupNew)
        {
            DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
            DateTime? dtFirstRenewalDate = ConvertDate(firstRenewalDate);
            DateTime? dtFinalBillingPeriodStartDate = ConvertDate(finalBillingPeriodStartDate);
            DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);


            ContractMaintenanceGroupService contractManitenanceGroupService = new ContractMaintenanceGroupService();
            List<ContractMaintenanceVO> contractMaintenanceVOList = new List<ContractMaintenanceVO>();

            contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupList(contractId, periodFrequencyId,
                                                                                                        dtFirstPeriodStartDate, dtFirstRenewalDate,dtFinalBillingPeriodStartDate,
                                                                                                        dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                                        invoiceAdvancedArrears, invoiceInAdvance, isGroupNew);

            #region CommentedCode
            //if (firstPeriodStartDate != "" && finalBillingPeriodEndDate != "")
            //{
            //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //    contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupList(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate, isGroupNew);
            //}
            //else if (firstPeriodStartDate == "" && finalBillingPeriodEndDate != "")
            //{
            //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //    contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupList(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate, isGroupNew);
            //}
            //else if (firstPeriodStartDate != "" && finalBillingPeriodEndDate == "")
            //{
            //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //    contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupList(contractId, periodFrequencyId, dtFirstPeriodStartDate, null, isGroupNew);
            //}
            //else
            //{
            //    contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupList(contractId, periodFrequencyId, null, null, isGroupNew);
            //} 
            #endregion

            return contractMaintenanceVOList;
        }

        /// <summary>
        /// Apply Grouping on selected billing lines
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public ActionResult ApplyGrouping(List<int> Ids, int defaultInGroupId)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
                contractMaintenanceGroupService.GroupContractMaintenance(Ids, userId, defaultInGroupId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Returns is contractMaintenance grouped or not
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <param name="chargeFrequencyId">chargeFrequencyId</param>
        /// <returns>Is contractMaintenance Grouped or not</returns>
        public ActionResult IsContractMaintenanceGrouped(int contractId, int periodFrequencyId,
                                                         string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                         string finalBillingPeriodEndDate, int documentTypeId,
                                                         int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            try
            {
                string userType = Session.GetUserType();
                if (userType == Constants.VIEWER_USER)
                {
                    throw new ApplicationException(Constants.UNAUTHORISED_USER);
                }
                else
                {
                    bool isGrouped;

                    DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
                    DateTime? dtFirstRenewalDate = ConvertDate(firstRenewalDate);
                    DateTime? dtFinalBillingPeriodStartDate = ConvertDate(finalBillingPeriodStartDate);
                    DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);
                    

                    ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();

                    //If first period start date and first renewal date is different for charge frequency other than credit and adhoc
                    if ((periodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) && (periodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
                    {
                        if (dtFirstPeriodStartDate != null && dtFirstPeriodStartDate != dtFirstRenewalDate)
                        {
                            throw new ApplicationException(Constants.FIRST_PERIOD_START_DATE_AND_FIRST_RENEWAL_DATE_SAME);
                        }
                    }
                    
                    List<ContractMaintenanceVO> contractMaintenanceVOList = contractMaintenanceGroupService.GetContractMaintenanceGroupList(contractId, periodFrequencyId, 
                                                                                                                                            dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                                                                            dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                                                                             invoiceAdvancedArrears, invoiceInAdvance, false);
                    if (contractMaintenanceVOList.Count < 2)
                    {
                        if ((periodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) && (periodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
                        {

                            throw new ApplicationException(Constants.TWO_RECORDS_NEEDED_TO_CREATE_GROUP);
                        }
                        else
                        {
                            throw new ApplicationException("You need at least two records with same final billing period start date, final billing period end date, charge frequency, document type, invoice in advance and its value to create a group");
                        }
                    }
                    else
                    {
                        isGrouped = contractMaintenanceGroupService.IsContractMaintenanceGrouped(contractId, periodFrequencyId,
                                                                                               dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                               dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                               invoiceAdvancedArrears, invoiceInAdvance);
                    }

                    #region commentedCode
                    //if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate != "null")
                    //{
                    //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
                    //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
                    //    isGrouped = contractMaintenanceGroupService.IsContractMaintenanceGrouped(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate);
                    //}
                    //else if (firstPeriodStartDate == "null" && finalBillingPeriodEndDate != "null")
                    //{
                    //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
                    //    isGrouped = contractMaintenanceGroupService.IsContractMaintenanceGrouped(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate);
                    //}
                    //else if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate == "null")
                    //{
                    //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
                    //    isGrouped = contractMaintenanceGroupService.IsContractMaintenanceGrouped(contractId, periodFrequencyId, dtFirstPeriodStartDate, null);
                    //}
                    //else
                    //{
                    //    isGrouped = contractMaintenanceGroupService.IsContractMaintenanceGrouped(contractId, periodFrequencyId, null, null);
                    //}
                    #endregion

                    return Json(isGrouped, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Returns record count which are Grouped
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <returns></returns>
        public ActionResult GetRecordCountOfGroupedElements(int contractId, int periodFrequencyId, 
                                                            string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                            string finalBillingPeriodEndDate, int documentTypeId,
                                                            int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            try
            {
                DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
                DateTime? dtFirstRenewalDate = ConvertDate(firstRenewalDate);
                DateTime? dtFinalBillingPeriodStartDate = ConvertDate(finalBillingPeriodStartDate);
                DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);

                ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
                var recordCount = 0;

                recordCount = contractMaintenanceGroupService.GetRecordCountOfGroupedElements(contractId, periodFrequencyId,
                                                                                                dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                                dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                                invoiceAdvancedArrears, invoiceInAdvance);

                #region Commented Code
                //if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate != "null")
                //{
                //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
                //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfGroupedElements(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate);
                //}
                //else if (firstPeriodStartDate == "null" && finalBillingPeriodEndDate != "null")
                //{
                //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfGroupedElements(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate);
                //}
                //else if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate == "null")
                //{
                //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfGroupedElements(contractId, periodFrequencyId, dtFirstPeriodStartDate, null);
                //}
                //else
                //{
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfGroupedElements(contractId, periodFrequencyId, null, null);
                //} 
                #endregion

                return Json(recordCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }

            //ContractMaintenanceService contractManitenanceService = new ContractMaintenanceService();
            //var recordCount = contractManitenanceService.GetRecordCount(contractId, periodFrequencyId, firstPeriodStartDate, finalBillingPeriodEndDate);

            //return recordCount;
        }

        /// <summary>
        /// Returns record count which are ungrouped
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <returns></returns>
        public ActionResult GetRecordCountOfUngroupedElements(int contractId, int periodFrequencyId, 
                                                              string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                              string finalBillingPeriodEndDate, int documentTypeId,
                                                              int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            try
            {
                ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();

                DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
                DateTime? dtFirstRenewalDate = ConvertDate(firstRenewalDate);
                DateTime? dtFinalBillingPeriodStartDate = ConvertDate(finalBillingPeriodStartDate);
                DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);

                var recordCount = 0;

                recordCount = contractMaintenanceGroupService.GetRecordCountOfUngroupedElements(contractId, periodFrequencyId,
                                                                                                dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                                dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                                invoiceAdvancedArrears, invoiceInAdvance);


                #region commentedCode
                //if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate != "null")
                //{
                //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
                //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfUngroupedElements(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate);
                //}
                //else if (firstPeriodStartDate == "null" && finalBillingPeriodEndDate != "null")
                //{
                //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfUngroupedElements(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate);
                //}
                //else if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate == "null")
                //{
                //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfUngroupedElements(contractId, periodFrequencyId, dtFirstPeriodStartDate, null);
                //}
                //else
                //{
                //    recordCount = contractMaintenanceGroupService.GetRecordCountOfUngroupedElements(contractId, periodFrequencyId, null, null);
                //}
                #endregion

                return Json(recordCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Gets list of grouped items 
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="groupId">Group Id</param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByGroupName(int contractId, int periodFrequencyId, 
                                                                                      string firstPeriodStartDate, string firstRenewalDate, string finalBillingPeriodStartDate,
                                                                                      string finalBillingPeriodEndDate, int documentTypeId, 
                                                                                      int invoiceAdvancedArrears, int invoiceInAdvance, string groupName)
        {
            ContractMaintenanceGroupService contractManitenanceGroupService = new ContractMaintenanceGroupService();
            List<ContractMaintenanceVO> contractVOList = new List<ContractMaintenanceVO>();

            DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
            DateTime? dtFirstRenewalDate = ConvertDate(firstRenewalDate);
            DateTime? dtFinalBillingPeriodStartDate = ConvertDate(finalBillingPeriodStartDate);
            DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);

            if (groupName == null)
            {
                contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId,
                                                                                                          dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                                          dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                                          invoiceAdvancedArrears, invoiceInAdvance, groupName);
            }
            else
            {
                contractVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, 
                                                                                                            dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                                            dtFinalBillingPeriodEndDate, documentTypeId, 
                                                                                                            invoiceAdvancedArrears, invoiceInAdvance, groupName);
            }
            return contractVOList;

            #region commentedCode
            //if (groupName == "null")
            //    {
            //        if (firstPeriodStartDate != "" && finalBillingPeriodEndDate != "")
            //        {
            //            dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //            dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate);
            //        }
            //        else if (firstPeriodStartDate == "" && finalBillingPeriodEndDate != "")
            //        {
            //            dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate);
            //        }
            //        else if (firstPeriodStartDate != "" && finalBillingPeriodEndDate == "")
            //        {
            //            dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, dtFirstPeriodStartDate, null);
            //        }
            //        else
            //        {
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, null, null);
            //        }
            //    }
            //    else
            //    {
            //        if (firstPeriodStartDate != "" && finalBillingPeriodEndDate != "")
            //        {
            //            dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //            dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate, groupName);
            //        }
            //        else if (firstPeriodStartDate == "" && finalBillingPeriodEndDate != "")
            //        {
            //            dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate, groupName);
            //        }
            //        else if (firstPeriodStartDate != "" && finalBillingPeriodEndDate == "")
            //        {
            //            dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, dtFirstPeriodStartDate, null, groupName);
            //        }
            //        else
            //        {
            //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, null, null, groupName);
            //        }
            //    } 
            //foreach (var item in contractVOList)
            //{
            //    contractMaintenanceGroup.Add(new Models.ContractMaintenanceGroup(item));
            //}

            ////get the field on with sorting needs to happen and set the
            ////ordering function/delegate accordingly.
            ////int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            ////var orderingFunction = GetContractMaintenanceOrderingFunction(sortColumnIndex);

            //var result = GetFilteredObjects(param, contractMaintenanceGroup);
            //return result;
            //}
            //catch (Exception e)
            //{
            //    return new HttpStatusCodeAndErrorResult(500, e.Message);
            //}
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<string> GetContractMaintenanceGroupNameList(int contractId, int periodFrequencyId, 
                                                                DateTime? dtFirstPeriodStartDate, DateTime? dtFirstRenewalDate, DateTime? dtFinalBillingPeriodStartDate,
                                                                DateTime? dtFinalBillingPeriodEndDate, int documentTypeId,
                                                                int invoiceAdvancedArrears, int invoiceInAdvance)
        {

            ContractMaintenanceGroupService contractManitenanceGroupService = new ContractMaintenanceGroupService();

            //MODEL.ContractMaintenanceGroup contractMaintenanceGroup = new MODEL.ContractMaintenanceGroup();
            List<string> contractMaintenancesGroupList = contractManitenanceGroupService.GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, 
                                                                                                                             dtFirstPeriodStartDate, dtFirstRenewalDate, dtFinalBillingPeriodStartDate,
                                                                                                                             dtFinalBillingPeriodEndDate, documentTypeId,
                                                                                                                             invoiceAdvancedArrears, invoiceInAdvance);

            //foreach (var item in contractMaintenancesGroupList)
            //{
            //    contractMaintenanceGroup.GroupNameList.Add(item);
            //}

            return contractMaintenancesGroupList;
        }

        /// <summary>
        /// Get contract maintenance details by Id
        /// </summary>
        /// <param name="contractMaintenanceId">contract maintenanceId id</param>
        /// <returns>Contract Maintenace Details</returns>
        public ActionResult GetContractMaintenanceById(int contractMaintenanceId)
        {
            MODEL.ContractMaintenanceGroup contractMaintenanceGroup = new MODEL.ContractMaintenanceGroup();

            try
            {
                string userType = Session.GetUserType();
                if (userType == Constants.VIEWER_USER)
                {
                    throw new ApplicationException(Constants.UNAUTHORISED_USER);
                }
                else
                {
                    //Get contractLine details
                    ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
                    ContractMaintenanceVO contractMaintenanceVO = contractMaintenanceService.GetContractMaintenanceById(contractMaintenanceId);
                    contractMaintenanceGroup = new MODEL.ContractMaintenanceGroup(contractMaintenanceVO);
                    return Json(contractMaintenanceGroup, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Get total no of records in contract maintenance group list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodfrequency Id</param>
        /// <param name="groupId">group Id</param>
        /// <returns>Total no of records in list</returns>
        public ActionResult GetRecordCountofGroupedItemsByGroupId(int contractId, int periodFrequencyId, int groupId)
        {
            try
            {
                ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
                var recordCount = 0;
                if (groupId != null)
                {
                    recordCount = contractMaintenanceGroupService.GetRecordCountofGroupedItemsByGroupId(contractId, periodFrequencyId, groupId);
                }
                else
                {
                    throw new ApplicationException("GroupId not found");
                }
                return Json(recordCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Ungroup the contractmaintenance Group
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupId"></param>
        public ActionResult UngroupContractMaintenance(int contractId, int periodFrequencyId, int groupId)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
                contractMaintenanceGroupService.UngroupContractMaintenance(contractId, periodFrequencyId, groupId, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Ungroup the contractMaintenance item
        /// </summary>
        /// <param name="contractMaintenanceId">contractMaintenance Id</param>        
        public ActionResult UngroupContractMaintenanceItem(int contractMaintenanceId)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
                contractMaintenanceGroupService.UngroupContractMaintenanceItem(contractMaintenanceId, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Return a date if not null else null
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        private DateTime? ConvertDate(string inputDate)
        {
            DateTime outParamDate;

            DateTime? date = !DateTime.TryParse(inputDate, out outParamDate)
                         ? (DateTime?)null
                         : outParamDate;

            return date;
        }

        #region Commented Code

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        //public ActionResult ContractMaintenanceExistingGroupIndex(int contractId, int periodFrequencyId, string firstPeriodStartDate, string finalBillingPeriodEndDate)
        //{

        //    MODEL.ContractMaintenanceGroup contractMaintenanceGroup = new MODEL.ContractMaintenanceGroup();

        //    DateTime dtFirstPeriodStartDate = new DateTime();
        //    DateTime dtFinalBillingPeriodEndDate = new DateTime();

        //    contractMaintenanceGroup.ContractId = contractId;
        //    contractMaintenanceGroup.PeriodFrequencyId = periodFrequencyId;
        //    contractMaintenanceGroup.GroupNameList = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId);

        //    if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate != "null")
        //    {
        //        dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
        //        dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
        //        contractMaintenanceGroup.FirstPeriodStartDate = dtFirstPeriodStartDate;
        //        contractMaintenanceGroup.FinalRenewalEndDate = dtFinalBillingPeriodEndDate;
        //    }
        //    else if (firstPeriodStartDate == "null" && finalBillingPeriodEndDate != "null")
        //    {                
        //        dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
        //        contractMaintenanceGroup.FirstPeriodStartDate = null;
        //        contractMaintenanceGroup.FinalRenewalEndDate = dtFinalBillingPeriodEndDate;
        //    }
        //    else if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate == "null")
        //    {
        //        dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
        //        contractMaintenanceGroup.FirstPeriodStartDate = dtFirstPeriodStartDate;
        //        contractMaintenanceGroup.FinalRenewalEndDate = null;
        //    }
        //    else
        //    {
        //        contractMaintenanceGroup.FirstPeriodStartDate = null;
        //        contractMaintenanceGroup.FinalRenewalEndDate = null;
        //    }                               

        //    return PartialView("ContractMaintenanceExistingGroupIndex", contractMaintenanceGroup);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodFrequencyId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        //public ActionResult GetContractMaintenanceUngroupedItemsList(MODEL.jQueryDataTableParamModel param, int contractId, int periodFrequencyId, string firstPeriodStartDate, string finalBillingPeriodEndDate)
        //{            
        //    try
        //    {
        //        DateTime dtFirstPeriodStartDate = new DateTime();
        //        DateTime dtFinalBillingPeriodEndDate = new DateTime();

        //        ContractMaintenanceGroupService contractManitenanceGroupService = new ContractMaintenanceGroupService();
        //        List<MODEL.ContractMaintenanceGroup> contractMaintenanceGroup = new List<MODEL.ContractMaintenanceGroup>();
        //        List<ContractMaintenanceVO> contractVOList = new List<ContractMaintenanceVO>();

        //        if (firstPeriodStartDate != "" && finalBillingPeriodEndDate != "")
        //        {
        //            dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
        //            dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
        //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate);
        //        }
        //        else if (firstPeriodStartDate == "" && finalBillingPeriodEndDate != "")
        //        {
        //            dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
        //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate);
        //        }
        //        else if (firstPeriodStartDate != "" && finalBillingPeriodEndDate == "")
        //        {
        //            dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
        //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, dtFirstPeriodStartDate, null);
        //        }
        //        else
        //        {
        //            contractVOList = contractManitenanceGroupService.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, null, null);
        //        }

        //        foreach (var item in contractVOList)
        //        {
        //            contractMaintenanceGroup.Add(new Models.ContractMaintenanceGroup(item));
        //        }

        //        //get the field on with sorting needs to happen and set the
        //        //ordering function/delegate accordingly.
        //        //int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
        //        //var orderingFunction = GetContractMaintenanceOrderingFunction(sortColumnIndex);

        //        var result = GetFilteredObjects(param, contractMaintenanceGroup);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeAndErrorResult(500, e.Message);
        //    }
        //}

        /// <summary>
        /// Get contract Maintenance group list for given criteria
        /// </summary>
        /// <param name="param"></param>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        //public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByGroupNameForDefaultLine(MODEL.jQueryDataTableParamModel param, int contractId, int periodFrequencyId, string groupName)
        //{
        //    try
        //    {
        //        ContractMaintenanceGroupService contractManitenanceGroupService = new ContractMaintenanceGroupService();
        //        List<ContractMaintenanceVO> contractMaintenanceVOList = new List<ContractMaintenanceVO>();

        //        if (groupName != "")
        //        {
        //            contractMaintenanceVOList = contractManitenanceGroupService.GetContractMaintenanceGroupListByGroupNameForDefaultLine(contractId, periodFrequencyId, groupName);

        //            foreach (var item in contractMaintenanceVOList)
        //            {
        //                contractMaintenanceGroupList.Add(new Models.ContractMaintenanceGroup(item));
        //            }
        //        }

        //        //get the field on with sorting needs to happen and set the
        //        //ordering function/delegate accordingly.
        //        //int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
        //        //var orderingFunction = GetContractMaintenanceOrderingFunction(sortColumnIndex);

        //        var result = GetFilteredObjects(param, contractMaintenanceGroupList);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeAndErrorResult(500, e.Message);
        //    }
        //}

        /// <summary>
        /// Return number of records in the list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="firstPeriodStartDate">firstPeriodStart Date</param>
        /// <param name="finalBillingPeriodEndDate">Final Billing Period End Date</param>
        /// <returns>record Count</returns>
        //public ActionResult GetRecordCount(int contractId, int periodFrequencyId, string firstPeriodStartDate, string finalBillingPeriodEndDate)
        //{
        //    try
        //    {
        //        //DateTime? dtFirstPeriodStartDate;
        //        //DateTime? dtFinalBillingPeriodEndDate;

        //        ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
        //        var recordCount = 0;
        //        DateTime? dtFirstPeriodStartDate = ConvertDate(firstPeriodStartDate);
        //        DateTime? dtFinalBillingPeriodEndDate = ConvertDate(finalBillingPeriodEndDate);

        //        recordCount = contractMaintenanceGroupService.GetRecordCount(contractId, periodFrequencyId, dtFirstPeriodStartDate,dtFinalBillingPeriodEndDate);

        //        #region Commented Code
        //        //if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate != "null")
        //        //{
        //        //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
        //        //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
        //        //    recordCount = contractMaintenanceGroupService.GetRecordCount(contractId, periodFrequencyId, dtFirstPeriodStartDate, dtFinalBillingPeriodEndDate);
        //        //}
        //        //else if (firstPeriodStartDate == "null" && finalBillingPeriodEndDate != "null")
        //        //{
        //        //    dtFinalBillingPeriodEndDate = DateTime.Parse(finalBillingPeriodEndDate);
        //        //    recordCount = contractMaintenanceGroupService.GetRecordCount(contractId, periodFrequencyId, null, dtFinalBillingPeriodEndDate);
        //        //}
        //        //else if (firstPeriodStartDate != "null" && finalBillingPeriodEndDate == "null")
        //        //{
        //        //    dtFirstPeriodStartDate = DateTime.Parse(firstPeriodStartDate);
        //        //    recordCount = contractMaintenanceGroupService.GetRecordCount(contractId, periodFrequencyId, dtFirstPeriodStartDate, null);
        //        //}
        //        //else
        //        //{
        //        //    recordCount = contractMaintenanceGroupService.GetRecordCount(contractId, periodFrequencyId, null, null);
        //        //} 
        //        #endregion

        //        return Json(recordCount, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new ApplicationException(e.Message));
        //    }
        //}

        #endregion
    }
}
