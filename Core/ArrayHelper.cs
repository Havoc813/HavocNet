namespace Core
{
    public static class ArrayHelper
    {
        public static string printArr(int[,] arr)
        {
            var returnStr = "";

            for (var i = 0; i <= arr.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    returnStr += arr[i, j] + ", ";
                }
                returnStr += "\n";
            }

            return returnStr;
        }

        public static int[,] GetArrayAt(int horizStart, int vertStart, int[,] sourceArr)
        {
            var arr = new int[9, 9];

            for (var i = horizStart; i < 9 + horizStart; i++)
            {
                for (var j = vertStart; j < 9 + vertStart; j++)
                {
                    arr[i - horizStart, j - vertStart] = sourceArr[i, j];
                }
            }

            return arr;
        }

        public static void PutArrayAt(int horizStart, int vertStart, int[,] sourceArr, int[,] destinationArr)
        {
            for (var i = horizStart; i < 9 + horizStart; i++)
            {
                for (var j = vertStart; j < 9 + vertStart; j++)
                {
                    destinationArr[i, j] = sourceArr[i - horizStart, j - vertStart];
                }
            }
        }
    }
}
