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
            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) != null);

            Worker newWork = new(_id, _name, _alias, _b, _bdt);

            Worker newWork = new(_idW, _name, WorkersEmail[index], _hourSalary, _experience);

            s_dalWorker!.Create(newWork);

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




            bool? _b = (_id % 2) == 0 ? true : false;

            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _bdt = start.AddDays(s_rand.Next(range));

        int _idA;
        do
            _idA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idA) == null);

        int _idPA;
        do
            _idPA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idPA) == null);

        Link newL = new(_idL, _idA, _idPA);
        s_dalLink!.Create(newL);
    }
}



    }
}
