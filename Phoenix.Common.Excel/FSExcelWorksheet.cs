using System.Data;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Phoenix.Common.Excel.Cell;
using Phoenix.Core.Tables;

namespace Phoenix.Common.Excel
{
    public class FSExcelWorksheet
    {
        private readonly FSTableColumns _columns;
        public ExcelWorksheet ExcelSheet { get; private set; }
        private readonly int _numberOfColumns;
        private int _numberOfRows;
        private readonly FSExcelDocument _document;
        private readonly string _worksheetName;

        public FSExcelWorksheet(FSExcelDocument document, string worksheetName, FSTableColumns columns)
        {
            _columns = columns;
            _numberOfColumns = columns.Items.Count;
            _document = document;
            _worksheetName = worksheetName;
        }

        public void AddDataTable(DataTable dataTable)
        {
            AddDataTable(dataTable, FSExcelTableTheme.None);
        }

        public void AddDataTable(DataTable dataTable, FSExcelTableTheme excelTableTheme)
        {
            _numberOfRows = dataTable.Rows.Count;
            ExcelSheet = _document.Workbook.Worksheets.Add(_worksheetName);
            ExcelSheet.Cells["A1"].LoadFromDataTable(dataTable, true, excelTableTheme.Theme());
            if (excelTableTheme == FSExcelTableTheme.None)
                ApplyWorksheetHeaderFormatting();
            ApplyWorksheetFormatting(false);
        }

        public void AddWorksheet(DataTable dataTable, bool addTableFilter)
        {
            AddDataTable(dataTable);

            ApplyWorksheetHeaderFormatting();
            ApplyWorksheetFormatting(true);

            if (!addTableFilter)
            {
                ExcelSheet.Tables[0].ShowFilter = false;
            }
        }

        public void AddDataTableByCell(DataTable dataTable)
        {
            _numberOfRows = dataTable.Rows.Count;
            ExcelSheet = _document.Workbook.Worksheets.Add(_worksheetName);
            AddWorksheetHeader();
            var cell = new FSExcelWorksheetCell(_document, _worksheetName);
            var rowIndex = 1;
            foreach (DataRow dr in dataTable.Rows)
            {
                rowIndex++;
                for (var colIndex = 1; colIndex <= _numberOfColumns; colIndex++)
                {
                    var cellValue = dr.ItemArray[colIndex - 1];
                    //var column = _columns.Items[colIndex - 1];
                    cell.SetCellValue(colIndex, rowIndex, cellValue);
                }
            }
            ApplyWorksheetHeaderFormatting();
            ApplyWorksheetFormatting(true);
        }

        private void AddWorksheetHeader()
        {
            for (var colIndex = 1; colIndex <= _numberOfColumns; colIndex++)
                ExcelSheet.Cells[1, colIndex].Value = _columns.Items[colIndex - 1].Text;
        }

        private void ApplyWorksheetHeaderFormatting()
        {
            var headerRange = ExcelSheet.Cells[1, 1, 1, _numberOfColumns];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);
        }

        private void ApplyWorksheetFormatting(bool autoAdjustColumns)
        {
            if (_numberOfRows == 0) return;
            for (var colIndex = 1; colIndex <= _numberOfColumns; colIndex++)
            {
                var column = _columns.Items[colIndex - 1];
                var excelColumnRange = ExcelSheet.Cells[2, colIndex, _numberOfRows + 1, colIndex];
                excelColumnRange.Style.Numberformat.Format = column.Format;
                excelColumnRange.Style.HorizontalAlignment = Alignment.Horizontal(column.HorizontalAlignment);
            }

            //borders
            //var tableRange = _excelSheet.Range(FSExcelHelper.GetExcelRange(1, _numberOfColumns, 1, _numberOfRows + 1));
            //tableRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            if (autoAdjustColumns)
                ExcelSheet.Cells[1, 1, _numberOfRows + 1, _numberOfColumns].AutoFitColumns();
        }

        private void AddTableFilter()
        {
            ExcelSheet.Cells[1, 1, _numberOfRows + 1, _numberOfColumns].AutoFilter = true;
        }

        public void Hide()
        {
            ExcelSheet.Hidden = eWorkSheetHidden.Hidden;
        }
    }
}
