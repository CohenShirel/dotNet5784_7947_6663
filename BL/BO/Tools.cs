using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BO.Exceptions;

namespace BO
{
    static class Tools
    {
        public static string ToStringProperty<T>(this T t)
        {
            string str = "";
            foreach (PropertyInfo item in t.GetType().GetProperties()) 
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
            if (name!=" ")
            {
                throw new BlNotCorrectDetailsException("It's cannot be null");
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
            if(d<=0)
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
