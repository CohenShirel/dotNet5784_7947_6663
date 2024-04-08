using BlImplementation;
using DalApi;
using DO;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Reflection;
using static BO.Exceptions;

namespace BO
{
    public static class Tools
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static readonly IDal _dal = DalApi.Factory.Get;
        public static void ScheduleProject(BO.Assignment ass)
        {
            var lstPLinks = _dal.Link.ReadAll(d => d.IdAssignment == ass.IdAssignment).Select(d => d.IdPAssignment)
                .ToHashSet();
            if (lstPLinks.Count() is 0)
            {
                ass.DateBegin = s_bl.Clock.GetStartProject();
            }
            else
            {
                var assignments = s_bl.Assignment.ReadAll(a => lstPLinks.Contains(a.Id));
                var maxDeadline = assignments!.MaxBy(a => a.DeadLine);
                if (maxDeadline!.DateBegin == null)
                    throw new FormatException("ERROR! There aren't dateBegin for previous assignment");
                Random random = new Random();
                int daysToAdd = random.Next(8);
                ass.DateBegin = maxDeadline!.DeadLine + TimeSpan.FromDays(daysToAdd);
            }
            ass.DeadLine = ass.DateBegin + TimeSpan.FromDays(ass.DurationAssignment);
            s_bl.Assignment!.Update(ass);
        }
        //function that convert BOToDO
        public static DO.Worker ConvertWrkBOToDO(ref BO.Worker doWorker)
        {
            return new DO.Worker
            {
                Id = doWorker.Id,
                Name = doWorker.Name,
                Email = doWorker.Email,
                Experience = doWorker.Experience,
                HourSalary = doWorker.HourSalary,
            };
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
        public static Status GetProjectStatus() =>
            (_dal.Clock.GetStartProject(), _dal.Assignment) switch
            {
                (null, _) => Status.Unscheduled,
                (_, var assignment) when (assignment.ReadAll().Any(a => a.DateBegin is null) || (assignment.ReadAll().Count() == 0)) => Status.Scheduled,
                _ => Status.OnTrack
            };
        public static string ToStringProperty<T>(this T t)
        {
            string str = "";
            foreach (PropertyInfo item in t!.GetType().GetProperties())
            {
                str += "\n" + item.Name + ": " + item.GetValue(t, null);
            }
            return str;
        }
        public static string ToStringPropertyArray<T>(this T[] t)
        {
            string str = "";
            foreach (var elem in t)
            {
                foreach (PropertyInfo item in t.GetType().GetProperties())
                {
                    str += "\n" + item.Name + ": " + item.GetValue(t, null);
                }
            }
            return str;
        }
        public static void checkCost(int a)
        {
            if (a < 0)
            {
                throw new BlNotCorrectDetailsException("It's cannot be a negative number");
            }
        }
        public static void IsName(string name)
        {
            if (name == " ")
            {
                throw new BlNotCorrectDetailsException("It's cannot be null");
            }
        }
        public static void IsEnum(int l)
        {
            if (l < 0 || l > 5)
            {
                throw new BlNotCorrectDetailsException("This level doesn't exist");
            }
        }
        public static void CheckId(int d)
        {
            if (d < 0)
                throw new BlNotCorrectDetailsException("The id can't be a negative number");
        }
        public static void IsMail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                string[] parts = email.Split('@');
                if (parts.Length != 2)
                {
                    throw new BlNotCorrectDetailsException("THE EMAIL IS NOT CORRECT");
                }
                string domain = parts[1].ToLower(); 
                if (domain != "gmail.com")
                {
                    throw new BlNotCorrectDetailsException("THE EMAIL IS NOT CORRECT");
                }
            }
            catch (FormatException)
            {
                throw new BlNotCorrectDetailsException("THE EMAIL IS NOT CORRECT");
            }
        }
    }
}