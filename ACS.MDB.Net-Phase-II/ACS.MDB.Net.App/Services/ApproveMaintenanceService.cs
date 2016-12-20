using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.FileUtility;

namespace ACS.MDB.Net.App.Services
{
    public class ApproveMaintenanceService
    {
        ApproveMaintenanceDAL approveMaintenanceDAL = null;
        List<string> errorList = new List<string>();
        bool isValid = true;

        /// <summary>
        /// Default constructor 
        /// </summary>
        public ApproveMaintenanceService()
        {
            approveMaintenanceDAL = new ApproveMaintenanceDAL();
        }

        /// <summary>
        /// Get milestone based on filtered criteria
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="invoiceCustomerId">The customer id</param>
        /// <param name="divisionId">The division id</param>
        /// <param name="milestoneStatusId">The milestone status id</param>
        /// <param name="startDate">The invoice date(start date)</param>
        /// <param name="endDate">The invoice date (end date)</param>
        /// <param name="userId">The logged in user id</param>
        /// <returns></returns>
        public List<MilestoneVO> GetApproveMaintenance(int? companyId, int? invoiceCustomerId, int? divisionId, int? milestoneStatusId,
                                                            DateTime? startDate, DateTime? endDate, int? userId)
        {
            return approveMaintenanceDAL.GetApproveMaintenanceList(companyId, invoiceCustomerId, divisionId, milestoneStatusId,
                                                            startDate, endDate, userId);
        }

        /// <summary>
        /// Deleted selected milestone
        /// </summary>
        /// <param name="Ids">List of selected milestone ids</param>
        /// <param name="userId">Logged in user id</param>
        public void DeleteApproveMaintenance(List<int> Ids, int? userId)
        {
            approveMaintenanceDAL.DeleteApproveMaintenance(Ids, userId);
        }

        /// <summary>
        /// Approve selected milestone ids
        /// </summary>
        /// <param name="Ids">List of selected milestone ids</param>
        /// <param name="userId">Logged in user id</param>
        public void ApproveAllMaintenance(List<int> Ids, int? userId)
        {
            approveMaintenanceDAL.ApproveAllMaintenance(Ids, userId);
        }

        /// <summary>
        /// Unapprove milestones
        /// </summary>
        /// <param name="Ids">selected milestone ids</param>
        /// <param name="userId">The logged in user id</param>
        public void UnApproveAllMaintenance(List<int> Ids, int? userId)
        {
            approveMaintenanceDAL.UnApproveAllMaintenance(Ids, userId);
        }

