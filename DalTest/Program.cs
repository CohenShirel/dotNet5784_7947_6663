//Shirel Cohen 214377947
//Neomi Golkin 325946663
//עשינו את התוספת של TryParse
using Dal;
using DalApi;
using DalFacade;
using DO;
using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Xml.Linq;
namespace DalTest
{
    public enum ENTITY
    {
        EXIT,
        WORKER,
        LINK,
        ASSIGNMENT
    }
    public enum CRUD
    {
        EXIT,
        CREATE,
        READ,
        READ_ALL,
        UPDATE,
        DELETE
    }
    //rivate static void creatLink(out Link link,int id=0)
    //{
    //    Console.WriteLine("enter AssignmentsID of the link: ");
    //    if (!int.TryParse(Console.ReadLine(), out int idAssigment1))
    //        throw new FormatException("Wrong input");

    //    Console.WriteLine("enter AssignmentsID of the link: ");
    //    if (!int.TryParse(Console.ReadLine(), out int idAssigment2))
    //        throw new FormatException("Wrong input");

    //    link = new Link(id, idAssigment1, idAssigment2);
    //}
    internal class Program
    {
        private static readonly Random s_rand = new();

        private static IWorker? s_dalWorker = new WorkerImplementation();
        private static IAssignments? s_dalAssignments = new AssignmentsImplementation();
        private static ILink? s_dalLinks = new LinkImplementation();
        
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalWorker, s_dalAssignments, s_dalLinks);


                //Console.WriteLine("choose 0 for exit main menu");
                //Console.WriteLine("choose 1 for Worker");
                //Console.WriteLine("choose 2 for Link");
                //Console.WriteLine("choose 3 for ASSIGNMENT");


