using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Freethware.UI
{
    public enum SelectionType
    {
        FirstEncounterSelection,
        PiercingUISelection,
        PiercingSceneSelection,
        PiercingAllSelection,
    }

    public interface IPointDown { void OnPointDown(PointerEventData _eventData); }
    public interface IPointUp { }


    public class PointerInputModule : EventSystem
    {

        public GraphicRaycaster ActiveCanvas;

        public Camera currentCamera;
        public PhysicsRaycaster cameraRaycast;

        public void UpdateCamera(Camera p_currentCamera)
        {
            currentCamera = p_currentCamera;
            cameraRaycast = currentCamera.GetComponent<PhysicsRaycaster>();
            if (cameraRaycast == null)
                cameraRaycast = currentCamera.gameObject.AddComponent<PhysicsRaycaster>();

        }

        public LayerMask SelectionsAllowed;
        public SelectionType SelectionType;


        protected override void Awake()
        {
            base.Awake();
            UpdateCamera(Camera.main);
        }
        public bool toggle;
        private Ray ray;


        protected void GetDownResults(Vector3 p_position)
        {
            List<RaycastResult> resultsUI = new List<RaycastResult>();
            List<RaycastResult> resultsObjects = new List<RaycastResult>();
            PointerEventData eventData = new PointerEventData(this);

            eventData.position = p_position;

            //eventData.position = currentCamera.WorldToViewportPoint(Input.mousePosition);
            ActiveCanvas.Raycast(eventData, resultsUI);
            if (resultsUI != null && resultsUI.Count > 0)
            {
                ProcessResults(resultsUI, eventData);
                if (SelectionType != SelectionType.PiercingAllSelection)
                   return;
            }

            eventData.position = Input.mousePosition;
            RaycastAll(eventData, resultsObjects);
            ProcessResults(resultsObjects, eventData);
        }

        protected void ProcessResults(List<RaycastResult> p_list, PointerEventData _eventData)
        {
            int t_count = p_list.Count;

            for (int i = 0; i < t_count; i++)
            {
                var t_result = p_list[i].gameObject;
                if (SelectionsAllowed == (SelectionsAllowed | (1 << t_result.layer)))
                {
                    
                    IPointDown t_inputObject = t_result.GetComponent<IPointDown>();
                    if (t_inputObject != null)
                    {
                        Debug.Log("Calling method for " + t_result);
                        t_inputObject.OnPointDown(_eventData);
                    }

                    if (SelectionType == SelectionType.FirstEncounterSelection)
                        break;
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetMouseButtonDown(0))
                GetDownResults(Input.mousePosition);

            for (var i = 0; i < Input.touchCount; ++i)
                if (Input.GetTouch(i).phase == TouchPhase.Began && i == 0)
                    GetDownResults(Input.GetTouch(0).position);
        }
    }
}

