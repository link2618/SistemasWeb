using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SistemasWeb.Library
{
    public class ExportData : Controller
    {
        private List<String[]> _listData;
        private String _fileName, _table;
        private String[] _titles;
        private IWebHostEnvironment _hostingEnvironment;

        public ExportData(IWebHostEnvironment hostingEnvironment, List<String[]> listData, String[] titles, String fileName, String table)
        {
            _table = table;
            _fileName = fileName;
            _titles = titles;
            _listData = listData;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> ExportExcelAsync()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            var memory = new MemoryStream();

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, _fileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                //Vamos a crear una hoja de ecxel el parametro _table le da el nombre al documento
                ISheet excelSheet = workbook.CreateSheet(_table);
                IRow row = excelSheet.CreateRow(0); //Primera fila de ecxel
                //columnas
                for (int i = 0; i < _titles.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(_titles[i]);
                }

                //creamos filas en las columnas ya creadas
                int count = 1; //variable para contar las filas que vamos a crear 
                for (int i = 0; i < _listData.Count; i++)
                {
                    row = excelSheet.CreateRow(count);
                    var list = _listData[i];

                    for (int j = 0; j < _titles.Length; j++)
                    {
                        row.CreateCell(j).SetCellValue(list[j]);
                    }
                    count++;
                }
                //creamos la hoja ecxel
                workbook.Write(fs);
            }
            //vamos a guardar el archivo en nuestra pc
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, _fileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _fileName);
        }

    }
}
