namespace Phoenix.Common.Excel.PivotTable
{
    public class PivotTableValue
    {
        public string ColumnName { get; set; }
        public string Format { get; set; }
        public ValueSummarisedBy SummarisedBy { get; set; }

        public PivotTableValue(string columnName)
        {
            Init(columnName, @"#,##0.00", ValueSummarisedBy.Sum);
        }

        public PivotTableValue(string columnName, string format)
        {
            Init(columnName, format, ValueSummarisedBy.Sum);
        }

        public PivotTableValue(string columnName, ValueSummarisedBy summarisedBy)
        {
            Init(columnName, @"#,##0.00", summarisedBy);
        }

        public PivotTableValue(string columnName, string format, ValueSummarisedBy summarisedBy)
        {
            Init(columnName, format, summarisedBy);
        }

        private void Init(string columnName, string format, ValueSummarisedBy summarisedBy)
        {
            ColumnName = columnName;
            Format = format;
            SummarisedBy = summarisedBy;
        }
    }
}
