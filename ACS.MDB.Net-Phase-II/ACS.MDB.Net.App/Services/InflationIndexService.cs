using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Services
{
    public class InflationIndexService : BaseService
    {
        InflationIndexDAL inflationIndexDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InflationIndexService()
        {
            inflationIndexDAL = new InflationIndexDAL();
        }

        /// <summary>
        /// Gets the list of inflation indexes
        /// </summary>
        /// <returns>List of Product</returns>
        public List<InflationIndexVO> GetInflationIndexList()
        {
            return inflationIndexDAL.GetInflationIndexList();
        }

        /// <summary>
        /// Get Inflation index details by Id
        /// </summary>
        /// <param name="indexId">index Id</param>
        /// <returns>IndexId Details</returns>
        public InflationIndexVO GetInflationIndexById(int indexId = 0)
        {
            return inflationIndexDAL.GetInflationIndexById(indexId);
        }

        /// <summary>
        /// Save the Inflation index
        /// </summary>
        /// <param name="inflationIndexVO">Value Object InflationIndex</param>
        public void SaveInflationIndex(InflationIndexVO inflationIndexVO)
        {
            //Get and check whether index already exist with same name or not
            InflationIndexVO inflationIndexExist = inflationIndexDAL.GetInflationIndexByName(inflationIndexVO.InflationIndexName);
            if (inflationIndexExist != null && inflationIndexVO.InflationIndexId != inflationIndexExist.InflationIndexId)
            {
                throw new ApplicationException(Constants.INDEX_ALREADY_EXIST);
            }
            else
            {
                inflationIndexDAL.SaveInflationIndex(inflationIndexVO);
            }
        }

        /// <summary>
        /// Delete inflation index and associated index rate(s)
        /// </summary>
        /// <param name="Ids">Ids of inflation index to be deleted</param>
        public void DeleteInflationIndex(List<int> Ids, int? userId)
        {
            inflationIndexDAL.DeleteInflationIndex(Ids, userId);
        }

        /// <summary>
        /// Request for recalculation
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="userId"></param>
        public void Recalculation(List<int> Ids, int? userId)
        {
            RecalculationVO recalculationVO = new RecalculationVO();

            string ids = null;
            foreach (var id in Ids)
            {
                ids += id + ";";
            }

            ids = ids.Remove(ids.Length - 1);

            recalculationVO.IndexIds = ids;
            recalculationVO.IsForUpliftRequired = (Ids.Count == 1 && Ids[0] == 0) ? false : true;
            recalculationVO.Status = Convert.ToInt32(Constants.RecalculationStatus.PENDING);
            recalculationVO.RecalculationDate = DateTime.Now;
            recalculationVO.CreatedByUserId = userId;

            inflationIndexDAL.RequestForRecalculation(recalculationVO);
        }

        /// <summary>
        /// Gets the List of Recalculation requests
        /// </summary>
        /// <returns>Recalculation Request List</returns>
        public List<RecalculationVO> GetRecalculationRequestList()
        {
            return inflationIndexDAL.GetRecalculationRequestList();
        }

        /// <summary>
        /// Delete the recalculation Record(s)
        /// </summary>
        /// <param name="Ids">The recalculation id list to remove</param>
        /// <param name="userId">user id</param>
        public void DeleteRecalculation(List<int> Ids, int? userId)
        {
            inflationIndexDAL.DeleteRecalculation(Ids, userId);
        }
    }
}