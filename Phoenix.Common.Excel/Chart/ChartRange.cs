using System.Collections.Generic;

namespace Phoenix.Common.Excel.Chart
{
    public class ChartRange
    {
        public string DataWorksheetName { get; set; }
        private readonly FSExcelRange _axis;
        private readonly List<ChartSeries> _series;

        public ChartRange(string dataWorksheet, FSExcelRange axis)
        {
            DataWorksheetName = dataWorksheet;
            _axis = axis;
            _series = new List<ChartSeries>();
        }

        public ChartRange(string dataWorksheet, FSExcelRange axis, ChartSeries series)
        {
            DataWorksheetName = dataWorksheet;
            _axis = axis;
            _series = new List<ChartSeries> { series };
        }

        public void AddSeries(ChartSeries series)
        {
            _series.Add(series);
        }

        public string GetAxisRange()
        {
            return _axis.Range;
        }

        public int GetSeriesCount()
        {
            return _series.Count;
        }

        public string GetSeriesRange(int index)
        {
            return _series[index].GetRange();
        }

        public string GetSeriesName(int index)
        {
            return _series[index].Name;
        }
    }
}
