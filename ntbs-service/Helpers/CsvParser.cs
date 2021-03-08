using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace ntbs_service.Helpers
{
    public static class CsvParser
    {
        public static List<T> GetRecordsFromCsv<T>(string relativePathToFile, Func<CsvReader, T> getRecord)
        {
            var filePath = GetFullFilePath(relativePathToFile);

            var records = new List<T>();

            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    records.Add(getRecord(csvReader));
                }
            }

            return records;
        }

        public static IEnumerable<T> GetRecordsFromCsv<T>(string relativePathToFile)
        {
            var filePath = GetFullFilePath(relativePathToFile);

            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csvReader.GetRecords<T>().ToList();
            }
        }

        private static string GetFullFilePath(string relativePathToFile)
            => Path.Combine(Environment.CurrentDirectory, relativePathToFile);
    }
}
