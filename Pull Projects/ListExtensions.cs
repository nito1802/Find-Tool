using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Projects
{
    public static class ListExtensions
    {
        public static List<T> ShiftToFirst<T>(this List<T> list, Func<T, bool> predicate)
        {
            if (list == null || list.Count < 2)
                return list;

            var targetItem = list.SingleOrDefault(predicate);
            if (targetItem == null)
                return list;

            return new List<T> { targetItem }
                .Concat(list.Where(item => !predicate(item)))
                .ToList();
        }
    }
}
