using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlFuzzing
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlHandler xmlHandler = new XmlHandler();
            Console.WriteLine(xmlHandler.Import(AppDomain.CurrentDomain.BaseDirectory + "test.xml"));
        }
    }
}