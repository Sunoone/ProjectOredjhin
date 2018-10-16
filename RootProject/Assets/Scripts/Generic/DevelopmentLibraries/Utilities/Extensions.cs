using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StateMachine;
using UnityEngine.Events;
using System.Linq;

public delegate bool RequirementCheck<T>(T t);

public delegate void Callback();
public delegate void Callback<X>(X x);
public delegate void Callback<X, Y>(X x, Y y);
public delegate void Callback<X, Y, Z>(X x, Y y, Z z);
public delegate Z ReturnCallback<X, Y, Z>(X x, Y y);

public enum AnimationState
{
    Idle,
    Play,
    Pause,
}

public static class Constants
{
    public const string DefaultString = "Default";
}

public static class Extensions
{
    public static void Call(this Callback cb) { if (cb != null) cb.Invoke(); }
    public static void Call<X>(this Callback<X> cb, X x) { { if (cb != null) cb.Invoke(x); } }
    public static void Call<X>(this Callback<X, X> cb, X x1, X x2) { { if (cb != null) cb.Invoke(x1, x2); } }
    public static void Call<X, Y>(this Callback<X, Y> cb, X x, Y y) { if (cb != null) cb.Invoke(x, y); }
    public static void Call<X, Y, Z>(this Callback<X, Y, Z> cb, X x, Y y, Z z) { if (cb != null) cb.Invoke(x, y, z); }
    public static Z Call<X, Y, Z>(this ReturnCallback<X,Y, Z> cb, X x, Y y) { return (cb != null)?  cb.Invoke(x, y) : default(Z); }

    public static bool IsDefault<T>(this T item) where T : class { return item == default(T); }

    public static IEnumerator SetTimer(this MonoBehaviour target, float duration, Callback cb)
    {
        IEnumerator timer = Timer(duration, cb);
        if (duration > 0)
            target.StartCoroutine(timer);
        return timer;
    }
    public static IEnumerator Timer(float duration, Callback cb)
    {
        yield return new WaitForSeconds(duration);
        cb.Call();
    } 

    public static IEnumerable<T> MoveSection<T>(this IEnumerable<T> @this, int insertionPoint, int startIndex, int endIndex)
    {
        var counter = 0;
        var numElements = endIndex - startIndex;
        var range = Enumerable.Range(startIndex, numElements);
        foreach (var i in @this)
        {
            if (counter == insertionPoint)
            {
                foreach (var j in @this.Skip(startIndex).Take(numElements))
                {
                    yield return j;
                }
            }
            if (!range.Contains(counter))
            {
                yield return i;
            }
            counter++;
        }
        //The insertion point might have been after the entire list:
        if (counter++ == insertionPoint)
        {
            foreach (var j in @this.Skip(startIndex).Take(numElements))
            {
                yield return j;
            }
        }
    }






    #region List Extensions
    public static X Get<X>(this List<X> list, X item)  where X : class
    {
        int length = list.Count;
        for (int i = 0; i < length; i++)
            if (list[i] == item)
                return list[i];
        return null;
    }
    public static X GetLast<X>(this List<X> list) where X : class {
        int index = list.Count - 1;
        if ((list.IsValidIndex(index)))
            return list[index];

        Debug.LogError("No items in list. Returning null.");
        return  default(X);
    } 
    public static bool Has<X>(this List<X> list, X item) where X : class
    {
        int length = list.Count;
        for (int i = 0; i < length; i++)
            if (list[i] == item)
                return true;
        return false;
    }
    public static bool IsValidIndex<X>(this List<X> list, int index) { return (index >= 0 && index < list.Count); }