                ENTITY myChoice;// = ENTITY.ASSIGNMENT;
                do
                {
                    Console.WriteLine("choose 0 for exit main menu");
                    Console.WriteLine("choose 1 for Worker");
                    Console.WriteLine("choose 2 for Link");
                    Console.WriteLine("choose 3 for ASSIGNMENT");
                    //Array OptionArray = Enum.GetValues(typeof(ENTITY));
                    // LevelWorker randomLevel = (LevelWorker)levelArray.GetValue(s_rand.Next(levelArray.Length));
                    //op = (Option)Enum.Parse(typeof(Option), Console.ReadLine() ?? string.Empty);
                    if (Enum.TryParse(Console.ReadLine(), out myChoice))
                    {
                        switch (myChoice)
                        {
                            case ENTITY.EXIT:
                                // יציאה
                                break;
                            case ENTITY.WORKER:
                                // פעולות לקריאת כל האובייקטים
                                optionWorker();
                                break;
                            case ENTITY.LINK:
                                // פעולות לעדכון
                                optionLink();
                                break;
                            case ENTITY.ASSIGNMENT:
                                optionAssignment();
                                // פעולת למחיקה
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }

                } while (myChoice != ENTITY.EXIT);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }
        private static void optionWorker()
        {
            CRUD myChoice;

            do
            {
                Console.WriteLine("Choose operation:");
                Console.WriteLine("1 - Create");
                Console.WriteLine("2 - Read");
                Console.WriteLine("3 - Read All");
                Console.WriteLine("4 - Update");
                Console.WriteLine("5 - Delete");
                Console.WriteLine("0 - Exit");

                // Get input from the user and try to parse it to the Enum type
                if (Enum.TryParse(Console.ReadLine(), out myChoice))
                {
                    switch (myChoice)
                    {
                        case CRUD.CREATE:
                            // Perform create operation
                            Console.WriteLine("Enter worker ID, Cost, name and Email, level : ");
                            if (!int.TryParse(Console.ReadLine(), out int id))
                                throw new Exception("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int cost))
                                throw new Exception("Wrong input");
                            string? name = Console.ReadLine() ?? throw new Exception("Wrong input");
                            string? email = Console.ReadLine() ?? throw new Exception("Wrong input");
                            Level level = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");

                            s_dalWorker!.Create(new(id, level, cost, name, email));
                            
                            break;
                        case CRUD.READ:
                            // Perform read operation
                        
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new Exception("Wrong input"); 
                            Worker? readWorker = s_dalWorker!.Read(ID);
                            Console.WriteLine(readWorker);
                            break;
                        case CRUD.READ_ALL:
                            // Perform read all operation
                            foreach(var Workers in s_dalWorker!.ReadAll())
                            {
                                Console.WriteLine(Workers);
                            }
                            break;
                        case CRUD.UPDATE:
                            // Perform update operation
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int iD))
                                throw new Exception("Wrong input");
                            Worker updatedWorker = s_dalWorker!.Read(iD)! ?? throw new Exception($"Can't update, worker does not exist!!");
                            Console.WriteLine(updatedWorker);
                            Console.WriteLine("Please update the details -- name, email, level .\n");
                            string? updatedName = Console.ReadLine() ?? throw new Exception("Wrong input");
                            string? updetedEmail = Console.ReadLine() ?? throw new Exception("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int updatedCost))
                                throw new Exception("Wrong input");
                            Level updatedLevel = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            Worker worker = new Worker(iD, updatedLevel, updatedCost, updatedName, updetedEmail);
                            s_dalWorker!.Update(ref worker);

                            break;
                        case CRUD.DELETE:
                            // Perform delete operation
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int Id))
                                throw new Exception("Wrong input"); 
                            s_dalWorker!.Delete(Id);
                                break;
                        case CRUD.EXIT:
                            Console.WriteLine("Exiting program");
                            break;
                        default:
                            Console.WriteLine("ERROR");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

            } while (myChoice != CRUD.EXIT);



        }
        private static void optionLink()
        {
            CRUD myChoice;

            do
            {
                Console.WriteLine("Choose operation:");
                Console.WriteLine("1 - Create");
                Console.WriteLine("2 - Read");
                Console.WriteLine("3 - Read All");
                Console.WriteLine("4 - Update");
                Console.WriteLine("5 - Delete");
                Console.WriteLine("0 - Exit");

                // Get input from the user and try to parse it to the Enum type
                if (Enum.TryParse(Console.ReadLine(), out myChoice))
                {
                    switch (myChoice)
                    {
                        case CRUD.CREATE:
                            int idAssigment1, idAssigment2;
                            //it will get into the loop *there are item in the s_dalAssignments **if idAssigment1 exist
                            Console.WriteLine("enter AssignmentsID of the link: ");
                            if (!int.TryParse(Console.ReadLine(), out idAssigment1))
                                throw new Exception("Wrong input");
                           Console.WriteLine("enter AssignmentsID of the link: ");
                            if (!int.TryParse(Console.ReadLine(), out idAssigment2))
                                throw new Exception("Wrong input");
                            //do
                            //{
                               
                            //} while (s_dalAssignments!.Read(idAssigment2) != null && s_dalAssignments!=null);
                            s_dalLinks!.Create(new(0, idAssigment1, idAssigment2));
                            break;
                        case CRUD.READ:
                            // Perform read operation
                            Console.WriteLine("Enter Link ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int readId))
                                throw new Exception("Wrong input");
                            Link ? readLink = s_dalLinks!.Read(readId);
                            Console.WriteLine(readLink is null ? "Link was not found!\n" : readLink);
                            break;
                        case CRUD.READ_ALL:
                            Console.WriteLine("List of Links: ");
                            foreach (var item in s_dalLinks!.ReadAll())
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        case CRUD.UPDATE:
                            // Perform update operation
                            Console.WriteLine("Enter the requested link number, and two updated task codes:");
                            if (!int.TryParse(Console.ReadLine(), out int updatedId))
                                throw new Exception("Wrong input");
                            Link? updatedDependency = s_dalLinks!.Read(updatedId) ?? throw new Exception("Wrong input");
                            Console.WriteLine(updatedDependency);
                            Console.WriteLine("Enter the  two updated task codes:");
                            if (!int.TryParse(Console.ReadLine(), out int updaupdatedAssignment))
                                throw new Exception("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int updatedPAssignment))
                                throw new Exception("Wrong input");
                            Link a=new(updatedId, updaupdatedAssignment, updatedPAssignment);
                            s_dalLinks!.Update(ref a);
                            break;
                        case CRUD.DELETE:
                            Console.WriteLine("Enter Link ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                throw new Exception("Wrong input");
                            s_dalLinks!.Delete(idDelete);
                            break;

                        case CRUD.EXIT:
                            Console.WriteLine("Exiting program");
                            break;

                        default:
                            Console.WriteLine("Invalid choice! Please enter a valid option");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

            } while (myChoice != CRUD.EXIT);
        }
        private static void optionAssignment()
        {
            CRUD myChoice;

            do
            {
                Console.WriteLine("Choose operation:");
                Console.WriteLine("1 - Create");
                Console.WriteLine("2 - Read");
                Console.WriteLine("3 - Read All");
                Console.WriteLine("4 - Update");
                Console.WriteLine("5 - Delete");
                Console.WriteLine("0 - Exit");

                // Get input from the user and try to parse it to the Enum type
                if (Enum.TryParse(Console.ReadLine(), out myChoice))
                {
                    switch (myChoice)
                    {
                        case CRUD.CREATE:
                            Console.WriteLine("enter Name of the Assignment: ");
                            string? name = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter Description of the Assignment: ");
                            string? Description = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter Remarks of the Assignment: ");
                            string? Remarks = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter ResultProduct of the Assignment: ");
                            string? ResultProduct = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter DurationAssignments of the Assignment: ");
                            if (!int.TryParse(Console.ReadLine(), out int DurationAssignments))
                                throw new Exception("Wrong input");
                            Console.WriteLine("enter id of the worker : ");
                            if (!int.TryParse(Console.ReadLine(), out int IdWorker))
                                throw new Exception("Wrong input");
                            Console.WriteLine("enter level of the worker : ");
                            Level level = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            Console.WriteLine("enter milestone-False/True : ");
                            if (!bool.TryParse(Console.ReadLine(), out bool milestone))
                                throw new Exception("Wrong input");

                            Console.WriteLine("enter the datestart &  DateBegin & DeadLine & DateFinish of the Assignment: ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime datestart))
                                throw new Exception("datestart is not correct");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateBegin))
                                throw new Exception("datestart is not correct");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine))
                                throw new Exception("datestart is not correct");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateFinish))
                                throw new Exception("datestart is not correct");
                            s_dalAssignments!.Create(new(0, DurationAssignments, level, IdWorker, datestart, DateBegin, DeadLine,
                             DateFinish, name, Description, Remarks, ResultProduct, milestone));
                            break;

                        case CRUD.READ:
                            Console.WriteLine("Enter Assignment ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new Exception("Wrong input");
                            Assignments? readAssignment = s_dalAssignments!.Read(ID);
                            Console.WriteLine(readAssignment);
                            break;

                        case CRUD.READ_ALL:
                            Console.WriteLine("List of Assigments: ");
                            foreach (var item in s_dalAssignments!.ReadAll())
                                Console.WriteLine(item);
                            break; 

                        case CRUD.UPDATE:
                            Console.WriteLine("Enter Assignments Id: ");
                            if (!int.TryParse(Console.ReadLine(), out int Id))
                                throw new Exception("Wrong input");
                            Console.WriteLine(s_dalAssignments!.Read(Id));
                            Console.WriteLine("enter Name of the Assignment: ");
                            string? name1 = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter Description of the Assignment: ");
                            string? Description1 = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter Remarks of the Assignment: ");
                            string? Remarks1 = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter ResultProduct of the Assignment: ");
                            string? ResultProduct1 = Console.ReadLine() ?? throw new Exception("Wrong input");

                            Console.WriteLine("enter DurationAssignments of the Assignment: ");
                            if (!int.TryParse(Console.ReadLine(), out int DurationAssignments1))
                                throw new Exception("Wrong input");
                            Console.WriteLine("enter id of the worker : ");
                            if (!int.TryParse(Console.ReadLine(), out int IdWorker1))
                                throw new Exception("Wrong input");
                            Level level1 = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            Console.WriteLine("enter milestone-False/True : ");
                            if (!bool.TryParse(Console.ReadLine(), out bool milestone1))
                                throw new Exception("Wrong input");

                            Console.WriteLine("enter the datestart &  DateBegin & DeadLine & DateFinish of the Assignment: ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime datestart1))
                                throw new Exception("datestart is not correct");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateBegin1))
                                throw new Exception("datestart is not correct");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine1))
                                throw new Exception("datestart is not correct");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateFinish1))
                                throw new Exception("datestart is not correct");
                            Assignments ass = new Assignments(Id, DurationAssignments1, level1, IdWorker1, datestart1, DateBegin1, DeadLine1,
                            DateFinish1, name1, Description1, Remarks1, ResultProduct1, milestone1);
                            s_dalAssignments!.Update(ref ass);
                            break;

