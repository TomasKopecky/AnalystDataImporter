using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.Services;
using AnalystDataImporter.Utilities;

namespace AnalystDataImporterTestsProject
{
    [TestClass]
    public class ParsingCsvTest
    {
        private string _testFilesPath;
        private string _utf8ParsingBasicUzcFileWithValidStructure;
        private string _utf8ParsingUzcFileWithInvalidHeadingStructure;
        private string _utf8ParsingUzcFileWithInvalidLinesStructure;
        private List<string[]> _expectedJsonOutput;

        [TestInitialize]
        public void Initialize()
        {
            _testFilesPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "testFiles", "csvFiles");
            _utf8ParsingBasicUzcFileWithValidStructure = Path.Combine(_testFilesPath, "parsing_uzc_valid_structure.csv");
            _utf8ParsingUzcFileWithInvalidHeadingStructure = Path.Combine(_testFilesPath, "parsing_uzc_invalid_structure_heading_error.csv");
            _utf8ParsingUzcFileWithInvalidLinesStructure = Path.Combine(_testFilesPath, "parsing_uzc_invalid_structure_lines_error.csv");
            _expectedJsonOutput = new List<string[]>
            {
                new[] {"IMEI","SIM","IMSI","Datum od","Cas od","Datum do","Cas do","Pocet pouziti"},
                new[] {"35994918905259","792967870","230029011511648", "02.02.2023", "00:00:01", "01.08.2023", "23:59:59", ""},
                new[] {"35994918905259","380999382417","255011440794124", "02.02.2023", "00:00:01", "01.08.2023", "23:59:59", ""},
                new[] {"35299453954172","380999382417","255011440794124", "02.02.2023", "00:00:01", "01.08.2023", "23:59:59", ""},
                new[] {"35011436935926","79153026421","250016315439884", "02.02.2023", "00:00:01", "01.08.2023", "23:59:59", ""},
                new[] {"35299453954172","380999382417","255011440794124","17.06.2023", "00:00:01", "23.06.2023", "23:59:59", "2956"},
                new[] {"35577134131195","775836716","230030002253507", "02.03.2023", "00:00:01", "07.03.2023", "23:59:59", "4"},
            };
        }

        [TestMethod]
        public void testBasicUzcCsvFileWithValidStructure()
        {

            CsvParserService csvParserService = new CsvParserService();
            bool validHeading = csvParserService.IsValidCsvStructure(_utf8ParsingBasicUzcFileWithValidStructure, ';');
            Assert.IsTrue(validHeading);
        }

        [TestMethod]
        public void testBasicUzcCsvFileWithInvalidHeadingStructure()
        {
            CsvParserService csvParserService = new CsvParserService();
            bool validHeading = csvParserService.IsValidCsvStructure(_utf8ParsingUzcFileWithInvalidHeadingStructure, ';');
            Assert.IsFalse(validHeading);
        }

        [TestMethod]
        public void testBasicUzcCsvFileWithInvalidLinesStructure()
        {
            CsvParserService csvParserService = new CsvParserService();
            bool validHeading = csvParserService.IsValidCsvStructure(_utf8ParsingUzcFileWithInvalidLinesStructure, ';');
            Assert.IsFalse(validHeading);
        }

        [TestMethod]
        public void loadBasicUzcCsvFileWithValidStructure()
        {
            CsvParserService csvParserService = new CsvParserService();
            var currentJsonResult = csvParserService.ParseCsv(_utf8ParsingBasicUzcFileWithValidStructure, Encoding.UTF8, ';');
            int i = 0;
            foreach ( var line in currentJsonResult ) 
            {
                var expectedJsonOutput = _expectedJsonOutput[i];
                CollectionAssert.AreEqual(expectedJsonOutput, line);
                i++;
            }
        }
    }
}
