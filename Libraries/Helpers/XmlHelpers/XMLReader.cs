using System.Xml.Linq;

namespace OnlineStore.Libraries.Helpers.XmlHelpers
{
    public static class XMLReader
    {
        public static string GetDataFromXMLFile(string path, string xmlTag) => XElement.Load(path).Element(xmlTag).Value;
    }
}