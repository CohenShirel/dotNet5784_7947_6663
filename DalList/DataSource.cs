

namespace Dal;
//using DO;
internal static class DataSource
{
    internal static class Config
    {
        /*
         ללכת לחמש ב דחוףףףףףףףףףףףףףף
         * עבור כל מספר מזהה רץ שנחוץ לכם לצורך הישויות, הוסיפו למחלקה Config שלוש הגדרות:
שדה מספרי קבוע (const), בהרשאת internal, שיקבל ערך התחלתי למספר הרץ: המספר המזהה הקטן ביותר לפי דרישת כל ישות.
שדה מספרי סטטי (static), בהרשאת private, שיקבל כערך התחלתי את השדה הקבוע הקודם.
הוסיפו מאפיין עם get בלבד שיקדם את השדה הפרטי אוטומטית,  במספר שגדול מהקודם ב-1
*/
    }
    internal static List<DO.Worker> Workers { get; } = new();

}