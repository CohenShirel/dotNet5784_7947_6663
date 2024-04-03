// פה צריך ללכת לשלוש ג ללכת לעשות שלוש שדות פה ווכו
namespace Dal;
internal static class DataSource
{
    internal static class Config
    {
        //Config to Assignment
        internal const int IdAssignments = 1;
        private static int IdPAssignments = IdAssignments;
        internal static int idPAssignments { get => IdPAssignments++; }

        //Config to link
        internal const int IdLink = 1;
        private static int IDLink = IdLink;
        internal static int idlink { get => IDLink++; }
        // public static DateTime? dateStart { get; set; } = null;
        // internal static DateTime?
        // Time { get; set; } = DateTime.Now;
        internal static DateTime? StartDate = DateTime.Now;
        //internal static DateTime? EndDate = null;
    }
    //list of name,assigment,link
    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Assignment> Assignmentss { get; } = new();
    internal static List<DO.Link> Links { get; } = new();
}
