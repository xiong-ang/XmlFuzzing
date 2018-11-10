using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlFuzzing
{
    public class XmlHandler
    {
        public XmlDocument XDoc { get; set; }
        public bool Import(string filePath)
        {
            XDoc = new XmlDocument();
            try
            {
                XDoc.Load(filePath);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
