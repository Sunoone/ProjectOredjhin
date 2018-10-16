using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Tracable<Y, Z> : MonoBehaviour where Y : MonoBehaviour
{
    private void Awake()
    {
        TracableObject = gameObject.GetComponent<Y>();
    }
    public Y TracableObject;
    public Z Owner;

    public virtual void Refresh()
    {

    }

    protected virtual void Reset() { TracableObject = gameObject.SearchComponent<Y>(); }
}

/// <summary>
/// A garbage collector which will only use as many as needed, across all objects. Being able to recycle their own active ones.
/// </summary>
/// <typeparam name="X">Tracable Class, this class should contain your callback functionality</typeparam>
/// <typeparam name="Y">Element Type</typeparam>
/// <typeparam name="Z">ID Type</typeparam>
public abstract class Recycler<X, Y, Z> : Pool<X> where X : Tracable<Y, Z> where Y : MonoBehaviour where Z : class{

    public bool WillRecycle = true;
    /*public static Recycler<X, Y, Z> GetRecycler()
    {
        return 
    }*/

    public virtual Y GetObject(Z _owner, bool _newObject = false , Callback<X> _cb = null)
    {
        int t_start = 0;
        X t_recycable = Recycle(_owner, _newObject, ref t_start, _cb);
        if (t_recycable != null)
            return t_recycable.TracableObject;

        t_recycable = GetObjectFromPool(_owner, _cb);
        if (t_recycable != null)
            return t_recycable.TracableObject;

        t_recycable = Grow(_owner, _cb);
        return t_recycable.TracableObject;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_amount">How many elements you want to use</param>
    /// <param name="_newObject">If you want to recycle or add a new elements</param>
    /// <param name="_owner">ID Type</param>
    /// <param name="_cb">Call back on every element found</param>
    /// <returns></returns>
    public virtual List<Y> GetObjects(int _amount = -1, bool _newObject = false, Z _owner = default(Z), Callback<X> _cb = null)
    {
        List<Y> t_TList = new List<Y>();
        int t_count = PoolList.Count;
        int t_lastIndex = 0;
        for (int i = 0; i < t_count; i++)
            //if (_amount == -1 || t_TList.Count < _amount)
                AddObjectsToList(i, Recycle(_owner, _newObject, ref t_lastIndex), ref t_TList, _cb);

        int t_start = t_TList.Count;
        if (t_start < _amount)
        {
            for (int i = t_start; i < _amount; i++)
                if (!AddObjectsToList(i, GetObjectFromPool(_owner), ref t_TList, _cb))
                    AddObjectsToList(i, Grow(_owner), ref t_TList, _cb);
            return t_TList;
        }
        
        t_count = t_TList.Count;
        for (int i = _amount; i < t_count; i++)
            t_TList[i].gameObject.SetActive(false);
        return t_TList;
    }

    private bool AddObjectsToList(int _index, X _x, ref List<Y> _list, Callback<X> _cb)
    {
        if (_x.Compare(default(X)))
            return false;

        _list.Add(_x.TracableObject);
        return true;
    }

    protected virtual X Recycle(Z _owner, bool _newObject, ref int _startIndex, Callback<X> _cb = null)
    {
        if (_newObject || !WillRecycle)
            return null;

        int t_count = PoolList.Count;
        for (int i = _startIndex; i < t_count; i++)
        {
            X t_X = PoolList[i];
            if (_owner.Compare(t_X.Owner))// && PoolList[i].gameObject.activeInHierarchy)
            {
                _startIndex = i+1;

                t_X.Refresh();
                if (_cb != null)
                    _cb.Invoke(t_X);

                return t_X;
            }
        }
        return null;
    }
    protected virtual X GetObjectFromPool(Z _owner, Callback<X> _cb = null)
    {
        int t_count = PoolCount;
        for (int i = 0; i < t_count; i++)
        {
            X t_X = PoolList[i].GetComponent<X>();
            GameObject t_go = t_X.gameObject;
            if (!t_go.activeInHierarchy)
            {          
                t_X.Owner = _owner;
                t_go.SetActive(true);

                t_X.Refresh();
                if (_cb != null)
                    _cb.Invoke(t_X);

                return t_X;
            }
        }
        return null;
    }
    protected virtual X Grow(Z _owner, Callback<X> _cb = null)
    {
        if (WillGrow)
        {
            {
                X t_X;
                if (Parent != null)
                    t_X = GameObject.Instantiate(pooledObject, Parent);
                else
                    t_X = GameObject.Instantiate(pooledObject);
                PoolList.Add(t_X);
                t_X.Owner = _owner;

                t_X.Refresh();
                if (_cb != null)
                    _cb.Invoke(t_X);

                t_X.gameObject.SetActive(true);
                return t_X;
            }
        }
        return null;
    }

    /// <summary>
    /// Disabled all objects associated with an owner.
    /// </summary>
    /// <param name="_owner">ID Type</param>
    public virtual void PoolAllFromOwner(Z _owner, Callback<X> _cb = null)
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
        {
            X t_X = PoolList[i];
            if (_owner == t_X.Owner)
            {
                if (_cb != null)
                    _cb.Invoke(t_X);

                t_X.gameObject.SetActive(false);
            }
        }
    }
}
