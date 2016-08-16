using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Core;

namespace HavocNet.Puzzles
{
    public class SuDokuKiller : SuDoku
    {
        private readonly List<KillerHelper> _killerConditions;

        public SuDokuKiller(List<KillerHelper> killerConditions, string status = "blank")
            : base(
                new KillerRenderEngine(9, 3, killerConditions, status == "solved"),
                new KillerCheckEngine(9, 3, killerConditions),
                new int[9, 9],
                status
                )
        {
            this._killerConditions = killerConditions;
        }

        public string MakeKillerHidden()
        {
            var strHid = "";

            foreach (var aKillerHelper in _killerConditions)
            {
                var result = "";
                foreach (var pair in aKillerHelper.ijPairs)
                {
                    result += (pair[0] + ";" + pair[1] + "|");
                }
                strHid += result + "," + aKillerHelper.SumRef + "&";
            }

            return strHid;
        }
    }

    public class KillerCheckEngine : ICheckEngine
    {
        private readonly int[, ,] _checkMatrix;
        private readonly int _size;
        private readonly int _box;
        private readonly Dictionary<int, KillerHelper> _killerConditions = new Dictionary<int, KillerHelper>();
        private readonly int[,] _killerIDs = new int[9, 9];
        private readonly int[,] _solution = new int[9, 9];

        public KillerCheckEngine(int size, int box, IEnumerable<KillerHelper> KillerConditions)
        {
            _checkMatrix = new int[4, size + 1, size + 1];

            _size = size;
            _box = box;
            
            foreach (var killerCondition in KillerConditions)
            {
                foreach (var ijPair in killerCondition.ijPairs)
                {
                    _killerIDs[ijPair[0], ijPair[1]] = _killerConditions.Count();
                }
                _killerConditions.Add(_killerConditions.Count(), killerCondition);
            }
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
            _solution[j, i] = onOff == 1 ? value : 0;
        }

        public bool CheckItem(int i, int j, int value)
        {
            return _checkMatrix[0, i, value - 1] == 1 ||
                   _checkMatrix[1, j, value - 1] == 1 ||
                   _checkMatrix[2, BoxValue(i, j), value - 1] == 1 ||
                   _killerConditions[_killerIDs[j, i]].CheckSum(_solution, i, j, value);
        }

