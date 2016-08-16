using OfficeOpenXml.Style;
using Phoenix.Core.Tables;

namespace Phoenix.Common.Excel.Cell
{
    public static class Alignment
    {
        public static ExcelHorizontalAlignment Horizontal(FSAlignment horizontalAlignment)
        {
            switch (horizontalAlignment)
            {
                case FSAlignment.Centre:
                    return ExcelHorizontalAlignment.Center;
                case FSAlignment.Justify:
                    return ExcelHorizontalAlignment.Justify;
                case FSAlignment.Right:
                    return ExcelHorizontalAlignment.Right;
            }

            return ExcelHorizontalAlignment.Left;
        }
    }
}
