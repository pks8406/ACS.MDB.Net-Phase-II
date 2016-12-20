
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class CustomerCommentVO : BaseVO
    {
        public int CustomerCommentId { get; set; }

        public int InvoiceCustomerId { get; set; }

        public string OldCustomerId { get; set; }
       
        public string InvoiceCustomerName { get; set; }

        public string InvoiceCustomer { get; set; }

        public int CompanyId { get; set; }
       
        public string CompanyName { get; set; }

        public string Company { get; set; }
       
        public string CustomerComment { get; set; }
       
        public bool Group { get; set; }

        //public List<CompanyVO> companyVOList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerCommentVO()
        { 
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="customerComment">LINQ customerComment object</param>
        public CustomerCommentVO(CustomerComment customerComment)
        {
            CustomerCommentId = customerComment.ID;
            InvoiceCustomerId = customerComment.CustomerID;
            OldCustomerId = customerComment.OACustomer.CustomerID;
            InvoiceCustomerName = customerComment.CustomerName;              
            //InvoiceCustomerName = customerComment.OACustomer.CustomerName;// customerComment.CustomerName;    
            InvoiceCustomer = InvoiceCustomerName + '-' + OldCustomerId;
            CompanyId = customerComment.CompanyID;
            CompanyName = customerComment.OACompany.CompanyName;
            Company = customerComment.OACompany.CompanyName + '-' + customerComment.CompanyID;
            CustomerComment = customerComment.Comment;
            Group = (customerComment.Group ? customerComment.Group : false);
            CreatedByUserId = customerComment.CreatedBy;
            LastUpdatedByUserId = customerComment.LastUpdatedBy;
        }

        /// <summary>
        /// Transpose Model object to Value object
        /// </summary>
        /// <param name="customerComment">model object</param>
        //public CustomerCommentVO(CustomerComment customerComment, int? userId)
        //{
        //    CustomerCommentId = customerComment.ID;
        //    InvoiceCustomerId = customerComment.InvoiceCustomerId;
        //    OldCustomerId = customerComment.OldCustomerId;
        //    InvoiceCustomerName = customerComment.InvoiceCustomerName;
        //    InvoiceCustomer = InvoiceCustomerName + '-' + OldCustomerId;
        //    CompanyId = customerComment.CompanyId;
        //    CompanyName = customerComment.CompanyName;
        //    Company = customerComment.CompanyName + '-' + customerComment.CompanyId;            
        //    CustomerComment = customerComment.Comment;
        //    Group = customerComment.Group;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}
    }
}