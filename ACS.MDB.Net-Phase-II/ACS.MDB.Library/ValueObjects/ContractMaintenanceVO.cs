using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;


namespace ACS.MDB.Library.ValueObjects
{
    public class ContractMaintenanceVO : BaseVO
    {
        /// <summary>
        /// gets or sets ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// gets or sets CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// gets or sets CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// gets or sets ContractId
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// gets or sets ContractNumber
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// gets or sets ContractLineId
        /// </summary>
        public int ContractLineId { get; set; }

        /// <summary>
        /// gets or sets ActivityCodeId
        /// </summary>
        public int ActivityCodeId { get; set; }

        /// <summary>
        /// gets or sets PeriodFrequency
        /// </summary>
        public string PeriodFrequency { get; set; }

        /// <summary>
        /// gets or sets PeriodFrequencyInitial
        /// </summary>
        public string PeriodFrequencyInitial { get; set; }

        /// <summary>
        /// gets or sets ProductId
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// gets or sets SubProductId
        /// </summary>
        public int? SubProductId { get; set; }

        /// <summary>
        /// gets or sets InflationIndexId
        /// </summary>
        public int? InflationIndexId { get; set; }

        /// <summary>
        /// gets or sets InvoiceAdvancedId
        /// </summary>
        public int InvoiceAdvancedId { get; set; }

        /// <summary>
        /// gets or sets ReasonId
        /// </summary>
        public int ReasonId { get; set; }

        //public bool IncludeInForecast { get; set; }
        /// <summary>
        /// gets or sets IncludeInForecast
        /// </summary>
        public int IncludeInForecast { get; set; }

        /// <summary>
        /// gets or sets InflationIndexName
        /// </summary>
        public string InflationIndexName { get; set; }

        /// <summary>
        /// gets or sets InvoiceCustomer
        /// </summary>
        public string InvoiceCustomer { get; set; }

        /// <summary>
        /// gets or sets CustomerCode
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// gets or sets PeriodFrequencyId
        /// </summary>
        public int PeriodFrequencyId { get; set; }

        /// <summary>
        /// gets or sets FirstPeriodStartDate
        /// </summary>
        public DateTime? FirstPeriodStartDate { get; set; }

        /// <summary>
        /// gets or sets FirstPeriodAmount
        /// </summary>
        public decimal FirstPeriodAmount { get; set; }

        /// <summary>
        /// gets or sets FirstRenewalDate
        /// </summary>
        public DateTime? FirstRenewalDate { get; set; }

        /// <summary>
        /// gets or sets UpliftRequired
        /// </summary>
        public Nullable<bool> UpliftRequired { get; set; }

        /// <summary>
        /// gets or sets FirstAnnualUpliftDate
        /// </summary>
        public Nullable<System.DateTime> FirstAnnualUpliftDate { get; set; }

        /// <summary>
        /// gets or sets BaseAnnualAmount
        /// </summary>
        public decimal BaseAnnualAmount { get; set; }

        /// <summary>
        /// gets or sets InflationFixedAdditional
        /// </summary>
        public Nullable<decimal> InflationFixedAdditional { get; set; }

        /// <summary>
        /// gets or sets TerminationDescription
        /// </summary>
        public string TerminationDescription { get; set; }

        /// <summary>
        /// gets or sets Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// gets or sets FinalRenewalStartDate
        /// </summary>
        public Nullable<System.DateTime> FinalRenewalStartDate { get; set; }

        /// <summary>
        /// gets or sets FinalRenewalEndDate
        /// </summary>
        public Nullable<System.DateTime> FinalRenewalEndDate { get; set; }

        /// <summary>
        /// gets or sets EndAmount
        /// </summary>
        public Nullable<decimal> EndAmount { get; set; }

        /// <summary>
        /// gets or sets CreationDate
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// gets or sets ReasonDate
        /// </summary>
        public Nullable<System.DateTime> ReasonDate { get; set; }

        /// <summary>
        /// gets or sets DeleteReason
        /// </summary>
        public Nullable<int> DeleteReason { get; set; }

