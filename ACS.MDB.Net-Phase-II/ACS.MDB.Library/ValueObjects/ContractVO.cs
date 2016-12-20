using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class ContractVO : BaseVO
    {
        /// <summary>
        /// gets or sets ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// gets or sets ContractNumber
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// gets or sets EndUserId
        /// </summary>
        public string EndUserId { get; set; }

        /// <summary>
        /// gets or sets InvoiceCustomerId
        /// </summary>
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// gets or sets CurrencyId
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// gets or sets CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// gets or sets DivisionId
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// gets or sets EndUserName
        /// </summary>
        public string EndUserName { get; set; }

        /// <summary>
        /// gets or sets InvoicecustomerName
        /// </summary>
        public string InvoiceCustomerName { get; set; }

        /// <summary>
        /// Gets or set customer short name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Customer name with the combination of Customer Name + Account Number + Shortname 
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or set cobination of customer name and short name
        /// </summary>
        public string CustomerNameAndShortName { get; set; }

        /// <summary>
        /// Customer code
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// gets or sets CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// gets or sets DivisionName
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// gets or sets Currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// gets or sets PO ReferenceNumber
        /// </summary>
        public string POReferenceNumber { get; set; }

        /// <summary>
        /// gets or sets IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// gets or sets DatabaseID
        /// </summary>
        public Nullable<int> DatabaseID { get; set; }

        /// <summary>
        /// gets or sets List of ContractLinesVO
        /// </summary>
        public List<ContractLineVO> ContractLinesVO { get; set; }

        /// <summary>
        /// gets or sets ContractMaintenanceVOList
        /// </summary>
        public List<ContractMaintenanceVO> ContractMaintenanceVOList { get; set; }

        /// <summary>
        /// gets or sets MaintenanceBillingLineVOList
        /// </summary>
        public List<MaintenanceBillingLineVO> MaintenanceBillingLineVOList { get; set; }

        /// <summary>
        /// gets or sets BillingLines
        /// </summary>
        public string BillingLines { get; set; }

        /// <summary>
        /// gets or sets AtRisk
        /// </summary>
        public int? AtRisk {get; set;}

        /// <summary>
        /// gets or sets EarlyTerminationDate
        /// </summary>
        public DateTime? EarlyTerminationDate { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractVO()
        {
            ContractLinesVO = new List<ContractLineVO>();
            ContractMaintenanceVOList = new List<ContractMaintenanceVO>();
            MaintenanceBillingLineVOList = new List<MaintenanceBillingLineVO>();
        }

        /// <summary>
        /// Transpose model object to value object
        /// </summary>
        /// <param name="contract">The contract model</param>
        /// <param name="userId">The user id</param>
        //public ContractVO(MODEL.Contract contract, int? userId)
        //{
        //    ID = contract.ID;
        //    ContractNumber = contract.ContractNumber;
        //    CompanyId = contract.CompanyId;
        //    EndUserId = contract.EndUserId;
        //    InvoiceCustomerId = contract.InvoiceCustomerId;
        //    DivisionId = contract.DivisionId;
        //    CurrencyId = contract.CurrencyId;
        //    Currency = contract.Currency;
        //    CompanyName = contract.CompanyName;
        //    DivisionName = contract.DivisionName;
        //    InvoiceCustomerName = contract.InvoiceCustomer;
            
        //    IsDeleted = contract.IsDeleted;
        //    POReferenceNumber = contract.POReferenceNumber;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;           
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contract">The contract LINQ object</param>
        public ContractVO(Contract contract)
        {
            ID = contract.ID;
            ContractNumber = contract.ContractNumber;
            CurrencyId = contract.Currency.ID;
            Currency = contract.Currency.CurrencyName;
            EndUserId = contract.EndUserID;
            EndUserName = contract.OACustomer.CustomerName;
            InvoiceCustomerId = contract.InvoiceCustomerID;
            InvoiceCustomerName = contract.OACustomer.CustomerName;
            ShortName = contract.OACustomer.ShortName;
            CompanyId = contract.CompanyID;
            CompanyName = contract.OACompany.CompanyName;
            DivisionId = contract.DivisionID;
            DivisionName = contract.Division.DivisionName;
            AtRisk = contract.AtRisk;
            EarlyTerminationDate = contract.EarlyTerminationDate;
            IsDeleted = contract.IsDeleted;
            POReferenceNumber = contract.POReferenceNo;
            CreatedByUserId = contract.CreatedBy;
            LastUpdatedByUserId = contract.LastUpdatedBy;
            ContractMaintenanceVOList = new List<ContractMaintenanceVO>();
            MaintenanceBillingLineVOList = new List<MaintenanceBillingLineVO>();
        }

      
        ///// <summary>
        ///// Get contract lines value objects from contract lines linq objects.
        ///// </summary>
        ///// <param name="contractLines"></param>
        ///// <returns></returns>
        //private List<ContractLineVO> GetContractLines(List<ContractLine> contractLines)
        //{
        //    List<ContractLineVO> contractLinesVO = new List<ContractLineVO>();
        //    foreach (var item in contractLines)
        //    {

        //    }
        //    return contractLinesVO;
        //}

        ///// <summary>
        ///// Transpose contract maintenance to contract Maintenance value objects.
        ///// </summary>
        ///// <param name="ContractMaintenance"></param>
        ///// <returns></returns>
        //public List<ContractMaintenanceVO> Transpose(List<ContractMaintenance> contractMaintenance)
        //{
        //    List<ContractMaintenanceVO> contractMaintenanceVO = new List<ContractMaintenanceVO>();
        //    foreach (var item in contractMaintenance)
        //    {

        //    }
        //    return contractMaintenanceVO;
        //}
    }
}