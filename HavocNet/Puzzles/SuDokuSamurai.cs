using System.Web.UI.WebControls;
using Core;

namespace HavocNet.Puzzles
{
    public class SuDokuSamurai
    {
        private readonly int[,] _initialConditions;
        private string _suDokuStatus;
        private string _error;
        private readonly int[,] _solution;
        private readonly int _size;
        private readonly int[,] _initialConditions2;

        public string SuDokuStatus
        {
            get { return this._suDokuStatus; }
        }

        public string Error
        {
            get { return this._error; }
        }

        public SuDokuSamurai(int[,] initialConditions, string status = "blank")
        {
            this._suDokuStatus = status;
            this._initialConditions = initialConditions;
            this._size = 21;
            this._solution = new int[_size, _size];
            this._initialConditions2 = new int[_size, _size];
        }

        public void Solve()
        {
            for (var i = 0; i <= this._solution.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= this._solution.GetUpperBound(1); j++)
                {
                    this._solution[i, j] = this._initialConditions[i, j];
                    this._initialConditions2[i, j] = this._initialConditions[i, j];
                }
            }

            SolveAndPlace(6, 6, this._initialConditions);

            while (!ValidateMiddle())
            {
                SolveAndPlaceNext(6, 6, this._initialConditions);
            }

            ArrayHelper.PutArrayAt(6, 6, ArrayHelper.GetArrayAt(6, 6, this._solution), this._initialConditions2);

            if (this._suDokuStatus != "errored") SolveAndPlace(0, 0, this._initialConditions2);
            if (this._suDokuStatus != "errored") SolveAndPlace(12, 0, this._initialConditions2);
            if (this._suDokuStatus != "errored") SolveAndPlace(0, 12, this._initialConditions2);
            if (this._suDokuStatus != "errored") SolveAndPlace(12, 12, this._initialConditions2);

            while (this._suDokuStatus == "errored")
            {
                do
                {
                    SolveAndPlaceNext(6, 6, this._initialConditions);
                }
                while (!ValidateMiddle());

                ArrayHelper.PutArrayAt(6, 6, ArrayHelper.GetArrayAt(6, 6, this._solution), this._initialConditions2);

                if (this._suDokuStatus != "errored") SolveAndPlaceNext(0, 0, this._initialConditions2);
                if (this._suDokuStatus != "errored") SolveAndPlaceNext(12, 0, this._initialConditions2);
                if (this._suDokuStatus != "errored") SolveAndPlaceNext(0, 12, this._initialConditions2);
                if (this._suDokuStatus != "errored") SolveAndPlaceNext(12, 12, this._initialConditions2);
            }
            this._suDokuStatus = "solved";
        }

        private void SolveAndPlace(int x, int y, int[,] initialConditions)
        {
            var aSudoku = new SuDokuClassic(ArrayHelper.GetArrayAt(x, y, initialConditions), "solved");
            aSudoku.Solve();
            ArrayHelper.PutArrayAt(x, y, aSudoku._solution, this._solution);
        }

        private void SolveAndPlaceNext(int x, int y, int[,] initialConditions)
        {
            this._suDokuStatus = "blank";

            var aSudoku = new SuDokuClassic(ArrayHelper.GetArrayAt(x, y, initialConditions), "solved");
            aSudoku.SolveNext(ArrayHelper.GetArrayAt(x, y, this._solution));
            ArrayHelper.PutArrayAt(x, y, aSudoku._solution, this._solution);
        }

        private bool ValidateMiddle()
        {
            SetCheckEngine(ArrayHelper.GetArrayAt(0, 0, this._solution));
            if (this._suDokuStatus != "errored") SetCheckEngine(ArrayHelper.GetArrayAt(12, 0, this._solution));
            if (this._suDokuStatus != "errored") SetCheckEngine(ArrayHelper.GetArrayAt(0, 12, this._solution));
            if (this._suDokuStatus != "errored") SetCheckEngine(ArrayHelper.GetArrayAt(12, 12, this._solution));

            return this._suDokuStatus != "errored";
        }

