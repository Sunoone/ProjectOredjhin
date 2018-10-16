using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public interface ICollidable
{
    void OnEnter();
    void OnExit();
}

public class Collidable : Collidable<Transform> { }

[RequireComponent(typeof(Collider2D))]
public class Collidable<T> : MonoBehaviour where T : Component
{
    public Callback<T> OnNewPriority;
    public Callback<T> OnLostPriority;

    public T GetPriorityCollision() { return _priorityCollision; }
    public LayerMask WhiteList;

    public bool Active = false;
    public BoxCollider2D _boxCollider;
    public RectTransform _rectTransform;
    public bool KeepCollisions;
    public List<T> Collisions = new List<T>();
    protected T _priorityCollision;
    protected T _previousCollision;

    protected virtual void Awake() { }
    protected virtual void Reset() {
        //_rectTransform = ((RectTransform)transform);
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.enabled = false;
    }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void Update()
    {
        if (Collisions.Count == 0 || !Active)
            return;

        Sort();
        _priorityCollision = Collisions[0];
        if (_priorityCollision != _previousCollision)
        {
            LostPriority();         
            Debug.Log("New priority: " + _priorityCollision.gameObject.name);
            _previousCollision = _priorityCollision;
            NewPriority();
        }
    }
    protected virtual void LostPriority() { OnLostPriority.Call(_priorityCollision); }
    protected virtual void NewPriority() { OnNewPriority.Call(_priorityCollision); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (WhiteList == (WhiteList | (1 << collision.gameObject.layer)))
        {
            T otherObject = collision.gameObject.GetComponent<T>();
            if (otherObject != null)
                OnEnter(otherObject);
        }
    }
    //private void OnTriggerStay2D(Collider2D collision) { OnStay(collision); }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (WhiteList == (WhiteList | (1 << collision.gameObject.layer)))
        {
            T otherObject = collision.gameObject.GetComponent<T>();
            if (otherObject != null)
                OnExit(otherObject);
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision) { }
    protected void OnCollisionExit2D(Collision2D collision) { }
    protected void OnCollisionStay2D(Collision2D collision) { }

    protected virtual void OnEnter(T otherObject)
    {
        if (!Collisions.Contains(otherObject))
        {
            //Debug.Log("Added other object");
            Collisions.Add(otherObject);
        }
    }    
    //protected virtual void OnStay(T otherObject) { }
    protected virtual void OnExit(T otherObject)
    {
        if (!KeepCollisions && Collisions.Contains(otherObject))
        {
            Collisions.Remove(otherObject);
            if (Collisions.Count == 0)
            {
                LostPriority();
                _priorityCollision = null;
                _previousCollision = null;
            }
            else
                GetClosest();
        }
    }

    public virtual void Refresh()
    {
        Collisions.Clear();
        _priorityCollision = null;
        _previousCollision = null;
        Active = false;
    }

 

    [EasyButtons.Button("Sort")]
    public virtual void Sort()
    {
        if (Collisions.Count == 0)
            return;
        Collisions = Collisions.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();
    }
    /// <summary>
    /// Call the method Sort first in order to enable this method.
    /// </summary>
    /// <param name="index">Which index of closest to this you want to get.</param>
    /// <returns>Closest object</returns>
    public virtual T GetClosestByIndex(int index = 0) { return Collisions[index]; }
    public virtual T GetClosest()
    {
        if (_rectTransform == null || Collisions.Count == 0)
            return null;

        int targetIndex = 0;
        float shortestDistance = float.MaxValue;
        int length = Collisions.Count;
        for (int i = 0; i < length; i++)
        {
            RectTransform target = ((RectTransform)Collisions[i].transform);
            Vector2 offset = (Input.mousePosition.x > target.position.x) ?
                new Vector2(target.sizeDelta.x / 2, -target.sizeDelta.y / 2) :
                new Vector2(-target.sizeDelta.x / 2, target.sizeDelta.y / 2);
            Vector2 targetPosition = target.position ;
            targetPosition.x = target.position.x + offset.x + offset.y;

            float distance = Vector2.Distance(Input.mousePosition, targetPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                targetIndex = i;
            }
        }
        Collisions.SetIndexToFirst(targetIndex);
        return Collisions[0];

        //Collisions = Collisions.OrderBy(x => Vector2.Distance(/*this.transform.position*/ Input.mousePosition, ((RectTransform)x.transform).position)).ToList();
        //return (Collisions.Count > 0) ? Collisions[0] : default(T);
    }

    public virtual List<T> GetList() { return Collisions; }
    public virtual List<T> GetListExclude(T item)
    {
        List<T> tempList = new List<T>();
        int length = Collisions.Count;
        for (int i = 0; i < length; i++)
        {
            if (Collisions[i] == item)
                continue;
            tempList.Add(Collisions[i]);
        }
        return tempList;
    }
    public virtual List<T> GetListExclude(int index)
    {
        List<T> tempList = new List<T>();
        int length = Collisions.Count;
        for (int i = 0; i < length; i++)
        {
            if (i == index)
                continue;
            tempList.Add(Collisions[i]);
        }
        return tempList;
    }
    public virtual List<T> GetListFrom(int index = 1)
    {
        List<T> tempList = new List<T>();
        int length = Collisions.Count;
        for (int i = index; i < length; i++)
        {
            tempList.Add(Collisions[i]);
        }
        return tempList;
    }
    public virtual void UpdateColliderSize()
    {
        if (_rectTransform != null)
            _boxCollider.size = _rectTransform.sizeDelta;
    }

    public virtual void RestoreList() { }
    public virtual void RestoreListExclude(T item) { }
    public virtual void RestoreListExclude(int index) { }
    public virtual void RestoreListFrom(int index = 1) { }

}
