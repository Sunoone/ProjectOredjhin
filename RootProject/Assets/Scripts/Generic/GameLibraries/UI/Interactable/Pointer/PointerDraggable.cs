using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using Freethware.Inputs;

public class PointerDraggable : PointerDraggable<Transform> { }

public enum EDragType
{
    DragSelf,
    CreateDragCopyOnce,
    CreateDragCopy,
    CannotDrag
}


/// <summary>
/// In hindsight an attach to everything option would've been better. Then from there specific interactions can be arranged, including getting the opinion of the object you're attaching too.
/// </summary>
/// <typeparam name="X"></typeparam>
public class PointerDraggable<X> : PointerSelectable, IInitiateDragHandler where X : Component {


    // Realtime variables
    public Entity currentUser;

    private static bool dragEnabled = true;
    public static bool DragEnabled
    {
        get { return dragEnabled; }
        set
        {
            dragEnabled = value;
            if (!dragEnabled)
                DropAll.Call();
        }
    }
    public static Callback DropAll;
    public virtual void ForceDropAll() { DropAll.Call(); }
    public virtual void ForceDrop(bool canAttach = false) { EndDrag(); }

    public RectTransform RectTransform;
    protected override void Awake()
    {
        originalColor = Image.color;
#if UNITY_ANDROID 
        ShakesRequired /= 2;
#endif
    }
    protected override void Update()
    {
        base.Update();

        if (_dragging)
        {
            CheckForShake();
            OnDrag(Input.mousePosition);
        }
    }
    protected override void Reset()
    {
        base.Reset();
        RectTransform = (RectTransform)transform;
    }
    protected virtual void OnEnable()
    {

    }
    protected virtual void OnDisable()
    {

    }
    #region Shake
    protected Vector2 _lastPosition;
    protected Vector2 _newPosition;
    protected bool? _dir = null;
    protected bool? _lastDir = null;
    //private bool sideTracker = false;
    public int ShakesRequired = 5;
    protected int _shakeCounter = 0;
    protected float _shakeTimer = 0;
    protected float _shakeResetTime = .2f;
    public virtual void CheckForShake()
    {
        ResetShakeTimer();

        _newPosition = RectTransform.position;
        if (_newPosition.x == _lastPosition.x)
            return;

        // Checks if direction changed. Updates shakecounter if it did.
        _dir = _newPosition.x > _lastPosition.x;
        if (_lastDir != _dir)
        {
            _lastDir = _dir;
            _shakeCounter++;
            _shakeTimer = 0;
        }

        if (_shakeCounter > ShakesRequired)
        {
            Shake();
            ResetShake();
        }
        _lastPosition = _newPosition;
    }
    public virtual void Shake()
    {
        Debug.Log("CHECK");
    }
    private void ResetShakeTimer()
    {
        if (_shakeCounter > 0)
        {
            _shakeTimer += Time.deltaTime;
            if (_shakeTimer > _shakeResetTime)
                ResetShake();
        }
    }
    protected virtual void ResetShake()
    {
        //Debug.Log("ShakeReseted");
        _dir = null;
        _lastDir = null;
        _lastPosition = Vector2.zero;
        _newPosition = Vector2.zero;
        _shakeCounter = 0;
        _shakeTimer = 0;
    }
    #endregion

    public override void PointerDown(Vector2 pos) { }
    public override void PointerReleased(Vector2 pos) { OnEndDrag(); }

    protected bool _dragging = false;
    public X data;

    public UnityEvent OnGrab;
    public UnityEvent OnDrop;
    public UnityEvent OnShake;


    public EDragType DragType;
    public bool CanDrag = true;
    [ReadOnly]
    public bool HasCreatedCopy;
    [ReadOnly]
    public bool IsCopy = false;
    public PointerDraggable<X> Original;
    

    [HideInInspector]
    public Vector2 StartPosition;
    protected Vector2 _dragDelta = Vector2.zero;
    protected Color originalColor;

    /*protected override void TouchBeginDrag(Vector2 pos) { if (DragEnabled && DragType != EDragType.CannotDrag) BeginDrag(pos); }
    protected override void TouchDrag(Vector2 pos) { OnDrag(pos); }
    protected override void TouchEndDrag(Vector2 pos) { OnEndDrag(); }*/

    public virtual void InitiateDrag(Entity user, Vector2 pos) { if (DragEnabled && DragType != EDragType.CannotDrag) BeginDrag(user, pos); }
    

    //public void OnBeginDrag(PointerEventData eventData) { if (DragEnabled && DragType != EDragType.CannotDrag) BeginDrag(eventData.position); }
    //public void OnDrag(PointerEventData eventData) { }
    //public void OnEndDrag(PointerEventData eventData) { }

    public void OnDrag(Vector2 mousePosition)
    {
        if (DragEnabled && DragType != EDragType.CannotDrag && CanDrag)
            Drag(mousePosition);
    }
    public void OnEndDrag()
    {
        if (DragType != EDragType.CannotDrag)
            EndDrag();
    }

    #region Copy
    protected virtual PointerDraggable<X> CreateCopy()
    {
        GameObject go = GameObject.Instantiate(gameObject, transform.parent);
        return AssignCopyValues(go);
    }
    protected virtual PointerDraggable<X> AssignCopyValues(GameObject go)
    {
        go.transform.localPosition = Vector2.zero;
        go.transform.localScale = Vector2.zero;
        HasCreatedCopy = true;

        PointerDraggable<X> otherDraggable = go.GetComponent<PointerDraggable<X>>();
        otherDraggable.transform.position = transform.position;
        otherDraggable.Original = this;
        otherDraggable.IsCopy = true;
        if (DragType == EDragType.CreateDragCopyOnce)
            otherDraggable.DragType = EDragType.DragSelf;
        else
            otherDraggable.DragType = EDragType.CreateDragCopy;

        EventSystem eventData = EventSystem.current;
        eventData.SetSelectedGameObject(go);
        return otherDraggable;
    }
    #endregion

