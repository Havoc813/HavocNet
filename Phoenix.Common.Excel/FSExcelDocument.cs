using System.IO;
using OfficeOpenXml;

namespace Phoenix.Common.Excel
{
    public class FSExcelDocument
    {
        private readonly ExcelPackage _package;
        public ExcelWorkbook Workbook { get; private set; }

        public FSExcelDocument(string excelFileName, bool createNewDocument = true)
        {
            var workbookFileInfo = new FileInfo(excelFileName);
            if (createNewDocument && workbookFileInfo.Exists)
            {
                workbookFileInfo.Delete();
                workbookFileInfo = new FileInfo(excelFileName);
            }
            _package = new ExcelPackage(workbookFileInfo);
            Workbook = _package.Workbook;
        }

        public void SaveDocument()
        {
            _package.Save();
            _package.Dispose();
        }

        public void CloseDocument()
        {
            _package.Dispose();
        }
    }
}
