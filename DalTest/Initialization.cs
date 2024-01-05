ששזusing DalApi;
using DO;
namespace DalTest;

public static class Initialization
{
    private static IWorker? s_dalWorker;
    private static IAssignments? s_dalAssignments; //stage 1
    private static ILink? s_dalLink; //stage 1


    //בנוסף, נצטרך שדה אחד, שכל הישויות יעשו בו שימוש, ליצירת מספרים רנדומליים בזמן מילוי ערכי האובייקטים. 

    private static readonly Random s_rand = new();


    private static void creatWorkers ()
    {
        string[] WorkersNames =
        {
           "Dani Levi", "Eli Amar", "Yair Cohen",
           "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in WorkersNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_id) != null);

            Worker newWork = new(_id, _name, _alias, _b, _bdt);

            s_dalWorker!.Create(newWork);








            bool? _b = (_id % 2) == 0 ? true : false;

            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _bdt = start.AddDays(s_rand.Next(range));

            string? _alias = (_id % 2) == 0 ? _name + "ALIAS" : null;

          
        }

    }
    //int IdWorker,
   /// int Experience,
    //double HourSalary,
   // string? Name = null,
   // string? Email = null

    private static void creatAssignmentss()
    {

    }
    private static void creatLnk()
    {

    }
}
