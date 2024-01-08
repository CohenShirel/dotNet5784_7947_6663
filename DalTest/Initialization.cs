using DalApi;
using DO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
namespace DalTest;

public static class Initialization
{
    private static IWorker? s_dalWorker;
    private static IAssignments? s_dalAssignments; //stage 1
    private static ILink? s_dalLink; //stage 1

    private static readonly Random s_rand = new();


    //Initialization class from a public method, which will schedule
    //the private methods we have prepared and trigger the initialization of the lists.
    public static void Do(IWorker? dalWorker, IAssignments? dalAssignments, ILink? dalLinks)
    {
        s_dalWorker= dalWorker ?? throw new NullReferenceException("DAL can not be null!");
        s_dalAssignments = dalAssignments ?? throw new NullReferenceException("DAL can not be null!");
        s_dalLink = dalLinks ?? throw new NullReferenceException("DAL can not be null!");

    }

    private static void creatWorkers()
    {
        string[] WorkersNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
        };
        string[] WorkersEmail =
        {
        "Dani@gmail.com", "Eli@gmail.com", "Yair@gmail.com",
        "Ariela@gmail.com", "Dina@gmail.com", "Shira@gmail.com"
        };

        foreach (var _name in WorkersNames)
        {
            int index = 0;
            //id randomly
            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) != null);

            int _hourSalary = s_rand.Next(50, 600);

            // בחירת ערך רנדומלי מהמערך של Enums.Level
            Array levelArray = Enum.GetValues(typeof(Level));
            Level randomLevel = (Level)levelArray.GetValue(s_rand.Next(levelArray.Length));

            Worker newWork = new(_idW, randomLevel, _hourSalary,_name,WorkersEmail[index]);

            s_dalWorker!.Create(newWork);
            index++;
        }
    }
    private static void creatAssignmentss()
    {
        string[] AssignmentssNames =
        {
           "confirmation","budget_planning","Architectural_design", "Building_foundations", "Sewage_infrastructure",
           "Electrical_Systems", "floors","Climate_systems","Windows_doors","elevators","Emergency_systems",
           "Automation_networks","Finishes_colors","Safety_systems","Technological_spaces","parkingLots","yards","External_installations",
           "final_inspections","residents_Transferring"
        };
        foreach (var _name in AssignmentssNames)
        {
            int _durationA = s_rand.Next(50, 600);

            // בחירת ערך רנדומלי מהמערך של Enums.Level
            Array levelArray = Enum.GetValues(typeof(Level));
            Level randomLevel = (Level)levelArray.GetValue(s_rand.Next(levelArray.Length));

            //להוסיף כאן תאריך תחילת עבודה
            //לעשות את זה עם רנדומליות ולא על "עכשיו" נגריל על חודש אחורה
            //כדי לשא כולם יווצרו באותו הזמן

            //לתקן את הגרלת עובד
            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) == null);//if you find it!

            //לבדןק שוב את ההגרלות תאריכים
            //בין תאריך תחילת הפרויקט
            int rangeYears = s_rand.Next(1, 6);//randomly num on the range of year
            int rangeWeeks = s_rand.Next(1,13);//randomly num on the range of weeks
            int rangeDays = s_rand.Next(7, 57);//randomly num on the range of days-one week to 2 month
            DateTime _dateStart = DateTime.Now;//תאריך התחלה
            DateTime _dateFinish = _dateStart.AddYears(rangeYears);//תאריך סיום 
            DateTime _dateBegin = _dateStart.AddDays(-rangeDays);//תאריך מתוכנן לתחילת העבודה
            DateTime _deadLine = _dateFinish.AddDays(rangeDays);// תאריך מתוכנן לסיום

            string? _description = null;
            string? _remarks = null;
            string? _resultProduct = null;
            bool _milestone = false;
            Assignments newA = new(0, _durationA, randomLevel, _idW, _dateStart, _dateBegin, _deadLine,
                _dateFinish, _name, _description, _remarks, _resultProduct,_milestone);

            s_dalAssignments!.Create(newA);
        }

    }
    private static void creatLink()
    {
        s_dalLink!.Create(new Link(0, 0, 1));
        s_dalLink!.Create(new Link(0, 1, 2));
        s_dalLink!.Create(new Link(0, 2, 3));
        s_dalLink!.Create(new Link(0, 3, 4));
        s_dalLink!.Create(new Link(0, 3, 5));
        s_dalLink!.Create(new Link(0, 5, 10));
        s_dalLink!.Create(new Link(0, 10, 11));
        s_dalLink!.Create(new Link(0, 11, 13));
        s_dalLink!.Create(new Link(0, 13, 17));
        s_dalLink!.Create(new Link(0, 17, 14));
        s_dalLink!.Create(new Link(0, 18, 19));
    }
}
