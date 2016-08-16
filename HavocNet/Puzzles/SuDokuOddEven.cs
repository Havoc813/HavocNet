using System;
using System.Drawing;
using System.Web.UI.WebControls;

namespace HavocNet.Puzzles
{
    public class SuDokuOddEven : SuDoku
    {
        private readonly int[,] _oddEvenConditions;

        public SuDokuOddEven(int[,] initialConditions, int[,] oddEvenConditions, string status = "blank") : base(
            new OddEvenRenderEngine(9, 3, oddEvenConditions),
            new OddEvenCheckEngine(9, 3, oddEvenConditions), 
            initialConditions, 
            status
            )
        {
            this._oddEvenConditions = oddEvenConditions;
        }

        public string MakeOddEvenHidden()
        {
            var strHid = "";
            
            for (var i = 0; i <= _oddEvenConditions.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= _oddEvenConditions.GetUpperBound(1); j++)
                {
                    if (_oddEvenConditions[i,j] == 1) strHid += ";" + j.ToString("00") + i.ToString("00");
                }
            }

            return strHid.Substring(1);
        }
    }

    public class OddEvenCheckEngine : ICheckEngine
    {
        private readonly int[, ,] _checkMatrix;
        private readonly int _size;
        private readonly int _box;
        private readonly int[,] _oddEvenConditions;

        public OddEvenCheckEngine(int size, int box, int[,] oddEvenConditions)
        {
            _checkMatrix = new int[4, size + 1, size + 1];

            _size = size;
            _box = box;
            _oddEvenConditions = oddEvenConditions;
        }

        public int Size()
        {
            return _size;
        }

        public void SetCheckValue(int i, int j, int value, int onOff)
        {
            _checkMatrix[0, i, value - 1] = onOff;
            _checkMatrix[1, j, value - 1] = onOff;
            _checkMatrix[2, BoxValue(i, j), value - 1] = onOff;
        }

        public bool CheckItem(int i, int j, int value)
        {
            return _checkMatrix[0, i, value - 1] == 1 ||
                   _checkMatrix[1, j, value - 1] == 1 ||
                   _checkMatrix[2, BoxValue(i, j), value - 1] == 1;
        }

        public string ErrorItem(int i, int j, int value)
        {
            if (_checkMatrix[0, i, value - 1] == 1) return "Error on Column " + (i + 1) + " - too many " + value + "s";
            if (_checkMatrix[1, j, value - 1] == 1) return "Error on Row " + (j + 1) + " - too many " + value + "s";
            if (_checkMatrix[2, BoxValue(i, j), value - 1] == 1) return "Error in Box " + Math.Floor((double)(i / _box) + 1) + ", " + Math.Floor((double)(j / _box) + 1) + " - too many " + value + "s";
            return "";
        }

        private int BoxValue(int i, int j)
        {
            return (int)(_box * Math.Floor((double)j / _box) + Math.Floor((double)i / _box));
        }

        public int NextValue(int i, int j, int value)
        {
            return (value == 0 && _oddEvenConditions[j, i] == 1) ? 1 : value + 2;
        }
        
        public int Reset(int i, int j)
        {
            return 0;
        }
    }
    
    public class OddEvenRenderEngine : IRenderEngine
    {
        private readonly int _size;
        private readonly int _box;
        private readonly int[,] _oddEvenConditions;

        public OddEvenRenderEngine(int size, int box, int[,] oddEvenConditions)
        {
            this._size = size;
            this._box = box;
            this._oddEvenConditions = oddEvenConditions;
        }

        public void SetStyle(ref TableCell aCell, int i, int j)
        {
            if (j % _box == 0) aCell.Style["border-left"] = "2px solid black";
            if (i % _box == 0) aCell.Style["border-top"] = "2px solid black";
            if (i == _size - 1) aCell.Style["border-bottom"] = "2px solid black";
            if (j == _size - 1) aCell.Style["border-right"] = "2px solid black";

            if (_oddEvenConditions[i, j] != 1) return;
            aCell.BackColor = Color.LightGray;
            ((TextBox)aCell.Controls[0]).BackColor = Color.LightGray;
        }
    }

    public class OddEvenDemo
    {
        public readonly int[,] OddEvenConditions = new int[9, 9];
        public readonly int[,] DemoConditions = new int[9, 9];

        public OddEvenDemo()
        {
            this.DemoConditions[0, 1] = 2;
            this.DemoConditions[1, 2] = 5;
            this.DemoConditions[1, 7] = 7;
            this.DemoConditions[2, 0] = 7;
            this.DemoConditions[2, 2] = 9;
            this.DemoConditions[4, 2] = 6;
            this.DemoConditions[5, 5] = 7;
            this.DemoConditions[5, 7] = 8;
            this.DemoConditions[5, 8] = 3;
            this.DemoConditions[7, 0] = 4;
            this.DemoConditions[7, 3] = 5;
            this.DemoConditions[8, 5] = 4;
            this.DemoConditions[8, 7] = 3;

            this.OddEvenConditions[0, 4] = 1;
            this.OddEvenConditions[0, 5] = 1;
            this.OddEvenConditions[0, 6] = 1;
            this.OddEvenConditions[0, 7] = 1;
            this.OddEvenConditions[0, 8] = 1;
            this.OddEvenConditions[1, 0] = 1;
            this.OddEvenConditions[1, 2] = 1;
            this.OddEvenConditions[1, 3] = 1;
            this.OddEvenConditions[1, 5] = 1;
            this.OddEvenConditions[1, 7] = 1;
            this.OddEvenConditions[2, 0] = 1;
            this.OddEvenConditions[2, 1] = 1;
            this.OddEvenConditions[2, 2] = 1;
            this.OddEvenConditions[2, 4] = 1;
            this.OddEvenConditions[2, 6] = 1;

            this.OddEvenConditions[3, 2] = 1;
            this.OddEvenConditions[3, 5] = 1;
            this.OddEvenConditions[3, 6] = 1;
            this.OddEvenConditions[3, 7] = 1;
            this.OddEvenConditions[3, 8] = 1;
            this.OddEvenConditions[4, 0] = 1;
            this.OddEvenConditions[4, 1] = 1;
            this.OddEvenConditions[4, 3] = 1;
            this.OddEvenConditions[4, 4] = 1;
            this.OddEvenConditions[4, 6] = 1;
            this.OddEvenConditions[5, 1] = 1;
            this.OddEvenConditions[5, 2] = 1;
            this.OddEvenConditions[5, 3] = 1;
            this.OddEvenConditions[5, 5] = 1;
            this.OddEvenConditions[5, 8] = 1;

            this.OddEvenConditions[6, 0] = 1;
            this.OddEvenConditions[6, 1] = 1;
            this.OddEvenConditions[6, 2] = 1;
            this.OddEvenConditions[6, 3] = 1;
            this.OddEvenConditions[6, 4] = 1;
            this.OddEvenConditions[7, 3] = 1;
            this.OddEvenConditions[7, 4] = 1;
            this.OddEvenConditions[7, 5] = 1;
            this.OddEvenConditions[7, 7] = 1;
            this.OddEvenConditions[7, 8] = 1;
            this.OddEvenConditions[8, 0] = 1;
            this.OddEvenConditions[8, 1] = 1;
            this.OddEvenConditions[8, 6] = 1;
            this.OddEvenConditions[8, 7] = 1;
            this.OddEvenConditions[8, 8] = 1;
        }
    }
}
