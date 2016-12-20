
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class DivisionVO : BaseVO
    {
        /// <summary>
        /// Gets or Sets division id
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or Sets company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or Sets company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or Sets division name
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or Sets division is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or Sets division document type CR
        /// </summary>
        public string DocumentTypeCR { get; set; }

        /// <summary>
        /// Gets or Sets division document type IN
        /// </summary>
        public string DocumentTypeIN { get; set; }

        /// <summary>
        /// Gets or Sets Document Type Deposit Invoices
        /// </summary>
        public string DocumentTypeDepositInvoices { get; set; }

        /// <summary>
        /// Gets or Sets Document Type Deposit Credits
        /// </summary>
        public string DocumentTypeDepositCredits { get; set; }

        /// <summary>
        /// Gets or set DefaultInvoiceInAdvance
        /// </summary>
        public int DefaultInvoiceInAdvance { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DivisionVO()
        {

        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="division">LINQ division object</param>
        public DivisionVO(Division division)
        {
            DivisionId = division.ID;
            CompanyId = division.CompanyID;
            DivisionName = division.DivisionName;
            IsActive = division.IsActive;
            DocumentTypeCR = division.DocumentTypeCR;
            DocumentTypeIN = division.DocumentTypeIN;
            DocumentTypeDepositInvoices = division.DepositDocumentTypeIN;
            DocumentTypeDepositCredits = division.DepositDocumentTypeCR;
            DefaultInvoiceInAdvance = division.DefaultInvoiceInAdvanced;
            CreatedByUserId = division.CreatedBy;
            LastUpdatedByUserId = division.LastUpdatedBy;
        }

        /// <summary>
        /// Transpose Model object to Value object
        /// </summary>
        /// <param name="division"></param>
        //public DivisionVO(Division division, int userId)
        //{
        //    DivisionId = division.ID;
        //    CompanyId = division.CompanyId;
        //    DivisionName = division.DivisionName;
        //    CompanyName = division.CompanyName;
        //    IsActive = division.IsActive;
        //    DocumentTypeCR = division.DocumentTypeCR;
        //    DocumentTypeIN = division.DocumentTypeIN;
        //    DefaultInvoiceInAdvance = division.DefaultInvoiceInAdvance;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}