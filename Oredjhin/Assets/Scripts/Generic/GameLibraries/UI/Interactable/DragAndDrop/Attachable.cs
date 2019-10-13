using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : Attachable<Transform> { }

[RequireComponent(typeof(Collider2D))]
public class Attachable<T> : MonoBehaviour where T : Component {

    public T Draggable;

    List<AttacherUI<T>> AcceptedAttachers = new List<AttacherUI<T>>();

    protected virtual void Reset()
    {
        
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }
}