        /// <summary>
        /// Generate invoice of milestone which has status "Approved for Payment" and approved
        /// in text file as per Open Account format
        /// </summary>
        /// <param name="companyId">Company id</param>
        /// <param name="divisionId">Division id</param>
        /// <param name="invoiceCustomerId">Invoice customer id</param>
        /// <param name="fromDate">The from date</param>
        /// <param name="toDate">The end date</param>
        /// <param name="userId"></param>
        /// <param name="invoiceDate"></param>
        public void GenerateInvoice(int companyId, int divisionId, int invoiceCustomerId, DateTime fromDate,
                                    DateTime toDate, int? userId, DateTime invoiceDate)
        {
            // Get contract details based on filtered criteria
            List<InvoiceHeaderVO> invoiceHeaderVos = approveMaintenanceDAL.GenerateInvoice(companyId, divisionId,
                                                                                           invoiceCustomerId,
                                                                                           fromDate, toDate, userId);

            if (invoiceHeaderVos.Count <= 0)
            {
                throw new ApplicationException(Constants.NO_MILESTONE_FOUND_FOR_AP);
            }

            // List of milestone all milestones
            var invoiceHeaderVOList = new List<InvoiceHeaderVO>();

            // Already processed contract ids 
            List<string> processedInvoice = new List<string>();

            //Process invoice by group
            foreach (var invoiceHeaderVo in invoiceHeaderVos)
            {
                //Check generate invoice by group
                bool isCustomerGroupBy = CheckCustomerGroupBy(invoiceHeaderVo.CompanyId,
                                                              invoiceHeaderVo.InvoiceCustomerId);
                if (isCustomerGroupBy)
                {

                    var invoiceHeaderByGroup = new List<InvoiceHeaderVO>();
                    if (!processedInvoice.Contains(Convert.ToString(invoiceHeaderVo.ContractId + "-" + invoiceHeaderVo.DocumentTypeID)))
                    {
                        invoiceHeaderByGroup =
                            invoiceHeaderVos.Where(m => m.InvoiceCustomerId == invoiceHeaderVo.InvoiceCustomerId &&
                                                        m.CompanyId == invoiceHeaderVo.CompanyId &&
                                                        m.Currency == invoiceHeaderVo.Currency &&
                                                        m.DocumentTypeID == invoiceHeaderVo.DocumentTypeID).ToList();

                        List<InvoiceHeaderVO> groupByCustomer = ProcessInvoiceByGroup(invoiceHeaderByGroup, fromDate, toDate, invoiceDate);

                        // Set posting periad and year
                        foreach (var invoiceHeaderVO in groupByCustomer)
                        {
                            // Set posting perios & posting year for invoice header
                            GetPostingPeriodAndYear(invoiceHeaderVO);
                        }

                        invoiceHeaderVOList.AddRange(groupByCustomer);

                        //Process invoice by grouping of customer details
                        //InvoiceHeaderVOList.Add(ProcessInvoiceByGroup(InvoiceHeaderByGroup, fromDate, toDate));

                        // Add contract id in already generated invoice list so next time it will not generate invoice
                        processedInvoice.AddRange(invoiceHeaderByGroup.Select(invoice => invoice.ContractId + "-" + invoice.DocumentTypeID));
                    }

                }
            }

            // Process each invoice by contract id
            foreach (var invoiceHeaderVo in invoiceHeaderVos)
            {
                // Check is invoice already processed in group by customer 
                if (!processedInvoice.Contains(Convert.ToString(invoiceHeaderVo.ContractId + "-" + invoiceHeaderVo.DocumentTypeID)))
                {
                    //Generate invoice by contract, company, customer 
                    InvoiceHeaderVO invoiceHeaderVO = approveMaintenanceDAL.GetContarctDetails(invoiceHeaderVo, invoiceDate);

                    // Set posting perios & posting year for invoice header
                    GetPostingPeriodAndYear(invoiceHeaderVO);

                    // Process each invoice detail
                    List<InvoiceGLDetailVO> invoiceDetails =
                        approveMaintenanceDAL.GetInvoiceDetails(invoiceHeaderVO, fromDate, toDate);

                    // Process each invoice line Generate N & X
                    List<InvoiceDetailVO> invoiceDetailVos = ProcessInvoiceDetails(invoiceDetails, invoiceHeaderVO, fromDate, toDate);

                    decimal totalAmount = 0, crTotalAmount = 0;
                    InvoiceHeaderVO crHeaderVO = null;

                    foreach (var invoiceDetailVo in invoiceDetailVos)
                    {
                        foreach (var detailVo in invoiceDetailVo.NominalLinesList)
                        {

                            // Check milestone is credit or not
                            if (detailVo.Value < 0)
                            {
                                // Create new header for creadit invoice
                                if (crHeaderVO == null)
                                {
                                    crHeaderVO = new InvoiceHeaderVO(invoiceHeaderVO, invoiceDate);
                                }

                                crTotalAmount += detailVo.Value;
                            }
                            else
                            {
                                // Set total amount for invoice header
                                totalAmount += detailVo.Value;
                            }

                        }

                        if (invoiceDetailVo.isCreaditInvoice)
                        {
                            if (crHeaderVO != null) crHeaderVO.InvoiceDetailVos.Add(invoiceDetailVo);
                        }
                        else
                        {
                            invoiceHeaderVO.InvoiceDetailVos.Add(invoiceDetailVo);
                            // Check if invoice or credit note, negative value = credit
                            invoiceHeaderVO.Description = "Maintenance Invoice";
                        }

                        //// Check milestone is credit or not
                        //if (invoiceDetailVo.InvoiceGlDetails.Value < 0)
                        //{
                        //    // Create new header for creadit invoice
                        //    if (crHeaderVO == null)
                        //    {
                        //        crHeaderVO = new InvoiceHeaderVO(invoiceHeaderVO, invoiceDate);
                        //    }

                        //    crTotalAmount += invoiceDetailVo.InvoiceGlDetails.Value;
                        //    crHeaderVO.InvoiceDetailVos.Add(invoiceDetailVo);

                        //    //ValidateInvoiceHeader(crHeaderVO);

                        //    //InvoiceHeaderVOList.Add(crHeaderVO);
                        //    //creaditInvoiceHeaderVOs.Add(crHeaderVO);

                        //}
                        //else
                        //{
                        //    // Set total amount for invoice header
                        //    totalAmount += invoiceDetailVo.InvoiceGlDetails.Value;

                        //    invoiceHeaderVO.InvoiceDetailVos.Add(invoiceDetailVo);
                        //    // Check if invoice or credit note, negative value = credit
                        //    invoiceHeaderVO.Description = "Maintenance Invoice";
                        //}
                    }

                    // Validate credit note invoice
                    // CR-ARBS-46
                    if (crHeaderVO != null && crHeaderVO.InvoiceDetailVos.Count > 0)
                    {
                        crHeaderVO.Description = Constants.MAINTENANCE_CREDIT_NOTE;
                        crHeaderVO.TotalAmount = crTotalAmount;

                        ValidateInvoiceHeader(crHeaderVO);

                        // Add item in invoice list
                        invoiceHeaderVOList.Add(crHeaderVO);
                    }

                    // If invoice header is not credit & has invoice deatils 
                    //then add into invoice header list for print
                    if (invoiceHeaderVO.InvoiceDetailVos.Count > 0)
                    {
                        // Set aggregate amount of invoice header 
                        invoiceHeaderVO.TotalAmount = totalAmount;

                        ValidateInvoiceHeader(invoiceHeaderVO);
                        invoiceHeaderVOList.Add(invoiceHeaderVO);
                    }
                }
            }

            //Throw Error message if any error is there after completion whole process
            if (errorList.Count > 0)
            {
                TextFileUtility textFile = new TextFileUtility();
                string errorLogPath = ApplicationConfiguration.GetBillToOAErrorLogPath();
                string errorLogFileName = Constants.BILL_TO_OA_ERROR_LOG_FILENAME;

                // Add user id in file name so for each user unique gets created
                errorLogFileName += userId + ".txt";

                // wrire Bill to OA error log
                textFile.WriteLog(errorLogPath, errorLogFileName, errorList);

                throw new ApplicationException(Constants.INVALID_MILESTONE_BILL_TO_OA);
            }
            else
            {
                //Create flat file of invoice details
                CreateInvoiceFile(invoiceHeaderVOList);
            }
        }

