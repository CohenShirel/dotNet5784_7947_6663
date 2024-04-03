using System.Xml.Linq;

namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int IdPAssignments { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "IdPAssignments"); }
    internal static int IDLink { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "IDLink"); }
    internal static DateTime? StartProjectTime { get; set; } = DateTime.Now;

    //public static DateTime Clock
    //{
    //    get { return LoadDateTimeFromXml(s_data_config_xml, "Clock") ?? DateTime.Now; }
    //    set { SaveDateTimeToXml(s_data_config_xml, "Clock", value); }
    //}

    //private static DateTime? LoadDateTimeFromXml(string xmlFileName, string elementName)
    //{
    //    try
    //    {
    //        string filePath = Path.Combine(Environment.CurrentDirectory, xmlFileName + ".xml");
    //        if (File.Exists(filePath))
    //        {
    //            XDocument doc = XDocument.Load(filePath);
    //            XElement element = doc.Descendants(elementName).FirstOrDefault();
    //            if (element != null)
    //            {
    //                if (DateTime.TryParse(element.Value, out DateTime dateTime))
    //                {
    //                    return dateTime;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error loading datetime from XML: {ex.Message}");
    //    }
    //    return null;
    //}

    //private static void SaveDateTimeToXml(string xmlFileName, string elementName, DateTime? dateTime)
    //{
    //    try
    //    {
    //        string filePath = Path.Combine(Environment.CurrentDirectory, xmlFileName + ".xml");
    //        XDocument doc;
    //        if (File.Exists(filePath))
    //        {
    //            doc = XDocument.Load(filePath);
    //        }
    //        else
    //        {
    //            doc = new XDocument();
    //            doc.Add(new XElement(xmlFileName));
    //        }
    //        XElement element = doc.Descendants(elementName).FirstOrDefault()!;
    //        if (element == null)
    //        {
    //            element = new XElement(elementName);
    //            doc.Root.Add(element);
    //        }
    //        element.SetValue(dateTime?.ToString() ?? "");
    //        doc.Save(filePath);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error saving datetime to XML: {ex.Message}");
    //    }
    //}
}