        public string ErrorItem(int i, int j, int value)
        {
            if (_checkMatrix[0, i, value - 1] == 1) return "Error on Column " + (i + 1) + " - too many " + value + "s";
            if (_checkMatrix[1, j, value - 1] == 1) return "Error on Row " + (j + 1) + " - too many " + value + "s";
            if (_checkMatrix[2, BoxValue(i, j), value - 1] == 1) return "Error in Box " + Math.Floor((double)(i / _box) + 1) + ", " + Math.Floor((double)(j / _box) + 1) + " - too many " + value + "s";
            return "Killer condition violated";
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

    public class KillerRenderEngine : IRenderEngine
    {
        private readonly int _size;
        private readonly int _box;
        private readonly List<KillerHelper> _KillerConditions;
        private bool _solved;

        public KillerRenderEngine(int size, int box, List<KillerHelper> KillerConditions, bool solved = false)
        {
            this._size = size;
            this._box = box;
            this._KillerConditions = KillerConditions;
            this._solved = solved;
        }

        public void SetStyle(ref TableCell aCell, int i, int j)
        {
            if (j % _box == 0) aCell.Style["border-left"] = "2px solid black";
            if (i % _box == 0) aCell.Style["border-top"] = "2px solid black";
            if (i == _size - 1) aCell.Style["border-bottom"] = "2px solid black";
            if (j == _size - 1) aCell.Style["border-right"] = "2px solid black";

            aCell.Attributes["onclick"] = "SelectCell(this);";
            aCell.Style["text-align"] = "center";

            if (!_solved)
            {
                aCell.Controls.Clear();
                aCell.Width = 30;
                foreach (var aKillerHelper in _KillerConditions)
                {
                    if (aKillerHelper.ContainsIJPair(i, j))
                    {
                        aCell.Text = aKillerHelper.SumRef.ToString("");
                        aCell.Attributes["onclick"] = "";

                        if (aKillerHelper.ContainsIJPair(i + 1, j)) aCell.Style["border-bottom"] = "solid 0px white";
                        else aCell.Style["border-bottom"] = "solid 2px black";

                        if (aKillerHelper.ContainsIJPair(i - 1, j)) aCell.Style["border-top"] = "solid 0px white";
                        else aCell.Style["border-top"] = "solid 2px black";

                        if (aKillerHelper.ContainsIJPair(i, j + 1)) aCell.Style["border-right"] = "solid 0px white";
                        else aCell.Style["border-right"] = "solid 2px black";

                        if (aKillerHelper.ContainsIJPair(i, j - 1)) aCell.Style["border-left"] = "solid 0px white";
                        else aCell.Style["border-left"] = "solid 2px black";
                    }
                }
            }
        }
    }

    public class KillerHelper
    {
        public readonly List<int[]> ijPairs;
        public readonly int SumRef;

        public KillerHelper(int sumRef)
        {
	        this.SumRef = sumRef;

	        ijPairs = new List<int[]>();
        }

        public bool ContainsIJPair(int i, int j)
        {
            return ijPairs.Any(aIJPair => aIJPair[0] == i && aIJPair[1] == j);
        }

        public Boolean CheckSum(int[,] solution, int i, int j, int value)
        {
	        var testSum = value;

            foreach (int[] aIJPair in ijPairs) {
		        var newItem = solution[aIJPair[0], aIJPair[1]];
		        if (newItem == 0 && (j != aIJPair[0] || i != aIJPair[1])) {
			        return false;
		        }
		        testSum += newItem;
	        }
	        if (testSum < this.SumRef) {
		        return true;
	        } 
            if (testSum > this.SumRef) {
		        return true;
	        } 
            return false;
        }

    }

    public class KillerDemo
    {
        public readonly List<KillerHelper> KillerConditions = new List<KillerHelper>();

        public KillerDemo()
        {
            var aKillerHelper = new KillerHelper(17);
            aKillerHelper.ijPairs.Add(new[] { 0, 0 });
            aKillerHelper.ijPairs.Add(new[] { 0, 1 });
            aKillerHelper.ijPairs.Add(new[] { 0, 2 });
            this.KillerConditions.Add(aKillerHelper);

            var aKillerHelper1 = new KillerHelper(30);
            aKillerHelper1.ijPairs.Add(new[] { 0, 3 });
            aKillerHelper1.ijPairs.Add(new[] { 1, 1 });
            aKillerHelper1.ijPairs.Add(new[] { 1, 2 });
            aKillerHelper1.ijPairs.Add(new[] { 1, 3 });
            this.KillerConditions.Add(aKillerHelper1);

            var aKillerHelper2 = new KillerHelper(7);
            aKillerHelper2.ijPairs.Add(new[] { 0, 4 });
            aKillerHelper2.ijPairs.Add(new[] { 0, 5 });
            aKillerHelper2.ijPairs.Add(new[] { 0, 6 });
            this.KillerConditions.Add(aKillerHelper2);

            var aKillerHelper3 = new KillerHelper(19);
            aKillerHelper3.ijPairs.Add(new[] { 0, 7 });
            aKillerHelper3.ijPairs.Add(new[] { 0, 8 });
            aKillerHelper3.ijPairs.Add(new[] { 1, 7 });
            this.KillerConditions.Add(aKillerHelper3);

            var aKillerHelper4 = new KillerHelper(7);
            aKillerHelper4.ijPairs.Add(new[] { 1, 0 });
            aKillerHelper4.ijPairs.Add(new[] { 2, 0 });
            aKillerHelper4.ijPairs.Add(new[] { 3, 0 });
            this.KillerConditions.Add(aKillerHelper4);

            var aKillerHelper5 = new KillerHelper(4);
            aKillerHelper5.ijPairs.Add(new[] { 1, 4 });
            aKillerHelper5.ijPairs.Add(new[] { 1, 5 });
            this.KillerConditions.Add(aKillerHelper5);

            var aKillerHelper6 = new KillerHelper(29);
            aKillerHelper6.ijPairs.Add(new[] { 1, 6 });
            aKillerHelper6.ijPairs.Add(new[] { 2, 5 });
            aKillerHelper6.ijPairs.Add(new[] { 2, 6 });
            aKillerHelper6.ijPairs.Add(new[] { 3, 5 });
            this.KillerConditions.Add(aKillerHelper6);

            var aKillerHelper7 = new KillerHelper(16);
            aKillerHelper7.ijPairs.Add(new[] { 1, 8 });
            aKillerHelper7.ijPairs.Add(new[] { 2, 8 });
            aKillerHelper7.ijPairs.Add(new[] { 3, 8 });
            this.KillerConditions.Add(aKillerHelper7);

            var aKillerHelper8 = new KillerHelper(7);
            aKillerHelper8.ijPairs.Add(new[] { 1, 0 });
            aKillerHelper8.ijPairs.Add(new[] { 2, 0 });
            aKillerHelper8.ijPairs.Add(new[] { 3, 0 });
            this.KillerConditions.Add(aKillerHelper8);

            var aKillerHelper9 = new KillerHelper(15);
            aKillerHelper9.ijPairs.Add(new[] { 2, 1 });
            aKillerHelper9.ijPairs.Add(new[] { 3, 1 });
            aKillerHelper9.ijPairs.Add(new[] { 4, 1 });
            aKillerHelper9.ijPairs.Add(new[] { 4, 0 });
            this.KillerConditions.Add(aKillerHelper9);

            var aKillerHelper10 = new KillerHelper(10);
            aKillerHelper10.ijPairs.Add(new[] { 2, 2 });
            aKillerHelper10.ijPairs.Add(new[] { 3, 2 });
            this.KillerConditions.Add(aKillerHelper10);

            var aKillerHelper11 = new KillerHelper(10);
            aKillerHelper11.ijPairs.Add(new[] { 2, 3 });
            aKillerHelper11.ijPairs.Add(new[] { 3, 3 });
            this.KillerConditions.Add(aKillerHelper11);

            var aKillerHelper12 = new KillerHelper(14);
            aKillerHelper12.ijPairs.Add(new[] { 2, 4 });
            aKillerHelper12.ijPairs.Add(new[] { 3, 4 });
            this.KillerConditions.Add(aKillerHelper12);

            var aKillerHelper13 = new KillerHelper(11);
            aKillerHelper13.ijPairs.Add(new[] { 2, 7 });
            aKillerHelper13.ijPairs.Add(new[] { 3, 7 });
            aKillerHelper13.ijPairs.Add(new[] { 3, 6 });
            this.KillerConditions.Add(aKillerHelper13);

            var aKillerHelper14 = new KillerHelper(27);
            aKillerHelper14.ijPairs.Add(new[] { 4, 2 });
            aKillerHelper14.ijPairs.Add(new[] { 4, 3 });
            aKillerHelper14.ijPairs.Add(new[] { 4, 4 });
            aKillerHelper14.ijPairs.Add(new[] { 4, 5 });
            aKillerHelper14.ijPairs.Add(new[] { 4, 6 });
            this.KillerConditions.Add(aKillerHelper14);

            var aKillerHelper15 = new KillerHelper(20);
            aKillerHelper15.ijPairs.Add(new[] { 4, 8 });
            aKillerHelper15.ijPairs.Add(new[] { 4, 7 });
            aKillerHelper15.ijPairs.Add(new[] { 5, 7 });
            aKillerHelper15.ijPairs.Add(new[] { 6, 7 });
            this.KillerConditions.Add(aKillerHelper15);

            var aKillerHelper16 = new KillerHelper(24);
            aKillerHelper16.ijPairs.Add(new[] { 5, 0 });
            aKillerHelper16.ijPairs.Add(new[] { 6, 0 });
            aKillerHelper16.ijPairs.Add(new[] { 7, 0 });
            this.KillerConditions.Add(aKillerHelper16);

            var aKillerHelper17 = new KillerHelper(17);
            aKillerHelper17.ijPairs.Add(new[] { 5, 1 });
            aKillerHelper17.ijPairs.Add(new[] { 5, 2 });
            aKillerHelper17.ijPairs.Add(new[] { 6, 1 });
            this.KillerConditions.Add(aKillerHelper17);

            var aKillerHelper18 = new KillerHelper(11);
            aKillerHelper18.ijPairs.Add(new[] { 5, 3 });
            aKillerHelper18.ijPairs.Add(new[] { 6, 3 });
            aKillerHelper18.ijPairs.Add(new[] { 6, 2 });
            aKillerHelper18.ijPairs.Add(new[] { 7, 2 });
            this.KillerConditions.Add(aKillerHelper18);

            var aKillerHelper19 = new KillerHelper(5);
            aKillerHelper19.ijPairs.Add(new[] { 5, 4 });
            aKillerHelper19.ijPairs.Add(new[] { 6, 4 });
            this.KillerConditions.Add(aKillerHelper19);

            var aKillerHelper20 = new KillerHelper(7);
            aKillerHelper20.ijPairs.Add(new[] { 5, 5 });
            aKillerHelper20.ijPairs.Add(new[] { 6, 5 });
            this.KillerConditions.Add(aKillerHelper20);

            var aKillerHelper21 = new KillerHelper(12);
            aKillerHelper21.ijPairs.Add(new[] { 5, 6 });
            aKillerHelper21.ijPairs.Add(new[] { 6, 6 });
            this.KillerConditions.Add(aKillerHelper21);

            var aKillerHelper22 = new KillerHelper(18);
            aKillerHelper22.ijPairs.Add(new[] { 5, 8 });
            aKillerHelper22.ijPairs.Add(new[] { 6, 8 });
            aKillerHelper22.ijPairs.Add(new[] { 7, 8 });
            this.KillerConditions.Add(aKillerHelper22);

            var aKillerHelper23 = new KillerHelper(13);
            aKillerHelper23.ijPairs.Add(new[] { 7, 1 });
            aKillerHelper23.ijPairs.Add(new[] { 8, 0 });
            aKillerHelper23.ijPairs.Add(new[] { 8, 1 });
            this.KillerConditions.Add(aKillerHelper23);

            var aKillerHelper24 = new KillerHelper(10);
            aKillerHelper24.ijPairs.Add(new[] { 7, 3 });
            aKillerHelper24.ijPairs.Add(new[] { 7, 4 });
            this.KillerConditions.Add(aKillerHelper24);

            var aKillerHelper25 = new KillerHelper(22);
            aKillerHelper25.ijPairs.Add(new[] { 7, 5 });
            aKillerHelper25.ijPairs.Add(new[] { 7, 6 });
            aKillerHelper25.ijPairs.Add(new[] { 7, 7 });
            aKillerHelper25.ijPairs.Add(new[] { 8, 5 });
            this.KillerConditions.Add(aKillerHelper25);

            var aKillerHelper26 = new KillerHelper(14);
            aKillerHelper26.ijPairs.Add(new[] { 8, 6 });
            aKillerHelper26.ijPairs.Add(new[] { 8, 7 });
            aKillerHelper26.ijPairs.Add(new[] { 8, 8 });
            this.KillerConditions.Add(aKillerHelper26);

            var aKillerHelper27 = new KillerHelper(16);
            aKillerHelper27.ijPairs.Add(new[] { 8, 2 });
            aKillerHelper27.ijPairs.Add(new[] { 8, 3 });
            aKillerHelper27.ijPairs.Add(new[] { 8, 4 });
            this.KillerConditions.Add(aKillerHelper27);
        }
    }
}