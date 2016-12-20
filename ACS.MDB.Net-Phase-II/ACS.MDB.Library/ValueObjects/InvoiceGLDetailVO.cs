
using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class InvoiceGLDetailVO
    {
        /// <summary>
        /// Gets or set record type - 1st column
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Gets or set Associate Cost Centre Code - 2nd column
        /// </summary>
        public string CostCentre { get; set; }

        /// <summary>
        /// Gets or set Associate Expense Code/Account Code - 3rd column
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// Gets or set Associate JobCode/Project code - 4th column
        /// </summary>
        public string JobCode { get; set; }

        /// <summary>
        /// Gets or sets job code id
        /// </summary>
        public int JobCodeId { get; set; }

        /// <summary>
        /// Gets or set Associate Activity Code - 5th column
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// Gets or set Billing line amount - 6th column
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or set Vat code contracts - 7th column
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Gets or set Contract details - 8th column
        /// </summary>
        public string ContractDetails { get; set; }

        /// <summary>
        /// Gets or set blank fields
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// Gets or set blank extra fields. [9-24] column
        /// </summary>
        public string Field { get; set; } 

        /// <summary>
        /// Gets or set contract line id
        /// </summary>
        public int ContractLineId { get; set; }

        /// <summary>
        /// Gets or set associated contract id
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or set associated contract maintenance id
        /// </summary>
        public int ContractMaintenanceId { get; set; }

        /// <summary>
        /// Gets or set associated milestone id
        /// </summary>
        public int MilestoneId { get; set; }

        /// <summary>
        /// Gets or set renewal start date for - 8th column
        /// </summary>
        public string RenewalStartDate { get; set; }

        /// <summary>
        /// Gets or set renewal end date for - 8th column
        /// </summary>
        public string RenewalEndDate { get; set; }

        /// <summary>
        /// Gets or sets contract line is grouped or not
        /// </summary>
        public bool? IsGrouped { get; set; } 

        /// <summary>
        /// Gets or set group id
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Gets or set period frequency id
        /// </summary>
        public int PeriodFrequencyId { get; set; }

        /// <summary>
        /// Gets or set billing lines for invoice
        /// </summary>
        public List<InvoiceBillingLineVO> InvoiceBillingLines { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvoiceGLDetailVO()
        {
            Fields = new string[16];
            InvoiceBillingLines = new List<InvoiceBillingLineVO>();
        }

        /// <summary>
        /// Constructor for generating invoice billing details
        /// </summary>
        public InvoiceGLDetailVO(ContractLine contractLine )
        {
            Fields = new string[16];
            ContractLineId = contractLine.ID;
            ContractId = contractLine.ContractID;

            RecordType = "N";
            CostCentre = contractLine.OACostCentre.CostCentreID;
            AccountCode = contractLine.OAAccountCode.AccountID;
            JobCode = contractLine.OAJobCode.JobCodeID;
            JobCodeId = contractLine.JobCodeID;
            ActivityCode = contractLine.OAActivityCode.ActivityID;
            Value = 0;
            TaxCode = string.Empty;
            Field = String.Join("|", Fields);

            InvoiceBillingLines = new List<InvoiceBillingLineVO>();
        }

        /// <summary>
        /// Create clone object 
        /// </summary>
        /// <param name="invoiceGlDetail"></param>
        public InvoiceGLDetailVO(InvoiceGLDetailVO invoiceGlDetail)
        {
            Fields = new string[16];
            ContractLineId = invoiceGlDetail.ContractLineId;
            ContractId = invoiceGlDetail.ContractId;

            RecordType = "N";
            CostCentre = invoiceGlDetail.CostCentre;
            AccountCode = invoiceGlDetail.AccountCode;
            JobCode = invoiceGlDetail.JobCode;
            JobCodeId = invoiceGlDetail.JobCodeId;
            ActivityCode = invoiceGlDetail.ActivityCode;
            Value = 0;
            TaxCode = string.Empty;
            Field = String.Join("|", Fields);

            InvoiceBillingLines = new List<InvoiceBillingLineVO>();
        }

        /// <summary>
        /// Generate Invoice GL line VO for grouped billing lines 
        /// </summary>
        /// <param name="milestone"></param>
        public InvoiceGLDetailVO(Milestone milestone)
        {
            Fields = new string[16];
            ContractLineId = milestone.ContractLineID;
            ContractId = milestone.ContractID;

            RecordType = "N";
            CostCentre = Convert.ToString(milestone.ContractLine.OACostCentre.CostCentreID);
            AccountCode = Convert.ToString(milestone.ContractLine.OAAccountCode.AccountID);
            JobCode = Convert.ToString(milestone.ContractLine.OAJobCode.JobCodeID);
            JobCodeId = milestone.ContractLine.JobCodeID;
            ActivityCode = Convert.ToString(milestone.ContractLine.OAActivityCode.ActivityID);
            Value = 0;
            TaxCode = string.Empty;
            Field = String.Join("|", Fields);

            IsGrouped = milestone.ContractMaintenance.IsGrouped;
            GroupId = milestone.ContractMaintenance.GroupId;
            PeriodFrequencyId = milestone.ContractMaintenance.ChargeFrequencyID;
            ContractMaintenanceId = milestone.ContractMaintenance.ID;
            MilestoneId = milestone.ID;

            InvoiceBillingLines = new List<InvoiceBillingLineVO>();
        }
    }
}