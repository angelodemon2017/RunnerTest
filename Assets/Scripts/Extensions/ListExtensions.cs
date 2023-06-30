using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T RandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T RandomElementByIWeight<T>(this List<T> elements) where T : IWeight
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < elements.Count; i++)
        {
            for (int j = 0; j < elements[i].Weight; j++)
            {
                indexes.Add(i);
            }
        }

        return elements[indexes.RandomElement()];
    }
}