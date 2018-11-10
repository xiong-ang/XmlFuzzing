using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XmlFuzzing;

namespace XmlFuzzing.Test
{
    [TestClass]
    public class FuzzImportTest
    {
        private void GenerateFiles(string fuzzingFilePath, string fuzzingTemplateFile)
        {
            FuzzGenerator.Generate(fuzzingFilePath, fuzzingTemplateFile);
        }

        private static void WriteLogs(string logFile, string logMsg)
        {
            using (var sw = File.AppendText(logFile))
            {
                sw.WriteLine(DateTime.Now + " - " + logMsg);
            }
        }

        [TestMethod]
        public void TestImport()
        {
            string fuzzingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FuzzingTest");
            string fuzzingTemplateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template.xml");
            string fuzzingLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

            //Create Fuzzing files based on template
            GenerateFiles(fuzzingFilePath, fuzzingTemplateFile);

            //Obtain Fuzzing files
            List<string> fileList = new List<string>(Directory.EnumerateFiles(fuzzingFilePath));
            //fileList.AddRange(Directory.EnumerateFiles(fuzzingFilePath2));

            //Test import and record logs
            int count = 0;
            foreach (var file in fileList)
            {
                var fileInfo = new FileInfo(file);
                if (!string.Equals(fileInfo.Extension, ".xml", StringComparison.OrdinalIgnoreCase)) continue;

                XmlHandler xmlHandler = new XmlHandler();
                try
                {
                    xmlHandler.Import(fileInfo.FullName);
                    WriteLogs(fuzzingLogFile, "Import " + fileInfo.FullName + " succeed!");
                    count++;
                }
                catch (Exception ex)
                {
                    WriteLogs(fuzzingLogFile, ex.ToString());
                }
            }
            WriteLogs(fuzzingLogFile, "Fuzzing test done for " + count + " files of " + fileList.Count);
        }
    }
}