        /// <summary>
        /// Process invoice details - N & X line 
        /// </summary>
        /// <param name="invoiceDetails">Invoice details object</param>
        /// <param name="invoiceHeaderVO">Invoice header object</param>
        /// <param name="fromDate">from date</param>
        /// <param name="toDate">to date</param>
        /// <returns></returns>
        private List<InvoiceDetailVO> ProcessInvoiceDetails(List<InvoiceGLDetailVO> invoiceDetails,
                                                            InvoiceHeaderVO invoiceHeaderVO, DateTime fromDate,
                                                            DateTime toDate)
        {
            var invoiceDetailVos = new List<InvoiceDetailVO>();

            //Process only grouped billing details/ milestone lines
            invoiceDetailVos.AddRange(ProcessGroupedBillingLines(invoiceDetails, invoiceHeaderVO, fromDate, toDate));


            // process line for ungrouped milestone & billing details
            foreach (var invoiceGlDetail in invoiceDetails)
            {
                if (invoiceGlDetail.IsGrouped != true)
                {
                    // Get milestone details
                    MilestoneVO milestoneVos = approveMaintenanceDAL.GetMilestoneDetails(invoiceHeaderVO,
                                                                                               invoiceGlDetail,
                                                                                               fromDate, toDate);

                    //foreach (var milestoneVo in milestoneVos)
                    //{
                    if (milestoneVos.IsGrouped == null || milestoneVos.IsGrouped == false)
                    {

                        InvoiceDetailVO invoiceDetailVO = new InvoiceDetailVO();

                        // Create clone object of GL line object
                        InvoiceGLDetailVO nominalLine = new InvoiceGLDetailVO(invoiceGlDetail);

                        if (ValidateJobCode(invoiceHeaderVO, nominalLine) == false)
                        {
                            //ARBS-144-to have customer name and customer code in error file
                            string error =
                                String.Format(Constants.JOB_CODE_MISSING + "'" + invoiceHeaderVO.ContractNumber +
                                              "', "
                                              + "Company - '" + invoiceHeaderVO.CompanyId + " - " +
                                              invoiceHeaderVO.CompanyName +" , Customer Name - "+invoiceHeaderVO.CustomerName+" , Customer Code - "+invoiceHeaderVO.CustomerCode +"'");
                            if (!errorList.Contains(error))
                            {
                                errorList.Add(error);
                            }
                            isValid = false;
                        }

                        CreateNominalLine(invoiceHeaderVO, nominalLine, milestoneVos);

                        //glDetail.TaxCode = invoiceHeaderVO.VatCode;
                        //glDetail.Value = milestoneVo.Amount;
                        //glDetail.RenewalStartDate = String.Format("{0:ddMMyyyy}",
                        //                                          milestoneVo.RenewalStartDate.Value);
                        //glDetail.RenewalEndDate = String.Format("{0:ddMMyyyy}", milestoneVo.RenewalEndDate.Value);
                        //glDetail.ContractDetails = invoiceHeaderVO.CompanyId == 159 ? "N,D," : "Y,D,";
                        //glDetail.ContractDetails += glDetail.RenewalStartDate + " to " +
                        //                            glDetail.RenewalEndDate + ",M" + milestoneVo.ID;

                        //Add GL details to invoice header
                        //invoiceDetailVO.InvoiceGlDetails = glDetail;

                        if (!invoiceDetailVO.NominalLinesList.Contains(nominalLine))
                        {
                            invoiceDetailVO.NominalLinesList.Add(nominalLine);
                        }

                        string billingLines = milestoneVos.MilestoneBillingLineVos.Aggregate(string.Empty,
                                                                                            (current, billingLine)
                                                                                            =>
                                                                                            current +
                                                                                            billingLine.LineText);

                        //Generate Billing line details for - extra line for invoice flat file
                        if (string.IsNullOrEmpty(billingLines))
                        {
                            string error = String.Format(Constants.MISSING_BILLING_LINES + "'" + invoiceHeaderVO.ContractNumber + "'," + "Company - '" + invoiceHeaderVO.CompanyId + "'");
                            if (!errorList.Contains(error))
                            {
                                errorList.Add(error);
                            }
                        }

                        InvoiceBillingLineVO invoiceBillingLine =
                            new InvoiceBillingLineVO(milestoneVos.MilestoneBillingLineVos);

                        invoiceBillingLine.VatCode = invoiceHeaderVO.VatCode;
                        invoiceBillingLine.Amount = milestoneVos.Amount;
                        invoiceBillingLine.Qty = milestoneVos.QTY;
                        invoiceBillingLine.UnitPrice =
                            Convert.ToDecimal(String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE,
                                                            (milestoneVos.Amount / invoiceBillingLine.Qty)));


                        invoiceDetailVO.isCreaditInvoice = nominalLine.Value < 0;

                        invoiceDetailVO.InvoiceBillingLines = invoiceBillingLine;

                        invoiceDetailVos.Add(invoiceDetailVO);
                        //}
                    }
                }
            }
            // Invoice details associated with header
            return invoiceDetailVos;
        }

