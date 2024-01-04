

namespace DO;

public record Assignments
(
    int IdAssignments,//מספר מזהה
    string Name,//כינוי
    string Description,//תיאור
    DateTime DateBegin,//תאריך מתוכנן לתחילת העבודה
    DateTime DeadLine,//תאריך מתוכנן לסיום 
    DateTime DateStart,//תאריך תחילת העבודה על המשימה
    DateTime DateFinish,//תאריך סיום בפועל
    int DurationAssignments,//משך הזמן הנדרש לביצוע המשימה
    int LevelAssignments,//רמת קושי משימה
    int IdWorker,//מזהה  המהנדס שהוקצה למשימה
    string Remarks,//הערות
    bool Milestone = false//אבן דרך
)
{
    public DateTime Date => DateTime.Now;//תאריך יצירת המשימה                                 
}


