using System.Collections.Generic;
using System.Linq;

namespace DotNet.Library.Extensions
{
    public static class GenericListEx
    {
        public static List<T> Add<T>(this List<T> list, params T[] items)
        {
            items.ToList().ForEach(list.Add);
            return list;
        }

        public static void AddIfNotNull<T>(this List<T> list, T value)
        {
            if (value.IsNull()) { return; }
            list.Add(value);
        }

        public static List<string> AddIfNotExists(this List<string> list, string value)
        {
            if (list.Count(x => x.EqualsIgnoreCase(value)).IsZero()) { list.Add(value); }
            return list;
        }

        public static List<string> AddRangeNoDuplicates(this List<string> list, List<string> range)
        {
            range.RunIfNotNullOrEmpty(() => range.ForEach((item, _) => list.AddIfNotExists(item)));
            return list;
        }

        public static bool ContainsIgnoreCase(this List<string> list, string toCompare)
        {
            return list.Any(x => x.EqualsIgnoreCase(toCompare));
        }
    }
}
