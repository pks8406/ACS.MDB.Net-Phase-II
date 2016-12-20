using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class CostCenterDAL : BaseDAL
    {
        /// <summary>
        /// Gets list of Costcenter based on companyId
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>Value object of costcener list</returns>
        public List<CostCentreVO> GetCostCenterList(int? companyId)
        {
            List<CostCentreVO> costcenterVOList = new List<CostCentreVO>();

            List<OACostCentre> costcenterList = mdbDataContext.OACostCentres.Where(c => c.CompanyID == companyId && c.IsDeleted == false).OrderBy(c => c.CostCentreName).ToList();

            foreach (OACostCentre item in costcenterList)
            {
                costcenterVOList.Add(new CostCentreVO(item));
            }

            return costcenterVOList;
        }
    }
}