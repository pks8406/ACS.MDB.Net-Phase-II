
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class EndUserVO : BaseVO
    {
        /// <summary>
        /// gets or sets id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// gets or sets end user id
        /// </summary>
        public string EndUserId { get; set; }
        
        /// <summary>
        /// gets or sets end user text id
        /// </summary>
        public string EndUserTextId { get; set; }

        /// <summary>
        /// gets or sets company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// gets or sets invoice customer id
        /// </summary>
        public int InvoiceCustomerId { get; set; }

        /// <summary>
        /// gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// gets or sets end user name with customer code
        /// </summary>
        public string EndUserNameWithCustomerCode { get; set; }

        /// <summary>
        /// gets or sets to knwo whether it is new/edit edit user
        /// </summary>
        public bool IsNewEndUser { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndUserVO()
        {
        }

        /// <summary>
        /// Transpose Model object to value object
        /// </summary>
        /// <param name="endUser">model object of enduser</param>
        //public EndUserVO(MODEL.EndUser endUser, int? userId)
        //{
        //    ID = endUser.ID;
        //    EndUserId = endUser.EndUserId;       
        //    CompanyId = endUser.CompanyId;
        //    InvoiceCustomerId = endUser.InvoiceCustomerId;
        //    Name = endUser.Name;
        //    CreatedByUserId = userId;
        //    LastUpdatedByUserId = userId;
        //}

        /// <summary>
        /// Transpose LINQ object to value object
        /// </summary>
        /// <param name="endUser">LINQ object of enduser</param>
        public EndUserVO(EndUser endUser)
        {
            ID = endUser.ID;
            EndUserId = endUser.EndUserTextID;
            //CustomerID = endUser.ID;
            CompanyId = endUser.BusinessPartner.CompanyID;
            InvoiceCustomerId = endUser.BusinessPartner.InvoiceCustomerID;
            Name = endUser.EndUserName;
            CreatedByUserId = endUser.CreatedBy;
            LastUpdatedByUserId = endUser.LastUpdatedBy;
            EndUserNameWithCustomerCode = endUser.EndUserName;
            IsNewEndUser = true;
        }

        /// <summary>
        /// Transpose OACustomer LINQ object to value object
        /// </summary>
        /// <param name="oaCustomer"></param>
        public EndUserVO(OACustomer oaCustomer)
        {
            ID = oaCustomer.ID;
            EndUserId = oaCustomer.ID.ToString();
            Name = oaCustomer.CustomerName;
            CompanyId = oaCustomer.CompanyID;
            EndUserNameWithCustomerCode = oaCustomer.CustomerName + " - " + oaCustomer.CustomerID + " - " +
                                          oaCustomer.ShortName;
        }

    }
}