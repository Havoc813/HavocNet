using System.Collections.Generic;

namespace Phoenix.Core.Tables
{
    public class FSTableColumns
    {
        public List<FSColumn> Items { get; set; }

        public FSTableColumns()
        {
            Items = new List<FSColumn>();
        }

        public int IndexOfName(string itemName)
        {
            for (var colIndex = 0; colIndex < Items.Count; colIndex++)
            {
                if (Items[colIndex].Text == itemName)
                    return colIndex;
            }
            return -1;
        }
    }
}
