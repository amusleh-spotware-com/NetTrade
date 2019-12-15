using CsvHelper;
using NetTrade.Models;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using System.Globalization;
using NetTrade.Interfaces;

namespace NetTrade.Helpers
{
    public static class DataReader
    {
        public static List<Bar> GetBars(string filePath, Configuration csvConfiguration)
        {
            var result = new List<Bar>();

            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var textStream = new StreamReader(fileStream))
            {
                var reader = new CsvReader(textStream, csvConfiguration);

                var data = reader.GetRecords<Bar>();

                result.AddRange(data);
            }

            return result;
        }

        public static void SetSymbolsData(Configuration csvConfiguration, params ISymbol[] symbols)
        {
            foreach (var symbol in symbols)
            {
                symbol.Data = GetBars(symbol.DataFilePath, csvConfiguration);
            }
        }
    }
}