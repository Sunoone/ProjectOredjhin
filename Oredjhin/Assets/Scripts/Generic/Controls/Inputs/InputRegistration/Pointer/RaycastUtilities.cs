using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Freethware.Inputs
{
    public static class RaycastUtilities
    {
        public static GameObject RaycastPositionAll(GraphicRaycaster GRc, EventSystem ES, Vector2 pos)
        {
            GameObject target = RayCastPosisitionUI(GRc, ES, pos);
            if (target == null)
                target = RaycastUtilities.RayCastPosition2D(pos);
            return target;
        }


        public static GameObject RayCastPosisitionUI(GraphicRaycaster GRc, EventSystem ES, Vector2 pos, int index)
        {
            PointerEventData ped = new PointerEventData(ES); //Set up the new Pointer Event       
            ped.position = pos; //Set the Pointer Event Position to that of the mouse position         
            List<RaycastResult> results = new List<RaycastResult>(); //Create a list of Raycast Results     
            GRc.Raycast(ped, results);  //Raycast using the Graphics Raycaster and mouse click position     

            if (results.Count > index)
                return results[index].gameObject;
            return null;
        }
        public static GameObject RayCastPosisitionUI(GraphicRaycaster GRc, EventSystem ES, Vector2 pos)
        {
            PointerEventData ped = new PointerEventData(ES); //Set up the new Pointer Event       
            ped.position = pos; //Set the Pointer Event Position to that of the mouse position         
            List<RaycastResult> results = new List<RaycastResult>(); //Create a list of Raycast Results     
            GRc.Raycast(ped, results);  //Raycast using the Graphics Raycaster and mouse click position     

            int length = results.Count;
            for (int i = 0; i < length; i++)
            {
                GameObject component = results[i].gameObject;
                if (component != null)
                    return component;

                Image image = results[i].gameObject.GetComponent<Image>();
                if (image != null && image.raycastTarget) // Body blocked scenario.
                    break;
            }
            return null;
        }
        public static T RayCastPosisitionUI<T>(GraphicRaycaster GRc, EventSystem ES, Vector2 pos) where T : class
        {
            PointerEventData ped = new PointerEventData(ES); //Set up the new Pointer Event       
            ped.position = pos; //Set the Pointer Event Position to that of the mouse position         
            List<RaycastResult> results = new List<RaycastResult>(); //Create a list of Raycast Results     
            GRc.Raycast(ped, results);  //Raycast using the Graphics Raycaster and mouse click position     

            int length = results.Count;
            for (int i = 0; i < length; i++)
            {
                T component = results[i].gameObject.GetComponent<T>();
                if (component != null)
                    return component;

                Image image = results[i].gameObject.GetComponent<Image>();
                if (image != null && image.raycastTarget) // Body blocked scenario.
                    break;
            }
            return null;
        }

        public static GameObject RayCastPosition2D(Vector3 pos)
        {
            pos = Camera.main.ScreenToWorldPoint(pos);
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector3.zero);
            int length = hits.Length;
            for (int i = 0; i < length; i++)
            {
                if (hits[i].collider != null)
                {
                    GameObject component = hits[i].transform.gameObject;
                    if (component != null)
                    {
                        //Debug.Log("Hit: " + component.ToString());
                        return component;
                    }
                }
            }
            return null;
        }
        public static T RayCastPosition2D<T>(Vector3 pos) where T : class
        {
            //Debug.Log("Screenpos: " + pos);
            pos = Camera.main.ScreenToWorldPoint(pos);

            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector3.zero);
            //Debug.Log("WorldPos: " + pos);
            int length = hits.Length;
            for (int i = 0; i < length; i++)
            {
                if (hits[i].collider != null)
                {
                    T component = hits[i].transform.GetComponent<T>();
                    if (component != null)
                    {
                        //Debug.Log("Hit: " + component.ToString());
                        return component;
                    }
                }
            }
            return null;
        }
    }
}
