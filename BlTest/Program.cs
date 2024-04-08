using BlImplementation;
using BO;
using DalApi;
using static BO.Exceptions;
namespace BlTest;

internal class Program
{
    private static readonly Random s_rand = new();
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static readonly IDal _dal = DalApi.Factory.Get;
    IEnumerable<BO.Assignment> lstAss1 = s_bl.Assignment.ReadAllAss();
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
    static readonly IDal s_dal = DalApi.Factory.Get;
    IEnumerable<BO.Assignment> lstAss = s_bl.Assignment.ReadAllAss();
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
            Id = doWorker.Id,
            Name = doWorker.Name,
            Email = doWorker.Email,
            Experience = doWorker.Experience,
            HourSalary = doWorker.HourSalary,
        };
    }
    //function that convert DOToBO
    public static BO.Assignment ConvertAssDOToBO(DO.Assignment doAss)
    {
        return new BO.Assignment
        {
            IdAssignment = doAss.IdAssignment,
            DurationAssignment = doAss.DurationAssignment,
            LevelAssignment = doAss.LevelAssignment,
            IdWorker = doAss.WorkerId,
            dateSrart = doAss.DateSrart,
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
        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        if (ans == "Y")
            Bl.reset();
        ENTITY myChoice = ENTITY.ASSIGNMENT;
        do
        {
            try
            {
                Console.WriteLine("choose 0 for exit main menu");
                Console.WriteLine("choose 1 for Worker");
                Console.WriteLine("choose 2 for ASSIGNMENT");
                if (Enum.TryParse(Console.ReadLine(), out myChoice))
                {
                    switch (myChoice)
                    {
                        case ENTITY.EXIT:
                            break;
                        case ENTITY.WORKER:
                            optionWorker();
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
                if (Enum.TryParse(Console.ReadLine(), out myChoice))
                {
                    switch (myChoice)
                    {
                        case CRUD.CREATE:
                            Console.WriteLine("Enter worker ID, Cost, name and Email, level : ");
                            if (!int.TryParse(Console.ReadLine(), out int id))
                                throw new FormatException("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int cost))
                                throw new FormatException("Wrong input");
                            string? name = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            string? email = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            DO.Level userLevel;
                            if (!Enum.TryParse(Console.ReadLine(), out userLevel))
                                throw new FormatException("Wrong input");
                            BO.Worker wrk = new BO.Worker
                            {
                                Id = id,
                                Experience = userLevel,
                                HourSalary = cost,
                                Email = email,
                                Name = name,
                            };
                            s_bl.Worker!.Create(wrk);
                            break;
                        case CRUD.READ:
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new FormatException("Wrong input");
                            BO.Worker? rea = s_bl.Worker!.Read(ID);
                            Console.WriteLine(rea);
                            break;
                        case CRUD.READ_ALL:
                            foreach (var Workers in s_bl.Worker!.ReadAll())
                                Console.WriteLine(Workers);
                            break;
                        case CRUD.UPDATE:
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int iD))
                                throw new FormatException("Wrong input");
                            BO.Worker updatedWorker = s_bl.Worker!.Read(iD)! ?? throw new FormatException($"Can't update, worker does not exist!!");
                            Console.WriteLine(updatedWorker);
                            Console.WriteLine("Please update the details -- name, email, cost,level.\n");
                            string? updatedName = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            string? updetedEmail = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int updatedCost))
                                throw new FormatException("Wrong input");
                            DO.Level updatedLevel = (DO.Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            Console.WriteLine("Enter currentAssigment (idA) of this worker : ");
                            if (!int.TryParse(Console.ReadLine(), out int tzA))
                                throw new FormatException("Wrong input");
                            WorkerInAssignment a = new WorkerInAssignment { WorkerId = iD, AssignmentNumber = tzA , AssignmentName = updatedName };
                            BO.Worker wrk1 = new BO.Worker
                            {
                                Id = iD,
                                Experience = updatedLevel,
                                HourSalary = updatedCost,
                                Email = updetedEmail,
                                Name = updatedName,
                                currentAssignment = a,
                            };
                            s_bl.Worker!.Update(wrk1);
                            break;
                        case CRUD.DELETE:
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
                    Console.WriteLine("Invalid input. Please enter a valid number.");

            } while (myChoice != CRUD.EXIT);
        }
        catch (BlAlreadyExistsException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }

        }
        catch (BlDoesNotExistException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
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
                            List<AssignmentInList> list = new List<AssignmentInList>();
                            Console.WriteLine("Enter the Links(id & name of assigments)till -1: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new FormatException("Wrong input");
                            while (ID != -1)
                            {
                                string? nm = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                BO.AssignmentInList assl = new BO.AssignmentInList { Id = ID, AssignmentName = nm };
                                list.Add(assl);
                                Console.WriteLine("Enter the Links(id & name of assigments)till -1: ");
                                if (!int.TryParse(Console.ReadLine(), out ID))
                                    throw new FormatException("Wrong input");
                            }
                            BO.Assignment ass = new BO.Assignment
                            {
                                DurationAssignment = DurationAssignments,
                                Name = name,
                                Description = Description,
                                Remarks = Remarks,
                                ResultProduct = ResultProduct,
                                Links = list,
                            };
                            s_bl.Assignment!.Create(ass);
                            break;
                        case CRUD.READ:
                            Console.WriteLine("Enter Assignment ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID1))
                                throw new FormatException("Wrong input");
                            BO.Assignment rea = s_bl.Assignment!.Read(ID1)!;
                            Console.WriteLine(rea);
                            break;
                        case CRUD.READ_ALL:
                            Console.WriteLine("List of Assigments: ");
                            foreach (var item in s_bl.Assignment!.ReadAll())
                                Console.WriteLine(item);
                            break;

                        case CRUD.UPDATE:
                            Console.WriteLine("Enter Assignment Id: ");
                            if (!int.TryParse(Console.ReadLine(), out int Id))
                                throw new FormatException("Wrong input");
                            Console.WriteLine(s_bl.Assignment!.Read(Id));

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
                            if (!int.TryParse(Console.ReadLine(), out int WorkerId1))
                                throw new FormatException("Wrong input");
                            Console.WriteLine("enter level of the worker : ");
                            DO.Level level1 = (DO.Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            BO.Assignment ass1 = new BO.Assignment
                            {
                                IdAssignment = Id,
                                DurationAssignment = DurationAssignments1,
                                LevelAssignment = level1,
                                IdWorker = WorkerId1,
                                Name = name1,
                                Description = Description1,
                                Remarks = Remarks1,
                                ResultProduct = ResultProduct1,
                            };
                            Console.WriteLine("DO YOU WANT TO START TO UPDET DATES? Y/N");
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (ans == "N")
                                s_bl.Assignment!.Update(ass1);
                            else if ((ans == "Y"))
                                Tools.ScheduleProject(ass1);
                            break;

                        case CRUD.DELETE:
                            Console.WriteLine("Enter Assigments ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int idDelete))
                                throw new FormatException("Wrong input");
                            s_bl.Assignment!.Delete(idDelete);
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
        catch (BlAlreadyExistsException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
        catch (BlDoesNotExistException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name},Exception Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
            }
        }
    }

}


