//Shirel Cohen 214377947
//Neomi Golkin 325946663


using Dal;
using DalApi;
using DO;
namespace DalTest;

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
        {
            int op1 = Console.ReadLine();
            do
            {
               Console.WriteLine("choose 1-Worker,2-Assignments,3-Link");
               int op2 = Console.ReadLine();
}while(op2<1||op2>3)

            switch(op1)
            {
               try
              {
                case ADD:
               {
                if(op2==1)
                { 

                }
                else if (op2 == 2)
{

}
else
{

}
break;
             }
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
              catch(Exception ex)
              {
                   Console.WriteLine(ex.Message);
              }
           }
        }while (op1 != Exit)
        //Exception
    }



    
}
