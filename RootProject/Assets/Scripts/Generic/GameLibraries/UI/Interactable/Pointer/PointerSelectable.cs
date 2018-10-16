using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointerSelectable : PointerClickable, ISelectHandler, IDeselectHandler
{
    public static bool CanSelectNew = true;
    public bool CanSelect = true;
    public enum EDeselect
    {
        ClickAnyObject,
        ClickOtherObject,
        None,
        NoneAndCantSelectNew
    }
    public EDeselect DeselectType;
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    [ReadOnly]
    public bool Selected;

    // Any pointer action, except for clicking the same object, will deselect.
    public void OnSelect(BaseEventData eventData) { if (CanSelect) Select(); }
    public void OnDeselect(BaseEventData eventData)
    {
        if (DeselectType == EDeselect.None || DeselectType == EDeselect.NoneAndCantSelectNew)
            return;
        Deselect();
    }

    /*public override void TouchUp(Vector2 pos)
    {
        if (!Selected)
        {
            if (CanSelect) Select();
        }
        else
        {
            if (DeselectType == EDeselect.None || DeselectType == EDeselect.NoneAndCantSelectNew)
                return;
            Deselect();
        }
    }*/

    public override void Click()//PointerEventData eventData = null)
    {
        if (CanSelectNew)
        {
            EventSystem eventData = EventSystem.current;
            eventData.SetSelectedGameObject((!Selected) ? gameObject : (DeselectType == EDeselect.ClickAnyObject) ? null : gameObject);
        }
        OnClick.Invoke();
    }
    public virtual void Select()
    {
        if (DeselectType == EDeselect.NoneAndCantSelectNew)
            CanSelectNew = false;

        //Debug.Log("Selected " + this);
        Selected = true;
        OnSelected.Invoke();
    }
    public virtual void Deselect()
    {
        if (DeselectType == EDeselect.NoneAndCantSelectNew)
            CanSelectNew = true;

        Debug.Log("Deselected " + this);
        Selected = false;
        OnDeselected.Invoke();
    }
}
