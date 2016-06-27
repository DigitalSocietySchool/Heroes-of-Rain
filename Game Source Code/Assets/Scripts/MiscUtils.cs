using UnityEngine;
using System.Collections;

public static class MiscUtils
{
    public static void SwapArrayElements(System.Array array, int i, int j)
    {
        object temp = array.GetValue(i);
        array.SetValue(array.GetValue(j), i);
        array.SetValue(temp, j);
    }

    public static void ShuffleArray(System.Array array)
    {
        for (int i = 0; i < array.Length; i++)
            SwapArrayElements(array, i, (int)(Random.value * array.Length));
    }
}