        /// <summary>
        /// gets or sets DeleteDate
        /// </summary>
        public Nullable<System.DateTime> DeleteDate { get; set; }

        /// <summary>
        /// gets or sets ActivityCode
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// gets or sets OAActivityId
        /// </summary>
        public string OAActivityId { get; set; }

        /// <summary>
        /// gets or sets InvoiceAdvancedArrears
        /// </summary>
        public int InvoiceAdvancedArrears { get; set; }

        /// <summary>
        /// gets or sets OAJobCodeId
        /// </summary>
        public string OAJobCodeId { get; set; }

        /// <summary>
        /// gets or sets CostCenter
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// gets or sets Account
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// gets or sets Quantity
        /// </summary>
        public int QTY { get; set; }

        /// <summary>
        /// gets or sets BillingLines
        /// </summary>
        public string BillingLines { get; set; }

        /// <summary>
        /// gets or sets MilestoneCount
        /// </summary>
        public int MileStoneCount { get; set; }

        /// <summary>
        /// gets or sets MilestoneStatusCount
        /// </summary>
        public int MilestoneStatusCount { get; set; }

        /// <summary>
        /// gets or sets MilestoneStatusCountForLinkLoaded
        /// </summary>
        public bool MilestoneStatusCountForLinkLoaded { get; set; }

        /// <summary>
        /// gets or sets InvoiceCustomerId
        /// </summary>
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// gets or sets GroupId
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// gets or sets GroupName
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// gets or sets IsGrouped
        /// </summary>
        public bool? IsGrouped { get; set; }

        /// <summary>
        /// gets or sets IsDefaultLineInGroup
        /// </summary>
        public bool? IsDefaultLineInGroup { get; set; }

        /// <summary>
        /// gets or sets DocumentTypeId
        /// </summary>
        public int? DocumentTypeId { get; set; }

        /// <summary>
        /// gets or sets ForecastBillingStartDate
        /// </summary>
        public DateTime? ForecastBillingStartDate { get; set; }

        //public string PeriodFrequency {get; set;}
        //public AuditReason AuditReason { get; set; }
        //public AuditReason AuditReason1 { get; set; }
        //public ContractLine ContractLine { get; set; }
        //public string BillingLines { get; set; }

        //public List<MODEL.MaintenanceBillingLine> MaintenanceBillingLines { get; set; }


        public List<MaintenanceBillingLineVO> MaintenanceBillingLineVos { get; set; }

        public List<MaintenanceBillingLine> billingLinesToSave { get; set; }

        //public Contract Contract { get; set; }
        //public Product Product1 { get; set; }
        //public ProductVersion ProductVersion1 { get; set; }
        public List<MilestoneVO> Milestones { get; set; }

        /// <summary>
        /// gets and sets Maintenance Billing Line Text
        /// </summary>
        public List<String> MaintenanceBillingLineText { get; set; }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public ContractMaintenanceVO()
        {
            Milestones = new List<MilestoneVO>();
            //MaintenanceBillingLines = new List<MODEL.MaintenanceBillingLine>();
            MaintenanceBillingLineVos = new List<MaintenanceBillingLineVO>();
        }

        /// <summary>
        /// Transpose LINQ contract maintenance object to cotract maintenance value object
        /// </summary>
        /// <param name="contractMaintenance"></param>
        //public ContractMaintenanceVO(MODEL.ContractMaintenance contractMaintenance, int? userId)
        //    : this()
        //{
        //    ID = contractMaintenance.ID;
        //    ContractId = contractMaintenance.ContractId;
        //    ContractLineId = contractMaintenance.ContractLineId;
        //    InvoiceCustomerId = contractMaintenance.InvoiceCustomerId;
        //    PeriodFrequencyId = contractMaintenance.PeriodFrequencyId;
        //    PeriodFrequency = contractMaintenance.PeriodFrequency;
        //    ActivityCodeId = contractMaintenance.ActivityCodeId;

