using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NNUI1_GeneticAlgorithm
{
    public static class ExcelReader
    {
        public static Gene[] GetPubListFromExcel(string nameOfFile = @"data\Pubs.xlsx")
        {
            var projectFolder = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName;
            var path = Path.Combine(projectFolder, nameOfFile);

            var excelFile = new LinqToExcel.ExcelQueryFactory(path);

            var result =
                from row in excelFile.Worksheet<Gene>("List1")
                select row;
            return result.ToArray();
        }
    }
}
