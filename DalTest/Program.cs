//Shirel Cohen 214377947
//Neomi Golkin 325946663
using Dal;
using DalApi;
using DalFacade;
using DO;
using System;

namespace DalTest
{
    internal class Program
    {
        private static IWorker? s_dalWorker = new WorkerImplementation();
        private static IAssignments? s_dalAssignments = new AssignmentsImplementation();
        private static ILink? s_dalLinks = new LinkImplementation();

        static void Main(string[] args)
        {
            Initialization.Do(s_dalWorker, s_dalAssignments, s_dalLinks);

            Console.WriteLine("choose 1 for exit main menue");
            Console.WriteLine("choose 2 for creat a new object");
            Console.WriteLine("choose 3 for Object display by ID (Read)");
            Console.WriteLine("choose 4 for read all objects of the entity type");
            Console.WriteLine("choose 5 for updating existing object data");
            Console.WriteLine("choose 6 for deleting an existing object from a list");

            //Option op;
            do
            {
                Array OptionArray = Enum.GetValues(typeof(Option));
               // LevelWorker randomLevel = (LevelWorker)levelArray.GetValue(s_rand.Next(levelArray.Length));
                //op = (Option)Enum.Parse(typeof(Option), Console.ReadLine() ?? string.Empty);

                switch (OptionArray)
                {
                    case ADD:
                        // פעולות להוספה
                        break;
                    case READ:
                        // פעולות לקריאה
                        break;
                    case ReadAll:
                        // פעולות לקריאת כל האובייקטים
                        break;
                    case Update:
                        // פעולות לעדכון
                        break;
                    case Delete:
                        // פעולות למחיקה
                        break;
                    case Exit:
                        // יציאה
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (op != Option.Exit);
        }
    }
}