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

        internal static DateTime? StartDate = DateTime.Now;
    }
    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Assignment> Assignmentss { get; } = new();
    internal static List<DO.Link> Links { get; } = new();
}