    // Method for override player drag with an AI one.

    public virtual void BeginDrag(Entity user) { BeginDrag(user, transform.position); }
    public virtual void BeginDrag(Entity user, Vector2 dragPosition)
    {
        currentUser = user;

        CanDrag = true;
        if (DragType != EDragType.DragSelf)
        {
            PointerDraggable<X> otherDraggable = CreateCopy();
            otherDraggable.BeginDrag(user, dragPosition);
            return;
        }

        ResetShake();
        _pointerDown = true;
        _dragging = true;
        _attacherIndex = -1;
        EAS = EAttachState.Reject;
        Attacher = default(AttacherUI<X>);

        InitGrab();

        StartPosition = transform.position;
        _lastPosition = StartPosition;
        _dragDelta = StartPosition - dragPosition;

        if (IsCopy)
        {
            Original.GrabOriginal();
        }

        Grab();

        OnGrab.Call();

    }

    /// <summary>
    /// Drags the object. Only override if making changes to base functionality. To add functionality use Hold and HoldOriginal.
    /// </summary>
    /// <param name="dragPosition">New position</param>
    public virtual void Drag(Vector2 dragPosition)
    {
        if (!_dragging)
            return;

        transform.position = dragPosition + _dragDelta;

        if (IsCopy)
        {
            Original.HoldOriginal();
        }

        Hold();   
    }
    public virtual void EndDrag()
    {
        if (!_dragging)
            return;

        CanDrag = false;
        _pointerDown = false;
        _dragging = false;

        PrepareDrop();

        if (IsCopy)
        {
            Original.DropOriginal(Attacher, EAS);
        }

        Drop();
       
        _dragDelta = Vector2.zero;

        OnDrop.Call();

        currentUser = null;
    }
    public virtual void ForceDrop()
    {
        if (!_dragging)
            return;

        CanDrag = false;
        _pointerDown = false;
        _dragging = false;

        EAS = EAttachState.Reject;
        Attacher = null;
        if (IsCopy)
        {
            Original.DropOriginal(Attacher, EAS);
        }

        Drop();

        _dragDelta = Vector2.zero;

        if (!OnDrop.IsDefault())
            OnDrop.Invoke();

        currentUser = null;
    }

    //[ReadOnly]
    //public bool SameOwner = false;
    protected int _attacherIndex;
    public AttacherUI<X> Attacher;
    public EAttachState EAS = EAttachState.Reject;
    public virtual void PrepareDrop() { }

    protected virtual void InitGrab() { }
    protected virtual void Grab() { }
    protected virtual void Hold() { }
    protected virtual void Drop()
    {
        switch (EAS)
        {
            case EAttachState.Absorb:
                Absorb();
                if (OnAbsorb != null)
                    OnAbsorb.Invoke();
                break;
            case EAttachState.Accept:
                Accept();
                if (OnAccept != null)
                    OnAccept.Invoke();
                break;
            case EAttachState.Reject:
                Reject();
                if (OnReject != null)
                    OnReject.Invoke();
                break;
            case EAttachState.Replace:
                Replace();
                if (OnReplace != null)
                    OnReplace.Invoke();
                break;
            case EAttachState.Same:
                Same();
                if (OnSame != null)
                    OnSame.Invoke();
                break;
            case EAttachState.Switch:
                Switch();
                if (OnSwitch != null)
                    OnSwitch.Invoke();
                break;
            default:
                break;
        }
    }
    //protected virtual void Drop(EAttachState EAS) { }

    /// <summary>
    /// Only use this method when a copy is made of the picked up object. This method will execute its contents before the regular "Pickup" method,  but will do so with "this" referencing the original object. 
    /// </summary>
    protected virtual void GrabOriginal() { }
    protected virtual void HoldOriginal() { }
    /// <summary>
    /// Only use this method when a copy is made of the picked up object. This method will execute its contents before the regular "Drop" method, but will do so with "this" referencing the original object. 
    /// </summary>
    protected virtual void DropOriginal(AttacherUI<X> attacher, EAttachState eas) {
        Attacher = attacher;
        EAS = eas;
        switch (EAS)
        {
            case EAttachState.Absorb:
                AbsorbOriginal();
                break;
            case EAttachState.Accept:
                AcceptOriginal();
                break;
            case EAttachState.Reject:
                RejectOriginal();
                break;
            case EAttachState.Replace:
                ReplaceOriginal();
                break;
            case EAttachState.Same:
                SameOriginal();
                break;
            case EAttachState.Switch:
                SwitchOriginal();
                break;
            default:
                break;
        }
    }

    public UnityEvent OnSame;
    public UnityEvent OnAccept;
    public UnityEvent OnAbsorb;
    public UnityEvent OnReject;
    public UnityEvent OnReplace;
    public UnityEvent OnSwitch;
    public UnityEvent OnRestore;

    public virtual void Same() { }
    public virtual void Accept() { }
    public virtual void Absorb() { }
    public virtual void Reject() { }
    public virtual void Replace() { }
    public virtual void Switch() { }
    public virtual void Restore() { }

    public virtual void SameOriginal() { }
    public virtual void AcceptOriginal() { }
    public virtual void AbsorbOriginal() { }
    public virtual void RejectOriginal() { }
    public virtual void ReplaceOriginal() { }
    public virtual void SwitchOriginal() { }
    public virtual void RestoreOriginal() { }

    // Any pointer action, except for clicking the same object, will deselect.
    public override void Select() { base.Select(); }
    public override void Deselect() { base.Deselect(); }
}
