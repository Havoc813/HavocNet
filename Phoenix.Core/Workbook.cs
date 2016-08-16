using System.Collections;
using System.Linq;

namespace Phoenix.Core
{
    public class Sheet
    {
        public readonly ArrayList Columns = new ArrayList();
        public string SheetName { get; set; }
        public string TableName { get; set; }

        public string SheetColumnsToSQL()
        {
            return this.Columns.Cast<Column>().Aggregate("", (current, aColumn) => current + (", [" + aColumn.SheetColumnName + "]")).Substring(1);
        }

        public string TableColumnsToSQL()
        {
            return this.Columns.Cast<Column>().Aggregate("", (current, aColumn) => current + (", [" + aColumn.TableColumnName + "]")).Substring(1);
        }

        public string GetParamList()
        {
            var strSQL = "@Param0";

            for (var i = 1; i <= this.Columns.Count - 1; i++)
            {
                strSQL += ", @Param" + i;
            }

            return this.Columns.Count == 0 ? "" : strSQL;
        }
    }

    public class Column
    {
        public string SheetColumnName { get; set; }
        public string TableColumnName { get; set; }
    }
}
