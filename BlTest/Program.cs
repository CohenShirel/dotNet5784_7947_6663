using BO;
using BlApi;
using BlImplementation;
using BL;
using static BO.Exceptions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DalApi;
using System.Runtime.InteropServices;
using DO;
using System.Collections.Generic;
using System;
namespace BlTest;

internal class Program
{//program
 //
   

    private static readonly Random s_rand = new();
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    //BO.Worker? boWork = s_bl.Worker.Read(3);



    static readonly IDal _dal = DalApi.Factory.Get;
    IEnumerable<BO.Assignments> lstAss1 = s_bl.Assignments.ReadAllAss();

    //public static BO.Status ScheduleProject(int ID)
    //{
    //    IEnumerable<Link> lstPLinks;
    //    BO.Assignments ass = s_bl.Assignments.Read(ID)!;//מחזירה משימה נוכחית
    //    בדיקה אם למשימה שהכניס  אין משימות קודמות אז זה יהיה שווה למשימה הראשונה של הפרויקט
    //    lstPLinks = _dal.Link.ReadAll(d => d.IdAssignments == ass.IdAssignments);//the previes ass

    //    if (lstPLinks == null)//משימה ראשונית
    //    {
    //        ass.DateBegin = IBl.StartProjectTime;
    //        ass.DeadLine = ass.DateBegin + TimeSpan.FromDays(ass.DurationAssignments);
    //        ass.status = Tools.GetEmployeeStatus(lstPLinks!);
    //        s_bl.Assignments!.Update(ass);
    //    }
    //    var maxDeadline = lstPLinks!.MaxBy(a => s_bl.Assignments.Read(a.IdAssignments)!.DeadLine);
    //    if (maxDeadline == null)
    //        throw new FormatException("ERROR! There aren't dateBegin for previous assignments");
    //    תאריך התחלתי
    //     DateTime startDate = new DateTime(maxDeadline);

    //    יצירת מחולק רנדומלי
    //   Random random = new Random();
    //    הגרלת מספר ימים בטווח של שבוע
    //    int daysToAdd = random.Next(8);

    //    הוספת מספר הימים המקריים לתאריך ההתחלתי
    //    DateTime? dt = s_bl.Assignments.Read(maxDeadline.IdAssignments)!.DeadLine;

    //    ass.DateBegin = dt + TimeSpan.FromDays(daysToAdd);
    //    ass.DeadLine = ass.DateBegin + TimeSpan.FromDays(ass.DurationAssignments);
    //    ass.status = BO.Tools.GetEmployeeStatus(lstPLinks!);
    //    s_bl.Assignments!.Update(ass);

    //    return BO.Tools.GetEmployeeStatus(lstPLinks!);//מחשבת סטטוס
    //    DateTime? BiGTime = null;
    //    foreach (var a in lstPLinks!)
    //    {
    //        BO.Assignments assP = s_bl.Assignments.Read(a.IdAssignments)!;
    //        if (assP.DateBegin == null)
    //        {
    //            if (BiGTime < assP.DeadLine || BiGTime == null)
    //            {
    //                BiGTime = assP.DeadLine;
    //            }
    //        }
    //        throw new FormatException("ERROR! There aren't dateBegin for previous assignments");
    //    }
    //    Console.WriteLine($"The Last deadLine for that assigment is:{BiGTime}");
    //    if (!DateTime.TryParse(Console.ReadLine(), out DateTime dt))
    //        throw new FormatException("datestart is not correct");
    //    if (dt >= BiGTime)
    //    {
    //        ass.DateBegin = dt;
    //        ass.DeadLine = dt + TimeSpan.FromDays(ass.DurationAssignments);
    //        s_bl.Assignments!.Update(ass);
    //    }
    //    else//אם הכניס קוד שהוא קטן
    //        throw new FormatException("ERROR! You enter error date");
    //}

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
    //else
    //{
    //    foreach (var a in lstPLinks)
    //    {
    //        currentAss = s_bl.Assignments.Read(a.IdAssignments)!;
    //        if (currentAss.DateBegin==null)
    //            throw new FormatException("datestart is not reset");
    //    }
    //}
    // int IdAssignments,
    //int IdPAssignments
    static readonly IDal s_dal = DalApi.Factory.Get;
    IEnumerable<BO.Assignments> lstAss = s_bl.Assignments.ReadAllAss();
    //משך זמן ורמת מורכבות\
    //public BO.Status ScheduleProject()
    //{
    //    IEnumerable<Link> lstPLinks;
    //    foreach (var ass in lstAss)
    //    {
    //        lstPLinks = s_dal.Link.ReadAll(d => d.IdAssignments == ass.IdAssignments);//the previes ass
    //        if (lstPLinks == null)
    //        {
    //            ass.DateBegin = IBl.StartProjectTime;
    //            ass.DeadLine = ass.DateBegin + TimeSpan.FromDays(ass.DurationAssignments);
    //            s_bl.Assignments!.Update(ass);
    //        }
            
    //        //פונקציה לחישוב המשימה הבאה ועדכון הזמן שלה התלויות בה
    //        Rec(ass);
    //    }

