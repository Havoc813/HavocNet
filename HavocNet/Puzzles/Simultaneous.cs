using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocNet.Puzzles
{
    public class Simultaneous
    {
        public double[,] MatrixMultiply(double[,] aMatrix1, double[,] aMatrix2)
        {
            var ansMatrix = new double[aMatrix2.GetUpperBound(0) + 1, aMatrix2.GetUpperBound(1) + 1];

            var rA = aMatrix1.GetLength(0);
            var cA = aMatrix1.GetLength(1);
            var rB = aMatrix2.GetLength(0);
            var cB = aMatrix2.GetLength(1);

            if (cA == rB)
            {
                for (var i = 0; i < rA; i++)
                {
                    for (var j = 0; j < cB; j++)
                    {
                        double temp = 0;
                        for (var k = 0; k < cA; k++)
                        {
                            temp += aMatrix1[i, k] * aMatrix2[k, j];
                        }
                        ansMatrix[i, j] = temp;
                    }
                }
            }

            return ansMatrix;
        }

        public double[,] MatrixGetInverse(double[,] aMatrix)
        {
            var b = RemoveZeroRows(aMatrix);
            var m = b.GetLength(0);
            var n = b.GetLength(1);
            var c = -1; 
            var r = 0;

            var t = 0; 
            var k = 0;
            var bigR = new double[n];
            var bigK = new double[n];

            while (k < m)
            {
                //Finding the first non-zero column index*******************************        
                for (var i = t; i < n; i++)
                {
                    for (var j = k; j < m; j++)
                    {
                        if (b[j, i] != 0)
                        {
                            c = i;
                            break;
                        }
                    }
                    if (c == i)
                        break;
                }
                //***********************************************************************
                //Finding the first non-zero entry in the column "c" to start iterating**      
                for (var i = k; i < m; i++)
                {
                    if (b[i, c] != 0)
                    {
                        r = i;
                        break;
                    }
                }
                //*************************************************************************
                //Interchanging rows(if neccessary)to get the first entry of the located...
                //...column non-zero*******************************************************      
                if (r != k)
                {
                    for (var i = 0; i < n; i++)
                    {
                        bigK[i] = b[k, i];
                        bigR[i] = b[r, i];
                        b[k, i] = bigR[i];
                        b[r, i] = bigK[i];
                    }
                }
                //****************************************************************************
                //Checking for the first first entry from the previous iteration if it is 1...
                //...if not divide the rows by the multiplicative inverse of that number******  
                var p = b[k, c];
                if (p != 1)
                {
                    for (var i = 0; i < n; i++)
                        b[k, i] *= Math.Pow(p, -1);
                }
                //Then multiplying the first number times other non-zero entry rows to get all.
                //...numbers equal to 0 below the selected number*********************                    
                for (var i = 0; i < m; i++)
                {
                    if (i == k)
                        continue;
                    if (b[i, c] != 0)
                    {
                        var pp = b[i, c];
                        for (var j = 0; j < n; j++)
                        {
                            b[i, j] -= pp * b[k, j];
                        }
                    }
                }
                //********************************************************************
                //Adjusting the indexes for the next iteration************************    
                t = c + 1; k++;
            }
            //************************************************************************          
            //Adding the removed zero rows if there were any**************************
            if (GetZeroRows(aMatrix) != 0)
            {
                var g = b;
                for (var i = m + 1; i <= GetZeroRows(aMatrix); i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        g[i, j] = 0;
                    }
                }
                return g;
            }
            return b;
        }
        private static double[,] RemoveZeroRows(double[,] A)
        {
            var m = A.GetLength(0);
            var n = A.GetLength(1);
            var q = 0;
            var zeroIndexes = ZeroRowIndexes(A);
            var index = false;

            //Remove the zero rows (if any) and free up the matrix with nonzero parameters*****
            var w = GetZeroRows(A);
            var l = m - w;
            if (w != 0)
            {
                var b = new double[l, n];

                for (var i = 0; i < m; i++)
                {
                    for (var j = 0; j < w; j++)
                    {
                        if (zeroIndexes[j] != i)
                            index = true;
                        else
                        {
                            index = false;
                            break;
                        }
                    }

                    if (index)
                    {
                        for (var k = 0; k < n; k++)
                            b[q, k] = A[i, k];
                        q++;
                    }
                }
                return b;
            }
            return A;
        }
        private static int[] ZeroRowIndexes(double[,] A)
        {
            var m = A.GetLength(0);
            var n = A.GetLength(1);
            var r = GetZeroRows(A);
            var zeroRowIndexes = new int[r];
            var q = 0; 
            var zero = false;

            if (r != 0)
            {
                for (var i = 0; i < m; i++)
                {
                    if (A[i, 0] == 0)
                    {
                        for (var j = 0; j < n; j++)
                        {
                            if (A[i, j] == 0)
                            {
                                zero = true;
                                zeroRowIndexes[q] = i;
                            }
                            else
                            {
                                zero = false;
                                break;
                            }
                        }
                        if (zero)
                            q++;
                    }
                }
                return zeroRowIndexes;
            }
            return null;
        }
        private static int GetZeroRows(double[,] A)
        {
            var m = A.GetLength(0);
            var n = A.GetLength(1);
            var r = m; 
            var k = 0;

            for (var i = 0; i < m; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (A[i, j] != 0)
                    {
                        k++;
                        break;
                    }
                }
            }
            r -= k;
            return r;
        }
    }
}
