using BlApi;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlImplementation;
using static BO.Exceptions;

namespace BO
{
    public static class Tools
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static readonly IDal _dal = DalApi.Factory.Get;
        
        public static void ScheduleProject(BO.Assignments ass)
        {
            IEnumerable<Link> lstPLinks;
            //BO.Assignments ass = s_bl.Assignments.Read(ID)!;//מחזירה משימה נוכחית
            //בדיקה אם למשימה שהכניס  אין משימות קודמות אז זה יהיה שווה למשימה הראשונה של הפרויקט
            //if (lstPLinks == null)//משימה ראשונית
            //{



            lstPLinks = _dal.Link.ReadAll(d => d.IdAssignments == ass.IdAssignments) ?? null!;//the previes ass
            if (lstPLinks.Count() == 0) // אם אין משימות קודמות
            {
                ass.DateBegin = Bl.StartProjectTime;
                ass.DeadLine = ass.DateBegin + TimeSpan.FromDays(ass.DurationAssignments);
                ass.status = GetEmployeeStatus(lstPLinks!);
                s_bl.Assignments!.Update(ref ass);
            
            }
            else
            {
                var maxDeadline = lstPLinks!.MaxBy(a => s_bl.Assignments.Read(a.IdAssignments)!.DeadLine);
                if (maxDeadline == null)
                    throw new FormatException("ERROR! There aren't dateBegin for previous assignments");
                // תאריך התחלתי
                // DateTime startDate = new DateTime(maxDeadline);

                // יצירת מחולק רנדומלי
                Random random = new Random();
                // הגרלת מספר ימים בטווח של שבוע
                int daysToAdd = random.Next(8);

                // הוספת מספר הימים המקריים לתאריך ההתחלתי
                DateTime? dt = s_bl.Assignments.Read(maxDeadline.IdAssignments)!.DeadLine;

                ass.DateBegin = dt + TimeSpan.FromDays(daysToAdd);
                ass.DeadLine = ass.DateBegin + TimeSpan.FromDays(ass.DurationAssignments);
                ass.status = GetEmployeeStatus(lstPLinks!);
                s_bl.Assignments!.Update(ref ass);
            }
            
            //return GetEmployeeStatus(lstPLinks!);//מחשבת סטטוס
        }
        //function that convert BOToDO

