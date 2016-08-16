using System;
using System.Data;
using System.Globalization;
using Phoenix.Core.Tables;

namespace Phoenix.Common.Excel
{
    public static class FSExcelHelper
    {
        public static string GetExcelColumnName(int columnIndex)
        {
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString(CultureInfo.InvariantCulture);

            var firstChar = (char)('A' + (columnIndex / 26) - 1);
            var secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        public static string GetExcelRange(int columnIndex, int rowIndex)
        {
            return String.Format("{0}{1}", GetExcelColumnName(columnIndex - 1), rowIndex.ToString("0"));
        }

        public static string GetExcelRange(int columnIndexFrom, int columnIndexTo, int rowIndexFrom, int rowIndexTo)
        {
            return String.Format("{0}:{1}", GetExcelRange(columnIndexFrom, rowIndexFrom),
                GetExcelRange(columnIndexTo, rowIndexTo));
        }

        public static double GetExcelColumnWidth(int pixelWidth)
        {
            if (pixelWidth <= 0)
                return 0;
            return Math.Round((pixelWidth - 7) / 7d + 1, 2);
        }

        public static FSTableColumns ToFSTableColumns(DataColumnCollection columns)
        {
            var aFSTableCols = new FSTableColumns();

            foreach (DataColumn aCol in columns)
            {
                aFSTableCols.Items.Add(new FSColumn(aCol.ColumnName, aCol.DataType));
            }

            return aFSTableCols;
        }
    }
}
