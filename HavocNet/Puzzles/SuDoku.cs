using System;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using Core;

namespace HavocNet.Puzzles
{
    public class SuDoku
    {
        private string _suDokuStatus;
        private string _error;
        private readonly int[,] _initialConditions;
        public int[,] _solution;
        private readonly int _size;
        private readonly ICheckEngine _checkEngine;
        private readonly IRenderEngine _renderEngine;

        public string SuDokuStatus
        {
            get { return this._suDokuStatus; }
        }

        public string Error
        { 
            get { return this._error; } 
        }

        public SuDoku(IRenderEngine renderEngine, ICheckEngine checkEngine, int[, ] initialConditions, string status = "blank")
        {
            this._suDokuStatus = status;
            this._size = checkEngine.Size();
            this._initialConditions = initialConditions;
            this._solution = new int[_size, _size];

            this._checkEngine = checkEngine;
            this._renderEngine = renderEngine;
        }

        public Table Render()
        {
            var aTable = new Table { ID = "aPuzzle" };
            aTable.Style["table-layout"] = "fixed";
            var arr = this._suDokuStatus == "solved" ? this._solution : this._initialConditions;

            for (var i = 0; i < _size; i++)
            {
                var aRow = new TableRow();

                for (var j = 0; j < _size; j++)
                {
                    var aCell = new TableCell { CssClass = "sudokuCell", ID = "cell" + j.ToString("00") + i.ToString("00") };
                    var txt = new TextBox
                    {
                        CssClass = "sudokuTextBox",
                        ID = "txt" + j.ToString("00") + i.ToString("00"),
                        ReadOnly = this._suDokuStatus == "solved",
                        Text = arr[i, j] != 0 ? HexHelper.MyHex(arr[i, j]) : ""
                    };
                    txt.Attributes["onkeydown"] = "javascript:return aSudokuKeys.OnKeyPress(this);";
                    aCell.Controls.Add(txt);

                    _renderEngine.SetStyle(ref aCell, i, j);

                    if (this._initialConditions[i, j] != 0 && this.SuDokuStatus != "errored")
                        txt.ForeColor = Color.CornflowerBlue;

                    aRow.Cells.Add(aCell);
                }
                aTable.Rows.Add(aRow);
            }

            return aTable;
        }

        public static int[,] MakeInitialConditions(int size)
        {
            var initialConditions = new int[size, size];

            for (var i = 0; i <= size; i++)
            {
                for (var j = 0; j <= size; j++)
                {
                    var txtBoxVal = HttpContext.Current.Request["ctl00$cphMain$txt" + j.ToString("00") + i.ToString("00")];
                    if (!string.IsNullOrEmpty(txtBoxVal)) initialConditions[i, j] = HexHelper.UndoMyHex(txtBoxVal);
                }
            }

            return initialConditions;
        }

        public void Solve()
        {
            SetCheckEngine(this._initialConditions);

            SolveByForce();
        }

        public void SolveNext(int[,] currentSolution)
        {
            SetCheckEngine(currentSolution);

            SolveByForce(8, 8);
        }

        private void SolveByForce(int starti = 0, int startj = 0)
        {
            try
            {
                var i = starti;
                while (i < _size)
                {
                    var j = startj;
                    while (j < _size)
                    {
                        if (_initialConditions[j, i] == 0)
                        {
                            if (_solution[j, i] != 0) _checkEngine.SetCheckValue(i, j, _solution[j, i], 0);

                            do
                            {
                                _solution[j, i] = _checkEngine.NextValue(i, j, _solution[j, i]);
                            }
                            while (_solution[j, i] <= _size && _checkEngine.CheckItem(i, j, _solution[j, i]));

                            if (_solution[j, i] > _size)
                            {
                                _solution[j, i] = _checkEngine.Reset(i, j);

                                do
                                {
                                    if (j == 0)
                                    {
                                        j = _size - 1;
                                        i--;
                                    }
                                    else
                                    {
                                        j--;
                                    }
                                }
                                while (j >= 0 && i >= 0 && _initialConditions[j, i] != 0);
                            }
                            else
                            {
                                _checkEngine.SetCheckValue(i, j, _solution[j, i], 1);
                                j++;
                            }
                        }
                        else
                        {
                            j++;
                        }
                    }
                    startj = 0;
                    i++;
                }
            }
            catch (Exception)
            {
                this._error = "General Solve Error";
                this._suDokuStatus = "errored";
            }
        }

        private void SetCheckEngine(int[,] conditionArray)
        {
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    this._solution[i, j] = conditionArray[i, j];

                    if (conditionArray[j, i] == 0) continue;

                    if (_checkEngine.CheckItem(i, j, conditionArray[j, i]))
                    {
                        this._error = _checkEngine.ErrorItem(i, j, conditionArray[j, i]);
                        this._suDokuStatus = "errored";
                        return;
                    }
                    _checkEngine.SetCheckValue(i, j, conditionArray[j, i], 1);
                }
            }
        }
    }

    public interface ICheckEngine
    {
        int Size();
        void SetCheckValue(int i, int j, int value, int onOff);
        bool CheckItem(int i, int j, int value);
        string ErrorItem(int i, int j, int value);
        int NextValue(int i, int j, int value);
        int Reset(int i, int j);
    }

    public interface IRenderEngine
    {
        void SetStyle(ref TableCell aCell, int i, int j);
    }
}