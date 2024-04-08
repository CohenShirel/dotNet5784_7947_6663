
//Shirel Cohen 214377947
//Neomi Golkin 325946663
using DalApi;
using DO;
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
        static readonly IDal s_dal = Factory.Get;
        static void Main(string[] args)
        {
            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y")
            {
                Initialization.Do();
                s_dal.Worker!.DeleteAll();
                s_dal.Link!.DeleteAll();
                s_dal.Assignment!.DeleteAll();
            }
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
                                break;
                            case ENTITY.WORKER:
                                optionWorker();
                                break;
                            case ENTITY.LINK:
                                optionLink();
                                break;
                            case ENTITY.ASSIGNMENT:
                                optionAssignment();
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        }
                    }
                    else
                        Console.WriteLine("Invalid input. Please enter a valid number.");
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
                                Level userLevel;
                                if (!Enum.TryParse(Console.ReadLine(), out userLevel))
                                    throw new FormatException("Wrong input");
                                s_dal.Worker!.Create(new(id, userLevel, cost, name, email));
                                break;
                            case CRUD.READ:
                                // Perform read operation
                                Console.WriteLine("Enter worker ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int ID))
                                    throw new FormatException("Wrong input");
                                Worker? rea = s_dal.Worker!.Read(a => a.Id == ID);
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
                                Worker updatedWorker = s_dal.Worker!.Read(a => a.Id == iD)! ?? throw new FormatException($"Can't update, worker does not exist!!");
                                Console.WriteLine(updatedWorker);
                                Console.WriteLine("Please update the details -- name, email, level, cost.\n");
                                string? updatedName = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                string? updetedEmail = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                if (!int.TryParse(Console.ReadLine(), out int updatedCost))
                                    throw new FormatException("Wrong input");
                                Level updatedLevel = (Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                                Worker worker = new Worker(iD, updatedLevel, updatedCost, updatedName, updetedEmail);
                                s_dal.Worker!.Update(worker);
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
                        Console.WriteLine("Invalid input. Please enter a valid number.");
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
                                //it will get into the loop *there are item in the s_dal.Assignment **if idAssigment1 exist
                                Console.WriteLine("enter AssignmentsID of the link: ");
                                if (!int.TryParse(Console.ReadLine(), out idAssigment1))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine("enter AssignmentsID of the link: ");
                                if (!int.TryParse(Console.ReadLine(), out idAssigment2))
                                    throw new FormatException("Wrong input");
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
                                s_dal.Link!.Update(a);
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
                        Console.WriteLine("Invalid input. Please enter a valid number.");
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
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime datestart))
                                    throw new FormatException("datestart is not correct");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateBegin))
                                    throw new FormatException("datestart is not correct");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine))
                                    throw new FormatException("datestart is not correct");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateFinish))
                                    throw new FormatException("datestart is not correct");
                                s_dal.Assignment!.Create(new(0, DurationAssignments, level, IdWorker, datestart, DateBegin, DeadLine,
                                 DateFinish, name, Description, Remarks, ResultProduct, milestone));
                                break;

                            case CRUD.READ:
                                Console.WriteLine("Enter Assignment ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int ID))
                                    throw new FormatException("Wrong input");
                                Assignment rea = s_dal.Assignment!.Read(a => a.IdAssignment == ID)!;
                                Console.WriteLine(rea);
                                break;

                            case CRUD.READ_ALL:
                                Console.WriteLine("List of Assigments: ");
                                foreach (var item in s_dal.Assignment!.ReadAll())
                                    Console.WriteLine(item);
                                break;

                            case CRUD.UPDATE:
                                Console.WriteLine("Enter Assignment Id: ");
                                if (!int.TryParse(Console.ReadLine(), out int Id))
                                    throw new FormatException("Wrong input");
                                Console.WriteLine(s_dal.Assignment!.Read(a => a.IdAssignment == Id));

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
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime datestart1))
                                    throw new FormatException("datestart is not correct");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateBegin1))
                                    throw new FormatException("datestart is not correct");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine1))
                                    throw new FormatException("datestart is not correct");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateFinish1))
                                    throw new FormatException("datestart is not correct");
                                Assignment ass = new Assignment(Id, DurationAssignments1, level1, IdWorker1, datestart1, DateBegin1, DeadLine1,
                                DateFinish1, name1, Description1, Remarks1, ResultProduct1, milestone1);
                                s_dal.Assignment!.Update(ass);
                                break;

                            case CRUD.DELETE:
                                Console.WriteLine("Enter Assigments ID: ");
                                if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                    throw new FormatException("Wrong input");
                                s_dal.Assignment!.Delete(idDelete);
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