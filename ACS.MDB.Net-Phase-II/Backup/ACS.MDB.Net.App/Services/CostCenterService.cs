using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
namespace ACS.MDB.Net.App.Services
{
    public class CostCenterService : BaseService
    {
        CostCenterDAL costcenterDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CostCenterService()
        {
            costcenterDAL = new CostCenterDAL();
        }

        /// <summary>
        /// Get the list of CostCenter based on Company Id
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>Costcenter List</returns>
        public List<CostCentreVO> GetCostCenterList(int? companyId)
        {
            return costcenterDAL.GetCostCenterList(companyId);
        }
    }
}