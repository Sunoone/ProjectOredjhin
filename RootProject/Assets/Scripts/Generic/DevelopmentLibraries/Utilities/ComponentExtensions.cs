using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ComponentUtility
{
    public static void Unsupported<T>(this T t) where T : class
    {
        Debug.LogError("<color = red>" +  t + " doesn't use this method, try looking further in the inheritance tree</color>");
    }
    static bool Compare<T>(this T x, T y) where T : class
    {
        return x == y;
    }
    public static T GetComponent<T>(GameObject _go)
    {
        T t_T = default(T);
        if (_go != null)
        {
            t_T = _go.GetComponent<T>();
            if (t_T != null)
                return t_T;
            else
            {
                t_T = _go.GetComponentInChildren<T>();
                if (t_T != null)
                    return t_T;
            }
        }
        return default(T);
    }
}


public static  class ComponentExtensions {

    public enum PlacementType
    {
        Parent,
        Self,
        Child
    }
    public static T SolveComponent<T>(this GameObject _go, string _objectName = "", PlacementType _placementType = PlacementType.Self, List<T> _exclusionList = null) where T : Component
    {
        T t_T = _go.SearchComponent(_objectName, _exclusionList);
        if (t_T == default(T))
        {
            GameObject t_go;
            switch (_placementType)
            {              
                case PlacementType.Parent:                  
                    t_go = GameObject.Instantiate(new GameObject(), _go.transform.parent);
                    t_go.transform.position = _go.transform.position;
                    _go.transform.SetParent(t_go.transform);
                    if (_objectName != "")
                        t_go.name = _objectName;
                    Debug.LogWarning("Spawned new child for " + _go.name);
                    t_T = t_go.AddComponent<T>();
                    Debug.LogWarning("Added component of type " + typeof(T) + " to " + t_go.name);
                    return t_T;
                case PlacementType.Self:
                    t_T = _go.AddComponent<T>();
                    Debug.LogWarning("Added component of type " + typeof(T) + " to " + _go.name);
                    return t_T;
                case PlacementType.Child:
                    
                    t_go = GameObject.Instantiate(new GameObject(), _go.transform);
                    t_go.transform.localPosition = Vector3.zero;
                    if (_objectName != "")
                        t_go.name = _objectName;
                    Debug.LogWarning("Spawned new child for " + _go.name);
                    t_T = t_go.AddComponent<T>();
                    Debug.LogWarning("Added component of type " + typeof(T) + " to " + t_go.name);
                    return t_T;
                default:
                    break;
            }
            
        }
        return t_T;
    }
    public static T SearchComponent<T>(this GameObject _go, string _objectName = "", List<T> _exclusionList = null) where T : Component
    {      
        T t_T = default(T);
        t_T = CompareComponents<T>(_go.GetComponents<T>(), _exclusionList, _objectName);
        if (t_T != null)
            return t_T;

        t_T = CompareComponents<T>(_go.GetComponentsInChildren<T>(), _exclusionList, _objectName);
        if (t_T != null)
            return t_T;

        t_T = CompareComponents<T>(_go.GetComponentsInParent<T>(), _exclusionList, _objectName);
        if (t_T != null)
            return t_T;

        return default(T);
    }
    private static T CompareComponents<T>(T[] _resultList, List<T> _exclusionList, string _objectName = "") where T : Component
    {
        T t_T = default(T);
        if (_resultList != (default(T[])))
        {       
            int t_resultLength = _resultList.Length;
            if (t_resultLength > 0)
            {                                  
                int t_exclusionCount = 0;
                if (_exclusionList != null)
                    t_exclusionCount = _exclusionList.Count;

                if (t_exclusionCount > 0)
                {
                    for (int i = 0; i < t_exclusionCount; i++)
                        for (int e = t_resultLength - 1; e >= 0; e--)
                        {
                            if (_objectName != "" && !_objectName.Contains(_resultList[i].gameObject.name))
                                continue;

                            if (!_exclusionList[i].Compare(_resultList[e]))
                                t_T = _resultList[e];
                        }
                    return t_T;
                }

                for (int i = t_resultLength - 1; i >= 0; i--)
                {
                    if (_objectName != "" && !_objectName.Contains(_resultList[i].gameObject.name))
                        continue;
                    return _resultList[i];
                }  
            }
        }     
        return t_T;
    }
    public static T[] SearchComponents<T>(this GameObject _go)
    {
        T[] t_T = default(T[]);
        t_T = _go.GetComponents<T>();
        if (t_T != null)
            return t_T;
        else
        {
            t_T = _go.GetComponentsInChildren<T>();
            if (t_T != null)
                return t_T;
            else
            {
                t_T = _go.GetComponentsInParent<T>();
                if (t_T != null)
                    return t_T;
            }
        }
        return default(T[]);
    }
    public static bool Compare<T>(this T _t, T _other) where T : class
    {
        return _t == _other;
    }
    public static T GetInterface<T>(this GameObject inObj) where T : class
    {
        return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
    }
    public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
    {
        return inObj.GetComponents<Component>().OfType<T>();
    }

    /*public static void Timer(this MonoBehaviour target, float duration, Callback callback)
    {
        target.StartCoroutine()
        yield return new WaitForSeconds(duration);
        callback.Call();
    }*/


}
