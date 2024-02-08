using System.Collections.Generic;

public static class Utils
{
    public static Dictionary<string, T> ArrayToDictionary<T>(this T[] array) where T : DictionaryItem
    {
        Dictionary<string, T> dict = new Dictionary<string, T>();

        if (array == null)
            return dict;

        for (int i = 0; i < array.Length; ++i)
        {
            dict.Add(array[i].id, array[i]);
        }
        return dict;
    }
}