    private static System.Random rng = new System.Random();
    public static void Shuffle<X>(this List<X> list) where X : class
    {
        int n = list.Count;
        while(n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            X value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Creates a deep copy of the this list.
    /// </summary>
    /// <typeparam name="X">Clonable object</typeparam>
    /// <param name="list">The list to copy</param>
    /// <returns>Returns the deep copied list.</returns>
    public static List<X> CloneList<X>(this List<X> list) where X : class, ICloneable<X>
    {
        if (list == null)
            return default(List<X>);

        int length = list.Count;
        if (length == 0)
            return default(List<X>);
  
        for (int i = 0; i < length; i++)
        {
            if (list[i] == default(X))
                return default(List<X>);
        }

        List<X> newList = new List<X>();
        newList = list.ConvertAll(element => element.Clone());
        return newList;
    }


    public static void SetIndexToLast<X>(this List<X> list, int index) where X : class
    {
        X value = list[index];

        int length = list.Count;
        for (int i = index; i < length - 2; i++)
        {
            list[i] = list[i + 1];
        }
        list[length - 1] = value;
    }
    public static void SetItemToLast<X>(this List<X> list, X item) where X : class 
    {
        int index = list.FindIndex((x => x == item));
        X value = list[index];

        int length = list.Count;
        for (int i = index; i < length - 1; i++)
        {
            list[i] = list[i + 1];
        }
        list[length - 1] = value;
    }
    public static void SetIndexToFirst<X>(this List<X> list, int index) where X : class
    {
        X value = list[index];
        list.RemoveAt(index);
        list.Insert(0, value);
    }
    public static void SetItemToFirst<X>(this List<X> list, X item) where X : class
    {
        int index = list.FindIndex((x => x == item));
        X value = list[index];
        list.RemoveAt(index);
        list.Insert(0, value);
    }
    #endregion

    #region Array Extensions
    /// <summary>
    /// Creates a deep clone of an object using serialization.
    /// </summary>
    /// <typeparam name="T">The type to be cloned/copied.</typeparam>
    /// <param name="o">The object to be cloned.</param>
    public static T DeepCloneObject<T>(this T o)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
    public static X AddAndReturn<X>(this List<X> list, X item)
    {
        list.Add(item);
        return list[list.Count - 1];
    }
    public static bool IsValidIndex<X>(this X[] list, int index) { return (index > 0 && index < list.Length); }
    public static bool Contains<X>(this X[] list, X item) where X : class
    {
        int length = list.Length;
        for (int i = 0; i < length; i++)
            if (list[i] == item)
                return true;
        return false;
    }
    public static int GetIndex<X>(this X[] list, X item) where X : class
    {
        int length = list.Length;
        for (int i = 0; i < length; i++)
            if (list[i] == item)
                return i;
        return -1;
    }
    public static X Get<X>(this X[] list, X item) where X : class
    {
        int length = list.Length;
        for (int i = 0; i < length; i++)
            if (list[i] == item)
                return item;
        return null;
    }
   
    public static X[] MoveArrayItem<X>(this X[] array, int index)
    {
        if (index + 1 >= array.Length)
            return array;

        X shell;
        shell = array[index + 1];
        array[index + 1] = array[index];
        array[index] = shell;
        return array;
    }
    public static X[] DuplicateLastArrayItem<X>(this X[] array)
    {
        int length = array.Length;
        if (length == 0)
        {
            X[] newArray =  new X[1];
            newArray[0] = default(X);
        }

        X[] newBranches = new X[length + 1];
        for (int i = 0; i < length; i++)
            newBranches[i] = array[i];
        newBranches[length] = default(X);
        array = newBranches;
        return array;
    }
    public static X[] RemoveLastArrayItem<X>(this X[] array)
    {
        int length = array.Length;
        if (length <= 1)
            return new X[0];

        X[] newBranches = new X[length - 1];
        for (int i = 0; i < length - 1; i++)
            newBranches[i] = array[i];
        array = newBranches;
        return array;
    }
    public static X[] RemoveArrayItem<X>(this X[] array, int index)
    {
        int length = array.Length;
        if (length <= 1)
            return new X[0];

        X[] newBranches = new X[length - 1];
        int count = 0;
        for (int i = 0; i < length - 1; i++)
        {
            count = i;
            if (count == index)
                count++;
            newBranches[i] = array[count];
        }
        array = newBranches;
        return array;
    }
    #endregion
}

public static class UnityEventExtensions
{
    public static void Call(this UnityEvent ue)
    {
        if (ue != null)
            ue.Invoke();
    }
    public static void Call<T>(this UnityEvent<T> ue, T arg1)
    {
        if (ue != null)
            ue.Invoke(arg1);
    }
    public static void Call<X, Y>(this UnityEvent<X, Y> ue, X arg1, Y arg2)
    {
        if (ue != null)
            ue.Invoke(arg1, arg2);
    }
    public static void Call<X, Y, Z>(this UnityEvent<X, Y, Z> ue, X arg1, Y arg2, Z arg3)
    {
        if (ue != null)
            ue.Invoke(arg1, arg2, arg3);
    }
}

public static class MonoBehaviourExtensions
{
    public static void Invoke(this MonoBehaviour me, Action theDelegate, float time)
    {
        me.StartCoroutine(ExecuteAfterTime(theDelegate, time));
    }

    private static IEnumerator ExecuteAfterTime(Action theDelegate, float delay)
    {
        yield return new WaitForSeconds(delay);
        theDelegate();
    }
}

public enum Colors
{
    white,
    red,
    green,
    blue,
    cyan,
    magenta,
    yellow,
    orange
}
public static class FLog
{
    
}
