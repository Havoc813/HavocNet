namespace Phoenix.Common.Excel.Chart
{
    public class ChartDimension
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public ChartDimension()
        {
            Height = 400;
            Width = 800;
        }

        public ChartDimension(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
