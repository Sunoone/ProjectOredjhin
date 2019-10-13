using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;



public abstract class Touch_Inputs : MonoBehaviour {

    public GameObject Target;

    public UnityEvent OnClick;

    public UnityEvent OnDown;
    public UnityEvent OnUp;

    public UnityEvent OnPressure;
    public UnityEvent OnRelease;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnHover;

    public UnityEvent OnHold;

    public GraphicRaycaster m_Raycaster;
    //public PhysicsRaycaster
    public EventSystem m_EventSystem;

    PointerEventData m_PointerEventData;


    protected virtual void Start() { Init(); }
    protected virtual void Reset() { Init(); }

    private void Init()
    {
        if (Target == null)
            Target = gameObject;

        if (m_Raycaster == null)
        {
            Debug.LogError("Requires a Raycaster");
        }
           // m_Raycaster = GameManager.Instance.CurrentCanvas.GetComponent<GraphicRaycaster>(); //Fetch the Raycaster from the GameObject (the Canvas) 

        if (m_EventSystem == null)
            m_EventSystem = EventSystem.current;// GetComponent<EventSystem>(); //Fetch the Event System from the Scene
    }

    bool pressure = false;
    protected virtual void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        TouchInputs();
            return;
#endif
    }

    Vector2 touchPosition;
    GameObject touchTarget;
    private void TouchInputs()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchTarget = RayCastPosisition(touch.position);
            TouchClickInputs(touch);
        }
    }
    private void TouchClickInputs(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                SolveDown(RayCastPosisition(touch.position));
                touchPosition = touch.position;
                break;

            //Determine if the touch is a moving touch
            case TouchPhase.Stationary:
            case TouchPhase.Moved:
                touchPosition = touch.position;

                if (touchTarget == Target)
                {
                    if (!dragging)
                        TouchBeginDrag(touchPosition);
                    else
                    {
                        TouchDrag(touchPosition);
                        OnHold.Call();
                    }

                    TouchPressure();
                    //OnPressure.Call();
                    break;
                }
                break;

            case TouchPhase.Ended:
                if (dragging)
                    TouchEndDrag(touchPosition);

                SolveUp(RayCastPosisition(touchPosition));
                break;
        }
    }

    GameObject downTarget;
    private void SolveDown(GameObject obj)
    {
        downTarget = obj;
        if (downTarget == Target)
        {
            TouchDown();
            OnDown.Call();
        }
    }
    GameObject upTarget;

    bool dragging = false;

    private void SolveUp(GameObject obj)
    {
        upTarget = obj;
        if (upTarget == Target)
        {
            TouchUp();
            OnUp.Call();

            if (upTarget == downTarget)
            {
                TouchClick();
                OnClick.Call();
            }
        }
        else if (downTarget == Target)
        {
            TouchRelease();
            OnRelease.Call();
        }
        Clean();
    }

    private void Clean()
    {
        downTarget = null;
        upTarget = null;
        //mouseTarget = null;
        touchTarget = null;
        touchPosition = Vector2.zero;
        pressure = false;
        //enter = false;
    }

    private GameObject RayCastPosisition(Vector2 pos)
    {
        m_PointerEventData = new PointerEventData(m_EventSystem); //Set up the new Pointer Event       
        m_PointerEventData.position = pos; //Set the Pointer Event Position to that of the mouse position         
        List<RaycastResult> results = new List<RaycastResult>(); //Create a list of Raycast Results     
        m_Raycaster.Raycast(m_PointerEventData, results);  //Raycast using the Graphics Raycaster and mouse click position           
        if (results.Count > 0)
            return results[0].gameObject;
        return null;
        /*foreach (RaycastResult result in results)//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        {
            Debug.Log("Hit " + result.gameObject.name);
        }*/
    }

    public virtual void TouchClick() { Debug.Log("Click"); }
    public virtual void TouchDown() { Debug.Log("Down"); }

    public virtual void TouchUp() { Debug.Log("Up"); }
    public virtual void TouchRelease() { Debug.Log("UpExternal"); }

    public virtual void TouchApplyPressure() { Debug.Log("ApplyPressure"); }
    public virtual void TouchPressure() { Debug.Log("Pressure"); }
    public virtual void TouchRemovePressure() { Debug.Log("RemovePressure"); }

    protected virtual void TouchBeginDrag(Vector2 pos) { }
    protected virtual void TouchDrag(Vector2 pos) { }
    protected virtual void TouchEndDrag(Vector2 pos) { }
}
