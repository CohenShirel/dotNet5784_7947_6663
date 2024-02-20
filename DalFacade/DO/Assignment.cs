namespace DO;

/// <summary>
/// 
/// </summary>
/// <param מזהה מספר="IdAssignment"></param>
/// <param העבודה לתחילת מתוכנן תאריך="DateBegin"></param>
/// <param לסיום מתוכנן תאריך="DeadLine"></param>
/// <param המשימה על העבודה תחילת תאריך="DateStart"></param>
/// <param בפועל סיום תאריך="DateFinish"></param>
/// <param המשימה לביצוע הנדרש הזמן משך="DurationAssignment"></param>
/// <param משימה קושי רמת="LevelAssignment"></param>
/// <param למשימה שהוקצה המהנדס מזהה="WorkerId"></param>
/// <param כינוי="Name"></param>
/// <param תיאור="Description"></param>
/// <param הערות="Remarks"></param>
/// <param המשימה בסיום התוצר תוצאות תיאור="ResultProduct"></param>
/// <param דרן אבן="Milestone"></param>
public record Assignment
(

    int IdAssignment,
    int DurationAssignment,
    Level LevelAssignment,
    int WorkerId,
    DateTime? DateSrart = null,
    DateTime? DateBegin = null,
    DateTime? DeadLine = null,
    DateTime? DateFinish = null,
    string? Name = null,
    string? Description = null,
    string? Remarks = null,
    string? ResultProduct = null,
    bool Milestone = false

)
{

    //empty ctor
    public Assignment() : this(0, 0, 0, 0)
    {

    }
}


