
namespace ACS.MDB.Library.ValueObjects
{
    public class UserCompanyVO
    {
        /// <summary>
        /// Gets or Sets company id
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Gets or Sets company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserCompanyVO()
        {

        }    
   
        UserVO userVO = new UserVO();
    }
}