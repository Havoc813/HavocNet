using System;
using System.Globalization;

namespace HavocNet.Puzzles
{
    public class CubicSolve
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly double _d;
        private readonly double[] _solutions = new double[5];
        public readonly string[] SolutionsFormatted = new string[3];
        private SolutionType _solType;

        public enum SolutionType
        {
            RealRoots3 = 1,
            RealRoot1ImaginaryRoots2 = 2,
            NoSolution = 0
        }

        public CubicSolve(double x3Coeff, double x2Coeff, double xCoeff, double coeff)
        {
            this._a = x3Coeff;
            this._b = x2Coeff;
            this._c = xCoeff;
            this._d = coeff;
        }

        public string GetSolution(int i)
        {
            switch (this._solType)
            {
                case SolutionType.RealRoots3:
                    if (i < 1 | i > 3)
                    {
                        return "No Solution";
                    }
                    return _solutions[i - 1].ToString(CultureInfo.InvariantCulture);
                case SolutionType.RealRoot1ImaginaryRoots2:
                    if (i < 1 | i > 3)
                    {
                        return "No Solution";
                    }
                    if (i == 1)
                    {
                        return Convert.ToString(_solutions[0].ToString(CultureInfo.InvariantCulture));
                    }
                    if (i == 2)
                    {
                        return Convert.ToString(_solutions[1].ToString(CultureInfo.InvariantCulture)) + " + " + Convert.ToString(_solutions[2].ToString(CultureInfo.InvariantCulture)) + "i";
                    }
                    return Convert.ToString(_solutions[1].ToString(CultureInfo.InvariantCulture)) + " - " + Convert.ToString(_solutions[2].ToString(CultureInfo.InvariantCulture)) + "i";
                default:
                    return "No Solution";
            }
        }

        public void SolveStandard()
        {
            var p = (3.0 * this._a * this._c - this._b * this._b) / (9.0 * this._a * this._a);
            var q = (9.0 * this._a * this._b * this._c - 27.0 * this._a * this._a * this._d - 2.0 * this._b * this._b * this._b) / (54.0 * this._a * this._a * this._a);
            var delta = p * p * p + q * q;

            if (delta > 0)
            {
                //1 Real, Two Non-Real
                var s = Math.Pow((q + Math.Pow(delta, (1.0 / 2.0))), (1.0 / 3.0));
                var t = Math.Pow((q - Math.Pow(delta, (1.0 / 2.0))), (1.0 / 3.0));

                _solutions[0] = s + t - this._b / (3.0 * this._a);
                _solutions[1] = -(s + t) / 2.0 - this._b / (3.0 * this._a);
                _solutions[2] = (Math.Pow(3, (1.0 / 2.0)) / 2.0) * (s - t);

                this._solType = SolutionType.RealRoot1ImaginaryRoots2;
            }
            else
            {
                //Three Real Roots
                var rho = Math.Pow((-Math.Pow(p, 3)), 0.5);
                var theta = Math.Acos(q / rho);
                var sReal = Math.Pow(rho, (1.0 / 3.0)) * Math.Cos(theta / 3.0);
                var tReal = Math.Pow(rho, (1.0 / 3.0)) * Math.Cos(-theta / 3.0);
                var sImag = Math.Pow(rho, (1.0 / 3.0)) * Math.Sin(theta / 3.0);
                var tImag = Math.Pow(rho, (1.0 / 3.0)) * Math.Sin(-theta / 3.0);

                _solutions[0] = sReal + tReal - this._b / (3.0 * this._a);
                _solutions[1] = -(sReal + tReal) / 2.0 - this._b / (3.0 * this._a) + (Math.Pow(3, (1.0 / 2.0)) / 2.0) * (sImag - tImag);
                _solutions[2] = -(sReal + tReal) / 2.0 - this._b / (3.0 * this._a) - (Math.Pow(3, (1.0 / 2.0)) / 2.0) * (sImag - tImag);

                this._solType = SolutionType.RealRoots3;
            }

            SolutionsFormatted[0] = GetSolution(1);
            SolutionsFormatted[1] = GetSolution(2);
            SolutionsFormatted[2] = GetSolution(3);
        }
    }

    public class QuadraticSolve
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly double[] _solutions = new double[2];
        public readonly string[] SolutionsFormatted = new string[2];
        private SolutionType _solType;

        public enum SolutionType
        {
            RealRoots2 = 1,
            ImaginaryRoots2 = 2,
            RealRoot = 3,
            NoSolution = 0
        }

        public QuadraticSolve(double x2Coeff, double xCoeff, double coeff)
        {
            this._a = x2Coeff;
            this._b = xCoeff;
            this._c = coeff;
        }

        public string GetSolution(int i)
        {
            switch (this._solType)
            {
                case SolutionType.RealRoot:
                    if (i != 1)
                    {
                        return "No Solution";
                    }
                    return _solutions[0].ToString(CultureInfo.InvariantCulture);
                case SolutionType.RealRoots2:
                    if (i < 1 | i > 2)
                    {
                        return "No Solution";
                    }
                    return _solutions[i - 1].ToString(CultureInfo.InvariantCulture);
                case SolutionType.ImaginaryRoots2:
                    if (i < 1 | i > 2)
                    {
                        return "No Solution";
                    }
                    if (i == 1)
                    {
                        return _solutions[0] + " + " + Convert.ToString(_solutions[1]) + "i";
                    }
                    return _solutions[0] + " - " + Convert.ToString(_solutions[1]) + "i";
                default:
                    return "No Solution";
            }
        }

        public void LinearSolve()
        {
            if (Convert.ToInt32(_b) == 0)
            {
                this._solType = SolutionType.NoSolution;
            }
            else
            {
                this._solType = SolutionType.RealRoot;
                this._solutions[0] = -1 * this._c / this._b;
            }

        }

        public void QuadSolveStandard()
        {
            if (Convert.ToInt32(_a) == 0)
            {
                LinearSolve();
            }
            else
            {
                var _2A = 2 * this._a;
                var disc = this._b * this._b - 4 * this._a * this._c;

                if (disc > 0)
                {
                        this._solType = SolutionType.RealRoots2;
                        this._solutions[0] = (-1 * this._b + Math.Sqrt(disc)) / _2A;
                        this._solutions[1] = (-1 * this._b - Math.Sqrt(disc)) / _2A;
                }
                if (disc < 0)
                {
                    this._solType = SolutionType.ImaginaryRoots2;
                    this._solutions[0] = -1 * this._b / _2A;
                    this._solutions[1] = -1 * Math.Sqrt(-1 * disc) / _2A;
                }
                if (Convert.ToInt32(disc) == 0)
                {
                    this._solType = SolutionType.RealRoot;
                    this._solutions[0] = -1 * this._b / _2A;
                }
            }

            SolutionsFormatted[0] = GetSolution(1);
            SolutionsFormatted[1] = GetSolution(2);
        }
    }
}
