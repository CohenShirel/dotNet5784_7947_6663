
namespace BO;

//עשיתי את זה גם לרמת משימה וגם לרמת עובד
public enum Level
{
    Beginner, AdvancedBeginner, Intermediate, Advanced, Expert
}
//level of the project:
public enum Status
{
    //יש משימות 
    //יש משימות עם תאריך
    //ביצוע המשימות
    //רלוונטי ללוז אוטומטי**********************
    //הסתיים
    Unscheduled, Scheduled,OnTrack,InJeopardy,Done
}