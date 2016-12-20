using System;
using System.Collections.Generic;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;


namespace ACS.MDB.Library.ValueObjects
{
    public class InvoiceHeaderVO
    {

        /// <summary>
        /// Gets or set Contract id
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or set company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set Contract line id
        /// </summary>
        public int ContractLineId { get; set; }

        /// <summary>
        /// Gets or set Contract maintenance id
        /// </summary>
        public int MaintenanceId { get; set; }

        /// <summary>
        /// Gets or set contract number
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or set record types
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Gets or set Record type for company = 102 for footer
        /// </summary>
        public string RecordTypeForT { get; set; }

        /// <summary>
        /// Gets or set company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or set division name
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or set DocumentTypeId
        /// </summary>
        public int? DocumentTypeID { get; set; }

        /// <summary>
        /// Gets or set document type for credit notes - amount in negative
        /// </summary>
        public string DocumentTypeCR { get; set; }

        /// <summary>
        /// Gets or set document type for invoice notes
        /// </summary>
        public string DocumentTypeIN { get; set; }

        /// <summary>
        /// Gets or set reference - PO-Ref field in contract table
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or set currency name
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or set description - Credit invoice/Maintenance Invoice
        /// </summary>
        public string  Description { get; set; }

        /// <summary>
        /// Gets or set total amount of customer
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or set customer code
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// Gets or set invoice customer id - new customer id 
        /// </summary>
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// Gets or set posting period based on company
        /// </summary>
        public int PostingPeriod { get; set; }

        /// <summary>
        /// Gets or set posting year based on company
        /// </summary>
        public int PostingYear { get; set; }

        /// <summary>
        /// Gets or set document date
        /// </summary>
        public string DocumentDate { get; set; }

        /// <summary>
        /// Gets or set Status - Always S
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or set contract division id
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// Gets or set Vat code
        /// </summary>
        public string VatCode { get; set; }

        /// <summary>
        /// Gets or set blank fields
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// gets or sets Customer Name
        /// </summary>
        public string CustomerName { get; set; }

        public string Field { get; set; }

        /// <summary>
        /// Gets or set invoice details
        /// </summary>
        public List<InvoiceDetailVO> InvoiceDetailVos { get; set; }

