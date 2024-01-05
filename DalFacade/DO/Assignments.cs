

namespace DO;
/// <summary>
/// 
/// </summary>
/// <param מזהה מספר="IdAssignments"></param>
/// <param העבודה לתחילת מתוכנן תאריך="DateBegin"></param>
/// <param לסיום מתוכנן תאריך="DeadLine"></param>
/// <param המשימה על העבודה תחילת תאריך="DateStart"></param>
/// <param בפועל סיום תאריך="DateFinish"></param>
/// <param המשימה לביצוע הנדרש הזמן משך="DurationAssignments"></param>
/// <param משימה קושי רמת="LevelAssignments"></param>
/// <param למשימה שהוקצה המהנדס מזהה="IdWorker"></param>
/// <param כינוי="Name"></param>
/// <param תיאור="Description"></param>
/// <param הערות="Remarks"></param>
/// <param המשימה בסיום התוצר תוצאות תיאור="ResultProduct"></param>
/// <param דרן אבן="Milestone"></param>
public record Assignments
(
    int IdAssignments,
    int DurationAssignments,
    int LevelAssignments,
    int IdWorker,
    TimeSpan? DateBegin=null,
    TimeSpan? DeadLine = null,
    TimeSpan? DateStart = null,
    TimeSpan? DateFinish = null ,
    string? Name = null,
    string? Description = null,
    string? Remarks = null,
    string? ResultProduct = null,
    bool Milestone = false
)
{
    public DateTime Date => DateTime.Now;//תאריך יצירת המשימה         
    //empty ctor
    public Assignments() : this(0,0,0,0)
    {

    }                  
}


