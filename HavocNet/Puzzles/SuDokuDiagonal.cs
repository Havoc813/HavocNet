using System;
using System.Drawing;
using System.Web.UI.WebControls;

namespace HavocNet.Puzzles
{
    public class SuDokuDiagonal : SuDoku
    {
        public SuDokuDiagonal(int[,] initialConditions, string status = "blank") : base(
                new DiagonalRenderEngine(9, 3),
                new DiagonalCheckEngine(9, 3), 
                initialConditions, 
                status
            ) {}
    }

    public class DiagonalCheckEngine : ICheckEngine
    {
        private readonly int[, ,] _checkMatrix;
        private readonly int _size;
        private readonly int _box;

        public DiagonalCheckEngine(int size, int box)
        {
            _checkMatrix = new int[4, size + 1, size + 1];

            _size = size;
            _box = box;
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
            if (i == j) _checkMatrix[3, 0, value - 1] = onOff;
            if (j == 8 - i) _checkMatrix[3, 1, value - 1] = onOff;
        }

        public bool CheckItem(int i, int j, int value)
        {
            return _checkMatrix[0, i, value - 1] == 1 ||
                   _checkMatrix[1, j, value - 1] == 1 ||
                   _checkMatrix[2, BoxValue(i, j), value - 1] == 1 ||
                   (i == j && _checkMatrix[3, 0, value - 1] == 1) ||
                   (j == 8 - i && _checkMatrix[3, 1, value - 1] == 1);
        }

        public string ErrorItem(int i, int j, int value)
        {
            if (_checkMatrix[0, i, value - 1] == 1) return "Error on Column " + (i + 1) + " - too many " + value + "s";
            if (_checkMatrix[1, j, value - 1] == 1) return "Error on Row " + (j + 1) + " - too many " + value + "s";
            if (_checkMatrix[2, BoxValue(i, j), value - 1] == 1) return "Error in Box " + Math.Floor((double)i /_box + 1)+ ", " + Math.Floor((double)j / _box + 1) + " - too many " + value + "s";
            return "Error on Diagonal - too many " + value + "s";
        }

        private int BoxValue(int i, int j)
        {
            return (int)(_box * Math.Floor((double)j / _box) + Math.Floor((double)i / _box));
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
    
    public class DiagonalRenderEngine : IRenderEngine
    {
        private readonly int _size;
        private readonly int _box;

        public DiagonalRenderEngine(int size, int box)
        {
            this._size = size;
            this._box = box;
        }

        public void SetStyle(ref TableCell aCell, int i, int j)
        {
            if (j % _box == 0) aCell.Style["border-left"] = "2px solid black";
            if (i % _box == 0) aCell.Style["border-top"] = "2px solid black";
            if (i == _size - 1) aCell.Style["border-bottom"] = "2px solid black";
            if (j == _size - 1) aCell.Style["border-right"] = "2px solid black";

            if (i != j && i != 8 - j) return;
            aCell.BackColor = Color.LightGray;
            ((TextBox)aCell.Controls[0]).BackColor = Color.LightGray;
        }
    }

    public class DemoDiagonal
    {
        public readonly int[,] DemoConditions = new int[9,9];

        public DemoDiagonal()
        {
            this.DemoConditions[0, 1] = 7;
            this.DemoConditions[0, 3] = 2;
            this.DemoConditions[0, 5] = 8;
            this.DemoConditions[0, 7] = 9;
            this.DemoConditions[1, 0] = 2;
            this.DemoConditions[1, 8] = 3;
            this.DemoConditions[2, 0] = 3;
            this.DemoConditions[2, 1] = 9;
            this.DemoConditions[2, 7] = 5;
            this.DemoConditions[2, 8] = 6;
            this.DemoConditions[3, 2] = 9;
            this.DemoConditions[3, 6] = 5;
            this.DemoConditions[4, 6] = 9;
            this.DemoConditions[4, 4] = 4;
            this.DemoConditions[4, 2] = 1;
            this.DemoConditions[5, 6] = 7;
            this.DemoConditions[5, 2] = 5;
            this.DemoConditions[8, 3] = 4;
            this.DemoConditions[8, 5] = 1;
        }
    }
}
