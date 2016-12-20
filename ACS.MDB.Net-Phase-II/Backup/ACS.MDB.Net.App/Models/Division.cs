using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class Division : BaseModel
    {
        /// <summary>
        /// Gets or set Compnay Id
        /// </summary>
        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or set division name
        /// </summary>
        [Required(ErrorMessage = "Please enter Division name")]
        [RegularExpression("^([a-zA-Z0-9 &.'-]+)$", ErrorMessage = "Please enter valid division name")]
        [Display(Name = "Division Name")]
        [StringLength(50, ErrorMessage = "Divison name must be with a maximum length of 50")]
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or set division is active or inactive
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or set division is active or inactive
        /// </summary>
        [Display(Name = "Division")]
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or set DefaultInvoiceInAdvance
        /// </summary>
        [Display(Name = "Invoice in Advance")]
        public int DefaultInvoiceInAdvance { get; set; }

        /// <summary>
        /// Gets or set division Document Type CR
        /// </summary>
        [Required(ErrorMessage = "Please enter Document Type CR")]
        [RegularExpression("^([a-zA-Z0-9 &.'-]+)$", ErrorMessage = "Please enter valid document type CR")]
        [Display(Name = "Document Type CR")]
        [StringLength(50, ErrorMessage = "Document Type CR must be with a maximum length of 10")]
        public string DocumentTypeCR { get; set; }

        /// <summary>
        /// Gets or set division Document Type IN
        /// </summary>
        [Required(ErrorMessage = "Please enter Document Type IN")]
        [RegularExpression("^([a-zA-Z0-9 &.'-]+)$", ErrorMessage = "Please enter valid document type IN")]
        [Display(Name = "Document Type IN")]
        [StringLength(50, ErrorMessage = "Document Type IN must be with a maximum length of 10")]
        public string DocumentTypeIN { get; set; }

        /// <summary>
        /// Gets or set Document Type Deposit Invoices
        /// </summary>
        [Required(ErrorMessage = "Please enter Document Type Deposit Invoices")]
        [RegularExpression("^([a-zA-Z0-9 &.'-]+)$", ErrorMessage = "Please enter valid document type Deposit Invoices")]
        [Display(Name = "Document Type Deposit Invoices")]
        [StringLength(50, ErrorMessage = "Document Type Deposit Invoices must be with a maximum length of 10")]
        public string DocumentTypeDepositInvoices { get; set; }

        /// <summary>
        /// Gets or set Document Type Deposit Credits
        /// </summary>
        [Required(ErrorMessage = "Please enter Document Type Deposit Credits")]
        [RegularExpression("^([a-zA-Z0-9 &.'-]+)$", ErrorMessage = "Please enter valid document type Deposit Credits")]
        [Display(Name = "Document Type Deposit Credits")]
        [StringLength(50, ErrorMessage = "Document Type Deposit Credits must be with a maximum length of 10")]
        public string DocumentTypeDepositCredits { get; set; }


        /// <summary>
        /// Gets the list of Default Inoivce In advance
        /// </summary>        
        public List<InvoiceAdvanced> defaultInvoiceInAdvancedList { get; set; }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Division()
        {
            defaultInvoiceInAdvancedList = new List<InvoiceAdvanced>();
        }

        /// <summary>
        /// Transpose division value object to model object
        /// </summary>
        public Division(DivisionVO divisionVO)
        {
            ID = divisionVO.DivisionId;
            CompanyId = divisionVO.CompanyId;
            DivisionId = divisionVO.DivisionId;
            DivisionName = divisionVO.DivisionName;
            IsActive = divisionVO.IsActive;
            DocumentTypeCR = divisionVO.DocumentTypeCR;
            DocumentTypeIN = divisionVO.DocumentTypeIN;
            DocumentTypeDepositInvoices = divisionVO.DocumentTypeDepositInvoices;
            DocumentTypeDepositCredits = divisionVO.DocumentTypeDepositCredits;
            DefaultInvoiceInAdvance = divisionVO.DefaultInvoiceInAdvance;
            CompanyName = divisionVO.CompanyName;

        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public DivisionVO Transpose(int? userId)
        {
            DivisionVO divisionVO = new DivisionVO();

            divisionVO.DivisionId = this.ID;
            divisionVO.CompanyId = this.CompanyId;
            divisionVO.DivisionName = this.DivisionName;
            divisionVO.CompanyName = this.CompanyName;
            divisionVO.IsActive = this.IsActive;
            divisionVO.DocumentTypeCR = this.DocumentTypeCR;
            divisionVO.DocumentTypeIN = this.DocumentTypeIN;
            divisionVO.DocumentTypeDepositInvoices = this.DocumentTypeDepositInvoices;
            divisionVO.DocumentTypeDepositCredits = this.DocumentTypeDepositCredits;
            divisionVO.DefaultInvoiceInAdvance = this.DefaultInvoiceInAdvance;
            divisionVO.CreatedByUserId = userId;
            divisionVO.LastUpdatedByUserId = userId;

            return divisionVO;
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
            return (DivisionName != null && DivisionName.StartsWith(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Function called to return the value contained
        /// in the model as an array of strings (object).
        /// Typically used to fill up the datatable
        /// grid control.
        /// </summary>
        public override object[] GetModelValue()
        {
            object[] result = new object[] { "<input type='checkbox' name='check5' value='" + ID + "'>",
                ID, DivisionName, DocumentTypeCR, DocumentTypeIN, DocumentTypeDepositInvoices,DocumentTypeDepositCredits ,IsActive ? "Active" : "Inactive" };
            return result;
        }
    }
}