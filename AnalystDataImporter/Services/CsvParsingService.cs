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
        public string inputFilePath { get; set; }
        public char delimiter { get; set; }
        public bool isFirstRowHeading { get; set; }

        public List<string[]> ParseCsv(string filePath, Encoding encoding, char delimiter, List<int> columnIndexes, int maxLines = Constants.MaxLoadedCsvLines)
        {
            inputFilePath = filePath;
            this.delimiter = delimiter;
            if (encoding == null)
                encoding = Encoding.UTF8;

            List<string[]> result = new List<string[]>();
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            try
            {
                IEnumerable<string> lines;

                // If columnIndexes is not null and has elements, read all lines; otherwise, read up to maxLines
                if (columnIndexes != null && columnIndexes.Count > 0)
                {
                    lines = File.ReadLines(filePath, encoding);
                }
                else
                {
                    lines = File.ReadLines(filePath, encoding).Take(maxLines);
                }

                foreach (var line in lines)
                {
                    var values = line.Split(delimiter);

                    // If columnIndexes is null or empty, add all values; otherwise, add only selected columns
                    if (columnIndexes == null || columnIndexes.Count == 0)
                    {
                        result.Add(values);
                    }
                    else
                    {
                        var selectedValues = columnIndexes.Select(index => values.Length > index ? values[index] : "").ToArray();
                        result.Add(selectedValues);
                    }
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