                        case CRUD.DELETE:
                            Console.WriteLine("Enter Assigments ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                throw new Exception("Wrong input");
                            s_dalAssignments!.Delete(idDelete);
                            break;

                        case CRUD.EXIT:
                            Console.WriteLine("Exiting program");
                            break;

                        default:
                            Console.WriteLine("Invalid choice! Please enter a valid option");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

            } while (myChoice != CRUD.EXIT);



        }
    }
}


//Console.WriteLine("choose 1 for exit main menue");
//Console.WriteLine("choose 2 for creat a new object");
//Console.WriteLine("choose 3 for Object display by ID (Read)");
//Console.WriteLine("choose 4 for read all objects of the entity type");
//Console.WriteLine("choose 5 for updating existing object data");
//Console.WriteLine("choose 6 for deleting an existing object from a list");

////Option op;
//do
//{
//    Array OptionArray = Enum.GetValues(typeof(Option));
//   // LevelWorker randomLevel = (LevelWorker)levelArray.GetValue(s_rand.Next(levelArray.Length));
//    //op = (Option)Enum.Parse(typeof(Option), Console.ReadLine() ?? string.Empty);

//    switch (OptionArray)
//    {
//        case ADD:
//            // פעולות להוספה
//            break;
//        case READ:
//            // פעולות לקריאה
//            break;
//        case ReadAll:
//            // פעולות לקריאת כל האובייקטים
//            break;
//        case Update:
//            // פעולות לעדכון
//            break;
//        case Delete:
//            // פעולות למחיקה
//            break;
//        case Exit:
//            // יציאה
//            break;
//        default:
//            Console.WriteLine("ERROR");
//            break;
//    }
//} while (op != Option.Exit);