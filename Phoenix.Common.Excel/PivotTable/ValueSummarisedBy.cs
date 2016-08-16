using OfficeOpenXml.Table.PivotTable;

namespace Phoenix.Common.Excel.PivotTable
{
    public enum ValueSummarisedBy
    {
        Sum,
        Count,
        Average,
        Max,
        Min
    }

    public static class ValueSummarisedByExtensions
    {
        public static DataFieldFunctions CalculationType(this ValueSummarisedBy self)
        {
            switch (self)
            {
                case ValueSummarisedBy.Average:
                    return DataFieldFunctions.Average;
                case ValueSummarisedBy.Count:
                    return DataFieldFunctions.Count;
                case ValueSummarisedBy.Max:
                    return DataFieldFunctions.Max;
                case ValueSummarisedBy.Min:
                    return DataFieldFunctions.Min;

                //default (including Sum) is Sum
                default:
                    return DataFieldFunctions.Sum;
            }
        }
    }
}
