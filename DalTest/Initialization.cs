using DalApi;
using DO;
namespace DalTest;

public static class Initialization
{
    private static IWorker? s_dalWorker; //stage 1


    //בנוסף, נצטרך שדה אחד, שכל הישויות יעשו בו שימוש, ליצירת מספרים רנדומליים בזמן מילוי ערכי האובייקטים. 

    private static readonly Random s_rand = new();


    private static void creatWorker ()
    {

    }
}