        /// <summary>
        /// Gets or set footer line
        /// </summary>
        public InvoiceBillingLineVO FooterBillingLine { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvoiceHeaderVO()
        {
            RecordType = Constants.HEADER;
            InvoiceDetailVos = new List<InvoiceDetailVO>();
            Field = String.Join("|", new string[10]);
        }

        /// <summary>
        /// Convert DAL Contract object to value object
        /// </summary>
        /// <param name="contract">DAL Contract object</param>
        /// <param name="invoiceDate">Invoice date</param>
        public InvoiceHeaderVO(Contract contract, DateTime invoiceDate, int? DocumentTypeID)
        {
            ContractId = contract.ID;
            ContractNumber = contract.ContractNumber;
            RecordType = Constants.HEADER;
            CompanyId = contract.CompanyID;
            CompanyName = contract.OACompany.CompanyName;
            Field = String.Join("|", new string[12]);
            Currency = contract.Currency.CurrencyName;
            CustomerCode = contract.OACustomer.CustomerID;
            DivisionId = contract.Division.DivisionID;
            Reference = !string.IsNullOrEmpty(contract.POReferenceNo) ? contract.POReferenceNo : Constants.ADVANCE_MAINT;
            VatCode = contract.OACustomer.VatCode;
            DivisionName = contract.Division.DivisionName;
            //Set Document type based on DocumentTypeID
            this.DocumentTypeID = DocumentTypeID;
            if (DocumentTypeID == 1)
            {
                DocumentTypeCR = contract.Division.DocumentTypeCR;
                DocumentTypeIN = contract.Division.DocumentTypeIN;
            }
            else if (DocumentTypeID == 2)
            {
                DocumentTypeCR = contract.Division.DepositDocumentTypeCR;
                DocumentTypeIN = contract.Division.DepositDocumentTypeIN;
            }            
            InvoiceCustomerId = contract.InvoiceCustomerID;
            CustomerName = contract.OACustomer.CustomerName;
            Status = "S";
            DocumentDate = Convert.ToString(invoiceDate.ToShortDateString());

            InvoiceDetailVos = new List<InvoiceDetailVO>();
            FooterBillingLine = new InvoiceBillingLineVO();

            //Footer line for all companies
            FooterBillingLine.RecordType = Constants.RECORD_TYPE_X; ;
            FooterBillingLine.DocumentType = "HSINV";
            FooterBillingLine.Fields = String.Join("|", new string[21]);
            FooterBillingLine.Fields = "Yes|No";

            //Added one Extra footer Line 'T' for ComapnyId = 102
            if (contract.CompanyID == 102)
            {
                FooterBillingLine.RecordTypeForT = Constants.RECORD_TYPE_T;
                FooterBillingLine.DocumentTypeForT = Constants.DOCUMENT_TYPE_MASE;
                FooterBillingLine.Field3 = "10000";
                FooterBillingLine.VatCode = "0.00";
            }
            //else
            //{
            //    FooterBillingLine.RecordType = Constants.RECORD_TYPE_X; ;
            //    FooterBillingLine.DocumentType = "HSINV";
            //    FooterBillingLine.Fields = String.Join("|", new string[21]);
            //    FooterBillingLine.Fields = "Yes|No";    
            //}
        }

        /// <summary>
        /// Create clone object to invoice header
        /// </summary>
        /// <param name="invoiceHeaderVO">Invoice header value object</param>
        /// <param name="invoiceDate">Invoice date</param>
        public InvoiceHeaderVO(InvoiceHeaderVO invoiceHeaderVO, DateTime invoiceDate)
        {
            ContractId = invoiceHeaderVO.ContractId;
            ContractNumber = invoiceHeaderVO.ContractNumber;
            RecordType = Constants.HEADER;
            CompanyId = invoiceHeaderVO.CompanyId;
            Field = String.Join("|", new string[12]);
            Currency = invoiceHeaderVO.Currency;
            CustomerCode = invoiceHeaderVO.CustomerCode;
            DivisionId = invoiceHeaderVO.DivisionId;
            Reference = !string.IsNullOrEmpty(invoiceHeaderVO.Reference) ? invoiceHeaderVO.Reference : Constants.ADVANCE_MAINT;
            VatCode = invoiceHeaderVO.VatCode;
            PostingPeriod = invoiceHeaderVO.PostingPeriod;
            PostingYear = invoiceHeaderVO.PostingYear;
            DocumentTypeID = invoiceHeaderVO.DocumentTypeID;
            DocumentTypeCR = invoiceHeaderVO.DocumentTypeCR;
            DocumentTypeIN = invoiceHeaderVO.DocumentTypeIN;            
            InvoiceCustomerId = invoiceHeaderVO.InvoiceCustomerId;
            CustomerName = invoiceHeaderVO.CustomerName;
            DivisionName = invoiceHeaderVO.DivisionName;

            Status = "S";
            DocumentDate = Convert.ToString(invoiceDate.ToShortDateString());

            InvoiceDetailVos = new List<InvoiceDetailVO>();
            FooterBillingLine = new InvoiceBillingLineVO();

            //Footer line for all companies
            FooterBillingLine.RecordType = Constants.RECORD_TYPE_X;
            FooterBillingLine.DocumentType = "HSINV";
            FooterBillingLine.Fields = String.Join("|", new string[21]);
            FooterBillingLine.Fields = "Yes|No";

            //Added one Extra footer Line 'T' for ComapnyId = 102
            if (invoiceHeaderVO.CompanyId == 102)
            {
                FooterBillingLine.RecordTypeForT = Constants.RECORD_TYPE_T;
                FooterBillingLine.DocumentTypeForT = Constants.DOCUMENT_TYPE_MASE;
                FooterBillingLine.Field3 = "10000";
                FooterBillingLine.VatCode = "0.00";
            }
            //else
            //{
            //    FooterBillingLine.RecordType = Constants.RECORD_TYPE_X;
            //    FooterBillingLine.DocumentType = "HSINV";
            //    FooterBillingLine.Fields = String.Join("|", new string[21]);
            //    FooterBillingLine.Fields = "Yes|No";
            //}

        }
    }
}