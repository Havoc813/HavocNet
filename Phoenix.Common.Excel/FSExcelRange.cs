namespace Phoenix.Common.Excel
{
    public class FSExcelRange
    {
        public string Range { get; private set; }

        public FSExcelRange(int column, int row)
        {
            Range = FSExcelHelper.GetExcelRange(column, column, row, row);
        }

        public FSExcelRange(int columnFrom, int columnTo, int rowFrom, int rowTo)
        {
            Range = FSExcelHelper.GetExcelRange(columnFrom, columnTo, rowFrom, rowTo);
        }
    }
}
