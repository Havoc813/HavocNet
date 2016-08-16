using System;
using System.Data;
using OfficeOpenXml;

namespace Phoenix.Common.Excel
{
    public class FSExcelReader
    {
        private readonly FSExcelDocument _document;

        public FSExcelReader(string excelFileName)
        {
            _document = new FSExcelDocument(excelFileName, false);
        }

        public DataTable GetWorksheet(int worksheetPosition, string tableName, bool tableScan = false)
        {
            return GetSheet(_document.Workbook.Worksheets[worksheetPosition], tableName, tableScan);
        }

        public DataTable GetWorksheet(string worksheetName, string tableName)
        {
            return GetSheet(_document.Workbook.Worksheets[worksheetName], tableName);
        }

        private DataTable GetSheet(ExcelWorksheet worksheet, string tableName, bool tableScan = false)
        {
            var dt = new DataTable(tableName);
            var colCount = worksheet.Dimension.Columns;
            var rowCount = worksheet.Dimension.Rows;

            if (tableScan) SetColumns(dt, worksheet, colCount, rowCount);
            else SetColumns(dt, worksheet, colCount);

            for (var j = 2; j <= rowCount; j++)
            {
                var aDataBlock = new String[colCount];

                for (var i = 1; i <= colCount; i++)
                {
                    var value = worksheet.Cells[j, i].Value != null ? worksheet.Cells[j, i].Value.ToString() : "";

                    if (dt.Columns[i - 1].DataType != typeof (double))
                        aDataBlock[i - 1] = value;
                    else
                        aDataBlock[i - 1] = value == "" ? "0" : value;
                }

                dt.Rows.Add(aDataBlock);
            }

            return dt;
        }

        private void SetColumns(DataTable dt, ExcelWorksheet worksheet, int colCount)
        {
            for (var i = 1; i <= colCount; i++)
            {
                dt.Columns.Add(new DataColumn(
                    worksheet.Cells[1, i].Value != null ? worksheet.Cells[1, i].Value.ToString() : " ",
                    worksheet.Cells[2, i].Value != null ? worksheet.Cells[2, i].Value.GetType() : typeof(string))
                );    
            }
        }

        private void SetColumns(DataTable dt, ExcelWorksheet worksheet, int colCount, int rowCount)
        {
            SetColumns(dt, worksheet, colCount);

            for (var i = 1; i <= colCount; i++)
            {
                if (dt.Columns[i - 1].DataType == typeof(string)) continue;

                for (var j = 2; j <= rowCount; j++)
                {
                    var value = worksheet.Cells[j, i].Value.ToString();

                    if (dt.Columns[i - 1].DataType == typeof(double))
                    {
                        double result;
                        if (!double.TryParse(value, out result))
                        {
                            dt.Columns[i - 1].DataType = typeof(string);
                            break;
                        }
                    }
                    else if (dt.Columns[i - 1].DataType == typeof(DateTime))
                    {
                        DateTime result;
                        if (!DateTime.TryParse(value, out result))
                        {
                            dt.Columns[i - 1].DataType = typeof(string);
                            break;
                        }
                    }
                }
            }
        }
    }
}
