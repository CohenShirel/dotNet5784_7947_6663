using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }


}
