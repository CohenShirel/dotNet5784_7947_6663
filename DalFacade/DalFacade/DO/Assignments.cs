

namespace DO;

public record Assignments
(
    int Id,//מספר מזהה
    string Name,//כינוי
    string Description,//תיאור
    bool Milestone,//אבן דרך
    DateTime DateStart,//תאריך מתוכנן לתחילת העבודה
    DateTime DateFinish,//תאריך סיום בפועל
    DateTime DeadLine,//תאריך סיום אחרון אפשרי 

    int LevelAssignments,//רמת קושי משימה

    int IdWorker,//מזהה  המהנדס שהוקצה למשימה
    int DurationAssignments,//משך הזמן הנדרש לביצוע המשימה
    string Remarks //הערות

                                 //תוצר?          
)
{
    public DateTime Date => DateTime.Now;//תאריך יצירת המשימה   


}


