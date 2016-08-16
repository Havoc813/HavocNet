using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using Phoenix.Core.Tables;
namespace Phoenix.Common.Excel.PivotTable
{
    public class FSExcelPivotTable
    {
        private readonly FSExcelDocument _document;
        private int _numberOfColumns;
        private int _numberOfRows;
        private ExcelPivotTable _pivotTable;

        public FSExcelPivotTable(FSExcelDocument document)
        {
            _document = document;
        }

        public void AddPivotTable(DataTable dataTable, string pivotsheetName, FSTableColumns columns)
        {
            _numberOfRows = dataTable.Rows.Count;
            _numberOfColumns = columns.Items.Count;
            var dataWorksheet = new FSExcelWorksheet(_document, pivotsheetName + "Data", columns);
            dataWorksheet.AddDataTable(dataTable);
            var range = dataWorksheet.ExcelSheet.Cells[1, 1, _numberOfRows, _numberOfColumns];

            CreatePivotTable(pivotsheetName, range);
        }

        public void AddPivotTable(string datasheetName, string pivotsheetName)
        {
            var dataWorksheet = _document.Workbook.Worksheets[datasheetName];
            _numberOfRows = dataWorksheet.Dimension.End.Row;
            _numberOfColumns = dataWorksheet.Dimension.End.Column;
            var range = dataWorksheet.Cells[1, 1, _numberOfRows, _numberOfColumns];
            CreatePivotTable(pivotsheetName, range);
        }

        private void CreatePivotTable(string pivotsheetName, ExcelRange range)
        {
            var pivotSheet = _document.Workbook.Worksheets.Add(pivotsheetName);
            _pivotTable = pivotSheet.PivotTables.Add(pivotSheet.Cells["A1"], range, pivotsheetName);
        }

        public void AddPivotTableRowLabel(string rowLabel)
        {
            _pivotTable.RowFields.Add(_pivotTable.Fields[rowLabel]);
        }

        public void AddPivotTableColumnLabel(string columnLabel)
        {
            _pivotTable.ColumnFields.Add(_pivotTable.Fields[columnLabel]);
        }

        public void AddPivotTableValue(PivotTableValue value)
        {
            _pivotTable.DataFields.Add(_pivotTable.Fields[value.ColumnName]);
            _pivotTable.DataFields[_pivotTable.DataFields.Count - 1].Format = value.Format;
            _pivotTable.DataFields[_pivotTable.DataFields.Count - 1].Function = value.SummarisedBy.CalculationType();
        }

        public void AddPivotTableLabels(string rowLabel, string columnLabel, PivotTableValue value)
        {
            AddPivotTableRowLabel(rowLabel);
            AddPivotTableColumnLabel(columnLabel);
            AddPivotTableValue(value);
            ApplyPivotTableFormatting();
        }

        public void AddPivotTableLabels(string rowLabel, PivotTableValue value)
        {
            AddPivotTableRowLabel(rowLabel);
            AddPivotTableValue(value);
            ApplyPivotTableFormatting();
        }

        private void ApplyPivotTableFormatting()
        {
            _pivotTable.UseAutoFormatting = true;
            _pivotTable.ApplyWidthHeightFormats = true;
            _pivotTable.TableStyle = TableStyles.Light1;
            _pivotTable.ShowHeaders = false;
            //_pivotTable.ApplyNumberFormats = true;
        }

        public void ShowColumnGrandTotals(bool showTotals)
        {
            _pivotTable.ColumGrandTotals = showTotals;
        }

        public void ShowRowGrandTotals(bool showTotals)
        {
            _pivotTable.RowGrandTotals = showTotals;
        }
    }
}
