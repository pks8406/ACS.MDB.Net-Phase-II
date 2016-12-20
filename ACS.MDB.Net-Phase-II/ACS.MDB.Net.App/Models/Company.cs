using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Models
{
    public class Company : BaseModel
    {
        /// <summary>
        /// Gets or set company name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set company name with ID
        /// </summary>
        public string NameWithID { get; set; }

        /// <summary>
        /// Gets or set is company associated 
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Company()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Company(CompanyVO companyVO)
        {
            ID = companyVO.CompanyID;
            Name = companyVO.Name;
            NameWithID = companyVO.Name + " - " + ID;
            IsSelected = companyVO.IsSelected;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Company(Company company)
        {
            ID = company.ID;
            Name = company.Name;
            NameWithID = company.Name + " - " + ID;
            IsSelected = company.IsSelected;
        }

        /// <summary>
        /// Transpose Model object to Value Object
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Value object</returns>
        public CompanyVO Transpose()
        {
            CompanyVO companyVO = new CompanyVO();

            companyVO.Name = this.Name;
            companyVO.CompanyID = this.ID;
            companyVO.IsSelected = this.IsSelected;

            return companyVO;
        }
        
    }
}