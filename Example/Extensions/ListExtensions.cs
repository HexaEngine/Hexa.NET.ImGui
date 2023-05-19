namespace HexaEngine.Core.Extensions
{
    using System.Collections.Generic;

    public static class ListExtensions
    {
        public static T[] GetInternalArray<T>(this List<T> list)
        {
            return ArrayAccessor<T>.Getter(list);
        }
    }
}