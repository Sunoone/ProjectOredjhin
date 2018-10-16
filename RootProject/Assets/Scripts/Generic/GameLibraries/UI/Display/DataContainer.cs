using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DataContainer<X> where X : class 
{ 
    [SerializeField] protected X _data;
    public X Data { get { return _data; } }

    public virtual void SetData(X data)
    {
        Refresh();
        _data = data;
        if (_data.IsDefault())
        {
            DataIsDefault();
            return;
        }
    }

    protected virtual void DataIsDefault()
    {
        Debug.LogWarning("Default value data is being set.");
    }

    public virtual void Refresh() { _data = default(X); }
}
