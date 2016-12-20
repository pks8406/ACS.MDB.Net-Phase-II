
using System.Collections.Generic;

namespace ACS.MDB.Library.ValueObjects
{
    public class InvoiceDetailVO
    {
        /// <summary>
        /// Gets or set Invoice General Ledger/Project Details for header - Nominal line
        /// </summary>
        public InvoiceGLDetailVO InvoiceGlDetails { get; set; }

        /// <summary>
        /// Gets or set Invoice billing lines information - Invoice billing lines
        /// </summary>
        public InvoiceBillingLineVO InvoiceBillingLines { get; set; }

        /// <summary>
        /// List of nominal lines for invoice header
        /// </summary>
        public List<InvoiceGLDetailVO> NominalLinesList { get; set; }

        /// <summary>
        /// Gets or set invoice is credit invoice 
        /// </summary>
        public bool isCreaditInvoice { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public InvoiceDetailVO()
        {
            InvoiceGlDetails = new InvoiceGLDetailVO();
            InvoiceBillingLines = new InvoiceBillingLineVO();
            NominalLinesList = new List<InvoiceGLDetailVO>();
        }

    }
}