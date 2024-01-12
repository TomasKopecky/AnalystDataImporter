using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
    public class EncodingTest
    {
        private string _testFilesPath;
        private string _utf8EmptyFilePath;
        private string _utf8TextFilePath;
        private string _utf8CzechTextFilePath;

        [TestInitialize] public void Initialize()
        {
            _testFilesPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "testFiles", "csvFiles");
            _utf8EmptyFilePath = Path.Combine(_testFilesPath, "utf8_empty.csv");
            _utf8TextFilePath = Path.Combine(_testFilesPath, "utf8_text.csv");
            _utf8CzechTextFilePath = Path.Combine(_testFilesPath, "utf8_text_czech.csv");
        }

        [TestMethod]
        public void testUtf8EmptyFileEncoding()
        {
            Encoding result = EncodingDetector.DetectEncoding(_utf8EmptyFilePath);
            Assert.AreEqual(Encoding.UTF8, result);
        }

        [TestMethod]
        public void testUtf8TextFileEncoding()
        {
            Encoding result = EncodingDetector.DetectEncoding(_utf8TextFilePath);
            Assert.AreEqual(Encoding.UTF8, result);
        }

        [TestMethod]
        public void testUtf8CzechTextFileEncoding()
        {
            Encoding result = EncodingDetector.DetectEncoding(_utf8CzechTextFilePath);
            Assert.AreEqual(Encoding.UTF8, result);
        }
    }
}
