namespace Phoenix.Common.Excel.Chart
{
    public class ChartSeries
    {
        public string Name { get; set; }
        private readonly FSExcelRange _range;

        public ChartSeries(string seriesName, FSExcelRange seriesRange)
        {
            Name = seriesName;
            _range = seriesRange;
        }

        public string GetRange()
        {
            return _range.Range;
        }
    }
}
