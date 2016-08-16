using System;

namespace Phoenix.Core.Tables
{
    public class FSColumn
    {
        public string Text { get; set; }
        public string SqlName { get; set; }
        public Type DataType { get; set; }
        public string Format { get; set; }
        public FSAlignment HorizontalAlignment { get; set; }
        public int PixelWidth { get; set; }

        private void Init()
        {
            Text = "";
            SqlName = "";
            DataType = typeof(string);
            Format = "";
            HorizontalAlignment = FSAlignment.Left;
            PixelWidth = 100;
        }
        public FSColumn(string text, Type dataType)
        {
            Init();
            Text = text;
            DataType = dataType;
            HorizontalAlignment = FSAlignment.Left;
        }
        public FSColumn(string text, Type dataType, int pixelWidth)
        {
            Init();
            Text = text;
            DataType = dataType;
            HorizontalAlignment = FSAlignment.Left;
            PixelWidth = pixelWidth;
        }
        public FSColumn(string text, Type dataType, FSAlignment horizontalAlignment)
        {
            Init();
            Text = text;
            DataType = dataType;
            HorizontalAlignment = horizontalAlignment;
        }
        public FSColumn(string text, Type dataType, FSAlignment horizontalAlignment, int pixelWidth)
        {
            Init();
            Text = text;
            DataType = dataType;
            HorizontalAlignment = horizontalAlignment;
            PixelWidth = pixelWidth;
        }

        public FSColumn(string text, Type dataType, string format)
        {
            Init();
            Text = text;
            DataType = dataType;
            Format = format;
        }
        public FSColumn(string text, Type dataType, string format, int pixelWidth)
        {
            Init();
            Text = text;
            DataType = dataType;
            Format = format;
            PixelWidth = pixelWidth;
        }
        public FSColumn(string text, Type dataType, string format, FSAlignment horizontalAlignment)
        {
            Init();
            Text = text;
            DataType = dataType;
            Format = format;
            HorizontalAlignment = horizontalAlignment;
        }
        public FSColumn(string text, Type dataType, string format, FSAlignment horizontalAlignment, int pixelWidth)
        {
            Init();
            Text = text;
            DataType = dataType;
            Format = format;
            HorizontalAlignment = horizontalAlignment;
            PixelWidth = pixelWidth;
        }

        public FSColumn(string text, string sqlName, Type dataType, string format)
        {
            Init();
            Text = text;
            SqlName = sqlName;
            DataType = dataType;
            Format = format;
        }
        public FSColumn(string text, string sqlName, Type dataType, string format, int pixelWidth)
        {
            Init();
            Text = text;
            SqlName = sqlName;
            DataType = dataType;
            Format = format;
            PixelWidth = pixelWidth;
        }
        public FSColumn(string text, string sqlName, Type dataType, string format, FSAlignment horizontalAlignment)
        {
            Init();
            Text = text;
            SqlName = sqlName;
            DataType = dataType;
            Format = format;
            HorizontalAlignment = horizontalAlignment;
        }
        public FSColumn(string text, string sqlName, Type dataType, string format, FSAlignment horizontalAlignment, int pixelWidth)
        {
            Init();
            Text = text;
            SqlName = sqlName;
            DataType = dataType;
            Format = format;
            HorizontalAlignment = horizontalAlignment;
            PixelWidth = pixelWidth;
        }
    }
}
