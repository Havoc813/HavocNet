namespace Phoenix.Common.Excel.Chart
{
    public class FSExcelChart
    {
        private readonly FSExcelDocument _document;
        public ChartDimension Dimension { get; set; }
        public ChartPosition Position { get; set; }
        public ChartRange Range { get; set; }
        public bool ShowLegend { get; set; }
        public bool VaryColours { get; set; }
        public bool ShowGridLines { get; set; }

        public FSExcelChart(FSExcelDocument document, ChartRange range)
        {
            _document = document;
            Range = range;
            Position = new ChartPosition(1, 1);
            Dimension = new ChartDimension();
            VaryColours = true;
            ShowGridLines = true;
        }

        public void CreateChart(ChartType chartType, string chartTitle, string chartSheetName)
        {
            var dataWorksheet = _document.Workbook.Worksheets[Range.DataWorksheetName];
            var chartWorksheet = _document.Workbook.Worksheets[chartSheetName] ?? _document.Workbook.Worksheets.Add(chartSheetName);
            chartWorksheet.View.ShowGridLines = ShowGridLines;

            var chart = chartWorksheet.Drawings.AddChart(chartTitle, chartType.Chart());
            chart.SetPosition(Position.RowPosition, Position.RowPositionOffset,
                Position.ColumnPosition, Position.ColumnPositionOffset);
            chart.SetSize(Dimension.Width, Dimension.Height);

            var axisRange = dataWorksheet.SelectedRange[Range.GetAxisRange()];
            for (var index = 0; index < Range.GetSeriesCount(); index++)
            {
                var seriesRange = dataWorksheet.SelectedRange[Range.GetSeriesRange(index)];
                var series = chart.Series.Add(seriesRange, axisRange);
                series.Header = Range.GetSeriesName(index);
            }

            chart.Title.Text = chartTitle;
            if (!ShowLegend)
                chart.Legend.Remove();

            chart.VaryColors = VaryColours;

        }

        public void CreateChart(ChartType chartType, string chartTitle, string chartSheetName, ChartRange range)
        {
            Range = range;
            CreateChart(chartType, chartTitle, chartSheetName);
        }

        public void CreateChart(ChartType chartType, string chartTitle, string chartSheetName, ChartRange range, ChartPosition position)
        {
            Range = range;
            Position = position;
            CreateChart(chartType, chartTitle, chartSheetName);
        }

        public void CreateChart(ChartType chartType, string chartTitle, string chartSheetName, ChartRange range, ChartPosition position, ChartDimension dimension)
        {
            Range = range;
            Position = position;
            Dimension = dimension;
            CreateChart(chartType, chartTitle, chartSheetName);
        }
    }
}
