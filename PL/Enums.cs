using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;
//public class Enums
//{
//    public DO.Level level { get; set; } = DO.Level.None;
//}
    internal class StatussCollection : IEnumerable
    {
        static readonly IEnumerable<BO.Status> s_enums =
        (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class LevelsCollection : IEnumerable
    {
        static readonly IEnumerable<DO.Level> s_enums =
        (Enum.GetValues(typeof(DO.Level)) as IEnumerable<DO.Level>)!;
        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class LevelsCollectionExceptNone : IEnumerable
    {
     static readonly IEnumerable<DO.Level> s_enums =
    Enum.GetValues(typeof(DO.Level))
    .OfType<DO.Level>()
    .Where(x => x != DO.Level.None);

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }