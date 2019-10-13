using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Pool<X> : MonoBehaviour where X : Component
{
    public string PoolID;
    public Transform Parent;

    public List<X> PoolList = new List<X>();
    protected int PoolCount { get { return PoolList.Count; } }

    public X pooledObject;
    public int initialPoolAmount;
    public bool WillGrow = true;

    private void Awake()
    {
        //if (pooledObject.IsDefault())
        //    ErrorPopup.ShowError(this.GetType() +" hasn't been assigned which object to spawn.");

        Init();
    }

    private bool init = false;
    public void Init()
    {
        if (!init)
        {
            PoolList = new List<X>();
            for (int i = 0; i < initialPoolAmount; i++)
            {
                X t_go;
                if (Parent != null)
                    t_go = GameObject.Instantiate(pooledObject, Parent);
                else
                    t_go = GameObject.Instantiate(pooledObject);
                t_go.gameObject.SetActive(false);
                t_go.name = t_go.name + i;
                PoolList.Add(t_go);
            }
            init = true;
        }
    }

    public Y GetObject<Y>() where Y : MonoBehaviour { return GetObject() as Y; }
    //public List<Y> GetObjects<Y>() where Y : MonoBehaviour { return GetObjects() as List<Y>; }

    public virtual X GetObject()
    {
        X t_X = GetObjectFromPool();
        if (t_X != null)
            return t_X;

        t_X = Grow();
        return t_X;
    }
    public virtual X GetObjectHidden()
    {
        X t_X = GetObjectFromPoolHidden();
        if (t_X != null)
            return t_X;

        t_X = Grow();
        return t_X;
    }
    public virtual List<X> GetObjects(int _amount, Callback _cb = null)
    {
        List<X> t_list = new List<X>();
        for (int i = 0; i < _amount; i++)
        {
            X t_X = GetObject();
            if (t_X != null)
            {
                t_list.Add(t_X);
                if (_cb != null)
                    _cb.Invoke();
            }
        }
        return t_list;
    }

    protected virtual X GetObjectFromPoolHidden(Callback<X> _cb = null)
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
        {
            X t_X = PoolList[i];
            if (!t_X.gameObject.activeInHierarchy)
            {
                if (_cb != null)
                    _cb.Invoke(t_X);

                return t_X;
            }
        }
        return null;
    }

    protected virtual X GetObjectFromPool(Callback<X> _cb = null)
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
        {
            X t_X = PoolList[i];
            if (!t_X.gameObject.activeInHierarchy)
            {
                t_X.gameObject.SetActive(true);

                if (_cb != null)
                    _cb.Invoke(t_X);

                return t_X;
            }
        }
        return null;
    } 
    protected virtual X Grow(Callback<X> _cb = null)
    {
        if (WillGrow)
        {
            X t_X;
            if (Parent != null)
                t_X = GameObject.Instantiate(pooledObject, Parent);
            else
                t_X = GameObject.Instantiate(pooledObject);
            PoolList.Add(t_X);
            t_X.gameObject.name += PoolList.Count - 1;
            if (_cb != null)
                _cb.Invoke(t_X);

            t_X.gameObject.SetActive(true);
            return t_X;
        }
        return null;
    }
   
    public virtual void PoolFromRange(int _from, int _till = -1)
    {
        int t_count = PoolList.Count;
        if (_from > t_count)
            return;
    
        if (_till > t_count || _till < 1)
            _till = t_count;
        for (int i = _from; i < _till; i++)
            PoolList[i].gameObject.SetActive(false);
    }
    [EasyButtons.Button("Pool All")]
    public virtual void PoolAll()
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
            PoolList[i].gameObject.SetActive(false);
    }
    public virtual void PoolAllExcept(X _go)
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
            if (PoolList[i] != _go)
                PoolList[i].gameObject.SetActive(false);
    }


    protected virtual void Reset()
    {
        Parent = transform;
    }
}