        public static DO.Worker ConvertWrkBOToDO(ref BO.Worker doWorker)
        {
            return new DO.Worker
            {
                IdWorker = doWorker.Id,
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
        //public static Status calaStatus(DO.Assignments assignments)
        //{
        //    if (assignments.DateBegin is null)
        //        return BO.Status.Unscheduled;
        //    if (assignments.DeadLine is not null)
        //        return BO.Status.OnTrack;
        //    return BO.Status.Done;
        //}
        public static Status calaStatus(DO.Assignments assignments)
        {
            BO.Assignments boAss = ConvertAssDOToBO(assignments);
            if (boAss.DateBegin is null)
                return BO.Status.Unscheduled;
            if (boAss.DeadLine is not null && boAss.links == null)
                return BO.Status.OnTrack;
            IEnumerable<Link> lstLinks = _dal.Link.ReadAll(d => d.IdAssignments == boAss.IdAssignments) ?? null!;//the previes ass
            if (boAss.links != null)
                return GetEmployeeStatus(lstLinks);
            return BO.Status.Done;
        }

        public static Status GetEmployeeStatus(IEnumerable<Link> lstLink)
        {
            bool PartTime = false;// קיימת משימה עם תאריך
            bool allTime = true;//לכל המשימות יש תאריך
            BO.Assignments assignment;
            //משימה ראשונה
            int i = 0;
            foreach (DO.Link lnk in lstLink)
            {
                assignment = s_bl.Assignments.Read(lnk.IdAssignments)!;
                //if (task.DependencyTaskId != null)
                if (assignment.DateBegin!=null)
                    PartTime = true;// אז אם יש למשימה תלות, אז יש הקצאת זמן
                else
                    allTime = false;// אם יש משימה ללא הקצאת זמן, אז לא כל המשימות הוקצו
            }
            if (i==0)
            {
                //reset all the status
                foreach (DO.Link lnk in lstLink)
                {
                    assignment = s_bl.Assignments.Read(lnk.IdAssignments)!;
                    assignment.status = Status.Scheduled;
                }
                return Status.Scheduled; // אם אין הקצאת זמן למשימות, הסטטוס הוא התחלתי
            }
            else if (allTime)
            {
                //reset all the status
                foreach (DO.Link lnk in lstLink)
                {
                    assignment = s_bl.Assignments.Read(lnk.IdAssignments)!;
                    assignment.status = Status.OnTrack;
                }
                return Status.OnTrack; //אם כל המשימות הוקצו זמן, הסטטוס הוא סופי
            }
            else if (PartTime)
            {
                //reset all the status
                foreach (DO.Link lnk in lstLink)
                {
                    assignment = s_bl.Assignments.Read(lnk.IdAssignments)!;
                    assignment.status = Status.Scheduled;
                }
                return Status.Scheduled; // אם יש הקצאת זמן למשימות, הסטטוס הוא ביניים
            }
            else
            {
                //reset all the status
                foreach (DO.Link lnk in lstLink)
                {
                    assignment = s_bl.Assignments.Read(lnk.IdAssignments)!;
                    assignment.status = Status.Unscheduled;
                }
                return Status.Unscheduled; // אם אין הקצאת זמן למשימות, הסטטוס הוא התחלתי
            }
        }
        //public static Status GetEmployeeStatus(List<DO.Link> lstLink)
        //{
        //    bool PartTime = false;// קיימת משימה עם תאריך
        //    bool allTime = true;//לכל המשימות יש תאריך

        //    foreach (Assignments assignment in lstAssignment)
        //    {
        //        //if (task.DependencyTaskId != null)
        //        if (assignment.DateBegin != null)
        //            PartTime = true;// אז אם יש למשימה תלות, אז יש הקצאת זמן
        //        else
        //            allTime = false;// אם יש משימה ללא הקצאת זמן, אז לא כל המשימות הוקצו
        //    }
        //    if (allTime)
        //    {
        //        //reset all the status
        //        foreach (Assignments assignment in lstAssignment)
        //            assignment.status = Status.OnTrack;
        //        return Status.OnTrack; //אם כל המשימות הוקצו זמן, הסטטוס הוא סופי
        //    }
        //    else if (PartTime)
        //    {
        //        //reset all the status
        //        foreach (Assignments assignment in lstAssignment)
        //            assignment.status = Status.Scheduled;
        //        return Status.Scheduled; // אם יש הקצאת זמן למשימות, הסטטוס הוא ביניים
        //    }
        //    else
        //    {
        //        //reset all the status
        //        foreach (Assignments assignment in lstAssignment)
        //            assignment.status = Status.Unscheduled;
        //        return Status.Unscheduled; // אם אין הקצאת זמן למשימות, הסטטוס הוא התחלתי
        //    }
        //}

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
            foreach(var elem in t)
            {
                foreach (PropertyInfo item in t.GetType().GetProperties())
                {
                    str += "\n" + item.Name + ": " + item.GetValue(t, null);
                }
            }
            return str;
        }
        //האם צריך לעשות כאן בדיקה אם זה מספרר ?
        public static void checkCost(int a)
        {
            if(a<0)
            {
                throw new BlNotCorrectDetailsException("It's cannot be a negative number");
            }
        }
        //האם צקיך לבדוק שזה אותיות בעברית או אנגלית?
        public static void IsName(string name)
        {
            if (name==" ")
            {
                throw new BlNotCorrectDetailsException("It's cannot be null");
            }
        }
        public static void IsEnum(int l)
        {
            if (l<0 || l>5)
            {
                throw new BlNotCorrectDetailsException("This level doesn't exist");
            }
        }
        public static void IsMail(string s)
        {
            int t = 0, c = 0;
            for (int i = 0; i < s.Length; i++)
            {//בדיקה שאין אותיות בעברית
                if ((s[i] < 'א' || s[i] >= 'ת') && (s[i] == ' '))
                    throw new BlNotCorrectDetailsException("The mail can't include hebrow letters");
                if (s[i] == '@')
                {
                    c++;
                    if (c > 1)
                        throw new BlNotCorrectDetailsException("The mail can't include @ more than one time");
                }

            }
            if (!s.Contains("@"))//@ בדיקה אם יש 
                throw new BlNotCorrectDetailsException("The mail have include @");
            if (s.IndexOf('@') == 0)// לא ראשון @ בדיקה  
                throw new BlNotCorrectDetailsException("The mail can't include @ first");
            for (int i = s.IndexOf('@'); i < s.Length; i++)
            {
                if (s[i] == '.')
                {
                    if (t == 0)
                    {
                        t++;
                        if (s.IndexOf("@") + 1 >= i)//בדיקה שיש אחרי שטרודל נקודה אבל לא ברצף
                            throw new BlNotCorrectDetailsException("The mail can't include @ and point one by one");
                        if (i == s.Length - 1)//בדיקה שהנקודה לא אחרונה
                            throw new BlNotCorrectDetailsException("The mail can't include point in the end");
                    }
                }
            }
            if (t == 0)//בדיקה אם יש נקודה
                throw new BlNotCorrectDetailsException("The mail have include point");

        }
        //בדיקת תקינות ת.ז
        public static void CheckId(int d)
        {
            if(d<0)
                throw new BlNotCorrectDetailsException("The id can't be a negative number");
            //while (d.Length < 9)
            //{
            //    d = "0" + d;
            //}


            //int s = 0, t;
            //for (int i = 0; i < d.Length; i++)
            //{
            //    if (i % 2 == 0)// הראשון זוגי להכפיל ב1  
            //    {
            //        s += Convert.ToInt32(d[i].ToString());
            //    }
            //    if (i % 2 != 0)
            //    {
            //        t = Convert.ToInt32(d[i].ToString()) * 2;
            //        if (t < 10)
            //            s += t;
            //        else
            //            s += t % 10 + t / 10;

            //    }
            //}

            //if (s % 10 == 0)
            //    throw new BlNotCorrectDetailsException("The id isn't correct");
        }
    }


}
//check if there are ass that link
//private static bool IsLinks(BO.Assignments ass)
//{
//    for (int i = 0; i < ass.links!.Count; i++)
//        //if there is ass that wasnt finished && the ass will finish after the current ass??????????
//        if (ass.links[i].DateFinish != null && ass.links[i].DateFinish > ass.dateSrart)
//            return false;
//    return true;
//}
//private static bool NoStartDate(BO.Assignments ass)
//{
//    for (int i = 0; i < ass.links!.Count; i++)
//        //if there is ass that wasnt finished && the ass will finish after the current ass??????????
//        if (ass.links[i].dateSrart!=null)
//            return false;
//    return true;//there are start date for everyone
//}