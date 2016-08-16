using OfficeOpenXml.Drawing.Chart;

namespace Phoenix.Common.Excel.Chart
{
    public enum ChartType
    {
        BarClustered,
        BarClustered3D,
        BarStacked,
        BarStacked3D,
        Bubble,
        ColumnClustered,
        ColumnClustered3D,
        ColumnStacked,
        ColumnStacked3D,
        Column3D,
        Line,
        Line3D,
        Pie,
        Pie3D
    }

    public static class ChartTypeExtensions
    {
        public static eChartType Chart(this ChartType self)
        {
            switch (self)
            {
                case ChartType.BarClustered:
                    return eChartType.BarClustered;
                case ChartType.BarClustered3D:
                    return eChartType.BarClustered3D;
                case ChartType.BarStacked:
                    return eChartType.BarStacked;
                case ChartType.BarStacked3D:
                    return eChartType.BarStacked3D;
                case ChartType.Bubble:
                    return eChartType.Bubble;
                case ChartType.ColumnClustered:
                    return eChartType.ColumnClustered;
                case ChartType.ColumnClustered3D:
                    return eChartType.ColumnClustered3D;
                case ChartType.ColumnStacked:
                    return eChartType.ColumnStacked;
                case ChartType.ColumnStacked3D:
                    return eChartType.ColumnStacked3D;
                case ChartType.Line:
                    return eChartType.Line;
                case ChartType.Line3D:
                    return eChartType.Line3D;
                case ChartType.Pie:
                    return eChartType.Pie;
                case ChartType.Pie3D:
                    return eChartType.Pie3D;
                default:
                    return eChartType.Line;
            }
        }
    }
}
