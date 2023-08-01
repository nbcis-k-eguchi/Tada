using System.Data;
using System.Text;

using ClosedXML.Excel;

using Microsoft.AspNetCore.Components.Forms;

namespace Tada.Server.FileAccess
{
    public class FileConnection
    {
        public static readonly string ActivityReportUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "app_data");

        private readonly string _path;

        private readonly bool _isExcel = false;

        public FileConnection(string path)
        {
            _path = path;

            if (File.Exists(path))
            {
                throw new ArgumentException("指定したパスにファイルがありません。");
            }

            // TODO: xlsを対応させる場合、ClosedXMLが使えない
            if (Path.GetExtension(path) == ".xlsx")
            {
                _isExcel = true;
            }
        }

        public async Task FileUpload(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            await File.WriteAllBytesAsync(_path, buffer);
        }


        public bool Write(DataTable dt)
        {
            if (_isExcel)
            {
                return ExcelWrite(dt);
            }
            else
            {
                return TextWrite(ToCsv(dt));
            }
        }


        #region Private Methods

        #region Text
        private bool TextWrite(string text)
        {
            try
            {
                using (var sw = new StreamWriter(_path, false, Encoding.UTF8))
                {
                    sw.Write(text);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string TextRead()
        {
            try
            {
                using (var sr = new StreamReader(_path, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region Excel
        private bool ExcelWrite(DataTable dt)
        {
            try
            {
                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add(dt, "Sheet1");
                    wb.SaveAs(_path);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string ExcelRead()
        {
            try
            {
                using (var wb = new XLWorkbook(_path))
                {
                    var ws = wb.Worksheet(1);
                    IXLTable dt = ws.RangeUsed().AsTable();
                    return ToCsv(dt);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        private string ToCsv(DataTable dt)
        {
            var sb = new StringBuilder();
            // ヘッダー
            foreach (DataColumn col in dt.Columns)
            {
                sb.Append(col.ColumnName);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\r\n");
            // データ
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    var itemdata = item is null ? "" : item.ToString();
                    sb.Append(itemdata);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        private string ToCsv(IXLTable xLTable)
        {
            var sb = new StringBuilder();
            // ヘッダー
            foreach (var col in xLTable.Fields)
            {
                sb.Append(col.Name);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\r\n");
            // データ
            foreach (var row in xLTable.DataRange.Rows())
            {
                foreach (var item in row.Cells())
                {
                    sb.Append(item.Value.ToString());
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        #endregion

    }
}
