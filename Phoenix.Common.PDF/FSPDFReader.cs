using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Phoenix.Common.PDF
{
    public class FSPDFReader
    {
        private int _page;
        private readonly Dictionary<int, string> _pdfDoc; 

        public FSPDFReader(string fileName)
        {
            _page = 1;
            _pdfDoc = new Dictionary<int, string>();
            LoadDocument(fileName);
        }

        private void LoadDocument(string fileName)
        {
            var reader = new PdfReader(fileName);
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                ITextExtractionStrategy itExtractStategy = new SimpleTextExtractionStrategy();
                string pageText = PdfTextExtractor.GetTextFromPage(reader, page, itExtractStategy);

                _pdfDoc.Add(page,Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default,
                                                    Encoding.UTF8,
                                                    Encoding.Default.GetBytes(pageText)
                                                    )));
            }
            reader.Close();
            reader.Dispose();
        }

        public string ReadPage(int pageNumber)
        {
            _page = pageNumber;
            if (_page < 1 || _page > _pdfDoc.Count)
                return String.Format("Invalid Page - {0} pages in this document.", _pdfDoc.Count.ToString("0"));
            return _pdfDoc[_page];
        }

        public string Next()
        {
            return ReadPage(_page++);
        }

        public string Previous()
        {
            return ReadPage(_page--);
        }

        public string ReadDocument()
        {
            StringBuilder documentText = new StringBuilder("");
            foreach (string pageText in _pdfDoc.Values)
            {
                documentText.Append(pageText);
            }
            return documentText.ToString();
        }

        public Dictionary<int, string> GetDocument()
        {
            return _pdfDoc;
        }
    }
}
