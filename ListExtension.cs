using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel_accounting
{
    public static class ListExtension
    {
        public static List<string> MergeListsByIndex(this List<string> current, List<string> other)
        {
            for (int i = 0; i < current.Count; i++)
                current[i] += " " + other[i];
            return current;
        }
    }
}
