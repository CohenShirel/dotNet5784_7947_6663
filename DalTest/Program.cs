
//Shirel Cohen 214377947
//Neomi Golkin 325946663
//עשינו את התוספת של TryParse
using Dal;
using DalApi;
using DO;
//using DalXml;


//לינק ומטלות
//האם מספיק פורמט להכל וזהו
//האם הטריי והקאצ טובים?
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
    public enum Levels
    {
        Beginner, AdvancedBeginner, Intermediate, Advanced, Expert
    }
    internal class Program
    {
        private static readonly Random s_rand = new();

        //private static IWorker? s_dal.Worker = new WorkerImplementation();
        //private static IAssignments? s_dal.Assignments = new AssignmentsImplementation();
        //private static ILink? s_dal.Link = new LinkImplementation();


       // static readonly IDal s_dal = new Dal.DalList(); //stage 2
        static readonly IDal s_dal = new Dal.DalXml(); //stage 3


        static void Main(string[] args)
        {
            //Initialization.Do(s_dal);
            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
                Initialization.Do(s_dal); //stage 2
            ENTITY myChoice = ENTITY.ASSIGNMENT;
            do
            {

                try
                {
                    Console.WriteLine("choose 0 for exit main menu");
                    Console.WriteLine("choose 1 for Worker");
                    Console.WriteLine("choose 2 for Link");
                    Console.WriteLine("choose 3 for ASSIGNMENT");
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


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (myChoice != ENTITY.EXIT);


        }

        private static void optionWorker()
        {
            try
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
                                    throw new FormatException("Wrong input");
                                if (!int.TryParse(Console.ReadLine(), out int cost))
                                    throw new FormatException("Wrong input");
                                string? name = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                string? email = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                // if(!)
                                Level userLevel;
                                // Level level = (Level)int.Parse(Console.ReadLine() ?? throw new IndexOutOfRangeException("Out Of Array"));

                                if (!Enum.TryParse(Console.ReadLine(), out userLevel))
                                {
                                    throw new FormatException("Wrong input");
                                }
                                s_dal.Worker!.Create(new(id, userLevel, cost, name, email));
                                break;
                            case CRUD.READ:
                                // Perform read operation
                                Console.WriteLine("Enter worker ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int ID))
                                    throw new FormatException("Wrong input");
                                Worker? rea = s_dal.Worker!.Read(a => a.IdWorker == ID);
                                Console.WriteLine(rea);
                                break;
                            case CRUD.READ_ALL:
                                // Perform read all operation
                                foreach (var Workers in s_dal.Worker!.ReadAll())
                                {
                                    Console.WriteLine(Workers);
                                }
                                break;
                            case CRUD.UPDATE:
                                // Perform update operation
                                Console.WriteLine("Enter worker ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int iD))
                                    throw new FormatException("Wrong input");
                                Worker updatedWorker = s_dal.Worker!.Read(a => a.IdWorker == iD)! ?? throw new FormatException($"Can't update, worker does not exist!!");
                                Console.WriteLine(updatedWorker);
                                Console.WriteLine("Please update the details -- name, email, level, cost.\n");
                                string? updatedName = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                string? updetedEmail = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                if (!int.TryParse(Console.ReadLine(), out int updatedCost))
                                    throw new FormatException("Wrong input");
                                Level updatedLevel = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                                //if i got space/null save the old one
                                //if (string.IsNullOrWhiteSpace(updatedName))
                                //    updatedName = s_dal.Worker.Read(iD)!.Name;
                                //if (string.IsNullOrWhiteSpace(updetedEmail))
                                //    updetedEmail = s_dal.Worker.Read(iD)!.Email;
                                Worker worker = new Worker(iD, updatedLevel, updatedCost, updatedName, updetedEmail);
                                s_dal.Worker!.Update(ref worker);
                                break;
                            case CRUD.DELETE:
                                // Perform delete operation
                                Console.WriteLine("Enter worker ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int Id))
                                    throw new FormatException("Wrong input");
                                s_dal.Worker!.Delete(Id);
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
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void optionLink()
        {
            try
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
                                //it will get into the loop *there are item in the s_dal.Assignments **if idAssigment1 exist
                                Console.WriteLine("enter AssignmentsID of the link: ");
                                if (!int.TryParse(Console.ReadLine(), out idAssigment1))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter AssignmentsID of the link: ");
                                if (!int.TryParse(Console.ReadLine(), out idAssigment2))
                                    throw new FormatException("Wrong input");
                                //do
                                //{

//} while (s_dalAssignments!.Read(idAssigment2) != null && s_dal.Assignments!=null);
                                s_dal.Link!.Create(new(0, idAssigment1, idAssigment2));
                                break;
                            case CRUD.READ:
                                // Perform read operation
                                Console.WriteLine("Enter Link ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int readId))
                                    throw new FormatException("Wrong input");
                                Link? readLink = s_dal.Link!.Read(a => a.IdLink == readId);
                                Console.WriteLine(readLink is null ? "Link was not found!\n" : readLink);
                                break;
                            case CRUD.READ_ALL:
                                Console.WriteLine("List of Links: ");
                                foreach (var item in s_dal.Link!.ReadAll())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case CRUD.UPDATE:
                                // Perform update operation
                                Console.WriteLine("Enter the requested link number");
                                if (!int.TryParse(Console.ReadLine(), out int updatedId))
                                    throw new FormatException("Wrong input");
                                Link? updatedDependency = s_dal.Link!.Read(a => a.IdLink == updatedId) ?? throw new FormatException("Wrong input");
                                Console.WriteLine(updatedDependency);
                                Console.WriteLine("Enter the  two updated task codes:");
                                if (!int.TryParse(Console.ReadLine(), out int updaupdatedAssignment))
                                    throw new FormatException("Wrong input");
                                if (!int.TryParse(Console.ReadLine(), out int updatedPAssignment))
                                    throw new FormatException("Wrong input");
                                Link a = new(updatedId, updaupdatedAssignment, updatedPAssignment);
                                s_dal.Link!.Update(ref a);
                                break;
                            case CRUD.DELETE:
                                Console.WriteLine("Enter Link ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                    throw new FormatException("Wrong input");
                                s_dal.Link!.Delete(idDelete);
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
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void optionAssignment()
        {
            try
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
                                string? name = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter Description of the Assignment: ");
                                string? Description = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter Remarks of the Assignment: ");
                                string? Remarks = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter ResultProduct of the Assignment: ");
                                string? ResultProduct = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter DurationAssignments of the Assignment: ");
                                if (!int.TryParse(Console.ReadLine(), out int DurationAssignments))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter id of the worker : ");
                                if (!int.TryParse(Console.ReadLine(), out int IdWorker))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter level of the worker : ");
                                Level level = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                                Console.WriteLine("enter milestone-False/True : ");
                                if (!bool.TryParse(Console.ReadLine(), out bool milestone))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter the datestart &  DateBegin & DeadLine & DateFinish of the Assignment: ");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly datestart))
                                    throw new FormatException("datestart is not correct");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DateBegin))
                                    throw new FormatException("datestart is not correct");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DeadLine))
                                    throw new FormatException("datestart is not correct");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DateFinish))
                                    throw new FormatException("datestart is not correct");
                                s_dal.Assignments!.Create(new(0, DurationAssignments, level, IdWorker, datestart, DateBegin, DeadLine,
                                 DateFinish, name, Description, Remarks, ResultProduct, milestone));
                                break;

                            case CRUD.READ:
                                Console.WriteLine("Enter Assignment ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int ID))
                                    throw new FormatException("Wrong input");
                                Assignments rea = s_dal.Assignments!.Read(a => a.IdWorker == ID)!;
                                Console.WriteLine(rea);
                                break;

                            case CRUD.READ_ALL:
                                Console.WriteLine("List of Assigments: ");
                                foreach (var item in s_dal.Assignments!.ReadAll())
                                    Console.WriteLine(item);
                                break;

                            case CRUD.UPDATE:
                                Console.WriteLine("Enter Assignments Id: ");
                                if (!int.TryParse(Console.ReadLine(), out int Id))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine(s_dal.Assignments!.Read(a => a.IdWorker == Id));

                                Console.WriteLine("enter Name of the Assignment: ");
                                string? name1 = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter Description of the Assignment: ");
                                string? Description1 = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter Remarks of the Assignment: ");
                                string? Remarks1 = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter ResultProduct of the Assignment: ");
                                string? ResultProduct1 = Console.ReadLine() ?? throw new FormatException("Wrong input");

                                Console.WriteLine("enter DurationAssignments of the Assignment: ");
                                if (!int.TryParse(Console.ReadLine(), out int DurationAssignments1))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter id of the worker : ");
                                if (!int.TryParse(Console.ReadLine(), out int IdWorker1))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter level of the worker : ");
                                Level level1 = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                                Console.WriteLine("enter milestone-False/True : ");
                                if (!bool.TryParse(Console.ReadLine(), out bool milestone1))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter the datestart &  DateBegin & DeadLine & DateFinish of the Assignment: ");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly datestart1))
                                    throw new FormatException("datestart is not correct");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DateBegin1))
                                    throw new FormatException("datestart is not correct");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DeadLine1))
                                    throw new FormatException("datestart is not correct");
                                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DateFinish1))
                                    throw new FormatException("datestart is not correct");
                                //if i got space/null save the old one
                                //if (string.IsNullOrWhiteSpace(name1))
                                //    name1 = s_dal.Assignments.Read(Id)!.Name;
                                //if (string.IsNullOrWhiteSpace(Description1))
                                //    Description1 = s_dal.Assignments.Read(Id)!.Description;
                                //if (string.IsNullOrWhiteSpace(Remarks1))
                                //    Remarks1 = s_dal.Assignments.Read(Id)!.Remarks;
                                //if (string.IsNullOrWhiteSpace(ResultProduct1))
                                //    ResultProduct1 = s_dal.Assignments.Read(Id)!.ResultProduct;
                                Assignments ass = new Assignments(Id, DurationAssignments1, level1, IdWorker1, datestart1, DateBegin1, DeadLine1,
                                DateFinish1, name1, Description1, Remarks1, ResultProduct1, milestone1);
                                s_dal.Assignments!.Update(ref ass);
                                break;

                            case CRUD.DELETE:
                                Console.WriteLine("Enter Assigments ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                    throw new FormatException("Wrong input");
                                s_dal.Assignments!.Delete(idDelete);
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
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
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







