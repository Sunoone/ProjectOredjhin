using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Freethware.Inputs
{
    public class Mouse_Inputs_Player : MonoBehaviour
    {
        public Entity User;
        public UpTarget OnUp;

        public GraphicRaycaster GRc;
        public EventSystem ES;

        /*[Range(1, 3)] public int PriorityUI = 1;
        [Range(1, 3)] public int PrioritySprites = 2;
        [Range(1, 3)] public int PriorityObjects = 3;*/

        private void Reset()
        {
            GRc = GameObject.FindObjectOfType<GraphicRaycaster>();
            ES = GameObject.FindObjectOfType<EventSystem>();
        }


        void Update()
        {
            InputsUI();
        }


        Vector2 locatorPosition;
        public GameObject Target;
        public bool InputsUI()
        {
            if (MouseInputsUI())
                return true;
            return false;
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
                {
                    ID.Down(User, locatorPosition);
                    Debug.Log(ID.ToString());
                }
            }
        }
        public void Up(Vector2 pos)
        {
            GameObject prevTarget = Target;
            Target = RaycastUtilities.RaycastPositionAll(GRc, ES, pos);
            OnUp.Call(User, Target, pos);

            if (Target != null)
            {
                if (Target == prevTarget)
                {
                    IClickHandler IC = Target.GetComponent<IClickHandler>();
                    if (IC != null)
                        IC.Click(User, locatorPosition);
                }

                IUpHandler ILU = Target.GetComponent<IUpHandler>();
                if (ILU != null)
                    ILU.Up(User, locatorPosition);
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
                    IBD.InitiateDrag(User, locatorPosition);
            }
        }
        public void Drag(Vector2 pos)
        {
            if (Target != null)
            {
                IDragHandler ID = Target.GetComponent<IDragHandler>();
                if (ID != null)
                    ID.Dragging(User, locatorPosition);
            }
        }
        public void RemoveDrag(Vector2 pos)
        {
            if (Target != null)
            {
                IRemoveDragHandler IED = Target.GetComponent<IRemoveDragHandler>();
                if (IED != null)
                    IED.RemoveDrag(User, locatorPosition);
            }
        }

        public void Pinch(float dist) { }
    }
}

