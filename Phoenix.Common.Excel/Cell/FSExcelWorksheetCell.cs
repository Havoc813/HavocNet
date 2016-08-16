using OfficeOpenXml;

namespace Phoenix.Common.Excel.Cell
{
    public class FSExcelWorksheetCell
    {
        private readonly ExcelWorksheet _excelSheet;

        public FSExcelWorksheetCell(FSExcelDocument document, string worksheetName)
        {
            _excelSheet = document.Workbook.Worksheets[worksheetName] ?? document.Workbook.Worksheets.Add(worksheetName);
        }

        public void SetCellValue(int column, int row, object value)
        {
            _excelSheet.Cells[row, column].Value = value;
        }

        public void SetCellValue(string range, object value)
        {
            _excelSheet.Cells[range].Value = value;
        }

        public object GetCellValue(int column, int row)
        {
            return _excelSheet.Cells[row, column].Value;
        }

        public void SetCellFormula(int column, int row, string formula)
        {
            _excelSheet.Cells[row, column].Formula = formula;
        }

        public void SetCellFormula(string range, string formula)
        {
            _excelSheet.Cells[range].Formula = formula;
        }
    }
}
