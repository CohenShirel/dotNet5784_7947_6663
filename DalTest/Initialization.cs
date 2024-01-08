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

            // בחירת ערך רנדומלי מהמערך של Enums.LevelWorkers
            Array levelArray = Enum.GetValues(typeof(LevelWorker));
            LevelWorker randomLevel = (LevelWorker)levelArray.GetValue(s_rand.Next(levelArray.Length));

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
            //האם צריך להגדיר זאת גם למספר רץ?
            int _idA;
            do
                _idA = s_rand.Next(200000000, 400000001);
            while (s_dalAssignments!.Read(_idA) != null);

            int _durationA = s_rand.Next(50, 600);
            int _levelA = s_rand.Next(0, 11);

            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) == null);//if you find it!

           int _level = s_rand.Next(0, 11);

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

            Assignments newA = new(_idA, _durationA, _idW,_level,_dateBegin, _deadLine,
                _dateFinish, _name, _description, _remarks, _resultProduct,_milestone);

            s_dalAssignments!.Create(newA);
        }

    }
    //את מי צריך לקשר כאן?
    private static void creatLink()
    {
        //האם צריך להגדיר זאת גם למספר רץ?
        string[] AssignmentssNames2 =
        {
           "floors","Climate_systems","Windows_doors",
            "elevators","Finishes_colors","final_inspections","residents_Transferring","parkingLots","yards",

        };
        string[] AssignmentssNames1 =
        {
           "confirmation","budget_planning","Architectural_design", "Building_foundations",
           "Electrical_Systems","Emergency_systems","Automation_networks","Safety_systems",
            "External_installations","Technological_spaces",
        };
        int index = 1,_idL, _idPA, _idA;
        foreach (var _name in AssignmentssNames1.Take(AssignmentssNames1.Length-1))
        {
            do
                _idL = s_rand.Next(200000000, 400000001);
            while (s_dalLink!.Read(_idL) != null);
            _idPA = s_dalAssignments.ReadName(_name);
            _idA = s_dalAssignments.ReadName(AssignmentssNames1[index++]);
            if (_idA != 0 && _idPA != 0)
            {
                Link newL = new(_idL, _idA, _idPA);
                s_dalLink!.Create(newL);
            }
            else
                throw new Exception("Assignments are not exists");
        }
        //האם אתה מספר רץץץץץץץץ
        do
            _idL = s_rand.Next(200000000, 400000001);
        while (s_dalLink!.Read(_idL) != null);
        _idPA = s_dalAssignments.ReadName("Building_foundations");
        _idA = s_dalAssignments.ReadName("Sewage_infrastructure");
        if (_idA != 0 && _idPA != 0)
        {
            Link newL = new(_idL, _idA, _idPA);
            s_dalLink!.Create(newL);
        }
        else
            throw new Exception("Assignments are not exists");
        //connnnnnnnnn
        //האם אתה מספר רץץץץץץץץ
        do
            _idL = s_rand.Next(200000000, 400000001);
        while (s_dalLink!.Read(_idL) != null);
        _idPA = s_dalAssignments.ReadName("final_inspections");
        _idA = s_dalAssignments.ReadName("residents_Transferring");
        if (_idA != 0 && _idPA != 0)
        {
            Link newL = new(_idL, _idA, _idPA);
            s_dalLink!.Create(newL);
        }
        else
            throw new Exception("Assignments are not exists");


    }
}
