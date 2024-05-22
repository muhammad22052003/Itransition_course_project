using Aspose.Cells;
using CourseProject_backend.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using CourseProject_backend.Interfaces.Helpers;
using Org.BouncyCastle.Utilities;

namespace CourseProject_backend.Helpers
{
    public class CSVHepler : ICSVHepler
    {
        public byte[] GetStream(List<List<string>> data)
        {
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];


            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    //Console.WriteLine($"{(char)('A' + j)}{i + 1}");
                    worksheet.Cells[$"{(char)('A' + j)}{i + 1}"].PutValue(data[i][j]);
                }
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.Save(memoryStream, SaveFormat.Csv);

                byte[] byteArray = memoryStream.ToArray();

                return byteArray;
            }
        }
    }
}
