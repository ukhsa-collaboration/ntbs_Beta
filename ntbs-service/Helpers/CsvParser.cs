using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace ntbs_service.Helpers
{
    public static class CsvParser
    {
        public static List<T> GetRecordsFromCsv<T>(string relativePathToFile, Func<CsvReader, T> getRecord)
        {
            var filePath = GetFullFilePath(relativePathToFile);

            var records = new List<T>();

            using (StreamReader reader = new StreamReader(filePath))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
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

        private static string GetFullFilePath(string relativePathToFile)
            => Path.Combine(Environment.CurrentDirectory, relativePathToFile);
    }
}
