using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using Freethware.Inputs;

public class PointerClickable : MonoBehaviour, IClickHandler, IDownHandler, IUpHandler

{
    public bool CanClick = true; 
    public UnityEvent OnClick;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnDown;
    public UnityEvent OnUp;
    public UnityEvent OnHold;

    protected virtual void Awake() { }


    [ReadOnly]
    public Image Image;
    protected virtual void Reset()
    {
        //base.Reset();
        Image = gameObject.SolveComponent<Image>();
    }

    public virtual void Click(Entity user, Vector2 pos) { if (CanClick) Click(); }
    public virtual void Down(Entity user, Vector2 pos) { PointerDown(pos); }
    public virtual void Up(Entity user, Vector2 pos) { PointerUp(pos); }

    public virtual void Click()//PointerEventData eventData = null)
    {
        Debug.Log("Clicked the section");
        if (PointerSelectable.CanSelectNew)
        {
            EventSystem eventData = EventSystem.current;
            eventData.SetSelectedGameObject(gameObject);
        }
        OnClick.Invoke();
    }

    protected virtual void Update()
    {
        if (_pointerDown)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _pointerDown = false;
                PointerReleased(Input.mousePosition);
            }
            if (OnHold != null)
            {
                OnHold.Invoke();
            }
        }
    }
    protected bool _pointerDown = false;
    
    public virtual void PointerReleased(Vector2 pos) { }
    public virtual void PointerDown(Vector2 pos)
    {
        _pointerDown = true;
        OnDown.Invoke();
    }
    public virtual void PointerUp(Vector2 pos) { OnUp.Invoke(); }
    public virtual void PointerEnter(Vector2 pos) { OnEnter.Invoke(); }
    public virtual void PointerExit(Vector2 pos) { OnExit.Invoke(); }
}
