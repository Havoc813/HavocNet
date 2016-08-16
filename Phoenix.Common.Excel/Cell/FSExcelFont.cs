using System.Drawing;
using Phoenix.Core.Tables;

namespace Phoenix.Common.Excel.Cell
{
    public class FSExcelFont
    {
        public string FontName { get; set; }
        public int Size { get; set; }
        public Color ForeColour { get; set; }
        public FSAlignment HorizontalAlignment { get; set; }
        public string Format { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }

        public FSExcelFont(string font)
        {
            Init(font, 10, Color.Black, FSAlignment.Left, "General");
        }

        public FSExcelFont(string font, int size)
        {
            Init(font, size, Color.Black, FSAlignment.Left, "General");
        }

        public FSExcelFont(string font, string format)
        {
            Init(font, 10, Color.Black, FSAlignment.Left, format);
        }

        public FSExcelFont(Color foreColour)
        {
            Init(@"Arial", 10, foreColour, FSAlignment.Left, "General");
        }


        public FSExcelFont(string font, int size, Color foreColour, FSAlignment horizontalAlignment, string format)
        {
            Init(font, size, foreColour, horizontalAlignment, format);
        }

        private void Init(string font, int size, Color foreColour, FSAlignment horizontalAlign, string format)
        {
            FontName = font;
            Size = size;
            ForeColour = foreColour;
            HorizontalAlignment = horizontalAlign;
            Format = format;
        }
    }
}
