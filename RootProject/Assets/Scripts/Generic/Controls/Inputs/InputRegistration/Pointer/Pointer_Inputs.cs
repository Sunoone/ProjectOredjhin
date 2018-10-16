using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Freethware.Inputs.SingePlayer
{  
    public interface IClickHandler { void Click(Vector2 pos); }
    public interface IDownHandler { void Down(Vector2 pos); }

    public interface IUpHandler { void Up(Vector2 pos); }
    public interface IReleaseHandler { void Release(Vector2 pos); }

    public interface IBeginPressureHandler { void ApplyPressure(Vector2 pos); }
    public interface IPressureHandler { void Pressure(Vector2 pos); }
    public interface IEndPressureHandler { void RemovePressure(Vector2 pos); }

    public interface IInitiateDragHandler { void InitiateDrag(Vector2 pos); }
    public interface IDragHandler { void Dragging(Vector2 pos); }
    public interface IShakeHandler { void Shake(Vector2 pos); }
    public interface IRemoveDragHandler { void RemoveDrag(Vector2 pos); }

    [System.Serializable]
    public class UpTarget : UnityEvent<GameObject, Vector2> { }
    public class Pointer_Inputs : MonoBehaviour
    {
        public UpTarget OnUp;

        public GraphicRaycaster GRc;
        public EventSystem ES;

        [Range(1, 3)] public int PriorityUI = 1;
        [Range(1, 3)] public int PrioritySprites = 2;
        [Range(1, 3)] public int PriorityObjects = 3;


        private void Reset()
        {
            GRc = GameObject.FindObjectOfType<GraphicRaycaster>();
            ES = GameObject.FindObjectOfType<EventSystem>();
        }

        void Update()
        {
            InputsUI();

            // Insert player movement depending on target here...?
        }

        Vector2 locatorPosition;
        public GameObject Target;
        public bool InputsUI()
        {
            //#if UNITY_ANDROID
            if (TouchInputsUI())
                return true;
            //#endif

            //#if UNITY_WSA
            if (MouseInputsUI())
                return true;
            //#endif
            return false;
        }

        
        private bool TouchInputsUI()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                TouchClickInputs(touch);
                return true;
            }
            return false;
        }
        private void TouchClickInputs(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    locatorPosition = touch.position;
                    Down(locatorPosition);
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    locatorPosition = touch.position;
                    SolveMoveThreshold(locatorPosition);

                    if (bMoveThreshold)
                        Drag(locatorPosition);
                    break;

                case TouchPhase.Ended:

                    if (bMoveThreshold)
                    {
                        RemoveDrag(locatorPosition);
                        bMoveThreshold = false;
                    }
                    Up(locatorPosition);
                    break;
            }
        }


        private bool MouseInputsUI()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Down(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                locatorPosition = Input.mousePosition;
                SolveMoveThreshold(locatorPosition);

                if (bMoveThreshold)
                    Drag(locatorPosition);
                return true;
            }


            if (Input.GetMouseButtonUp(0))
            {
                if (bMoveThreshold)
                {
                    RemoveDrag(locatorPosition);
                    bMoveThreshold = false;
                }

                Up(Input.mousePosition);
            }
            return false;
        }

       
        

        public void Down(Vector2 pos)
        {
            Target = RaycastUtilities.RaycastPositionAll(GRc, ES, pos);  
            if (Target != null)
            {
                startPos = pos;
                IDownHandler ID = Target.GetComponent<IDownHandler>();
                if (ID != null)
                    ID.Down(locatorPosition);
            }
        }
        public void Up(Vector2 pos)
        {
            GameObject prevTarget = Target;
            Target = RaycastUtilities.RaycastPositionAll(GRc, ES, pos);
            OnUp.Call(Target, pos);
            if (Target != null)
            {
                if (Target == prevTarget)
                {
                    IClickHandler IC = Target.GetComponent<IClickHandler>();
                    if (IC != null)
                        IC.Click(locatorPosition);
                }

                IUpHandler ILU = Target.GetComponent<IUpHandler>();
                if (ILU != null)
                    ILU.Up(locatorPosition);
            }

        }
        Vector2 startPos;
        Vector2 diffPos;
        bool bMoveThreshold = false;
        public void SolveMoveThreshold(Vector2 newPos)
        {
            if (bMoveThreshold || Target == null)
                return;

            diffPos = startPos - newPos;
            float totDist = (diffPos.x * diffPos.x) + (diffPos.y * diffPos.y);
            float thresholdDist = (ES.pixelDragThreshold * ES.pixelDragThreshold);
            bMoveThreshold = (totDist >= thresholdDist);
            //Debug.Log("Tot: " + totDist + " & Threshold: " + thresholdDist);
            if (bMoveThreshold)
            {
                Debug.Log("ThresholdReached");
                InitiateDrag(newPos);
            }
        }
        public void InitiateDrag(Vector2 pos)
        {
            if (Target != null)
            {
                IInitiateDragHandler IBD = Target.GetComponent<IInitiateDragHandler>();
                if (IBD != null)
                    IBD.InitiateDrag(locatorPosition);
            }
        }
        public void Drag(Vector2 pos)
        {
            if (Target != null)
            {
                IDragHandler ID = Target.GetComponent<IDragHandler>();
                if (ID != null)
                    ID.Dragging(locatorPosition);
            }
        }
        public void RemoveDrag(Vector2 pos)
        {
            if (Target != null)
            {
                IRemoveDragHandler IED = Target.GetComponent<IRemoveDragHandler>();
                if (IED != null)
                    IED.RemoveDrag(locatorPosition);
            }
        }

        public void Pinch(float dist) { }
    }
}
