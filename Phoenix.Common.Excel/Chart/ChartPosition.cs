namespace Phoenix.Common.Excel.Chart
{
    public class ChartPosition
    {
        public int ColumnPosition { get; set; }
        public int ColumnPositionOffset { get; set; }
        public int RowPosition { get; set; }
        public int RowPositionOffset { get; set; }

        public ChartPosition(int columnPosition, int rowPosition)
        {
            ColumnPosition = columnPosition;
            ColumnPositionOffset = 0;
            RowPosition = rowPosition;
            RowPositionOffset = 0;
        }

        public ChartPosition(int columnPosition, int columnPositionOffset, int rowPosition, int rowPositionOffset)
        {
            ColumnPosition = columnPosition;
            ColumnPositionOffset = columnPositionOffset;
            RowPosition = rowPosition;
            RowPositionOffset = rowPositionOffset;
        }

        public void Move(int column, int row)
        {
            ColumnPosition = column;
            RowPosition = row;
        }
    }
}
