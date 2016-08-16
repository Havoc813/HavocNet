using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Phoenix.Common.Excel.Cell
{
    public class FSExcelCell
    {
        private ExcelRange _range;

        public FSExcelCell(FSExcelDocument document, string worksheetName, int column, int row)
        {
            var excelSheet = document.Workbook.Worksheets[worksheetName] ?? document.Workbook.Worksheets.Add(worksheetName);
            _range = excelSheet.Cells[row, column];
        }

        public void SetValue(object value)
        {
            _range.Value = value;
        }

        public object GetValue()
        {
            return _range.Value;
        }

        public void SetFont(FSExcelFont font)
        {
            if (!string.IsNullOrEmpty(font.FontName))
                _range.Style.Font.Name = font.FontName;

            if (font.Size > 0)
                _range.Style.Font.Size = font.Size;

            _range.Style.Font.Color.SetColor(font.ForeColour);

            _range.Style.HorizontalAlignment = Alignment.Horizontal(font.HorizontalAlignment);

            if (!string.IsNullOrEmpty(font.Format))
                _range.Style.Numberformat.Format = font.Format;

            _range.Style.Font.Bold = font.Bold;
            _range.Style.Font.Italic = font.Italic;
        }

        public void SetBackGroundColour(Color bgcolor)
        {
            _range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            _range.Style.Fill.BackgroundColor.SetColor(bgcolor);
        }

        public void AddComment(string comment, string author)
        {
            _range.AddComment(comment, author);
        }

        public void Formula(string formula)
        {
            _range.Formula = formula;
        }
    }
}