        //    ProductId = contractMaintenance.ProductId;
        //    SubProductId = contractMaintenance.SubProductId;
        //    if (SubProductId == -1)
        //    {
        //        SubProductId = null;
        //    }

        //    InflationIndexId = contractMaintenance.InflationIndexId;
        //    InflationIndexName = contractMaintenance.InflationIndexName;
        //    InvoiceAdvancedId = contractMaintenance.InvoiceInAdvance;
        //    ReasonId = contractMaintenance.ReasonCode;
        //    BaseAnnualAmount = contractMaintenance.BaseAnnualAmount;
        //    FirstPeriodAmount = contractMaintenance.FirstPeriodAmount;
        //    FirstPeriodStartDate = contractMaintenance.FirstPeriodStartDate;
        //    FirstRenewalDate = contractMaintenance.FirstRenewalDate;
        //    FirstAnnualUpliftDate = contractMaintenance.FirstAnnualUpliftDate;
        //    FinalRenewalStartDate = contractMaintenance.FinalRenewalStartDate;
        //    FinalRenewalEndDate = contractMaintenance.FinalRenewalEndDate;
        //    EndAmount = contractMaintenance.EndAmount;
        //    ActivityCode = contractMaintenance.ActivityCode;

        //    CreationDate = contractMaintenance.CreationDate;
        //    ReasonDate = contractMaintenance.ReasonDate;
        //    InvoiceAdvancedArrears = contractMaintenance.InvoiceAdvancedArrears.Value;
        //    IncludeInForecast = contractMaintenance.IncludeInForecast;
        //    Comment = contractMaintenance.Comment;
        //    UpliftRequired = contractMaintenance.UpliftRequired;
        //    InflationFixedAdditional = contractMaintenance.InflationFixedAdditional.HasValue ? contractMaintenance.InflationFixedAdditional / 100 : contractMaintenance.InflationFixedAdditional;
        //   // TerminationReason = contractMaintenance.TerminationReason;
        //    DeleteReason = contractMaintenance.DeleteReason;
        //    TerminationDescription = contractMaintenance.TerminationDescription;
        //    DeleteDate = contractMaintenance.DeleteDate;
        //    MileStoneCount = contractMaintenance.MileStoneCount;
        //    MilestoneStatusCount = contractMaintenance.MilestoneStatusCount;

        //    OAActivityId = contractMaintenance.OAActivityId;
        //    OAJobCodeId = contractMaintenance.OAJobCodeId;
        //    CostCenter = contractMaintenance.CostCenter;
        //    Account = contractMaintenance.Account;
        //    QTY = contractMaintenance.QTY;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;

        //    FillBillingLines(contractMaintenance);
        //}

