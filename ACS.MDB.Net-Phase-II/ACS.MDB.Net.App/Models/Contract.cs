using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;
using System.Configuration;

namespace ACS.MDB.Net.App.Models
{
    public class Contract : BaseModel
    {
        [Required(ErrorMessage = "Please enter Contract number")]
        [Display(Name = "Contract Number")]
        [RegularExpression("^([a-zA-Z0-9 &'-.#/]+)$", ErrorMessage = "Please enter valid Contract number")]
        [StringLength(25, ErrorMessage = "Contract number must be with a maximum length of 25")]
        public string ContractNumber { get; set; }

        [Required(ErrorMessage = "Please select End user")]        
        [RegularExpression("^(^(?!-1).+)$", ErrorMessage = "Please select End user")]
        public string EndUserId { get; set; }

        [Display(Name = "End User")]
        public string EndUser { get; set; }

        [Required(ErrorMessage = "Please select Invoice customer")]
        public int InvoiceCustomerId { get; set; }

        [Display(Name = "Invoice Customer")]
        public string InvoiceCustomer { get; set; }

        [Display(Name = "Invoice Customer")]
        public string CustomerNameCustomerCodeAndShortName { get; set; }

        [Display(Name = "Invoice Customer")]
        public string CustomerNameAndShortName { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Currency")]
        //[Required(ErrorMessage = " ")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Please select Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Please select Division")]
        [Display(Name = "Division")]
        public int DivisionId { get; set; }

        public bool? IsDeleted { get; set; }

        //[Required(ErrorMessage = "Please select division")]
        //[RegularExpression("^([a-zA-Z0-9 &'-]+)$", ErrorMessage = "Please enter valid division name.")]
        //[Display(Name = "Division")]
        //[StringLength(50, ErrorMessage = "Divison name must be with a maximum length of 50")]    
        [Display(Name = "Division")]
        public string DivisionName { get; set; }

        [Display(Name = "Customer Notes")]
        public string CustomerComment { get; set; }

        [Display(Name = "Cust PO Number")]
        //[RegularExpression("^([a-zA-Z0-9 &:_'-.#/]+)$", ErrorMessage = "Please enter valid Cust PO number")]
        [StringLength(30, ErrorMessage = "Cust PO number must be with a maximum length of 30")]
        public string POReferenceNumber { get; set; }

        [Display(Name = "At Risk")]
        public int? AtRisk { get; set; }

        [Display(Name = "Early Termination Date")]
        //[DataType(DataType.Date, ErrorMessage = "Date Should be in dd/mm/yyyy format"), DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime? EarlyTerminationDate { get; set; }

        public List<Currency> CurrencyList { get; set; }
        public List<EndUser> EndUserList { get; set;}
        public List<InvoiceCustomer> InvoiceCustomerList { get; set; }
        public List<Division> DivisionList { get; set;}
        public List<ContractLine> ContractLines { get; set; }
        public List<ContractMaintenance> ContractMaintenances { get; set; }
        public List<MilestoneBillingLine> MilestoneBillingLines { get; set; }
        public List<Milestone> Milestones { get; set; }

        public string Visible  {get; set;}

        public List<ContractMaintenanceVO> ContractMaintenanceVOList { get; set; }
        public List<MaintenanceBillingLineVO> MaintenanceBillingLineVOList { get; set; }
        public string BillingLines { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Contract()
        {
            ContractNumber = string.Empty;
            CurrencyList = new List<Currency>();
            EndUserList = new List<EndUser>();
            DivisionList = new List<Division>();
            InvoiceCustomerList = new List<InvoiceCustomer>();
            this.ContractMaintenances = new List<ContractMaintenance>();
            ContractMaintenanceVOList = new List<ContractMaintenanceVO>();
            MaintenanceBillingLineVOList = new List<MaintenanceBillingLineVO>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contractVO"></param>
        public Contract(ContractVO contractVO)
        {
            ID = contractVO.ID;
            ContractNumber = contractVO.ContractNumber;
            CurrencyId = contractVO.CurrencyId;
            DivisionId = contractVO.DivisionId;
            EndUserId = contractVO.EndUserId;
            InvoiceCustomerId = contractVO.InvoiceCustomerId;
            CompanyId = contractVO.CompanyId;
            CompanyName = contractVO.CompanyName;
            EndUser = contractVO.EndUserName;
            InvoiceCustomer = contractVO.InvoiceCustomerName;
            CustomerNameAndShortName = contractVO.InvoiceCustomerName + " - " + contractVO.ShortName;
            CustomerNameCustomerCodeAndShortName = contractVO.InvoiceCustomerName + " - " + contractVO.CustomerCode +
                                                   " - " + contractVO.ShortName;
            AtRisk = contractVO.AtRisk;
            EarlyTerminationDate = contractVO.EarlyTerminationDate;
            DivisionName = contractVO.DivisionName;
            Currency = contractVO.Currency;
            IsDeleted = contractVO.IsDeleted;
            POReferenceNumber = contractVO.POReferenceNumber;
            ContractMaintenanceVOList = contractVO.ContractMaintenanceVOList;
            MaintenanceBillingLineVOList = contractVO.MaintenanceBillingLineVOList;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contractVO"></param>
        public Contract(ContractVO contractVO, string visibleOrHidden = "visible") : this()
        {
            ID = contractVO.ID;
            ContractNumber = contractVO.ContractNumber;
            CurrencyId = contractVO.CurrencyId;
            DivisionId = contractVO.DivisionId;
            EndUserId = contractVO.EndUserId;
            InvoiceCustomerId = contractVO.InvoiceCustomerId;
            CompanyId = contractVO.CompanyId;
            CompanyName = contractVO.CompanyName;
            EndUser = contractVO.EndUserName;
            InvoiceCustomer = contractVO.InvoiceCustomerName;
            CustomerNameAndShortName = contractVO.InvoiceCustomerName + " - " + contractVO.ShortName;
            CustomerNameCustomerCodeAndShortName = contractVO.InvoiceCustomerName + " - " + contractVO.CustomerCode +
                                                  " - " + contractVO.ShortName;
            DivisionName = contractVO.DivisionName;
            Currency = contractVO.Currency;
            AtRisk = contractVO.AtRisk;
            EarlyTerminationDate = contractVO.EarlyTerminationDate;
            IsDeleted = contractVO.IsDeleted;
            POReferenceNumber = contractVO.POReferenceNumber;
            Visible = visibleOrHidden;
        }

        /// <summary>
        /// Transpose model object to value object
        /// </summary>
        /// <param name="contract">The contract model</param>
        /// <param name="userId">The user id</param>
        public ContractVO Transpose(int? userId)
        {
            ContractVO contractVO = new ContractVO();

            contractVO.ID = this.ID;
            contractVO.ContractNumber = this.ContractNumber;
            contractVO.CompanyId = this.CompanyId;
            contractVO.EndUserId = this.EndUserId;
            contractVO.InvoiceCustomerId = this.InvoiceCustomerId;
            contractVO.DivisionId = this.DivisionId;
            contractVO.CurrencyId = this.CurrencyId;
            contractVO.Currency = this.Currency;
            contractVO.CompanyName = this.CompanyName;
            contractVO.DivisionName = this.DivisionName;
            contractVO.InvoiceCustomerName = this.InvoiceCustomer;
            contractVO.AtRisk = this.AtRisk;
            contractVO.EarlyTerminationDate = this.EarlyTerminationDate;
            contractVO.IsDeleted = this.IsDeleted;
            contractVO.POReferenceNumber = this.POReferenceNumber;
            contractVO.CreatedByUserId = userId;
            contractVO.LastUpdatedByUserId = userId;

            return contractVO;
        }

        /// <summary>
        /// Function called to search the model
        /// for availability of the specified string.
        /// </summary>
        /// <param name="str">The search string</param>
        /// <returns>True, if the string is contained in the model, else false</returns>
        public override bool Contains(string str)
        {
            return (ContractNumber != null && ContractNumber.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (CustomerNameAndShortName != null && CustomerNameAndShortName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (DivisionName != null && DivisionName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (POReferenceNumber != null && POReferenceNumber.StartsWith(str, StringComparison.CurrentCultureIgnoreCase)) ||
                (EndUser != null && EndUser.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { 
                ID, 
                ContractNumber, 
                CustomerNameAndShortName, 
                EndUser ,
                POReferenceNumber,
                DivisionName, 
                Currency,
                null,
                BillingLines
            };
            return result;
        }
    }
}