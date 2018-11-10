using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlFuzzing.Test
{
    class FuzzGenerator
    {
        private static string fuzzingFilePath;
        private static string fuzzingTemplateFile;
        private static readonly Random random = new Random();

        public static void Generate(string fuzzingFilePath, string fuzzingTemplateFile)
        {
            FuzzGenerator.fuzzingFilePath = fuzzingFilePath;
            FuzzGenerator.fuzzingTemplateFile = fuzzingTemplateFile;

            if (!Directory.Exists(fuzzingFilePath))
                Directory.CreateDirectory(fuzzingFilePath);

            //Generate files randomly fuzzed tags
            FileTagFuzzing(100);

            //Generate files with random names from valid template
            FileNameFuzzing(100);

            //Generate files with random fuzzed content
            FileContentFuzzing(100);

        }

        private static void FileNameFuzzing(int num)
        {
            for (int i = 0; i < num; i++)
            {
                string fileName = RandomString(100) + ".xml";
                string filePath = Path.Combine(fuzzingFilePath, fileName);

                File.Copy(fuzzingTemplateFile, filePath);
                Console.WriteLine("Write file: " + filePath);
            }
        }

        private static void FileContentFuzzing(int num)
        {
            for (int i = 0; i < num; i++)
            {
                using (var readStream = new StreamReader(fuzzingTemplateFile))
                {
                    string filePath = Path.Combine(fuzzingFilePath, "Fuzz_Content_" + i + ".xml");
                    using (var writeStream = new StreamWriter(filePath, false))
                    {
                        do
                        {
                            char ch = (char)readStream.Read();

                            if (random.Next(100) > 95)
                                ch = (char)random.Next(255);

                            writeStream.Write(ch);
                        } while (!readStream.EndOfStream);
                    }
                    Console.WriteLine("Write file: " + filePath);
                }
            }
        }

        private static void FileTagFuzzing(int num)
        {
            string content = string.Empty;
            using (var file = new StreamReader(fuzzingTemplateFile))
            {
                content = file.ReadToEnd();
            }

            List<string> tags = new List<string>() { 
            "Computers",
            "Computer",
            "name",
            "price"
            };

            foreach (var tag in tags)
            {
                for (int i = 0; i < num; i++)
                {
                    string filePath = Path.Combine(fuzzingFilePath, "Fuzz_"+tag +"_"+i+".xml");
                    using (var file = new StreamWriter(filePath, false))
                    {
                        string newContent = content.Replace(tag, RandomString(100));
                        file.Write(newContent);
                    }
                    Console.WriteLine("Write file: " + filePath);
                }
            }
        }

        private static string RandomString(int maxLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789+-.,!@#$%^&()_{}[]=`~";
            var stringChars = new char[1 + random.Next(maxLength-1)];

            for (byte i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        private static void WriteFile(string fileName, string content)
        {
            fileName += ".xml";
            string filePath = Path.Combine(fuzzingFilePath, fileName);

            using (var file = new StreamWriter(filePath, false))
            {
                file.Write(content);
            }
            Console.WriteLine("Write file: " + filePath);
        }
    }
}
