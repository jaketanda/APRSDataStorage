using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp
{
    // class for the excel objects.
    // used for creating, saving, and editing excel files.

    class Excel
    {
        string Path = "";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;

        public Excel()
        {
        }

        public Excel(string Path, int Sheet)
        {
            this.Path = Path;
            wb = excel.Workbooks.Open(Path);
            ws = wb.Worksheets[Sheet];
        }

        public void CreateNewFile()
        {
            this.wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this.ws = wb.Worksheets[1];
        }

        public void CreateNewSheet()
        {
            Worksheet tempsheet = wb.Worksheets.Add(After: ws);
        }

        public string ReadCell(int r, int c)
        {
            r++;
            c++;
            if (ws.Cells[r, c].Value2 != null)
                return ws.Cells[r, c].Value2;
            else
                return "";
        }

        public void WriteToCell(int r, int c, string Content)
        {
            r++;
            c++;
            ws.Cells[r, c].Value2 = Content;
        }

        public void Save()
        {
            wb.Save();
        }

        public void SaveAs(string Path)
        {
            try
            {
                wb.SaveAs(Path);
            } catch
            {

            }

        }

        public void Close()
        {
            wb.Close();
        }
    }
}
