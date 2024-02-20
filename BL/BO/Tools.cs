using BlImplementation;
using DalApi;
using DO;
using System.ComponentModel.DataAnnotations;
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
                .ToHashSet();//the previes ass

            if (lstPLinks.Count() is 0) // אם אין משימות קודמות
            {
                ass.DateBegin = Bl.StartProjectTime;
            }

            else
            {
                var assignments = s_bl.Assignment.ReadAll(a => lstPLinks.Contains(a.Id));
                // if (lstPLinks.All(link => s_bl.Assignment.Read(link.IdAssignment)!.DeadLine != null))
                var maxDeadline = assignments!.MaxBy(a => a.DeadLine);

                if (maxDeadline!.DateBegin == null)
                    throw new FormatException("ERROR! There aren't dateBegin for previous assignment");
                // תאריך התחלתי
                // DateTime startDate = new DateTime(maxDeadline);

                // יצירת מחולק רנדומלי
                Random random = new Random();
                // הגרלת מספר ימים בטווח של שבוע
                int daysToAdd = random.Next(8);

                // הוספת מספר הימים המקריים לתאריך ההתחלתי
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
            (_dal.StartProjectTime, _dal.Assignment) switch
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

        //האם צריך לעשות כאן בדיקה אם זה מספרר ?
        public static void checkCost(int a)
        {
            if (a < 0)
            {
                throw new BlNotCorrectDetailsException("It's cannot be a negative number");
            }
        }
        //האם צקיך לבדוק שזה אותיות בעברית או אנגלית?
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
        public static void IsMail(string s) /*=> new EmailAddressAttribute().IsValid(s);*/
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
            if (d < 0)
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
//private static bool IsLinks(BO.Assignment ass)
//{
//    for (int i = 0; i < ass.Links!.Count; i++)
//        //if there is ass that wasnt finished && the ass will finish after the current ass??????????
//        if (ass.Links[i].DateFinish != null && ass.Links[i].DateFinish > ass.dateSrart)
//            return false;
//    return true;
//}
//private static bool NoStartDate(BO.Assignment ass)
//{
//    for (int i = 0; i < ass.Links!.Count; i++)
//        //if there is ass that wasnt finished && the ass will finish after the current ass??????????
//        if (ass.Links[i].dateSrart!=null)
//            return false;
//    return true;//there are start date for everyone
//}