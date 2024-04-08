using DalApi;
using System.Data.Common;
using System.Xml.Linq;

namespace Dal;

internal class ClockImplemetation : IClock
{
    private readonly string s_clock_xml = "data-config";

    public void FormatClockOneDay()
    {
        DateTime? startProject = GetStartProject(); // קריאה לשעה הנוכחית מהמסד נתונים
        if (startProject != null)
        {
            DateTime newTime = startProject.Value.AddDays(1); // הוספת שעה לשעה הנוכחית
            SetStartProject(newTime); // שמירת השעה החדשה במסד הנתונים
        }
    }

    public void FormatClockOneHour()
    {
        DateTime? startProject = GetStartProject(); // קריאה לשעה הנוכחית מהמסד נתונים
        if (startProject != null)
        {
            DateTime newTime = startProject.Value.AddHours(1); // הוספת שעה לשעה הנוכחית
            SetStartProject(newTime); // שמירת השעה החדשה במסד הנתונים
        }
    }

    public void FormatClockOneMonth()
    {
        DateTime? startProject = GetStartProject(); // קריאה לשעה הנוכחית מהמסד נתונים
        if (startProject != null)
        {
            DateTime newTime = startProject.Value.AddMonths(1); // הוספת שעה לשעה הנוכחית
            SetStartProject(newTime); // שמירת השעה החדשה במסד הנתונים
        }
    }

    public void FormatClockOneYear()
    {
        DateTime? startProject = GetStartProject(); // קריאה לשעה הנוכחית מהמסד נתונים
        if (startProject != null)
        {
            DateTime newTime = startProject.Value.AddYears(1); // הוספת שעה לשעה הנוכחית
            SetStartProject(newTime); // שמירת השעה החדשה במסד הנתונים
        }
    }
    public DateTime? GetStartProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml).Element("StartProject")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);
    }
    public void resetClock()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("StartProject")!.Value = "";
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }
    public DateTime? SetStartProject(DateTime startProject)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("StartProject")!.Value = startProject.ToString();
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
        return startProject;
    }
}
