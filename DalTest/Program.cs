//Shirel Cohen 214377947
//Neomi Golkin 325946663


using Dal;
using DalApi;
using DO;
namespace DalTest;
using DO;

internal class Program
{
    static void Main(string[] args)
    {
        Initialization.Do(s_dalWorker, s_dalAssignments, s_dalLinks);
        private static IWorker? s_dalWorker = new WorkerImplementation();
        private static IAssignments? s_dalAssignments = new AssignmentsImlementation();
        private static ILink? s_dalLinks = new LinkImplementation();
        Console.WriteLine("choose 1 for exit main menu");
        Console.WriteLine("choose 2 for creat a new object");
        Console.WriteLine("choose 3 for Object display by ID (Read)");
        Console.WriteLine("choose 4 for read all objects of the entity type");
        Console.WriteLine("choose 5 for updating existing object data");
        Console.WriteLine("choose 6 for deleting an existing object from a list");
        do
        {כככיכ
            int op1 = Console.ReadLine();
            do
            {
                case ADD:
                break;
                case READ:
                break;
                case ReadAll:
                break;
                case Update:
                break;
                case Delete:
                break;
                default:
                Console.WriteLine("ERROR");
			    break;
           }
        }while(op!=Exit)
        //Exception
    }



    
}
