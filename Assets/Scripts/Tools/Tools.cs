using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools 
{
    public static List<T> ShuffleList<T>(this List<T> stack)
    {
        var r = new System.Random();
        var stackarray = stack.ToArray();
        for (int i = stackarray.Length - 1; i > 0; --i)
        {
            int k = r.Next(i + 1);
            var temp = stackarray[i];
            stackarray[i] = stackarray[k];
            stackarray[k] = temp;
        }
        return new List<T>(stackarray);
    }

    //  Based on Fisher Yates shuffle from wikipedia:
    public static Stack<T> ShuffleStack<T>(this Stack<T> stack)
    {
        var r = new System.Random();
        var stackarray = stack.ToArray();
        for (int i = stackarray.Length - 1; i > 0; --i)
        {
            int k = r.Next(i + 1);
            var temp = stackarray[i];
            stackarray[i] = stackarray[k];
            stackarray[k] = temp;
        }
        return new Stack<T>(stackarray);
    }
}
