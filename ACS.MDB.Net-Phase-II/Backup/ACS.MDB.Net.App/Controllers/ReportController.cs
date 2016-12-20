using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.Mvc;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    [SessionHandler]
    public partial class ReportController : BaseController
    {
        #region Public Methods

        /// <summary>
        /// Get Invoice Customer List
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>List of invoice customervo list</returns>
        public JsonResult GetInvoiceCustomerList(int companyId)
        {
            try
            {
                List<InvoiceCustomerVO> invoiceCustomerVOList;
                InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
                invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(companyId);
                return Json(invoiceCustomerVOList);
            }
            catch (Exception e)
            {
                return Json(new ApplicationException(e.Message));
            }
        }

        /// <summary>
        /// Get Division List
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>The JsonResulte having division list</returns>
        public JsonResult GetDivisionList(int companyId)
        {
            try
            {
                List<DivisionVO> divisionVOList;
                DivisionService divisionService = new DivisionService();
                divisionVOList = divisionService.GetDivisionListByCompany(companyId);
                return Json(divisionVOList);
            }
            catch (Exception e)
            {
                return Json(new ApplicationException(e.Message));
            }
        }

        /// <summary>
        /// Get End User List
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>List of end users</returns>
        //public JsonResult GetEndUserList(int? companyId)
        //{
        //    try
        //    {
        //        EndUserService endUserService = new EndUserService();
        //        List<EndUserVO> endUserVOList = endUserService.GetEndUserList(companyId.Value);
        //        return Json(endUserVOList);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new ApplicationException());

        //    }
        //}

        /// <summary>
        ///  Gets List of Invoice Customers based on CompanyID
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>List of invoice customers</returns>
        public List<InvoiceCustomerVO> GetInvoiceCustomerListByCompanyID(int? companyId)
        {
            try
            {
                InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
                List<InvoiceCustomerVO> invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(companyId.Value);
                return invoiceCustomerVOList;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        /// <summary>
        ///  Gets List of Divisions based on CompanyID
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <returns>The divisionVO list</returns>
        public List<DivisionVO> GetDivisionListById(int? companyId)
        {
            try
            {
                DivisionService divisionService = new DivisionService();
                List<DivisionVO> divisionVOList = divisionService.GetDivisionListByCompany(companyId);
                //List<Division> divisionList = new List<Division>();

                //foreach (DivisionVO d in divisionVOList)
                //{
                //    divisionList.Add(new Division(d));
                //}
                return divisionVOList;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        /// <summary>
        /// Remove columns whose date is not between start date and end date
        /// </summary>
        /// <param name="dataTable">Data table</param>
        /// <param name="startDate">Start date provided by user</param>
        /// <param name="endDate">end date provided by user</param>
        public void RemoveExtraColumns(DataTable dataTable, DateTime startDate, DateTime endDate, int startColumnIndex)
        {
            //Remove extra columns from datatable onwards 21 column (column no 21 = 'No of Days')
            for (int col = dataTable.Columns.Count - 1; col >= startColumnIndex; col--)
            {
                string colunmName = Convert.ToString(dataTable.Columns[col]);

                string[] dates = colunmName.Split('_');

                if (dates.Length != 2) continue;
                int year = Convert.ToInt32(dates[1]);

                int monthNumber = DateTime.ParseExact(dates[0], "MMM", CultureInfo.CurrentCulture).Month;

                DateTime currentDate = new DateTime(year, monthNumber, 1);

                if (currentDate < startDate || currentDate > endDate)
                {
                    // Do not remove start date month & end date month
                    if ((currentDate.Year != startDate.Year || currentDate.Month != startDate.Month) &&
                        (currentDate.Year != endDate.Year || currentDate.Month != endDate.Month))
                    {
                        dataTable.Columns.RemoveAt(col);
                    }
                }
            }

            //Remove null rows from data table
            int totalColumn = dataTable.Columns.Count - 1;
            for (int i = dataTable.Rows.Count-1; i >= 0; i--)
            {
                DataRow row = dataTable.Rows[i];
                bool hasValue = false;
                for (int r = startColumnIndex; r <= totalColumn; r++)
                {
                    string value = row[r].ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        hasValue = true;
                        break;
                    }
                }
                if (!hasValue)
                {
                    dataTable.Rows.RemoveAt(i);
                }

            }

        }

        /// <summary>
        ///  Gets List of End Users based on CompanyID
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="invoiceCustomerId">The invoice customer id</param>
        /// <returns>List of end users</returns>
        //public List<EndUserVO> GetEndUserListByCompanyID(int? companyId)
        //{
        //    try
        //    {
        //        EndUserService endUserService = new EndUserService();
        //        List<EndUserVO> endUserVOList = endUserService.GetEndUserList(companyId.Value);
        //        return endUserVOList;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new ApplicationException(e.Message);
        //    }
        //}

        #endregion Public Methods
    }
}