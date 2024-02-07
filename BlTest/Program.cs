using BO;
using BlApi;
using BlImplementation;
using BL;
using static BO.Exceptions;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace BlTest;

internal class Program
{//program
    //
    private static readonly Random s_rand = new();
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    BO.Worker? boWork = s_bl.Worker.Read(3);
    public enum ENTITY
    {
        EXIT,
        WORKER,
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
    public void ScheduleProject()
    {
        Console.WriteLine("Enter startDate of the project");
        DateTime projectStartDate = DateTime.TryParse(Console.ReadLine());
        foreach (  in tasks)
        {
            if (task.Dependencies.Count == 0)
            {
                task.ScheduledDate = projectStartDate;
            }
            else
            {
                DateTime maxDependencyDate = DateTime.MinValue;
                foreach (var dependency in task.Dependencies)
                {
                    if (dependency.ScheduledDate > maxDependencyDate)
                    {
                        maxDependencyDate = dependency.ScheduledDate.Value;
                    }
                }
                task.ScheduledDate = maxDependencyDate;
            }

            Console.WriteLine($"Task: {task.Name}, Scheduled Date: {task.ScheduledDate}");
        }
    }
    public class ProjectScheduler
    {
        private List<Task> tasks;
        private DateTime? projectStartDate;

        public ProjectScheduler()
        {
            tasks = new List<Task>();
            projectStartDate = null;
        }

        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void SetProjectStartDate(DateTime startDate)
        {
            projectStartDate = startDate;
        }

        
    }

