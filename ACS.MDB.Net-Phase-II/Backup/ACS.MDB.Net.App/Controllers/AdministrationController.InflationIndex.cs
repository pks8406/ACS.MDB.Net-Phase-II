using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

using MODEL = ACS.MDB.Net.App.Models;
using System.IO;
using System.Web;

namespace ACS.MDB.Net.App.Controllers
{
	public partial class AdministrationController
	{
		/// <summary>
		/// Returns inflation index view
		/// </summary>
		/// <returns>inflation index view</returns>
		public ActionResult InflationIndex()
		{
			return IndexViewForAuthorizeUser();
		}

		/// <summary>
		/// Returns a list of inflation index for the selection criteria.
		/// </summary>
		/// <param name="param">The data table search criteria</param>
		/// <returns>List of inflation index</returns>
		public ActionResult InflationIndexList(MODEL.jQueryDataTableParamModel param)
		{
			try
			{
				List<MODEL.InflationIndex> inflationIndexList = new List<MODEL.InflationIndex>();
				InflationIndexService inflationIndexService = new InflationIndexService();
				List<InflationIndexVO> inflationIndexVOlist = inflationIndexService.GetInflationIndexList();
				foreach (InflationIndexVO inflationIndexVO in inflationIndexVOlist)
				{
					inflationIndexList.Add(new MODEL.InflationIndex(inflationIndexVO));
				}

				//get the field on with sorting needs to happen and set the
				//ordering function/delegate accordingly.
				int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
				var orderingFunction = GetInflationIndexOrderingFunction(sortColumnIndex);

				var result = GetFilteredObjectsOrderByAscending(param, inflationIndexList, orderingFunction);
				return result;
			}
			catch (Exception e)
			{
				return new HttpStatusCodeAndErrorResult(500, e.Message);
			}
		}

		/// <summary>
		/// Create new Inflation index
		/// </summary>
		/// <returns>The InflationIndexDetails view</returns>
		public ActionResult InflationIndexCreate()
		{
			try
			{
				MODEL.InflationIndex inflationIndex = new MODEL.InflationIndex();
				return PartialView("InflationIndexDetails", inflationIndex);
			}
			catch (Exception e)
			{
				return new HttpStatusCodeAndErrorResult(500, e.Message);
			}
		}

