using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace HavocNet.Puzzles
{
    public class Futoshiki : SuDoku
    {
        public Futoshiki(string gtltConditions, int[,] initialConditions, int order, bool isSuDoku,
                         string status = "blank") : base(
                             new FutoshikiRenderEngine(order, gtltConditions),
                             new FutoshikiCheckEngine(order, isSuDoku, gtltConditions),
                             initialConditions,
                             status
                             )
        {}

        public static Dictionary<string, FutoshikiCondition> MakeConditions(string gtltstring)
        {
            var conditions = new Dictionary<string, FutoshikiCondition>();

            foreach (var sCondition in gtltstring.Split(','))
            {
                if (sCondition == "") continue;

                var direction = sCondition.Split('|')[0];
                var i = sCondition.Split('|')[1].Split(';')[0];
                var j = sCondition.Split('|')[1].Split(';')[1];
                var jnew = direction == "H" ? (int.Parse(j) + 1) + "" : j;
                var inew = direction == "V" ? (int.Parse(i) + 1) + "" : i;

                if (!conditions.ContainsKey(i + j)) conditions.Add(i + j, new FutoshikiCondition(int.Parse(i), int.Parse(j)));
                if (!conditions.ContainsKey(inew + jnew)) conditions.Add(inew + jnew, new FutoshikiCondition(int.Parse(inew), int.Parse(jnew)));

                if (sCondition.Split('|')[2] != "1")
                {
                    conditions[i + j].GreaterThan.Add(new[] {int.Parse(inew), int.Parse(jnew)});
                    conditions[inew + jnew].LessThan.Add(new[] { int.Parse(i), int.Parse(j) });
                }
                else
                {
                    conditions[i + j].LessThan.Add(new[] { int.Parse(inew), int.Parse(jnew) });
                    conditions[inew + jnew].GreaterThan.Add(new[] { int.Parse(i), int.Parse(j) });
                }
            }

            return conditions;
        }
    }

    public class FutoshikiCondition
    {
        public readonly ArrayList GreaterThan;
        public readonly ArrayList LessThan;
        private readonly int _i;
        private readonly int _j;

        public FutoshikiCondition(int i, int j)
        {
            this._i = i;
            this._j = j;
            this.GreaterThan = new ArrayList();
            this.LessThan = new ArrayList();
        }

        public bool CheckConditions(int[,] solution, int value)
        {
            foreach (int[] aIJPair in GreaterThan)
            {
                if (solution[aIJPair[0], aIJPair[1]] != 0)
                {
                    if (value <= solution[aIJPair[0], aIJPair[1]]) return true;
                }
            }
            foreach (int[] aIJPair in LessThan)
            {
                if (solution[aIJPair[0], aIJPair[1]] != 0)
                {
                    if (value >= solution[aIJPair[0], aIJPair[1]]) return true;
                }
            }            
            return false;
        }

        public bool ContainsIJPairGreater(int i, int j)
        {
            foreach (int[] aIJPair in GreaterThan)
            {
                if (aIJPair[0] == i && aIJPair[1] == j) return true;
            }
            return false;
        }

        public bool ContainsIJPairLess(int i, int j)
        {
            foreach (int[] aIJPair in LessThan)
            {
                if (aIJPair[0] == i && aIJPair[1] == j) return true;
            }
            return false;
        }
    }

    public class FutoshikiRenderEngine : IRenderEngine
    {
        private readonly int _size;
        private Dictionary<string, FutoshikiCondition> _conditions;

        public FutoshikiRenderEngine(int size, string gtltConditions)
        {
            _size = size;

             _conditions = Futoshiki.MakeConditions(gtltConditions);
        }

        public void SetStyle(ref TableCell aCell, int i, int j)
        {
            aCell.BorderWidth = 0;
            aCell.Style["padding"] = "0 0 0 0";

            var aTable = new Table();
            var aRow = new TableRow();
            var aRow2 = new TableRow();
                
            var horizImage = new ImageButton
            {
                ImageUrl = GetImageURL(i,j,"Horiz"),
                OnClientClick = "javascript:return ChangeImage(this, 'Horiz');",
                ID = "H" + i + j
            };

            var vertImage = new ImageButton
            {
                ImageUrl = GetImageURL(i, j, "Vert"),
                OnClientClick = "javascript:return ChangeImage(this, 'Vert');",
                ID = "V" + i + j
            };

            aRow.Cells.Add(MakeCell(aCell.Controls[0], "0 0 0 0", "Solid 1px black", aCell.ID));
            if (j != _size-1) aRow.Cells.Add(MakeCell(horizImage,"0 0 0 0", "0px", aCell.ID + "horiz"));

            aRow2.Cells.Add(MakeCell(vertImage, "0 0 0 2px", "0px", aCell.ID + "vert"));
            if (j != _size-1) aRow2.Cells.Add(new TableCell());

            aTable.Rows.Add(aRow);
            if(i != _size-1) aTable.Rows.Add(aRow2);

            aCell.ID += "parent";
            aCell.Controls.Clear();
            aCell.Height = 10;
            aCell.Controls.Add(aTable);
        }

        private string GetImageURL(int i, int j, string direction)
        {
            var inew = direction == "Vert" ? i + 1 : i;
            var jnew = direction == "Horiz" ? j + 1 : j;
            
            if (_conditions.ContainsKey(i + "" + j))
            {
                if (_conditions[i + "" + j].ContainsIJPairGreater(inew, jnew)) return "~/App_Themes/HavocNet/Images/Puzzles/" + direction + "Greater.gif";
                if (_conditions[i + "" + j].ContainsIJPairLess(inew, jnew)) return "~/App_Themes/HavocNet/Images/Puzzles/" + direction + "Lesser.gif";    
            }
            
            return "~/App_Themes/HavocNet/Images/Puzzles/None.gif";
        }

        private TableCell MakeCell(Control ctrl, string padding, string border, string cellID)
        {
            var aCell = new TableCell {ID = cellID};
            aCell.Style["padding"] = padding;
            aCell.Controls.Add(ctrl);
            aCell.Style["border"] = border;
            aCell.HorizontalAlign = HorizontalAlign.Center;
            aCell.VerticalAlign = VerticalAlign.Middle;

            return aCell;
        }
    }

    public class FutoshikiCheckEngine : ICheckEngine
    {
        private readonly int[, ,] _checkMatrix = new int[2,9,2];
        private readonly int _size;
        private readonly bool _isSuDoku;
        private Dictionary<string, FutoshikiCondition> _conditions;
        private readonly int[,] _solution;

        public FutoshikiCheckEngine(int size, bool isSuDoku, string gtltConditions)
        {
            _size = size;
            _isSuDoku = isSuDoku;
            _solution = new int[size,size];
            _checkMatrix = new int[3,size + 1,size + 1];
            _conditions = Futoshiki.MakeConditions(gtltConditions);
        }

        public int Size()
        {
            return _size;
        }

        public void SetCheckValue(int i, int j, int value, int onOff)
        {
            _checkMatrix[0, i, value - 1] = onOff;
            _checkMatrix[1, j, value - 1] = onOff;
            if(_isSuDoku) _checkMatrix[2, BoxValue(i, j), value - 1] = onOff;

            _solution[j, i] = value;
        }

        public bool CheckItem(int i, int j, int value)
        {
            var test = (_checkMatrix[0, i, value - 1] == 1);

            if (!test) test = (_checkMatrix[1, j, value - 1] == 1);
            if (!test && _isSuDoku) test = (_checkMatrix[2, BoxValue(i, j), value - 1] == 1);
            if (!test && _conditions.ContainsKey(j + "" + i)) test = _conditions[j + "" + i].CheckConditions(_solution,value);
            return test;
        }

        public string ErrorItem(int i, int j, int value)
        {
            if (_checkMatrix[0, i, value - 1] == 1) return "Error on Column " + (i + 1) + " - too many " + HexHelper.MyHex(value) + "s";
            if (_checkMatrix[1, j, value - 1] == 1) return "Error on Row " + (j + 1) + " - too many " + HexHelper.MyHex(value) + "s";
            if (_isSuDoku) return "Error in Box " + Math.Floor((double)i / 3 + 1) + ", " + Math.Floor((double)j / 3 + 1) + " - too many " + HexHelper.MyHex(value) + "s";
            return "Unknown Error";
        }

        public int NextValue(int i, int j, int value)
        {
            return value + 1;
        }

        public int Reset(int i, int j)
        {
            _solution[j, i] = 0;
            return 0;
        }

        private int BoxValue(int i, int j)
        {
            return (int)(3 * Math.Floor((double)j / 3) + Math.Floor((double)i / 3));
        }
    }

    public class FutoshikiDemo
    {
        public readonly int[,] DemoConditions = new int[9, 9];

        public FutoshikiDemo()
        {
            this.DemoConditions[0, 0] = 4;
            this.DemoConditions[1, 0] = 7;
            this.DemoConditions[2, 0] = 6;
            this.DemoConditions[3, 0] = 2;
            this.DemoConditions[4, 0] = 1;
            this.DemoConditions[5, 5] = 2;
            this.DemoConditions[2, 6] = 1;
        }

        public string GTLTConditions()
        {
            return "H|0;4|1,V|0;6|1,V|0;4|2,H|1;3|1,H|1;1|1,V|1;3|2,H|2;3|2,V|2;4|1,V|2;5|1,V|3;3|1,H|4;2|2,V|4;2|1,V|4;3|1,V|4;4|1,V|5;4|2,H|4;5|1,H|3;5|1,H|5;2|2";
        }
    }
}
