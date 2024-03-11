using DalApi;
using DO;
namespace DalTest;

public static class Initialization
{
    private static IDal? s_dal;//stage 2
    //private static IWorker? s_dalWorker;//stage 1
    //private static IAssignments? s_dalAssignments; //stage 1
    //private static ILink? s_dalLink; //stage 1

    private static readonly Random s_rand = new();


    //Initialization class from a public method, which will schedule
    //the private methods we have prepared and trigger the initialization of the lists.
    public static void Do()
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = Factory.Get; //stage 4  
    }
    public static void Initialize()
    {
        Do();
        creatWorkers();
        creatAssignmentss();
        creatLink();
    }
    //function that creat random workers
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
        int index = 0;
        foreach (var _name in WorkersNames)
        {
            //id randomly
            int _idW;
            _idW = s_dal!.Worker != null ? s_rand.Next(200000000, 400000001) : 0;
            if( _idW == 0 )
                do
                    _idW = s_rand.Next(200000000, 400000001);
                while (s_dal.Worker!.Read(a => a.Id == _idW) != null);
            int _hourSalary = s_rand.Next(50, 600);

            // בחירת ערך רנדומלי מהמערך של Enums.Level
            Array levelArray = Enum.GetValues(typeof(Level));
            Level randomLevel = (Level)levelArray.GetValue(s_rand.Next(levelArray.Length))!;

            Worker newWork = new(_idW, randomLevel, _hourSalary, _name, WorkersEmail[index++]);

            s_dal!.Worker.Create(newWork);
        }
    }
    //function that creat random assignment
    private static void creatAssignmentss()
    {
        string[] AssignmentssNames =
        {
           "confirmation","budget_planning","Architectural_design", "Building_foundations", "Sewage_infrastructure",
           "Electrical_Systems","walls","ceilings" ,"paving","Climate_systems","Windows_doors","elevators","Emergency_systems","plumbing",
           "interior_design","Finishes_colors","Automation_networks","Safety_systems","External_installations","Technological_spaces",
           "final_inspections","4Form","residents_Transferring"
        };
        foreach (var _name in AssignmentssNames)
        {
            int _durationA = s_rand.Next(50, 600);

            // choose random level from Enums.Level
            Array levelArray = Enum.GetValues(typeof(Level));
            Level randomLevel = (Level)levelArray.GetValue(s_rand.Next(levelArray.Length-1))!;//?


            //I make new array just with all the worker with the same Experience
            List<Worker> workersLst = new List<Worker>();
            foreach (var worker in s_dal.Worker!.ReadAll())
            {
                if (worker.Experience == randomLevel)
                    workersLst.Add(worker);
            }
            int ID = workersLst.Count!=0?workersLst[s_rand.Next(workersLst.Count)].Id:0;
            //DateTime? dat = DateTime.FromDateTime(DateTime.Now);// help date
            DateTime? dat = DateTime.Now;

            DateTime? _dateStart = dat?.AddDays(s_rand.Next(30, 61));//random date for "datestart" in range of month=תאריך מתוכנן להתחלה
            DateTime? _deadLine = _dateStart?.AddYears(s_rand.Next(1, 4));// "_dateBegin" late in month from datestart=תאריך מתוכנן לסיום
            DateTime? _dateBegin = _dateStart?.AddDays(s_rand.Next(30, 61));//when we are start=תאריך התחלה בפועל
            DateTime? _dateFinish = _deadLine?.AddDays(s_rand.Next(30, 61));//when we are finish=,תאריך סיום בפועל
            string? _description = null;
            string? _remarks = null;
            string? _resultProduct = null;
            bool _milestone = false;
            Assignment newA = new(0, _durationA, randomLevel, ID, _dateStart, _dateBegin, _deadLine,
                _dateFinish, _name, _description, _remarks, _resultProduct, _milestone);

            s_dal.Assignment!.Create(newA);
        }

    }
    //function that creat random links
    private static void creatLink()
    {
        s_dal.Link!.Create(new Link(0, 1, 2));
        s_dal.Link!.Create(new Link(0, 2, 3));
        s_dal.Link!.Create(new Link(0, 3, 4));
        s_dal.Link!.Create(new Link(0, 4, 5));
        s_dal.Link!.Create(new Link(0, 4, 6));
        s_dal.Link!.Create(new Link(0, 5, 7));
        s_dal.Link!.Create(new Link(0, 5, 8));
        s_dal.Link!.Create(new Link(0, 6, 7));
        s_dal.Link!.Create(new Link(0, 6, 8));
        s_dal.Link!.Create(new Link(0, 7, 9));
        s_dal.Link!.Create(new Link(0, 7, 10));
        s_dal.Link!.Create(new Link(0, 7, 11));
        s_dal.Link!.Create(new Link(0, 7, 12));
        s_dal.Link!.Create(new Link(0, 7, 13));
        s_dal.Link!.Create(new Link(0, 7, 14));
        s_dal.Link!.Create(new Link(0, 8, 9));
        s_dal.Link!.Create(new Link(0, 8, 10));
        s_dal.Link!.Create(new Link(0, 8, 11));
        s_dal.Link!.Create(new Link(0, 8, 12));
        s_dal.Link!.Create(new Link(0, 8, 13));
        s_dal.Link!.Create(new Link(0, 8, 14));
        s_dal.Link!.Create(new Link(0, 9, 15));
        s_dal.Link!.Create(new Link(0, 9, 17));
        s_dal.Link!.Create(new Link(0, 10, 15));
        s_dal.Link!.Create(new Link(0, 10, 17));
        s_dal.Link!.Create(new Link(0, 11, 15));
        s_dal.Link!.Create(new Link(0, 11, 17));
        s_dal.Link!.Create(new Link(0, 12, 15));
        s_dal.Link!.Create(new Link(0, 12, 17));
        s_dal.Link!.Create(new Link(0, 13, 15));
        s_dal.Link!.Create(new Link(0, 13, 17));
        s_dal.Link!.Create(new Link(0, 14, 15));
        s_dal.Link!.Create(new Link(0, 14, 17));
        s_dal.Link!.Create(new Link(0, 15, 16));
        s_dal.Link!.Create(new Link(0, 17, 18));
        s_dal.Link!.Create(new Link(0, 18, 19));
        s_dal.Link!.Create(new Link(0, 19, 20));
        s_dal.Link!.Create(new Link(0, 20, 21));
        s_dal.Link!.Create(new Link(0, 21, 22));
        s_dal.Link!.Create(new Link(0, 22, 23));
        s_dal.Link!.Create(new Link(0, 16, 21));
    }
}