        /// <summary>
        /// Transpose LINQ contract maintenance object to cotract maintenance value object
        /// </summary>
        /// <param name="contractMaintenance"></param>
        public ContractMaintenanceVO(ContractMaintenance contractMaintenance, 
            List<AuditReason> auditReasonList = null)
            : this()
        {
            ID = contractMaintenance.ID;
            ContractId = contractMaintenance.ContractID;
            ContractLineId = contractMaintenance.ContractLineID;
            CompanyId = contractMaintenance.Contract.CompanyID;
            CompanyName = contractMaintenance.Contract.OACompany.CompanyName;
            ContractNumber = contractMaintenance.Contract.ContractNumber;
            InvoiceCustomerId = contractMaintenance.Contract.InvoiceCustomerID;
            InvoiceCustomer = contractMaintenance.Contract.OACustomer.CustomerName;
            CustomerCode = contractMaintenance.Contract.OACustomer.CustomerID;

            PeriodFrequencyId = contractMaintenance.ChargeFrequencyID;
            PeriodFrequency = contractMaintenance.ChargeFrequency.FrequencyName;
            PeriodFrequencyInitial = contractMaintenance.ChargeFrequency.Frequency;
            ActivityCodeId = contractMaintenance.ContractLine.ActivityCodeID;
            OAJobCodeId = contractMaintenance.ContractLine.OAJobCode.JobCodeID;
            CostCenter = contractMaintenance.ContractLine.OACostCentre.CostCentreID + '-' + contractMaintenance.ContractLine.OACostCentre.CostCentreName;
            Account = contractMaintenance.ContractLine.OAAccountCode.AccountName;
            QTY = contractMaintenance.QTY;

            ProductId = contractMaintenance.ProductID;
            SubProductId = contractMaintenance.SubProductID;
            InflationIndexId = contractMaintenance.InflationIndexID.HasValue ? contractMaintenance.InflationIndexID.Value : 0;

            //Check if index name is available
            if (contractMaintenance.ChargingIndex != null && contractMaintenance.ChargingIndex.ChargingIndex1 != null)
            {
                InflationIndexName = contractMaintenance.ChargingIndex.ChargingIndex1;
            }
            InvoiceAdvancedId = contractMaintenance.InvoiceInAdvance;
            
            IncludeInForecast = contractMaintenance.IncludeInForecast;
            BaseAnnualAmount = contractMaintenance.BaseAnnualAmount;
            FirstPeriodAmount = contractMaintenance.FirstPeriodAmount;
            FirstPeriodStartDate = contractMaintenance.FirstPeriodStartDate;
            FirstRenewalDate = contractMaintenance.FirstRenewalDate;
            FirstAnnualUpliftDate = contractMaintenance.FirstAnnualUpliftDate;
            FinalRenewalStartDate = contractMaintenance.FinalRenewalStartDate;
            FinalRenewalEndDate = contractMaintenance.FinalRenewalEndDate;
            EndAmount = contractMaintenance.EndAmount;

            ActivityCode = contractMaintenance.ContractLine.OAActivityCode.ActivityName;
            OAActivityId = contractMaintenance.ContractLine.OAActivityCode.ActivityID;

            CreationDate = contractMaintenance.CreationDate;
            DeleteDate = contractMaintenance.DeleteDate;
            ReasonDate = contractMaintenance.ReasonDate;
            InvoiceAdvancedArrears = contractMaintenance.InvoiceAdvancedArrears;
            Comment = contractMaintenance.Comment;
            UpliftRequired = contractMaintenance.UpliftRequired;
            InflationFixedAdditional = contractMaintenance.InflationFixedAdditional.HasValue ? contractMaintenance.InflationFixedAdditional * 100 : contractMaintenance.InflationFixedAdditional;
            DeleteReason = contractMaintenance.DeleteReason;            

            //For grouping of billing lines
            GroupId = contractMaintenance.GroupId;
            GroupName = contractMaintenance.GroupName;
            IsGrouped = contractMaintenance.IsGrouped;
            IsDefaultLineInGroup = contractMaintenance.IsDefaultLineInGroup;
            DocumentTypeId = contractMaintenance.DocumentTypeID;

            CreatedByUserId = contractMaintenance.CreatedBy;
            CreatedDate = contractMaintenance.CreatedDate;
            LastUpdatedDate = contractMaintenance.LastUpdatedDate.HasValue ? contractMaintenance.LastUpdatedDate : contractMaintenance.CreatedDate;
            LastUpdatedByUserId = contractMaintenance.LastUpdatedBy.HasValue ? contractMaintenance.LastUpdatedBy : contractMaintenance.CreatedBy;

            //Set termination reason
            if (auditReasonList != null)
            {
                TerminationDescription = contractMaintenance.DeleteReason.HasValue &&
                                         contractMaintenance.DeleteReason > 0
                                             ? auditReasonList.Find(x => x.ReasonCode == contractMaintenance.DeleteReason)
                                                              .ReasonDescription
                                             : string.Empty;
            }

            //TerminationDescription = DeleteReason.HasValue ? contractMaintenance.A.ReasonDescription : string.Empty;

            //TerminationDescription = contractMaintenance.AuditReason.ReasonDescription;
            ReasonId = contractMaintenance.ReasonCode;
            ForecastBillingStartDate = contractMaintenance.ForecastBillingStartDate;

            foreach (Milestone item in contractMaintenance.Milestones)
            {
                if (item.IsDeleted == false)
                {
                    MilestoneVO milestoneVO = new MilestoneVO();
                    Milestones.Add(milestoneVO.Transpose(item));
                }
            }
            //MileStoneCount = contractMaintenance.Milestones.Count;

            //get milestone count
            //This will be used to identify if count > 1 then disable some controls on billing line details
            foreach (var item in contractMaintenance.Milestones)
            {
                if (item.IsDeleted == false)
                {
                    MileStoneCount++;
                    //break;
                }

                if (item.IsDeleted == false && item.MilestoneStatusID != 9)
                {
                    MilestoneStatusCount++;
                    break;
                }
            }

            //To Check any of milestone status is other than Link loaded
            MilestoneStatusCountForLinkLoaded =
                    contractMaintenance.Milestones.Any(
                    m => m.MilestoneStatusID != Convert.ToInt32(Constants.MilestoneStatus.LINK_LOADED) && m.IsDeleted == false);

            string billingLines = string.Empty;

            foreach (var item in contractMaintenance.MaintenanceBillingLines.OrderBy(m => m.LineSequance))
            {
                if (item.IsDeleted == false)
                {
                    if (!String.IsNullOrEmpty(item.LineText))
                    {
                        if (item.LineText.Contains("<") || item.LineText.Contains(">"))
                        {
                            billingLines += item.LineText.Replace("<", "&lt;").Replace(">", "&gt;") + "<br>";
                            //BillingLines += Environment.NewLine;
                        }
                        else
                        {
                            billingLines += item.LineText + "<br>";
                        }
                    }

                    MaintenanceBillingLineVos.Add(new MaintenanceBillingLineVO(item));
                    //MaintenanceBillingLines.Add(new MODEL.MaintenanceBillingLine(item));
                }
            }

            FillBillingLinesOtherDetailsToDisplay(contractMaintenance, billingLines);
        }

        
        /// <summary>
        /// Gets value of all Line Text's into Maitenance Billing Line Text string
        /// </summary>
        /// <param name="contractMaintenances">List of contract maintenances</param>
        public ContractMaintenanceVO(List<ContractMaintenance> contractMaintenances)
            : this()
        {
            MaintenanceBillingLineText  = new List<String>();
            foreach (var item in contractMaintenances)
            {
                foreach (var billingLineText in item.MaintenanceBillingLines) 
                {
                    if (!string.IsNullOrEmpty(billingLineText.LineText))
                    {
                        if (!MaintenanceBillingLineText.Contains(billingLineText.LineText))
                        {
                            MaintenanceBillingLineText.Add(billingLineText.LineText);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fill billing lines to store in database
        /// </summary>
        /// <param name="contractMaintenance">The contract maintenance object</param>
        //private void FillBillingLines(MODEL.ContractMaintenance contractMaintenance)
        //{
        //    if (billingLinesToSave == null)
        //    {
        //        billingLinesToSave = new List<MaintenanceBillingLine>();
        //    }

        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText1))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID1, LineSequance = 1, LineText = contractMaintenance.billingText1 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText2))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID2, LineSequance = 2, LineText = contractMaintenance.billingText2 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText3))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID3, LineSequance = 3, LineText = contractMaintenance.billingText3 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText4))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID4, LineSequance = 4, LineText = contractMaintenance.billingText4 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText5))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID5, LineSequance = 5, LineText = contractMaintenance.billingText5 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText6))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID6, LineSequance = 6, LineText = contractMaintenance.billingText6 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText7))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID7, LineSequance = 7, LineText = contractMaintenance.billingText7 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText8))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID8, LineSequance = 8, LineText = contractMaintenance.billingText8 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText9))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID9, LineSequance = 9, LineText = contractMaintenance.billingText9 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText10))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID10, LineSequance = 10, LineText = contractMaintenance.billingText10 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText11))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID11, LineSequance = 11, LineText = contractMaintenance.billingText11 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText12))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID12, LineSequance = 12, LineText = contractMaintenance.billingText12 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText13))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID13, LineSequance = 13, LineText = contractMaintenance.billingText13 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText14))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID14, LineSequance = 14, LineText = contractMaintenance.billingText14 });
        //    //}
        //    //if (!String.IsNullOrEmpty(contractMaintenance.billingText15))
        //    //{
        //    billingLinesToSave.Add(new MaintenanceBillingLine() { ID = contractMaintenance.billingTextID15, LineSequance = 15, LineText = contractMaintenance.billingText15 });
        //    //}
        //}

        /// <summary>
        /// Fill other details to display along with billing lines on row + image click on Billing Details screen.
        /// </summary>
        /// <param name="contractMaintenance">The contractMaintenance linq object</param>
        /// <param name="billingLines">The billing line</param>
        private void FillBillingLinesOtherDetailsToDisplay(ContractMaintenance contractMaintenance, string billingLines)
        {
            string reasonDate = ReasonDate.HasValue ? ReasonDate.Value.ToString(Constants.STRING_FORMAT_FOR_DATE) : Constants.NOT_APPLICABLE;
            string firstAnnualUpliftDate = FirstAnnualUpliftDate.HasValue ? FirstAnnualUpliftDate.Value.ToString(Constants.STRING_FORMAT_FOR_DATE) : Constants.NOT_APPLICABLE;
            string deletedDate = contractMaintenance.DeleteDate.HasValue ? contractMaintenance.DeleteDate.Value.ToString(Constants.STRING_FORMAT_FOR_DATE) : Constants.NOT_APPLICABLE;
            string creationDate = contractMaintenance.CreationDate.ToString(Constants.STRING_FORMAT_FOR_DATE);
            string upliftRequired = UpliftRequired.HasValue ? UpliftRequired.Value.ToString() : Constants.NOT_APPLICABLE;
            string forecastBillingStartDate = contractMaintenance.ForecastBillingStartDate.HasValue ? ForecastBillingStartDate.Value.ToString(Constants.STRING_FORMAT_FOR_DATE) : Constants.NOT_APPLICABLE;
            string includeInForecast = IncludeInForecast.ToString();
            upliftRequired = upliftRequired == "True" ? "Yes" : "No";            

            if (IncludeInForecast == 0)
            {
                includeInForecast = "No";
            }
            else if (IncludeInForecast == 1)
            {
                includeInForecast = "Yes";
            }
            else
            {
                includeInForecast = "Cancel";
            }
            string inflationFixedAdditional = InflationFixedAdditional.HasValue ? InflationFixedAdditional.Value.ToString() : Constants.NOT_APPLICABLE;
            string productName = contractMaintenance.Product.ProductName;
            string subProductVersion = contractMaintenance.SubProduct == null ? Constants.NOT_APPLICABLE : contractMaintenance.SubProduct.Version;
            string documentType = string.Empty;
            if (DocumentTypeId == 1)
            {
                documentType = "Standard";
            }
            else
            {
                documentType = "Deposit";
            }
            

            BillingLines += @"<table style = 'width:100%'>
                               <th>&nbsp;&nbsp;&nbsp;Billing Lines </th><th>&nbsp;&nbsp;Other Details </th>
                               <tr>
                                   <td style='width:360px;vertical-align:top; padding-top:10px;'>" + billingLines + "</td>"
                                   + "<td style='vertical-align:top; padding:0;'>"
                                        + "<table>"
                                            + "<tr>"
                                                + "<td style='width:80px;vertical-align:top;'>Product :</td><td  style='width:100px;vertical-align:top;'>" + productName + "</td>"
                                                + "<td  style='width:100px;vertical-align:top;'>Uplift Required :</td><td style='width:80px;vertical-align:top;'>" + upliftRequired + "</td>"
                                                + "<td  style='width:80px;vertical-align:top;'>Live Date :</td><td  style='width:100px;vertical-align:top;'>" + reasonDate + "</td>"
                                                + "<td style='width:120px;vertical-align:top;'>Document Type :</td><td style='width:100px;vertical-align:top;'>" + documentType + "</td>"
                                                +"</tr>"                                                
                                            + "<tr>"
                                                + "<td style='width:80px;vertical-align:top;'>Sub Product :</td><td style='width:100px;vertical-align:top;'>" + subProductVersion + "</td>"
                                                + "<td style='width:100px;vertical-align:top;'>Date :</td><td style='width:80px;vertical-align:top;'>" + firstAnnualUpliftDate + "</td>"
                                                + "<td style='width:80px;vertical-align:top;'>Ended Date :</td><td style='width:100px;vertical-align:top;'>" + deletedDate + "</td>"
                                                 + "<td style='width:100px;vertical-align:top;'>Forecast Billing Start Date :</td><td style='width:80px;vertical-align:top;'>" + forecastBillingStartDate + "</td>"
                                            + "</tr>"
                                            + "<tr>"
                                                + "<td style='width:100px;vertical-align:top;'>Backlog :</td><td style='width:100px;vertical-align:top;'>" + includeInForecast + "</td>"
                                                + "<td style='width:100px;vertical-align:top;'>Inflation :</td><td style='width:80px;vertical-align:top;'>" + InflationIndexName + "</td>"
                                                + "<td style='width:80px;vertical-align:top;'>Comments :</td><td style='width:250px;vertical-align:top;'>" + Comment + "</td>"
                                            + "</tr>"
                                            + "<tr>"
                                                + "<td style='width:90px;vertical-align:top;'>Backlog Date :</td><td style='width:100px;vertical-align:top;'>" + creationDate + "</td>"
                                                + "<td style='width:100px;vertical-align:top;'>Add.% :</td><td style='width:80px;vertical-align:top;'>" + inflationFixedAdditional + "</td>"
                                                + "<td style='width:150px;vertical-align:top;'>Termination Reason :</td><td style='width:250px;vertical-align:top;'>" + TerminationDescription + "</td>"
                                                
                                            + "</tr>"                                            
                                         + "</table>"
                                   + "</td>"
                              + "</tr>"
                           + "</table>";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contractMaintenance"></param>
        public ContractMaintenanceVO(ContractMaintenance contractMaintenance)
        {
            MaintenanceBillingLineVos = new List<MaintenanceBillingLineVO>();
            ID = contractMaintenance.ID;
            ContractId = contractMaintenance.ContractID;
            ContractLineId = contractMaintenance.ContractLineID;
            ActivityCode = contractMaintenance.ContractLine.OAActivityCode.ActivityName;
            OAActivityId = contractMaintenance.ContractLine.OAActivityCode.ActivityID;
            OAJobCodeId = contractMaintenance.ContractLine.OAJobCode.JobCodeID;
            BaseAnnualAmount = contractMaintenance.BaseAnnualAmount;
            FirstPeriodAmount = contractMaintenance.FirstPeriodAmount;
            FirstPeriodStartDate = contractMaintenance.FirstPeriodStartDate;
            FinalRenewalEndDate = contractMaintenance.FinalRenewalEndDate;
            PeriodFrequencyId = contractMaintenance.ChargeFrequencyID;
            
            string billingLines = string.Empty;
            foreach (var item in contractMaintenance.MaintenanceBillingLines.OrderBy(m => m.LineSequance))
            {
                if (item.IsDeleted == false)
                {
                    if (!String.IsNullOrEmpty(item.LineText))
                    {
                        if (item.LineText.Contains("<") || item.LineText.Contains(">"))
                        {
                            billingLines += item.LineText.Replace("<", "&lt;").Replace(">", "&gt;") + "<br>";
                        }
                        else
                        {
                            billingLines += item.LineText + "<br>";
                        }
                    }

                    MaintenanceBillingLineVos.Add(new MaintenanceBillingLineVO(item));
                }
            }
        }
    }
}