    //function that convert DOToBO
    public static BO.Worker ConvertWrkDOToBO(DO.Worker doWorker)
    {
        return new BO.Worker
        {
            Id = doWorker.IdWorker,
            Name = doWorker.Name,
            Email = doWorker.Email,
            Experience = doWorker.Experience,
            HourSalary = doWorker.HourSalary,
        };
    }
    //function that convert DOToBO
    public static BO.Assignments ConvertAssDOToBO(DO.Assignments doAss)
    {
        return new BO.Assignments
        {
            IdAssignments = doAss.IdWorker,
            DurationAssignments = doAss.DurationAssignments,
            LevelAssignments = doAss.LevelAssignments,
            IdWorker = doAss.IdWorker,
            dateSrart = doAss.dateSrart,
            DateBegin = doAss.DateBegin,
            DeadLine = doAss.DeadLine,
            DateFinish = doAss.DateFinish,
            Name = doAss.Name,
            Description = doAss.Description,
            Remarks = doAss.Remarks,
            ResultProduct = doAss.ResultProduct,
        };
    }
    static void Main(string[] args)
    {
        //Initialization.Do(s_dal);
        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        if (ans == "Y") //stage 3
        {
            BlImplementation.Bl.reset();
           // DalTest.Initialization.Do(); 
           //reset();
            //s_bl.Worker!.DeleteAll();
            
            //s_bl.Assignments!.DeleteAll();
            
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
                            //SCUDLE
                            // יציאה
                            break;
                        case ENTITY.WORKER:
                            // פעולות לקריאת כל האובייקטים
                            optionWorker();
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
                            DO.Level userLevel;
                            // Level level = (Level)int.Parse(Console.ReadLine() ?? throw new IndexOutOfRangeException("Out Of Array"));

                            if (!Enum.TryParse(Console.ReadLine(), out userLevel))
                            {
                                throw new FormatException("Wrong input");
                            }
                            DO.Worker wrk = new DO.Worker(id, userLevel, cost, name, email);
                            Worker worker = ConvertWrkDOToBO(wrk);
                            s_bl.Worker!.Create(worker);
                            break;
                        case CRUD.READ:
                            // Perform read operation
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new FormatException("Wrong input");
                            Worker? rea = s_bl.Worker!.Read(ID);
                            Console.WriteLine(rea);
                            break;
                        case CRUD.READ_ALL:
                            // Perform read all operation
                            foreach (var Workers in s_bl.Worker!.ReadAll())
                            {
                                Console.WriteLine(Workers);
                            }
                            break;
                        case CRUD.UPDATE:
                            // Perform update operation
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int iD))
                                throw new FormatException("Wrong input");
                            Worker updatedWorker = s_bl.Worker!.Read(iD)! ?? throw new FormatException($"Can't update, worker does not exist!!");
                            Console.WriteLine(updatedWorker);
                            Console.WriteLine("Please update the details -- name, email, level, cost.\n");
                            string? updatedName = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            string? updetedEmail = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int updatedCost))
                                throw new FormatException("Wrong input");
                            DO.Level updatedLevel = (DO.Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            //if i got space/null save the old one
                            //if (string.IsNullOrWhiteSpace(updatedName))
                            //    updatedName = s_dal.Worker.Read(iD)!.Name;
                            //if (string.IsNullOrWhiteSpace(updetedEmail))
                            //    updetedEmail = s_dal.Worker.Read(iD)!.Email;
                            DO.Worker worker1 = new DO.Worker(iD,updatedLevel,updatedCost,updatedName,updetedEmail);
                            Worker wrk1 = ConvertWrkDOToBO(worker1);
                            s_bl.Worker!.Update(wrk1);
                            break;
                        case CRUD.DELETE:
                            // Perform delete operation
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int Id))
                                throw new FormatException("Wrong input");
                            s_bl.Worker!.Delete(Id);
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
        catch (BlAlreadyExistsException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            // Check if there is inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }

        }
        catch (BlDoesNotExistException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            // Check if there is inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            // Check if there is inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
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
                            DO.Level level = (DO.Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            //Console.WriteLine("enter milestone-False/True : ");
                            //if (!bool.TryParse(Console.ReadLine(), out bool milestone))
                            //    throw new FormatException("Wrong input");
                            Console.WriteLine("enter the datestart &  DateBegin & DeadLine & DateFinish of the Assignment: ");
                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly datestart))
                                throw new FormatException("datestart is not correct");
                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DateBegin))
                                throw new FormatException("datestart is not correct");
                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DeadLine))
                                throw new FormatException("datestart is not correct");
                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DateFinish))
                                throw new FormatException("datestart is not correct");
                            DO.Assignments ass = new DO.Assignments(0, DurationAssignments, level, IdWorker, datestart, DateBegin, DeadLine,
                            DateFinish, name, Description, Remarks, ResultProduct,false);
                            Assignments assig = ConvertAssDOToBO(ass);
                            s_bl.Assignments!.Create(assig);
                            break;

                        case CRUD.READ:
                            Console.WriteLine("Enter Assignment ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new FormatException("Wrong input");
                            Assignments rea = s_bl.Assignments!.Read(ID)!;
                            Console.WriteLine(rea);
                            break;

                        case CRUD.READ_ALL:
                            Console.WriteLine("List of Assigments: ");
                            foreach (var item in s_bl.Assignments!.ReadAll())
                                Console.WriteLine(item);
                            break;

                        case CRUD.UPDATE:
                            Console.WriteLine("Enter Assignments Id: ");
                            if (!int.TryParse(Console.ReadLine(), out int Id))
                                throw new FormatException("Wrong input");
                            Console.WriteLine(s_bl.Assignments!.Read(Id));

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
                            DO.Level level1 = (DO.Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
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
                            DO.Assignments ass1 = new DO.Assignments(Id, DurationAssignments1, level1,
                                IdWorker1, datestart1, DateBegin1, DeadLine1,
                            DateFinish1, name1, Description1, Remarks1, ResultProduct1, milestone1);
                            Assignments Ass= ConvertAssDOToBO(ass1);
                            s_bl.Assignments!.Update(Ass);
                            break;

                        case CRUD.DELETE:
                            Console.WriteLine("Enter Assigments ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                throw new FormatException("Wrong input");
                            s_bl.Assignments!.Delete(idDelete);
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
        catch (BlAlreadyExistsException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            // Check if there is inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
        catch (BlDoesNotExistException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            // Check if there is inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            // Check if there is inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
    }
}


