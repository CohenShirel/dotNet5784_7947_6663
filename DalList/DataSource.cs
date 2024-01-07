﻿// פה צריך ללכת לשלוש ג ללכת לעשות שלוש שדות פה ווכו
namespace Dal;
using DO;
using DalApi;
internal static class DataSource
{
    internal static class Config
    {
        internal const int IdAssignments = 1000;
        private static int IdPAssignments = IdAssignments;
        internal static int idPAssignments { get => IdPAssignments++; }
    }
    //לעשות קונפיג גם על תאריך?עמוד 8 למעלה בתיאור הכללי
    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Assignments> Assignmentss { get; } = new();
    internal static List<DO.Link> Links { get; } = new();



}