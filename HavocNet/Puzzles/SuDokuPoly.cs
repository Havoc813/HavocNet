using System;
using System.Drawing;
using System.Web.UI.WebControls;

namespace HavocNet.Puzzles
{
    public class SuDokuPoly : SuDoku
    {
        private readonly int[,] _polyConditions;

        public SuDokuPoly(int[,] polyConditions, int[,] initialConditions, string status = "blank") : base(
                new PolyRenderEngine(9, polyConditions),
                new PolyCheckEngine(9, 3, polyConditions), 
                initialConditions, 
                status
            )
        {
            this._polyConditions = polyConditions;
        }

        public string MakePolyHidden()
        {
            var strHid = "";

            for (var i = 0; i <= _polyConditions.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= _polyConditions.GetUpperBound(1); j++)
                {
                    if (_polyConditions[j, i] == 1) strHid += ";" + j.ToString("00") + i.ToString("00");
                }
            }

            return strHid.Substring(1);
        }
    }

    public class PolyCheckEngine : ICheckEngine
    {
        private readonly int[, ,] _checkMatrix;
        private readonly int _size;
        private readonly int _box;
        private readonly int[,] _polyConditions;

        public PolyCheckEngine(int size, int box, int[,] polyConditions)
        {
            _checkMatrix = new int[4, size + 1, size + 1];

            _size = size;
            _box = box;
            _polyConditions = polyConditions;
        }

        public int Size()
        {
            return _size;
        }

        public void SetCheckValue(int i, int j, int value, int onOff)
        {
            _checkMatrix[0, i, value - 1] = onOff;
            _checkMatrix[1, j, value - 1] = onOff;
            _checkMatrix[2, _polyConditions[j,i], value - 1] = onOff;
        }

        public bool CheckItem(int i, int j, int value)
        {
            return _checkMatrix[0, i, value - 1] == 1 ||
                   _checkMatrix[1, j, value - 1] == 1 ||
                   _checkMatrix[2, _polyConditions[j, i], value - 1] == 1;
        }

        public string ErrorItem(int i, int j, int value)
        {
            if (_checkMatrix[0, i, value - 1] == 1) return "Error on Column " + (i + 1) + " - too many " + value + "s";
            if (_checkMatrix[1, j, value - 1] == 1) return "Error on Row " + (j + 1) + " - too many " + value + "s";
            return "Error in Box " + Math.Floor((double)i / _box + 1) + ", " + Math.Floor((double)j / _box + 1) + " - too many " + value + "s";
        }

        public int NextValue(int i, int j, int value)
        {
            return value + 1;
        }

        public int Reset(int i, int j)
        {
            return 0;
        }
    }
    
    public class PolyRenderEngine : IRenderEngine
    {
        private readonly int _size;
        private readonly int[,] _polyConditions;

        public PolyRenderEngine(int size, int[,] polyConditions)
        {
            this._size = size;
            _polyConditions = polyConditions;
        }

        public void SetStyle(ref TableCell aCell, int i, int j)
        {
            if (j == 0) aCell.Style["border-left"] = "2px solid black";
            if (i == 0) aCell.Style["border-top"] = "2px solid black";
            if (i == _size - 1) aCell.Style["border-bottom"] = "2px solid black";
            if (j == _size - 1) aCell.Style["border-right"] = "2px solid black";

            aCell.BackColor = GetPolyColour(_polyConditions[j, i]);
            ((TextBox)aCell.Controls[0]).BackColor = GetPolyColour(_polyConditions[j, i]);
        }

        private static Color GetPolyColour(int polyCondition)
        {
            switch (polyCondition)
            {
                case 1:
                    return Color.LawnGreen;
                case 2:
                    return Color.Crimson;
                case 3:
                    return Color.SteelBlue;
                case 4:
                    return Color.Yellow;
                case 5:
                    return Color.Violet;
                case 6:
                    return Color.OrangeRed;
                case 7:
                    return Color.Cyan;
                case 8:
                    return Color.Pink;
                case 9:
                    return Color.Goldenrod;
                default:
                    return Color.White;
            }
        }
    }

    public class PolyDemo
    {
        public readonly int[,] PolyConditions = new int[9, 9];
        public readonly int[,] DemoConditions = new int[9, 9];

        public PolyDemo()
        {
            this.DemoConditions[0, 2] = 6;
            this.DemoConditions[0, 4] = 7;
            this.DemoConditions[0, 6] = 3;
            this.DemoConditions[1, 3] = 2;
            this.DemoConditions[1, 5] = 1;
            this.DemoConditions[2, 0] = 8;
            this.DemoConditions[2, 2] = 5;
            this.DemoConditions[2, 3] = 6;
            this.DemoConditions[2, 5] = 3;
            this.DemoConditions[2, 6] = 1;
            this.DemoConditions[2, 8] = 7;
            this.DemoConditions[3, 1] = 6;
            this.DemoConditions[3, 2] = 7;
            this.DemoConditions[3, 6] = 8;
            this.DemoConditions[3, 7] = 5;
            this.DemoConditions[4, 0] = 5;
            this.DemoConditions[4, 4] = 6;
            this.DemoConditions[4, 8] = 8;
            this.DemoConditions[5, 1] = 3;
            this.DemoConditions[5, 2] = 9;
            this.DemoConditions[5, 6] = 4;
            this.DemoConditions[5, 7] = 6;
            this.DemoConditions[6, 0] = 9;
            this.DemoConditions[6, 2] = 3;
            this.DemoConditions[6, 3] = 8;
            this.DemoConditions[6, 5] = 6;
            this.DemoConditions[6, 6] = 7;
            this.DemoConditions[6, 8] = 4;
            this.DemoConditions[7, 3] = 7;
            this.DemoConditions[7, 5] = 9;
            this.DemoConditions[8, 2] = 8;
            this.DemoConditions[8, 4] = 4;
            this.DemoConditions[8, 6] = 6;

            this.PolyConditions[0, 0] = 1;
            this.PolyConditions[0, 1] = 1;
            this.PolyConditions[0, 2] = 2;
            this.PolyConditions[0, 3] = 2;
            this.PolyConditions[0, 4] = 2;
            this.PolyConditions[0, 5] = 2;
            this.PolyConditions[0, 6] = 2;
            this.PolyConditions[0, 7] = 3;
            this.PolyConditions[0, 8] = 3;
            this.PolyConditions[1, 0] = 1;
            this.PolyConditions[1, 1] = 1;
            this.PolyConditions[1, 2] = 1;
            this.PolyConditions[1, 3] = 2;
            this.PolyConditions[1, 4] = 2;
            this.PolyConditions[1, 5] = 2;
            this.PolyConditions[1, 6] = 3;
            this.PolyConditions[1, 7] = 3;
            this.PolyConditions[1, 8] = 3;
            this.PolyConditions[2, 0] = 4;
            this.PolyConditions[2, 1] = 1;
            this.PolyConditions[2, 2] = 1;
            this.PolyConditions[2, 3] = 1;
            this.PolyConditions[2, 4] = 2;
            this.PolyConditions[2, 5] = 3;
            this.PolyConditions[2, 6] = 3;
            this.PolyConditions[2, 7] = 3;
            this.PolyConditions[2, 8] = 5;
                
            this.PolyConditions[3, 0] = 4;
            this.PolyConditions[3, 1] = 4;
            this.PolyConditions[3, 2] = 1;
            this.PolyConditions[3, 3] = 6;
            this.PolyConditions[3, 4] = 6;
            this.PolyConditions[3, 5] = 6;
            this.PolyConditions[3, 6] = 3;
            this.PolyConditions[3, 7] = 5;
            this.PolyConditions[3, 8] = 5;
            this.PolyConditions[4, 0] = 4;
            this.PolyConditions[4, 1] = 4;
            this.PolyConditions[4, 2] = 4;
            this.PolyConditions[4, 3] = 6;
            this.PolyConditions[4, 4] = 6;
            this.PolyConditions[4, 5] = 6;
            this.PolyConditions[4, 6] = 5;
            this.PolyConditions[4, 7] = 5;
            this.PolyConditions[4, 8] = 5;
            this.PolyConditions[5, 0] = 4;
            this.PolyConditions[5, 1] = 4;
            this.PolyConditions[5, 2] = 7;
            this.PolyConditions[5, 3] = 6;
            this.PolyConditions[5, 4] = 6;
            this.PolyConditions[5, 5] = 6;
            this.PolyConditions[5, 6] = 8;
            this.PolyConditions[5, 7] = 5;
            this.PolyConditions[5, 8] = 5;

            this.PolyConditions[6, 0] = 4;
            this.PolyConditions[6, 1] = 7;
            this.PolyConditions[6, 2] = 7;
            this.PolyConditions[6, 3] = 7;
            this.PolyConditions[6, 4] = 9;
            this.PolyConditions[6, 5] = 8;
            this.PolyConditions[6, 6] = 8;
            this.PolyConditions[6, 7] = 8;
            this.PolyConditions[6, 8] = 5;
            this.PolyConditions[7, 0] = 7;
            this.PolyConditions[7, 1] = 7;
            this.PolyConditions[7, 2] = 7;
            this.PolyConditions[7, 3] = 9;
            this.PolyConditions[7, 4] = 9;
            this.PolyConditions[7, 5] = 9;
            this.PolyConditions[7, 6] = 8;
            this.PolyConditions[7, 7] = 8;
            this.PolyConditions[7, 8] = 8;
            this.PolyConditions[8, 0] = 7;
            this.PolyConditions[8, 1] = 7;
            this.PolyConditions[8, 2] = 9;
            this.PolyConditions[8, 3] = 9;
            this.PolyConditions[8, 4] = 9;
            this.PolyConditions[8, 5] = 9;
            this.PolyConditions[8, 6] = 9;
            this.PolyConditions[8, 7] = 8;
            this.PolyConditions[8, 8] = 8;
        }
    }

    public class ChainDemo
    {
        public readonly int[,] PolyConditions = new int[9, 9];
        public readonly int[,] DemoConditions = new int[9, 9];

        public ChainDemo()
        {

            this.DemoConditions[0, 2] = 7;
            this.DemoConditions[0, 6] = 6;
            this.DemoConditions[1, 0] = 9;
            this.DemoConditions[1, 4] = 7;
            this.DemoConditions[1, 8] = 1;
            this.DemoConditions[2, 1] = 8;
            this.DemoConditions[2, 3] = 3;
            this.DemoConditions[2, 5] = 4;
            this.DemoConditions[2, 7] = 1;
            this.DemoConditions[3, 2] = 6;
            this.DemoConditions[3, 6] = 4;
            this.DemoConditions[4, 1] = 5;
            this.DemoConditions[4, 7] = 7;
            this.DemoConditions[5, 2] = 8;
            this.DemoConditions[5, 6] = 3;
            this.DemoConditions[6, 1] = 6;
            this.DemoConditions[6, 3] = 9;
            this.DemoConditions[6, 5] = 8;
            this.DemoConditions[6, 7] = 3;
            this.DemoConditions[7, 0] = 3;
            this.DemoConditions[7, 4] = 5;
            this.DemoConditions[7, 8] = 8;
            this.DemoConditions[8, 2] = 1;
            this.DemoConditions[8, 6] = 7;

            this.PolyConditions[0, 0] = 1;
            this.PolyConditions[0, 1] = 1;
            this.PolyConditions[0, 2] = 2;
            this.PolyConditions[0, 3] = 2;
            this.PolyConditions[0, 4] = 2;
            this.PolyConditions[0, 5] = 3;
            this.PolyConditions[0, 6] = 3;
            this.PolyConditions[0, 7] = 3;
            this.PolyConditions[0, 8] = 3;
            this.PolyConditions[1, 0] = 1;
            this.PolyConditions[1, 1] = 2;
            this.PolyConditions[1, 2] = 4;
            this.PolyConditions[1, 3] = 4;
            this.PolyConditions[1, 4] = 4;
            this.PolyConditions[1, 5] = 2;
            this.PolyConditions[1, 6] = 3;
            this.PolyConditions[1, 7] = 3;
            this.PolyConditions[1, 8] = 5;
            this.PolyConditions[2, 0] = 1;
            this.PolyConditions[2, 1] = 1;
            this.PolyConditions[2, 2] = 4;
            this.PolyConditions[2, 3] = 4;
            this.PolyConditions[2, 4] = 2;
            this.PolyConditions[2, 5] = 2;
            this.PolyConditions[2, 6] = 3;
            this.PolyConditions[2, 7] = 3;
            this.PolyConditions[2, 8] = 5;

            this.PolyConditions[3, 0] = 4;
            this.PolyConditions[3, 1] = 4;
            this.PolyConditions[3, 2] = 1;
            this.PolyConditions[3, 3] = 2;
            this.PolyConditions[3, 4] = 2;
            this.PolyConditions[3, 5] = 3;
            this.PolyConditions[3, 6] = 6;
            this.PolyConditions[3, 7] = 5;
            this.PolyConditions[3, 8] = 5;
            this.PolyConditions[4, 0] = 4;
            this.PolyConditions[4, 1] = 7;
            this.PolyConditions[4, 2] = 1;
            this.PolyConditions[4, 3] = 7;
            this.PolyConditions[4, 4] = 7;
            this.PolyConditions[4, 5] = 6;
            this.PolyConditions[4, 6] = 6;
            this.PolyConditions[4, 7] = 6;
            this.PolyConditions[4, 8] = 5;
            this.PolyConditions[5, 0] = 4;
            this.PolyConditions[5, 1] = 1;
            this.PolyConditions[5, 2] = 7;
            this.PolyConditions[5, 3] = 7;
            this.PolyConditions[5, 4] = 6;
            this.PolyConditions[5, 5] = 6;
            this.PolyConditions[5, 6] = 8;
            this.PolyConditions[5, 7] = 5;
            this.PolyConditions[5, 8] = 6;

            this.PolyConditions[6, 0] = 1;
            this.PolyConditions[6, 1] = 7;
            this.PolyConditions[6, 2] = 9;
            this.PolyConditions[6, 3] = 9;
            this.PolyConditions[6, 4] = 8;
            this.PolyConditions[6, 5] = 8;
            this.PolyConditions[6, 6] = 6;
            this.PolyConditions[6, 7] = 6;
            this.PolyConditions[6, 8] = 5;
            this.PolyConditions[7, 0] = 7;
            this.PolyConditions[7, 1] = 9;
            this.PolyConditions[7, 2] = 9;
            this.PolyConditions[7, 3] = 9;
            this.PolyConditions[7, 4] = 9;
            this.PolyConditions[7, 5] = 8;
            this.PolyConditions[7, 6] = 8;

            this.PolyConditions[7, 7] = 8;
            this.PolyConditions[7, 8] = 5;
            this.PolyConditions[8, 0] = 7;
            this.PolyConditions[8, 1] = 7;
            this.PolyConditions[8, 2] = 9;
            this.PolyConditions[8, 3] = 9;
            this.PolyConditions[8, 4] = 9;
            this.PolyConditions[8, 5] = 8;
            this.PolyConditions[8, 6] = 8;
            this.PolyConditions[8, 7] = 8;
            this.PolyConditions[8, 8] = 5;
        }
    }
}
