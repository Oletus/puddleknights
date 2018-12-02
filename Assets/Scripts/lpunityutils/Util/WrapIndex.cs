using System.Collections.Generic;

namespace LPUnityUtils
{

    static class WrapIndex
    {
        public static void Wrap<T>(ref int index, List<T> list)
        {
            index = (index % list.Count + list.Count) % list.Count;
        }
    }

}