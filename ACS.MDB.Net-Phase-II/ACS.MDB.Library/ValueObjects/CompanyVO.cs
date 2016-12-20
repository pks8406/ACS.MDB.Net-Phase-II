
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class CompanyVO
    {
        /// <summary>
        /// Gets or Sets company name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets company Id
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Gets or Sets selected value
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CompanyVO()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CompanyVO(OACompany company)
        {
            Name = company.CompanyName;
            CompanyID = company.ID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        //public CompanyVO(Company company)
        //{
        //    Name = company.Name;
        //    CompanyID = company.ID;
        //    IsSelected = company.IsSelected;
        //}
    }
}