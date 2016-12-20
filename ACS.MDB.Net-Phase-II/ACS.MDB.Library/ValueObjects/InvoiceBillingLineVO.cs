
using System;
using System.Collections.Generic;
using ACS.MDB.Library.Common;

namespace ACS.MDB.Library.ValueObjects
{
    public class InvoiceBillingLineVO
    {
        /// <summary>
        /// Gets or set Header type for extra line - 1st column
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// Gets or set Record type for CompanyId = 102 for footer
        /// </summary>
        public string RecordTypeForT { get; set; }

        /// <summary>
        /// Gets or set Document type -  Always "DSINV" - 2nd column
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or set Document type -  for CompanyId = 102 - Always "MASE"
        /// </summary>
        public string DocumentTypeForT { get; set; }

        /// <summary>
        /// Gets or set Field3 value - Always " " - 3rd column
        /// </summary>
        public string Field3 { get; set; }

        /// <summary>
        /// Gets or set vat code associated with customer - 4th column
        /// </summary>
        public string VatCode { get; set; }

        /// <summary>
        /// Gets or set Milestone billing lines - 15 lines - [5-19] column
        /// </summary>
        public string[] BillingLines = new string[15];

        /// <summary>
        /// Gets or set Unit price - 20th column
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or set contract quantity - 21st column
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or set amount - 22nd column
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or set field 23 & 24 as blank
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public InvoiceBillingLineVO()
        {
            RecordType = Constants.RECORD_TYPE_X;
        }

        /// <summary>
        /// Constructor for processing invoice billing line information
        /// </summary>
        /// <param name="milestoneBillingLines">milestone billing lines</param>
        public InvoiceBillingLineVO(List<MilestoneBillingLineVO> milestoneBillingLines)
        {
            RecordType = Constants.RECORD_TYPE_X;
            DocumentType = "DSINV";
            Field3 = " ";

            for (int i = 0; i < milestoneBillingLines.Count; i++)
            {
                // Implement CR ARBS-39
                if (i < 15)
                {
                    string billingLine = milestoneBillingLines[i].LineText;

                    // if billing line character more then 48 then append *
                    if (!string.IsNullOrEmpty(billingLine) && billingLine.Length > 48)
                    {
                        billingLine = billingLine.Substring(0, 47) + "*";
                    }

                    BillingLines[i] = billingLine;
                }
            }

            Fields = String.Join("|", new string[3]);
        }
    }
}