    //}
    //public void Rec(BO.Assignments ass)
    //{
    //    BO.Assignments currentAss;
    //    IEnumerable<Link> lstNLinks;

    //    lstNLinks = s_dal.Link.ReadAll(d => d.IdPAssignments == ass.IdAssignments);//the Next ass
    //    foreach (var a in lstNLinks)
    //    {
    //        Console.WriteLine($"Enter startDate of the project And the mininal start time that you can begin is: {ass.DeadLine}");
    //        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dt))
    //            throw new FormatException("datestart is not correct");
    //        if (dt >= ass.DeadLine)
    //        {
    //            currentAss = s_bl.Assignments.Read(a.IdAssignments)!;
    //            currentAss.DateBegin = dt;
    //            currentAss.DeadLine = currentAss.DateBegin + TimeSpan.FromDays(currentAss.DurationAssignments);
    //            s_bl.Assignments!.Update(currentAss);
    //            Rec(currentAss);

    //        }
    //        throw new FormatException("datestart is not correct");

    //    }

    //}

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
            IdAssignments = doAss.IdAssignments,
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
            Bl.reset();
        }
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
                            //Console.WriteLine("Enter currentAssigments (idA) of this worker : ");
                            //if (!int.TryParse(Console.ReadLine(), out int tzA))
                            //    throw new FormatException("Wrong input");
                            //WorkerInAssignments asss =new WorkerInAssignments { WorkerId= id, AssignmentsNumber =tzA};
                            //DO.Worker wrk = new DO.Worker(id, userLevel, cost, name, email);
                            //Worker worker = ConvertWrkDOToBO(wrk);
                            //s_bl.Worker!.Create(worker);
                            BO.Worker wrk = new BO.Worker
                            {
                                Id = id,
                                Experience = userLevel,
                                HourSalary = cost,
                                Email = email,
                                Name = name,
                                //currentAssignment = asss,
                            };
                            s_bl.Worker!.Create(wrk);

                            break;
                        case CRUD.READ:
                            // Perform read operation
                            Console.WriteLine("Enter worker ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new FormatException("Wrong input");
                            BO.Worker? rea = s_bl.Worker!.Read(ID);
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
                            BO.Worker updatedWorker = s_bl.Worker!.Read(iD)! ?? throw new FormatException($"Can't update, worker does not exist!!");
                            Console.WriteLine(updatedWorker);
                            Console.WriteLine("Please update the details -- name, email, cost,level.\n");
                            string? updatedName = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            string? updetedEmail = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (!int.TryParse(Console.ReadLine(), out int updatedCost))
                                throw new FormatException("Wrong input");
                            DO.Level updatedLevel = (DO.Level)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");
                            Console.WriteLine("Enter currentAssigments (idA) of this worker : ");
                            if (!int.TryParse(Console.ReadLine(), out  int tzA))
                                throw new FormatException("Wrong input");
                            WorkerInAssignments a = new WorkerInAssignments { WorkerId = iD, AssignmentsNumber = tzA };
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
                            //DO.Assignments ass = new DO.Assignments(DurationAssignments, level, IdWorker,
                            //name, Description, Remarks, ResultProduct);
                            //get from the user the link assigments
                            List<AssignmentsInList> list = new List<AssignmentsInList>();
                            Console.WriteLine("Enter the links(id & name of assigments)till -1: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID))
                                throw new FormatException("Wrong input");
                            while (ID != -1)
                            {
                                string? nm = Console.ReadLine() ?? throw new FormatException("Wrong input");
                                BO.AssignmentsInList assl = new BO.AssignmentsInList { Id = ID, AssignmentName = nm };
                                list.Add(assl);
                                Console.WriteLine("Enter the links(id & name of assigments)till -1: ");
                                if (!int.TryParse(Console.ReadLine(), out ID))
                                    throw new FormatException("Wrong input");
                            }

                            BO.Assignments ass=new BO.Assignments
                            {
                                DurationAssignments =DurationAssignments,
                                LevelAssignments = level,
                                IdWorker =IdWorker,
                                Name =name,
                                Description =Description,
                                Remarks =Remarks,
                                ResultProduct =ResultProduct,
                                links=list,
                            };
                            s_bl.Assignments!.Create(ass);
                            break;
                        case CRUD.READ:
                            Console.WriteLine("Enter Assignment ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int ID1))
                                throw new FormatException("Wrong input");
                            BO.Assignments rea = s_bl.Assignments!.Read(ID1)!;
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
                            BO.Assignments ass1 = new BO.Assignments
                            {
                                IdAssignments = Id,
                                DurationAssignments = DurationAssignments1,
                                LevelAssignments = level1,
                                IdWorker = IdWorker1,
                                Name = name1,
                                Description = Description1,
                                Remarks = Remarks1,
                                ResultProduct = ResultProduct1,
                                //status = Tools.ScheduleProject(Id),
                            };
                            Console.WriteLine("DO YOU WANT TO START TO UPDET DATES? Y/N");
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (ans == "N") 
                                s_bl.Assignments!.Update(ref ass1);
                            else if((ans == "Y"))
                                Tools.ScheduleProject(ass1);

                            //s_bl.Assignments!.Update(ass1);
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


