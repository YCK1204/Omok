public static class Extension
{
    public static T[] Concat<T>(this T[] elem1, T[] elem2) where T : new()
    {
        T[] ret = new T[elem1.Length + elem2.Length];
        Buffer.BlockCopy(elem1, 0, ret, 0, elem1.Length);
        Buffer.BlockCopy(elem2, 0, ret, elem1.Length, elem2.Length);
        return ret;
    }
}
