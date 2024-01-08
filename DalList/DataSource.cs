// פה צריך ללכת לשלוש ג ללכת לעשות שלוש שדות פה ווכו
namespace Dal;
using DO;
using DalApi;
internal static class DataSource
{
    internal static class Config
    {
        //Config to Assignments
        internal const int IdAssignments = 1;
        private static int IdPAssignments = IdAssignments;
        internal static int idPAssignments { get => IdPAssignments++; }

        //Config to link
        internal const int IdLink = 1;
        private static int IDLink = IdLink;
        internal static int idlink { get => IDLink++; }

        
        public static DateTime? dateSrart { get; set; } = null;
        public static DateTime? DateBegin { get; set; } = null;
    }
    //list of name,assigment,link
    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Assignments> Assignmentss { get; } = new();
    internal static List<DO.Link> Links { get; } = new();



}
