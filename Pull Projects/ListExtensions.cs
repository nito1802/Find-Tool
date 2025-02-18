using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Projects
{
    /*

     // Konwerter JSON z użyciem Newtonsoft.Json dla pola Errors
            modelBuilder.Entity<JobHandlerEntity>()
                .Property(e => e.Errors)
                .HasConversion(
                    new ValueConverter<List<HandlerDto>, string>(
                        v => JsonConvert.SerializeObject(v, Formatting.None),
                        v => string.IsNullOrEmpty(v)
                             ? new List<HandlerDto>()
                             : JsonConvert.DeserializeObject<List<HandlerDto>>(v)
                    )
                );

     */

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