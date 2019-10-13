using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public string PoolID;
    public Transform Parent;

    public List<GameObject> PoolList = new List<GameObject>();
    protected int PoolCount { get { return PoolList.Count; } }

    public GameObject pooledObject;
    public int initialPoolAmount;
    public bool WillGrow = true;

    void Start()
    {
        PoolList = new List<GameObject>();
        for (int i = 0; i < initialPoolAmount; i++)
        {
            GameObject t_go;
            if (Parent != null)
                t_go = GameObject.Instantiate(pooledObject, Parent);
            else
                t_go = GameObject.Instantiate(pooledObject);
            t_go.SetActive(false);
            PoolList.Add(t_go);
        }
    }

    public virtual GameObject GetObject()
    {
        GameObject t_GameObject = GetObjectFromPool();
        if (t_GameObject != null)
            return t_GameObject;

        t_GameObject = Grow();
        return t_GameObject;
    }
    public virtual List<GameObject> GetObjects(int _amount, Callback _cb = null)
    {
        List<GameObject> t_list = new List<GameObject>();
        for (int i = 0; i < _amount; i++)
        {
            GameObject t_GameObject = GetObject();
            if (t_GameObject != null)
            {
                t_list.Add(t_GameObject);
                if (_cb != null)
                    _cb.Invoke();
            }
        }
        return t_list;
    }
    public virtual T GetObject<T>()
    {
        GameObject t_go = GetObjectFromPool();
        if (t_go != null)
            return t_go.GetComponent<T>();

        t_go = Grow();
        return t_go.GetComponent<T>();
    }
    public virtual List<T> GetObjects<T>(int _amount, Callback _cb = null)
    {
        List<T> t_list = new List<T>();
        for (int i = 0; i < _amount; i++)
        {
            GameObject t_go = GetObject();
            if (t_go != null)
            {
                t_list.Add(t_go.GetComponent<T>());
                if (_cb != null)
                    _cb.Invoke();
            }
        }
        return t_list;
    }

    protected virtual GameObject GetObjectFromPool()
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
        {
            if (!PoolList[i].gameObject.activeInHierarchy)
            {
                PoolList[i].gameObject.SetActive(true);
                //Debug.Log("Got " + PoolList[i].name);
                return PoolList[i];
            }
        }
        return null;
    }
    protected virtual GameObject Grow()
    {
        if (WillGrow)
        {
            GameObject t_go;
            if (Parent != null)
                t_go = GameObject.Instantiate(pooledObject, Parent);
            else
                t_go = GameObject.Instantiate(pooledObject);
            PoolList.Add(t_go);
            //Debug.Log("Added " + t_go.name);
            t_go.SetActive(true);
            return t_go;
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

    public virtual void PoolAll()
    {
        int t_count = PoolList.Count;
        for (int i = 0; i < t_count; i++)
            PoolList[i].gameObject.SetActive(false);
    }
    public virtual void PoolAllExcept(GameObject _go)
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
