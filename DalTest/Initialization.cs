using DalApi;
using DO;
using System.Security.Cryptography;
namespace DalTest;

public static class Initialization
{
    private static IWorker? s_dalWorker;
    private static IAssignments? s_dalAssignments; //stage 1
    private static ILink? s_dalLink; //stage 1
    

    //בנוסף, נצטרך שדה אחד, שכל הישויות יעשו בו שימוש, ליצירת מספרים רנדומליים בזמן מילוי ערכי האובייקטים. 

    private static readonly Random s_rand = new();


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
            //האם צריך להגששזדיר זאת גם למספר רץ?
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

            //האם צריך לתת תאריכים רנדומלים או  שככה זה מספיק?
            TimeSpan? _dateBegin = null;
            TimeSpan? _deadLine = null;
            TimeSpan? _dateStart = null;
            TimeSpan? _dateFinish = null;
            string? _description = null;
            string? _remarks = null;
            string? _resultProduct = null;
            bool _milestone = false;

            Assignments newA = new(_idA, _durationA, _idW,_level,_dateBegin, _deadLine, _dateStart,
                _dateFinish, _name, _description, _remarks, _resultProduct,_milestone);

            s_dalAssignments!.Create(newA);
        }

    }
    //את מי צריך לקשר כאן?
    private static void creatLink()
    {
        string[] AssignmentssNames =
        {
           "confirmation","budget_planning","Architectural_design", "Building_foundations", "Sewage_infrastructure",
           "Electrical_Systems", "floors","Climate_systems","Windows_doors","elevators","Emergency_systems",
           "Automation_networks","Finishes_colors","Safety_systems","Technological_spaces","parkingLots","yards","External_installations",
           "final_inspections","residents_Transferring"
        };
        int index = 0;
        for(int i=0;i<41 && index<20;i++)
        {
            int _idL;
            do
                _idL = s_rand.Next(200000000, 400000001);
            while (s_dalLink!.Read(_idL) != null);

            int _idPA = s_dalAssignments.ReadName(AssignmentssNames[index++]);
            int _idA = s_dalAssignments.ReadName(AssignmentssNames[index]);

            if (_idA!=0 && _idPA!=0)
            {
                Link newL = new(_idL, _idA, _idPA);
                s_dalLink!.Create(newL);
            }
           else
                throw new Exception("Assignments are not exists");
        }
           
    }
}
