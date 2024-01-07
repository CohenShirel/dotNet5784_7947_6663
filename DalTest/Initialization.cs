using DalApi;
using DO;
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
<<<<<<< HEAD
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
     };
        string[] WorkersEmail =
       {
        "Dani@gmail.com", "Eli@gmail.com", "Yair@gmail.com",
        "Ariela@gmail.com", "Dina@gmail.com", "Shira@gmail.com"
     };

=======
           "Dani Levi", "Eli Amar", "Yair Cohen",
           "Ariela Levin", "Dina Klein", "Shira Israelof"
        };
        string[] WorkersEmail =
       {
           "Dani@gmail.com", "Eli@gmail.com", "Yair@gmail.com",
           "Ariela@gmail.com", "Dina@gmail.com", "Shira@gmail.com"
        };
     
>>>>>>> 3a32a4a9072af93759e46e193dc3d60d765a5a90
        foreach (var _name in WorkersNames)
        {
            int index = 0;
            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) != null);

            int _hourSalary = s_rand.Next(50, 600);
<<<<<<< HEAD
            int _experience = s_rand.Next(0, 60);
=======
            int _experience= s_rand.Next(0,60);
>>>>>>> 3a32a4a9072af93759e46e193dc3d60d765a5a90

            Worker newWork = new(_idW, _name, WorkersEmail[index], _hourSalary, _experience);

            s_dalWorker!.Create(newWork);
            index++;
<<<<<<< HEAD
        }
    }
    private static void creatAssignmentss()
    {
        string[] AssignmentssNames =
        {
        "confirmation","budget_planning","Architectural_design", "Building_foundations", "Sewage_infrastructure",
        "Electrical_Systems", "walls_roofs","Climate_systems","Windows_doors","elevators","Emergency_systems",
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
            int _levelA = s_rand.Next(50, 600);

            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) == null);//if you find it!

            //האם צריך לתת תאריכים רנדומלים או  שככה זה מספיק?
            TimeSpan? _dateBegin = null;
            TimeSpan? _deadLine = null;
            TimeSpan? _dateStart = null;
            TimeSpan? _dateFinish = null;
            string? _description = null;
            string? _remarks = null;
            string? _resultProduct = null;
            bool _milestone = false;

            Assignments newA = new(_idA, _durationA, _idW, _dateBegin, _deadLine, _dateStart,
                _dateFinish, _name, _description, _remarks, _resultProduct, _milestone);

            s_dalAssignments!.Create(newA);
=======
>>>>>>> 3a32a4a9072af93759e46e193dc3d60d765a5a90
        }
    }
<<<<<<< HEAD
    private static void creatLink()
    {
        int _idL;
        do
            _idL = s_rand.Next(200000000, 400000001);
        while (s_dalLink!.Read(_idL) != null);

        int _idA;
        do
            _idA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idA) == null);

=======
    private static void creatAssignmentss()
    {
        string[] AssignmentssNames =
        {
           "confirmation","budget_planning","Architectural_design", "Building_foundations", "Sewage_infrastructure",
           "Electrical_Systems", "walls_roofs","Climate_systems","Windows_doors","elevators","Emergency_systems",
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
            int _levelA = s_rand.Next(50, 600);

            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) == null);//if you find it!

            //האם צריך לתת תאריכים רנדומלים או  שככה זה מספיק?
            TimeSpan? _dateBegin = null;
            TimeSpan? _deadLine = null;
            TimeSpan? _dateStart = null;
            TimeSpan? _dateFinish = null;
            string? _description = null;
            string? _remarks = null;
            string? _resultProduct = null;
            bool _milestone = false;

            Assignments newA = new(_idA, _durationA, _idW, _dateBegin, _deadLine, _dateStart,
                _dateFinish, _name, _description, _remarks, _resultProduct, _milestone);

            s_dalAssignments!.Create(newA);
        }
           
    }
    private static void creatLink()
    {
        int _idL;
        do
            _idL = s_rand.Next(200000000, 400000001);
        while (s_dalLink!.Read(_idL) != null);

        int _idA;
        do
            _idA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idA) == null);

>>>>>>> 3a32a4a9072af93759e46e193dc3d60d765a5a90
        int _idPA;
        do
            _idPA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idPA) == null);

        Link newL = new(_idL, _idA, _idPA);
        s_dalLink!.Create(newL);
    }
    
}



// confirmation   
//budget planning  
//Architectural design  
//Building foundations   
//Sewage infrastructure   
//Electrical Systems      
//walls_roofs             
//Climate systems         
//Windows and doors        
//elevators               
//Emergency systems       
//Automation networks    
//Finishes and colors     
//Safety systems          
//Technological spaces    
//parking lots  
//yards          
//External installations  
//Final inspections       
<<<<<<< HEAD
//transfer to residents
=======
//transfer to residents   
>>>>>>> 3a32a4a9072af93759e46e193dc3d60d765a5a90
