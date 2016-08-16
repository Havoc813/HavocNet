using System;
using System.Web.UI.WebControls;
using Core;

namespace HavocNet.Puzzles
{
    public class SuDokuClassic : SuDoku
    {
        public SuDokuClassic(int[,] initialConditions, string status = "blank")
            : base(
                new RenderEngine(9, 3),
                new CheckEngine(9, 3), 
                initialConditions, 
                status
            ) {}
    }

    public class CheckEngine : ICheckEngine
    {
        private readonly int[,,] _checkMatrix;
        private readonly int _size;
        private readonly int _box;

        public CheckEngine(int size, int box)
        {
            _checkMatrix = new int[3, size + 1, size + 1];

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
        }

        public bool CheckItem(int i, int j, int value)
        {
            return _checkMatrix[0, i, value - 1] == 1 ||
                   _checkMatrix[1, j, value - 1] == 1 ||
                   _checkMatrix[2, BoxValue(i, j), value - 1] == 1;
        }

        public string ErrorItem(int i, int j, int value)
        {
            if (_checkMatrix[0, i, value - 1] == 1) return "Error on Column " + (i + 1) + " - too many " + HexHelper.MyHex(value) + "s";
            if (_checkMatrix[1, j, value - 1] == 1) return "Error on Row " + (j + 1) + " - too many " + HexHelper.MyHex(value) + "s";
            return "Error in Box " + Math.Floor((double)i / _box + 1) + ", " + Math.Floor((double)j / _box + 1) + " - too many " + HexHelper.MyHex(value) + "s";
        }

        private int BoxValue(int i, int j)
        {
            return (int) (_box * Math.Floor((double) j/_box) + Math.Floor((double) i/_box));
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

    public class RenderEngine : IRenderEngine
    {
        private readonly int _size;
        private readonly int _box;

        public RenderEngine(int size, int box)
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
        }
    }

    public class ClassicDemo
    {
        public readonly int[,] DemoConditions = new int[9, 9];

        public ClassicDemo()
        {
            this.DemoConditions[0, 1] = 4;
            this.DemoConditions[0, 2] = 7;
            this.DemoConditions[0, 6] = 9;
            this.DemoConditions[0, 7] = 3;
            this.DemoConditions[1, 5] = 3;
            this.DemoConditions[2, 0] = 3;
            this.DemoConditions[2, 4] = 7;
            this.DemoConditions[2, 7] = 4;
            this.DemoConditions[3, 1] = 3;
            this.DemoConditions[3, 2] = 2;
            this.DemoConditions[3, 3] = 5;
            this.DemoConditions[3, 4] = 1;
            this.DemoConditions[4, 0] = 6;
            this.DemoConditions[4, 1] = 8;
            this.DemoConditions[4, 7] = 2;
            this.DemoConditions[4, 8] = 1;
            this.DemoConditions[5, 4] = 2;
            this.DemoConditions[5, 5] = 6;
            this.DemoConditions[5, 6] = 8;
            this.DemoConditions[5, 7] = 7;
            this.DemoConditions[6, 1] = 5;
            this.DemoConditions[6, 4] = 8;
            this.DemoConditions[6, 8] = 7;
            this.DemoConditions[7, 3] = 2;
            this.DemoConditions[8, 1] = 2;
            this.DemoConditions[8, 2] = 4;
            this.DemoConditions[8, 6] = 3;
            this.DemoConditions[8, 7] = 5;
        }
    }
}
