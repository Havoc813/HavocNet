namespace HavocNet.Puzzles
{
    public class SuDokuHex : SuDoku
    {
        public SuDokuHex(int[,] initialConditions, string status = "blank") : base(
                new RenderEngine(16, 4), 
                new CheckEngine(16, 4), 
                initialConditions, 
                status
            ) { }
    }

    public class HexDemo
    {
        public readonly int[,] DemoConditions = new int[16,16];

        public HexDemo()
        {
            this.DemoConditions[0, 1] = 8;
            this.DemoConditions[0, 2] = 16;
            this.DemoConditions[0, 6] = 15;
            this.DemoConditions[0, 9] = 11;
            this.DemoConditions[0, 13] = 1;
            this.DemoConditions[0, 14] = 14;
            this.DemoConditions[1, 0] = 4;
            this.DemoConditions[1, 1] = 6;
            this.DemoConditions[1, 4] = 13;
            this.DemoConditions[1, 5] = 9;
            this.DemoConditions[1, 6] = 10;
            this.DemoConditions[1, 7] = 5;
            this.DemoConditions[1, 8] = 8;
            this.DemoConditions[1, 10] = 1;
            this.DemoConditions[2, 0] = 11;
            this.DemoConditions[2, 3] = 14;
            this.DemoConditions[2, 4] = 1;
            this.DemoConditions[2, 9] = 10;
            this.DemoConditions[2, 11] = 6;
            this.DemoConditions[2, 12] = 2;
            this.DemoConditions[2, 15] = 4;
            this.DemoConditions[3, 0] = 9;
            this.DemoConditions[3, 3] = 1;
            this.DemoConditions[3, 4] = 2;
            this.DemoConditions[3, 7] = 8;
            this.DemoConditions[3, 8] = 16;
            this.DemoConditions[3, 9] = 4;
            this.DemoConditions[3, 11] = 5;
            this.DemoConditions[3, 12] = 7;
            this.DemoConditions[4, 0] = 2;
            this.DemoConditions[4, 2] = 10;
            this.DemoConditions[4, 4] = 15;
            this.DemoConditions[4, 5] = 4;
            this.DemoConditions[4, 10] = 6;
            this.DemoConditions[4, 11] = 16;
            this.DemoConditions[4, 13] = 12;
            this.DemoConditions[4, 15] = 7;
            this.DemoConditions[5, 0] = 5;
            this.DemoConditions[5, 1] = 15;
            this.DemoConditions[5, 5] = 3;
            this.DemoConditions[5, 6] = 11;
            this.DemoConditions[5, 7] = 16;
            this.DemoConditions[5, 8] = 2;
            this.DemoConditions[5, 10] = 8;
            this.DemoConditions[5, 14] = 13;
            this.DemoConditions[6, 0] = 12;
            this.DemoConditions[6, 2] = 13;
            this.DemoConditions[6, 3] = 3;
            this.DemoConditions[6, 5] = 7;
            this.DemoConditions[6, 12] = 15;
            this.DemoConditions[6, 13] = 16;
            this.DemoConditions[6, 14] = 6;
            this.DemoConditions[6, 15] = 1;
            this.DemoConditions[7, 1] = 16;
            this.DemoConditions[7, 6] = 1;
            this.DemoConditions[7, 9] = 7;
            this.DemoConditions[7, 14] = 3;
            this.DemoConditions[8, 1] = 10;
            this.DemoConditions[8, 6] = 6;
            this.DemoConditions[8, 9] = 15;
            this.DemoConditions[8, 14] = 4;
            this.DemoConditions[9, 0] = 13;
            this.DemoConditions[9, 2] = 3;
            this.DemoConditions[9, 3] = 16;
            this.DemoConditions[9, 5] = 2;
            this.DemoConditions[9, 10] = 9;
            this.DemoConditions[9, 12] = 1;
            this.DemoConditions[9, 13] = 10;
            this.DemoConditions[9, 15] = 11;
            this.DemoConditions[10, 1] = 1;
            this.DemoConditions[10, 5] = 13;
            this.DemoConditions[10, 6] = 16;
            this.DemoConditions[10, 7] = 3;
            this.DemoConditions[10, 8] = 12;
            this.DemoConditions[10, 10] = 4;
            this.DemoConditions[10, 14] = 7;
            this.DemoConditions[11, 0] = 8;
            this.DemoConditions[11, 2] = 5;
            this.DemoConditions[11, 4] = 12;
            this.DemoConditions[11, 10] = 11;
            this.DemoConditions[11, 11] = 1;
            this.DemoConditions[11, 13] = 14;
            this.DemoConditions[11, 15] = 3;
            this.DemoConditions[12, 3] = 10;
            this.DemoConditions[12, 4] = 4;
            this.DemoConditions[12, 7] = 13;
            this.DemoConditions[12, 8] = 11;
            this.DemoConditions[12, 9] = 16;
            this.DemoConditions[12, 11] = 7;
            this.DemoConditions[12, 12] = 14;
            this.DemoConditions[13, 0] = 15;
            this.DemoConditions[13, 1] = 13;
            this.DemoConditions[13, 3] = 11;
            this.DemoConditions[13, 4] = 14;
            this.DemoConditions[13, 11] = 4;
            this.DemoConditions[13, 12] = 5;
            this.DemoConditions[13, 14] = 9;
            this.DemoConditions[13, 15] = 16;
            this.DemoConditions[14, 4] = 16;
            this.DemoConditions[14, 5] = 15;
            this.DemoConditions[14, 6] = 3;
            this.DemoConditions[14, 7] = 10;
            this.DemoConditions[14, 8] = 13;
            this.DemoConditions[14, 9] = 14;
            this.DemoConditions[14, 10] = 2;
            this.DemoConditions[14, 11] = 9;
            this.DemoConditions[15, 1] = 14;
            this.DemoConditions[15, 2] = 4;
            this.DemoConditions[15, 6] = 7;
            this.DemoConditions[15, 9] = 8;
            this.DemoConditions[15, 13] = 3;
            this.DemoConditions[15, 14] = 2;
        }
    }
}