        /// <summary>
        /// Process grouped billing lines with milestone
        /// </summary>
        /// <param name="invoiceDetails"></param>
        /// <param name="invoiceHeaderVO"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private List<InvoiceDetailVO> ProcessGroupedBillingLines(List<InvoiceGLDetailVO> invoiceDetails, InvoiceHeaderVO invoiceHeaderVO, DateTime fromDate,
                                                DateTime toDate)
        {
            List<InvoiceDetailVO> invoiceDetailVos = new List<InvoiceDetailVO>();

            List<MilestoneVO> milestoneVoList = approveMaintenanceDAL.GetGroupedMilestoneDetails(invoiceHeaderVO,
                                                                                                 invoiceDetails,
                                                                                                 fromDate, toDate);

            //Get milestone details by group
            var groupedMilestones = (from milestone in milestoneVoList
                                     select new
                                                {
                                                    milestone.ContractMaintenanceID,
                                                    milestone.ContractLineID,
                                                    milestone.ID,
                                                    milestone.GroupId,
                                                    milestone.PeriodFrequencyId
                                                }).GroupBy(g => new { g.GroupId, g.PeriodFrequencyId }).ToList();

            // How many nonimal line per group
            foreach (var groupedMilestone in groupedMilestones)
            {
                InvoiceDetailVO invoiceDetail = new InvoiceDetailVO();
                List<InvoiceGLDetailVO> nominalLineVos = new List<InvoiceGLDetailVO>();

                int totalQty = 0;
                foreach (var item in groupedMilestone)
                {
                    InvoiceGLDetailVO nominalLine =
                       invoiceDetails.FirstOrDefault(c => c.ContractLineId == item.ContractLineID &&
                                                          c.ContractMaintenanceId == item.ContractMaintenanceID &&
                                                          c.MilestoneId == item.ID &&
                                                          c.GroupId == groupedMilestone.Key.GroupId &&
                                                          c.PeriodFrequencyId == groupedMilestone.Key.PeriodFrequencyId);

                    MilestoneVO milestone = milestoneVoList.FirstOrDefault(m => m.ID == item.ID
                                                                                && m.GroupId == groupedMilestone.Key.GroupId
                                                                                && m.PeriodFrequencyId == groupedMilestone.Key.PeriodFrequencyId);

                    if (milestone != null)
                    {
                        InvoiceGLDetailVO invoiceGlDetailVO = new InvoiceGLDetailVO(nominalLine);
                        CreateNominalLine(invoiceHeaderVO, invoiceGlDetailVO, milestone);

                        totalQty += milestone.QTY;
                        nominalLineVos.Add(invoiceGlDetailVO);
                    }
                }

                // Check how many coding lines are grouped based on period frequency and group id
                //var grpedMilestoneVo = milestoneVoList.Where(m => m.GroupId == groupedMilestone.Key.GroupId
                //                                                  &&
                //                                                  m.PeriodFrequencyId ==
                //                                                  groupedMilestone.Key.PeriodFrequencyId)
                //                                      .GroupBy(g => g.ContractMaintenanceID).ToList();





                //foreach (var nLine in grpedMilestoneVo)
                //{
                //    InvoiceGLDetailVO nominalLine =
                //        invoiceDetails.FirstOrDefault(c => c.ContractMaintenanceId == nLine.Key &&
                //                                           c.GroupId == groupedMilestone.Key.GroupId &&
                //                                           c.PeriodFrequencyId == groupedMilestone.Key.PeriodFrequencyId);

                //    InvoiceGLDetailVO invoiceGlDetailVO = new InvoiceGLDetailVO(nominalLine);

                //    //InvoiceGLDetailVO nominalLine =
                //    //    approveMaintenanceDAL.GetContractLineBasedOnContractMaintenanceId(nLine.Key);

                //    MilestoneVO milestone =
                //        milestoneVoList.FirstOrDefault(m => m.ContractLineID == invoiceGlDetailVO.ContractLineId
                //                                            && m.ContractMaintenanceID == nLine.Key &&
                //                                            m.IsGrouped == true
                //                                            && m.PeriodFrequencyId == groupedMilestone.Key.PeriodFrequencyId
                //                                            && m.GroupId == groupedMilestone.Key.GroupId);


                //    CreateNominalLine(invoiceHeaderVO, invoiceGlDetailVO, milestone);

                //    totalQty += milestone.QTY;
                //    nominalLineVos.Add(invoiceGlDetailVO);
                //}

                MilestoneVO defaultPrintingBillingLine =
                    milestoneVoList.FirstOrDefault(m => m.GroupId == groupedMilestone.Key.GroupId
                                                        && m.PeriodFrequencyId == groupedMilestone.Key.PeriodFrequencyId
                                                        && m.IsDefaultLineInGroup == true);

                //If default print line milestone not found in milestone list then get default milestone line database
                if (defaultPrintingBillingLine == null)
                {
                    MilestoneVO milestoneVO = approveMaintenanceDAL.GetDefaultBillingLine(nominalLineVos[0].ContractId,
                                                                                          nominalLineVos[0].GroupId,
                                                                                          nominalLineVos[0].PeriodFrequencyId,
                                                                                          fromDate, toDate);
                    if (milestoneVO == null)
                    {
                        string error = String.Format(Constants.MILESTONE_MISSING + "'" +
                                                     invoiceHeaderVO.ContractNumber + "',"
                                                     + "Company - '" + invoiceHeaderVO.CompanyId + "'");
                        if (!errorList.Contains(error))
                        {
                            errorList.Add(error);
                        }
                    }
                    else
                    {
                        defaultPrintingBillingLine = milestoneVO;

                        string billingLines = defaultPrintingBillingLine.MilestoneBillingLineVos.Aggregate(string.Empty,
                                                                                   (current,
                                                                                    billingLine) =>
                                                                                   current +
                                                                                   billingLine
                                                                                       .LineText);

                        //Generate Billing line details for - extra line for invoice flat file
                        if (string.IsNullOrEmpty(billingLines))
                        {
                            string error =
                                String.Format(Constants.MISSING_BILLING_LINES + "'" + invoiceHeaderVO.ContractNumber + "',"
                                              + "Company - '" + invoiceHeaderVO.CompanyId + "'");

                            errorList.Add(error);
                        }
                    }
                }

                //To check defaultPrintingLine is not null
                if (defaultPrintingBillingLine != null)
                {
                    string billingLines = defaultPrintingBillingLine.MilestoneBillingLineVos.Aggregate(string.Empty,
                                                           (current,
                                                            billingLine) =>
                                                           current +
                                                           billingLine
                                                               .LineText);

                    //Generate Billing line details for - extra line for invoice flat file
                    if (string.IsNullOrEmpty(billingLines))
                    {
                        string error =
                            String.Format(Constants.MISSING_BILLING_LINES + "'" + invoiceHeaderVO.ContractNumber + "',"
                                          + "Company - '" + invoiceHeaderVO.CompanyId + "'");

                        if (!errorList.Contains(error))
                        {
                            errorList.Add(error);
                        }
                    }

                    decimal amount = nominalLineVos.Sum(nLine => nLine.Value);

                    invoiceDetail.isCreaditInvoice = amount < 0;

                    InvoiceBillingLineVO invoiceBillingLine =
                        new InvoiceBillingLineVO(defaultPrintingBillingLine.MilestoneBillingLineVos);

                    invoiceBillingLine.VatCode = invoiceHeaderVO.VatCode;
                    invoiceBillingLine.Amount = amount; //defaultPrintingBillingLine.Amount;
                    invoiceBillingLine.Qty = totalQty; //defaultPrintingBillingLine.QTY;
                    invoiceBillingLine.UnitPrice =
                        Convert.ToDecimal(String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE,
                                                        (amount / invoiceBillingLine.Qty)));

                    //Add nominal line 
                    invoiceDetail.NominalLinesList = nominalLineVos;
                    // Add X line billing printing line
                    invoiceDetail.InvoiceBillingLines = invoiceBillingLine;

                    //Add N & X line details in header
                    invoiceDetailVos.Add(invoiceDetail);
                }
            }

            return invoiceDetailVos;
        }

        /// <summary>
        /// Create nominal line
        /// </summary>
        /// <param name="invoiceHeaderVO">Invoice header value object</param>
        /// <param name="nominalLine">Nominal line</param>
        /// <param name="milestone">Milestone object</param>
        private void CreateNominalLine(InvoiceHeaderVO invoiceHeaderVO, InvoiceGLDetailVO nominalLine,
                                              MilestoneVO milestone)
        {
            nominalLine.TaxCode = invoiceHeaderVO.VatCode;
            nominalLine.Value = milestone.Amount;
            nominalLine.RenewalStartDate = String.Format("{0:ddMMyyyy}",
                                                         milestone.RenewalStartDate.Value);
            nominalLine.RenewalEndDate = String.Format("{0:ddMMyyyy}", milestone.RenewalEndDate.Value);
            nominalLine.ContractDetails = invoiceHeaderVO.CompanyId == 159 ? "N,D," : "Y,D,";
            nominalLine.ContractDetails += nominalLine.RenewalStartDate + " to " +
                                           nominalLine.RenewalEndDate + ",M" + milestone.ID;

            nominalLine.GroupId = milestone.GroupId;
            nominalLine.PeriodFrequencyId = milestone.PeriodFrequencyId;
        }

        /// <summary>
        /// Write invoice flat file on configured location
        /// </summary>
        /// <param name="InvoiceHeaderVOList">list of invoice details</param>
        private void CreateInvoiceFile(List<InvoiceHeaderVO> InvoiceHeaderVOList)
        {
            GenerateInvoiceService generateInvoiceService = new GenerateInvoiceService();
            generateInvoiceService.Write(InvoiceHeaderVOList);
        }

        /// <summary>
        /// Get Posting period & year for the company
        /// </summary>
        /// <param name="companyId"></param>
        private void GetPostingPeriodAndYear(InvoiceHeaderVO invoiceHeaderVO)
        {
            List<OAPeriodVO> oaPeriodVos = approveMaintenanceDAL.GetPeriod(invoiceHeaderVO.CompanyId);

            int postingPeriod = 0;
            DateTime currentDate = Convert.ToDateTime(invoiceHeaderVO.DocumentDate);

            foreach (var oaPeriodVo in oaPeriodVos)
            {
                List<string> dates = oaPeriodVo.PostingDates.Split(';').ToList();
                List<string> tempdates = new List<string>(dates);

                foreach (var dt in dates.Where(dt => dt.Contains("?")))
                {
                    tempdates.Remove(dt);
                }

                dates = tempdates;
                string startDate = (("01/" + dates[0].Substring(0, 3) + dates[0].Substring(6, 4)));

                DateTime date = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}",
                                                                 ("01/" + oaPeriodVo.PostingDates.Substring(0, 3) +
                                                                  oaPeriodVo.PostingDates.Substring(6, 4))));

                string date11 = (oaPeriodVo.PostingDates.Substring(110, 10));
                string date12 = (oaPeriodVo.PostingDates.Substring(121, 10));

                string[] oaDate;
                string lastDate;

                oaDate = oaPeriodVo.MaxPeriod == 11 ? date11.Split('/') : date12.Split('/');
                lastDate = oaDate[1] + "-" + oaDate[0] + "-" + oaDate[2];

                //Compare the system date against the 12 start and end dates to calculate the correct period
                if (currentDate >= date && currentDate <= Convert.ToDateTime(lastDate))
                {
                    if (currentDate >= Convert.ToDateTime(startDate) &&
                        currentDate <= ConvertDateTimeFormat(dates[0]))
                    {
                        postingPeriod = 1;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[0]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[1]))
                    {
                        postingPeriod = 2;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[1]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[2]))
                    {
                        postingPeriod = 3;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[2]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[3]))
                    {
                        postingPeriod = 4;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[3]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[4]))
                    {
                        postingPeriod = 5;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[4]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[5]))
                    {
                        postingPeriod = 6;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[5]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[6]))
                    {
                        postingPeriod = 7;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[6]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[7]))
                    {
                        postingPeriod = 8;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[7]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[8]))
                    {
                        postingPeriod = 9;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[8]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[9]))
                    {
                        postingPeriod = 10;
                    }
                    else if (currentDate >= ConvertDateTimeFormat(dates[9]).AddDays(1) &&
                             currentDate <= ConvertDateTimeFormat(dates[10]))
                    {
                        postingPeriod = 11;
                    }

                    else if (dates.Count > 11 && currentDate >= ConvertDateTimeFormat(dates[10]).AddDays(1)
                            && currentDate <= ConvertDateTimeFormat(dates[11]))
                    {
                        postingPeriod = 12;
                    }

                    invoiceHeaderVO.PostingPeriod = postingPeriod;
                    invoiceHeaderVO.PostingYear = Convert.ToInt32(oaPeriodVo.PostingYear);
                }
            }

            if (invoiceHeaderVO.PostingPeriod <= 0 || invoiceHeaderVO.PostingYear <= 0)
            {
                throw new ApplicationException(String.Format(Constants.OAPERIOD_DATA_NOT_AVAILABLE,
                                                             invoiceHeaderVO.CompanyName + " - " + invoiceHeaderVO.CompanyId));
            }

        }

        /// <summary>
        /// Convert date to dd-MM-yyyy format from dd/MM/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime ConvertDateTimeFormat(string date)
        {
            DateTime convertedDate = Convert.ToDateTime(
                DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));

            return convertedDate;

        }
        ///// <summary>
        ///// Process invoice header object
        ///// </summary>
        ///// <param name="invoiceHeaderVo"></param>
        //private InvoiceHeaderVO ProcessInvoice(InvoiceHeaderVO invoiceHeaderVo)
        //{
        //    invoiceHeaderVo = approveMaintenanceDAL.GetContarctDetails(invoiceHeaderVo);

        //    // Set posting perios & posting year for invoice header
        //    GetPostingPeriodAndYear(invoiceHeaderVo);

        //    // Get total amount for the contracts 
        //    //MilestoneService milestoneService = new MilestoneService();
        //    //invoiceHeaderVo.TotalAmount = milestoneService.GetTotalAmount(invoiceHeaderVo.ContractId, fromDate, toDate);

        //    // Check if invoice or credit note, negative value = credit
        //    invoiceHeaderVo.Description = "Maintenance Invoice";

        //    if (invoiceHeaderVo.TotalAmount < 0)
        //    {
        //        invoiceHeaderVo.Description = Constants.MAINTENANCE_CREDIT_NOTE;
        //    }

        //    ValidateInvoiceHeader(invoiceHeaderVo);

        //    return invoiceHeaderVo;
        //}

        /// <summary>
        /// Validate invoice header details
        /// </summary>
        /// <param name="invoiceHeaderVo"></param>
        private void ValidateInvoiceHeader(InvoiceHeaderVO invoiceHeaderVo)
        {
            if (string.IsNullOrEmpty(invoiceHeaderVo.CompanyId.ToString()))
            {
                errorList.Add(String.Format("The company is missing for contract number " + invoiceHeaderVo.ContractNumber));
                isValid = false;
                //throw new ApplicationException("The company is missing for contract number " + invoiceHeaderVo.ContractNumber);
            }
            if (string.IsNullOrEmpty(invoiceHeaderVo.PostingPeriod.ToString()))
            {
                errorList.Add(String.Format("The posting period is missing for contract number " + invoiceHeaderVo.ContractNumber));
                isValid = false;
                //throw new ApplicationException("The posting period is missing for contract number " + invoiceHeaderVo.ContractNumber);
            }
            if (string.IsNullOrEmpty(invoiceHeaderVo.CustomerCode))
            {
                errorList.Add(String.Format("The customer is missing for contract number " + invoiceHeaderVo.ContractNumber));
                isValid = false;
                //throw new ApplicationException("The customer is missing for contract number " + invoiceHeaderVo.ContractNumber);
            }
            if (string.IsNullOrEmpty(invoiceHeaderVo.PostingYear.ToString()))
            {
                errorList.Add(String.Format("The posting year is missing for contract number " + invoiceHeaderVo.ContractNumber));
                isValid = false;
                //throw new ApplicationException("The posting year is missing for contract number " + invoiceHeaderVo.ContractNumber);
            }
            if (string.IsNullOrEmpty(invoiceHeaderVo.VatCode))
            {
                errorList.Add(String.Format("The vat code is missing for customer no. " + invoiceHeaderVo.CustomerCode));
                isValid = false;
                //throw new ApplicationException("The vat code is missing for customer no. " + invoiceHeaderVo.CustomerCode);
            }
            if (string.IsNullOrEmpty(invoiceHeaderVo.Currency))
            {
                errorList.Add(String.Format("The currency is missing for contract number " + invoiceHeaderVo.ContractNumber));
                isValid = false;
                //throw new ApplicationException("The currency is missing for contract number " + invoiceHeaderVo.CustomerCode);
            }

            if (string.IsNullOrEmpty(invoiceHeaderVo.DocumentTypeCR) || string.IsNullOrEmpty(invoiceHeaderVo.DocumentTypeIN))
            {
                errorList.Add(String.Format("The document type is missing for division - " + invoiceHeaderVo.DivisionName));
                isValid = false;
            }
        }

        /// <summary>
        /// Process invoice by grouping of customer
        /// </summary>
        /// <param name="invoiceHeaderVo">invoice header details</param>
        /// <param name="fromDate">from date</param>
        /// <param name="toDate">end date</param>
        private List<InvoiceHeaderVO> ProcessInvoiceByGroup(List<InvoiceHeaderVO> invoiceHeaderVos, DateTime fromDate,
                                                      DateTime toDate, DateTime invoiceDate)
        {
            List<InvoiceHeaderVO> InvoiceHeaderVOList = new List<InvoiceHeaderVO>();

            //Check invoiceHeaderVos group by DocumentTypeId

            InvoiceHeaderVO groupedInvoiceVO = approveMaintenanceDAL.GetContarctDetails(invoiceHeaderVos[0], invoiceDate);
            InvoiceHeaderVO groupedCreditInvoiceVO = null;
            decimal totalAmount = 0, crTotalAmount = 0;

            foreach (var invoiceHeader in invoiceHeaderVos)
            {
                //InvoiceHeaderVO groupedInvoiceVO = approveMaintenanceDAL.GetContarctDetails(invoiceHeader, invoiceDate);
                InvoiceHeaderVO invoiceHeaderVo = approveMaintenanceDAL.GetContarctDetails(invoiceHeader, invoiceDate);

                // Process each invoice detail
                List<InvoiceGLDetailVO> invoiceDetails =
                    approveMaintenanceDAL.GetInvoiceDetails(invoiceHeaderVo, fromDate, toDate);

                // Process each invoice line Generate N & X
                List<InvoiceDetailVO> invoiceDetailVos = ProcessInvoiceDetails(invoiceDetails, invoiceHeaderVo, fromDate,
                                                                               toDate);

                foreach (var invoiceDetailVo in invoiceDetailVos)
                {
                    foreach (var detailVo in invoiceDetailVo.NominalLinesList)
                    {

                        // Check milestone is credit or not
                        if (detailVo.Value < 0)
                        {
                            // Create new header for creadit invoice
                            if (groupedCreditInvoiceVO == null)
                            {
                                groupedCreditInvoiceVO = new InvoiceHeaderVO(invoiceHeaderVo, invoiceDate);
                            }

                            crTotalAmount += detailVo.Value;
                        }
                        else
                        {
                            // Set total amount for invoice header
                            totalAmount += detailVo.Value;
                        }

                    }

                    if (invoiceDetailVo.isCreaditInvoice)
                    {
                        if (groupedCreditInvoiceVO != null)
                            groupedCreditInvoiceVO.InvoiceDetailVos.Add(invoiceDetailVo);
                    }
                    else
                    {
                        //invoiceHeaderVo.InvoiceDetailVos.Add(invoiceDetailVo);
                        groupedInvoiceVO.InvoiceDetailVos.Add(invoiceDetailVo);
                    }
                }
                //// Check milestone is credit or not
                //if (invoiceDetailVo.InvoiceGlDetails.Value < 0)
                //{
                //    // Create new header for credit invoice
                //    if (groupedCreditInvoiceVO == null)
                //    {
                //        groupedCreditInvoiceVO = new InvoiceHeaderVO(invoiceHeaderVo, invoiceDate);
                //    }

                //    crTotalAmount += invoiceDetailVo.InvoiceBillingLines.Amount;
                //    groupedCreditInvoiceVO.InvoiceDetailVos.Add(invoiceDetailVo);
                //}
                //else
                //{
                //    // Set total amount for invoice header
                //    totalAmount += invoiceDetailVo.InvoiceGlDetails.Value;
                //    groupedInvoiceVO.InvoiceDetailVos.Add(invoiceDetailVo);
                //}
            }
            //Validate and add credit invoice in invoice list
            if (groupedCreditInvoiceVO != null && groupedCreditInvoiceVO.InvoiceDetailVos.Count > 0)
            {
                groupedCreditInvoiceVO.Description = Constants.MAINTENANCE_CREDIT_NOTE;
                groupedCreditInvoiceVO.Reference = Constants.MAINTENANCE;
                groupedCreditInvoiceVO.TotalAmount = crTotalAmount;

                // Validate header object
                ValidateInvoiceHeader(groupedCreditInvoiceVO);

                // Add header object to list
                InvoiceHeaderVOList.Add(groupedCreditInvoiceVO);

            }

            // Validate and add invoice into invoice list
            if (groupedInvoiceVO.InvoiceDetailVos.Count > 0)
            {
                groupedInvoiceVO.Reference = Constants.MAINTENANCE;
                groupedInvoiceVO.Description = "Maintenance Invoice";
                groupedInvoiceVO.TotalAmount = totalAmount;

                ValidateInvoiceHeader(groupedInvoiceVO);
                InvoiceHeaderVOList.Add(groupedInvoiceVO);
            }



            return InvoiceHeaderVOList;
        }

        /// <summary>
        /// Validate Job code is associated with customer or not
        /// </summary>
        /// <param name="invoiceHeaderVO">Invoice header value object</param>
        /// <param name="glDetail">General ledger details</param>
        /// <returns>if job code associated with customer then true else false</returns>
        private bool ValidateJobCode(InvoiceHeaderVO invoiceHeaderVO, InvoiceGLDetailVO glDetail)
        {
            JobCodeService jobCodeService = new JobCodeService();

            List<JobCodeVO> jobCodeList = jobCodeService.GetJobCodeList(invoiceHeaderVO.CompanyId,
                                                                        invoiceHeaderVO.InvoiceCustomerId);

            bool isJobCodeExist = jobCodeList.Any(x => x.Id == Convert.ToInt32(glDetail.JobCodeId));

            return isJobCodeExist;
        }

        /// <summary>
        /// Check customer is grouped or not
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="customerId">The customer id</param>
        /// <returns>Return true if customer is grouped, otherwise false</returns>
        private bool CheckCustomerGroupBy(int companyId, int customerId)
        {
            CustomerCommentDAL customerCommentDal = new CustomerCommentDAL();

            CustomerCommentVO customerCommentVO = customerCommentDal.GetCustomerCommentByCompanyIdAndCustomerId(companyId, customerId);

            bool isGrouped = false;

            //if customer found
            if (customerCommentVO != null)
            {
                isGrouped = customerCommentVO.Group;
            }

            return isGrouped;
        }

        /// <summary>
        /// Set Milestone status for milestone
        /// </summary>
        /// <param name="companyId">The company</param>
        /// <param name="divisionId">The division id</param>
        /// <param name="invoiceCustomerId">The customer id</param>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="userId">logged in user id</param>
        /// <param name="milestoneStatusId"></param>
        public void UpdateMilestoneStatus(int companyId, int divisionId, int invoiceCustomerId,
            DateTime startDate, DateTime endDate, int? userId, int milestoneStatusId, DateTime invoiceDate)
        {
            approveMaintenanceDAL.UpdateMilestoneStatus(companyId, divisionId, invoiceCustomerId, startDate,
                                                                 endDate, userId, milestoneStatusId, invoiceDate);
        }
    }
}