		/// <summary>
		/// Edit inflation index
		/// </summary>
		/// <param name="id">Inflation Index Id</param>
		/// <returns>The Inflation Index Details view</returns>
		public ActionResult InflationIndexEdit(int id)
		{
			MODEL.InflationIndex inflationIndex = null;
			try
			{
				InflationIndexService inflationIndexService = new InflationIndexService();

				//Get index details
				InflationIndexVO inflationIndexVO = inflationIndexService.GetInflationIndexById(id);
				if (inflationIndexVO == null)
				{
					ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.INDEX));
				}
				else
				{
					inflationIndex = new MODEL.InflationIndex(inflationIndexVO);
				}
			}
			catch (Exception e)
			{
				ModelState.AddModelError("", e.Message);
			}
			return PartialView("InflationIndexDetails", inflationIndex);
		}

		/// <summary>
		/// Save inflation index details
		/// </summary>
		/// <param name="model">model Object</param>
		public ActionResult InflationIndexSave(MODEL.InflationIndex model)
		{
			try
			{
				bool ismodelValid = ModelState.IsValid;
				if (!ismodelValid)
				{
					ismodelValid = IsModelValidForMultilineTextbox("Description", model.Description, 30);
				}

			    if (ismodelValid)
			    {
			        //Get user id
			        int? userId = Session.GetUserId();
			        InflationIndexService inflationIndexService = new InflationIndexService();
			        //InflationIndexVO inflationIndexVO = new InflationIndexVO(model, userId);

			        InflationIndexVO inflationIndexVO = model.Transpose(userId);

			        inflationIndexService.SaveInflationIndex(inflationIndexVO);
			        return new HttpStatusCodeResult(200);
			    }
			    else
			    {
			        throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.INDEX));
			    }
			}
			catch (Exception e)
			{
				return new HttpStatusCodeAndErrorResult(500, e.Message);
			}
		}

		/// <summary>
		/// Delete inflation index and associated index rate(s)
		/// </summary>
		/// <param name="Ids">Ids of inflation index to be deleted</param>
		public ActionResult InflationIndexDelete(List<int> Ids)
		{
			try
			{
                //Get user id
                int? userId = Session.GetUserId();

				InflationIndexService inflationIndexService = new InflationIndexService();
                inflationIndexService.DeleteInflationIndex(Ids, userId);

				return new HttpStatusCodeResult(200);
			}
			catch (Exception e)
			{
				return new HttpStatusCodeAndErrorResult(500, e.Message);
			}
		}

        /// <summary>
        /// Recalculate milestone for Not applied index
        /// </summary>
        /// <returns></returns>
        public ActionResult RecalculateForNonIndex()
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                InflationIndexService inflationIndexService = new InflationIndexService();

                List<int> Ids = new List<int>();

                Ids.Add(0);
                inflationIndexService.Recalculation(Ids, userId);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

		/// <summary>
		/// Recalculate milestone
		/// </summary>
		/// <returns></returns>
		public ActionResult Recalculate(List<int> Ids)
		{
			try
			{
				InflationIndexService inflationIndexService = new InflationIndexService();

                //Get user id
                int? userId = Session.GetUserId();

                inflationIndexService.Recalculation(Ids, userId);

				return new HttpStatusCodeResult(200);
			}
			catch (Exception e)
			{
				return new HttpStatusCodeAndErrorResult(500, e.Message);
			}
		}

		/// <summary>
		/// The function used to return field used for sorting
		/// </summary>
		/// <param name="sortCol">The column number on which sorting needs to happen</param>
		/// <returns></returns>
		public Func<BaseModel, object> GetInflationIndexOrderingFunction(int sortCol)
		{
			Func<BaseModel, object> sortFunction = null;
			switch (sortCol)
			{
				case 2:
					sortFunction = obj => ((InflationIndex)obj).IndexName;
					break;

				case 3:
					sortFunction = obj => ((InflationIndex)obj).Description;
					break;

				case 4:
					sortFunction = obj => ((InflationIndex)obj).UseIndex;
					break;

				default:
					sortFunction = obj => ((InflationIndex)obj).Description;
					break;
			}

			return sortFunction;
		}

        /// <summary>
        ///  Gets the List of Recalculation requests
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Recalculation Request List</returns>
        public ActionResult GetRecalculationRequestList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                List<MODEL.Recalculation> recalculationList = new List<Recalculation>();
                InflationIndexService inflationIndexService = new InflationIndexService();
                List<RecalculationVO> recalculationVOList = inflationIndexService.GetRecalculationRequestList();

                foreach (RecalculationVO recalculationVO in recalculationVOList)
                {
                    recalculationList.Add(new MODEL.Recalculation(recalculationVO));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetRecalculationRequestOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, recalculationList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete inflation index and associated index rate(s)
        /// </summary>
        /// <param name="Ids">Ids of inflation index to be deleted</param>
        public ActionResult RecalculationDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                InflationIndexService inflationIndexService = new InflationIndexService();
                inflationIndexService.DeleteRecalculation(Ids, userId);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns></returns>
        public Func<BaseModel, object> GetRecalculationRequestOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((Recalculation)obj).IsForUpliftRequired;
                    break;

                case 3:
                    sortFunction = obj => ((Recalculation)obj).RecalculationDate;
                    break;

                case 4:
                    sortFunction = obj => ((Recalculation)obj).RecalculationStatus;
                    break;

                default:
                    sortFunction = obj => ((Recalculation)obj).RecalculationDate;
                    break;
            }

            return sortFunction;
        }

        /// <summary>
        /// Download log File 
        /// </summary>
        /// <param name="filePath">File path</param>
        public void Download(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            string fileName = string.Empty;
            if (file.Exists)
            {                
                fileName = Path.GetFileName(filePath);
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.TransmitFile((filePath));
                Response.End();
            }
        }
    }
}