using System.Collections.Generic;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ACS.MDB.Net.App.FileUtility
{
    public class ExcelUtility
    {

        //List of date field need to format in dd/mm/yyyy while export to Excel
        private List<string> DateFields = null;

        private ExcelPackage excelPackage = new ExcelPackage();

        public ExcelUtility()
        {
            DateFields = new List<string> { "End","End Date", "Created", "EstimatedDate", "Start", "Actual Bill Date", "First Annual Uplift Date","Forecast Billing Start Date" ,"Early Termination Date"};
            
        }

        /// <summary>
        /// Generate excel file from data table
        /// </summary>
        /// <param name="dataTable">Data table to generate excel file</param>
        /// <returns>Return excel package</returns>
        public ExcelPackage GenerateExcelReport(DataTable dataTable, string sheetName)
        {
            //Create the worksheet
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
            //excelWorkbook.Worksheets.Add(sheetName);

            //when no records are there in datatable
            if (dataTable.Columns.Count == 0)
            {
                return excelPackage;
            }
            else
            {
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                worksheet.Cells["A1"].Style.Font.Bold = true;

                for (int i = 1; i <= dataTable.Columns.Count; i++)
                {
                    string columnName = worksheet.Cells[1, i].Text;
                    if (DateFields.Contains(columnName))
                    {
                        worksheet.Column(i).Style.Numberformat.Format = "dd/MM/yyyy";
                    }

                    worksheet.Column(i).AutoFit();
                    worksheet.Cells[1, i].Style.Font.Bold = true;
                    worksheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }
            }
            return excelPackage;
        }

    }
}