        private void SetCheckEngine(int[,] conditionArray)
        {
            var checkEngine = new CheckEngine(9, 3);

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (conditionArray[j, i] == 0) continue;

                    if (checkEngine.CheckItem(i, j, conditionArray[j, i]))
                    {
                        this._error = checkEngine.ErrorItem(i, j, conditionArray[j, i]);
                        this._suDokuStatus = "errored";
                        return;
                    }
                    checkEngine.SetCheckValue(i, j, conditionArray[j, i], 1);
                }
            }
        }

        public Table Render()
        {
            var aSudoku = new SuDoku(new SamuraiRenderEngine(21, 3), new CheckEngine(21, 3), _initialConditions, this._suDokuStatus)
                {
                    _solution = this._solution
                };

            return aSudoku.Render();
        }
    }

    public class SamuraiRenderEngine : IRenderEngine
    {
        private readonly int _size;
        private readonly int _box;

        public SamuraiRenderEngine(int size, int box)
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

            if (j > 8 && j < 12 && i < 6) aCell.Style["border"] = "0px solid white";
            if (j > 8 && j < 12 && i > 14) aCell.Style["border"] = "0px solid white";
            if (j < 6 && i > 8 && i < 12) aCell.Style["border"] = "0px solid white";
            if (j > 14 && i > 8 && i < 12) aCell.Style["border"] = "0px solid white";

            if (i == 8) aCell.Style["border-bottom"] = "2px solid black";
            if (j == 8) aCell.Style["border-right"] = "2px solid black";
            if (i == 14) aCell.Style["border-bottom"] = "2px solid black";
            if (j == 14) aCell.Style["border-right"] = "2px solid black";
        }
    }

    public class SamuraiDemo
    {
        public readonly int[,] DemoConditions;

        public SamuraiDemo()
        {
            DemoConditions = new int[21,21];

            DemoConditions[0, 5] = 5;
            DemoConditions[0, 6] = 2;
            DemoConditions[0, 7] = 1;
            DemoConditions[0, 8] = 9;
            DemoConditions[1, 3] = 1;
            DemoConditions[1, 4] = 9;
            DemoConditions[2, 2] = 3;
            DemoConditions[3, 1] = 6;
            DemoConditions[4, 1] = 7;
            DemoConditions[5, 0] = 5;
            DemoConditions[6, 0] = 2;
            DemoConditions[7, 0] = 4;
            DemoConditions[8, 0] = 1;
            DemoConditions[3, 7] = 2;
            DemoConditions[3, 8] = 1;
            DemoConditions[4, 5] = 1;
            DemoConditions[4, 6] = 3;
            DemoConditions[5, 4] = 7;
            DemoConditions[6, 4] = 1;
            DemoConditions[7, 3] = 3;
            DemoConditions[8, 3] = 4;
            DemoConditions[6, 8] = 5;
            DemoConditions[7, 7] = 7;
            DemoConditions[8, 6] = 8;
            DemoConditions[0, 12] = 1;
            DemoConditions[0, 13] = 2;
            DemoConditions[0, 14] = 4;
            DemoConditions[0, 15] = 3;
            DemoConditions[1, 16] = 9;
            DemoConditions[1, 17] = 2;
            DemoConditions[2, 18] = 6;
            DemoConditions[3, 19] = 4;
            DemoConditions[4, 19] = 9;
            DemoConditions[5, 20] = 1;
            DemoConditions[6, 20] = 4;
            DemoConditions[7, 20] = 9;
            DemoConditions[8, 20] = 2;
            DemoConditions[3, 12] = 3;
            DemoConditions[3, 13] = 6;
            DemoConditions[4, 14] = 2;
            DemoConditions[4, 15] = 4;
            DemoConditions[5, 16] = 7;
            DemoConditions[6, 16] = 3;
            DemoConditions[7, 17] = 5;
            DemoConditions[8, 17] = 1;
            DemoConditions[6, 12] = 2;
            DemoConditions[7, 13] = 4;
            DemoConditions[8, 14] = 3;
            DemoConditions[12, 0] = 1;
            DemoConditions[13, 0] = 7;
            DemoConditions[14, 0] = 5;
            DemoConditions[15, 0] = 9;
            DemoConditions[16, 1] = 6;
            DemoConditions[17, 1] = 1;
            DemoConditions[18, 2] = 4;
            DemoConditions[19, 3] = 7;;
            DemoConditions[19, 4] = 6;
            DemoConditions[20, 5] = 3;
            DemoConditions[20, 6] = 5;
            DemoConditions[20, 7] = 2;
            DemoConditions[20, 8] = 1;
            DemoConditions[12, 3] = 4;
            DemoConditions[13, 3] = 2;
            DemoConditions[14, 4] = 7;
            DemoConditions[15, 4] = 1;
            DemoConditions[16, 5] = 5;
            DemoConditions[16, 6] = 1;
            DemoConditions[17, 7] = 4;
            DemoConditions[17, 8] = 2;
            DemoConditions[12, 6] = 2;
            DemoConditions[13, 7] = 1;
            DemoConditions[14, 8] = 3;
            DemoConditions[12, 14] = 4;
            DemoConditions[13, 13] = 6;
            DemoConditions[14, 12] = 1;
            DemoConditions[12, 17] = 1;
            DemoConditions[13, 17] = 7;
            DemoConditions[14, 16] = 3;
            DemoConditions[15, 16] = 4;
            DemoConditions[16, 15] = 3;
            DemoConditions[16, 14] = 1;
            DemoConditions[17, 13] = 3;
            DemoConditions[17, 12] = 2;
            DemoConditions[20, 12] = 8;
            DemoConditions[20, 13] = 1;
            DemoConditions[20, 14] = 7;
            DemoConditions[20, 15] = 2;
            DemoConditions[19, 16] = 7;
            DemoConditions[19, 17] = 3;
            DemoConditions[18, 18] = 5;
            DemoConditions[17, 19] = 1;
            DemoConditions[16, 19] = 7;
            DemoConditions[15, 20] = 6;
            DemoConditions[14, 20] = 4;
            DemoConditions[13, 20] = 1;
            DemoConditions[12, 20] = 2;
            DemoConditions[6, 9] = 6;
            DemoConditions[6, 10] = 7;
            DemoConditions[6, 11] = 8;
            DemoConditions[9, 6] = 5;
            DemoConditions[10, 6] = 3;
            DemoConditions[11, 6] = 7;
            DemoConditions[9, 14] = 7;
            DemoConditions[10, 14] = 1;
            DemoConditions[11, 14] = 8;
            DemoConditions[9, 10] = 2;
            DemoConditions[10, 11] = 6;
            DemoConditions[11, 10] = 1;
            DemoConditions[10, 9] = 7;
            DemoConditions[14, 9] = 4;
            DemoConditions[14, 10] = 5;
            DemoConditions[14, 11] = 9;
        }
    }
}
