using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.Globals;

namespace AnalystDataImporter.Services
{
    public class CsvParserService
    {

        public List<string[]> ParseCsv(string filePath, Encoding encoding, char delimiter, int maxLines = Constants.MaxLoadedCsvLines)
        {
            List<string[]> result = new List<string[]>();
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            try
            {
                var lines = File.ReadLines(filePath, encoding).Take(maxLines);

                foreach (var line in lines)
                {
                    var values = line.Split(delimiter);
                    result.Add(values);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle lack of file access permissions
            }
            catch (IOException ex)
            {
                // Handle other I/O errors
            }
            return result;
        }

        public bool IsValidCsvStructure(string filePath, char delimiter, int maxLines = Constants.MaxLoadedCsvLines)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            try
            {
                var lines = File.ReadLines(filePath).Take(maxLines);
                var headerColumns = lines.FirstOrDefault()?.Split(delimiter).Length ?? 0;

                foreach (var line in lines.Skip(1))
                {
                    if (line.Split(delimiter).Length != headerColumns)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle lack of file access permissions
                return false;
            }
            catch (IOException ex)
            {
                // Handle other I/O errors
                return false;
            }
        }
    }
}