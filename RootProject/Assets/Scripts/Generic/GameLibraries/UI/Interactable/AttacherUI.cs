using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacherUI : AttacherUI<Transform> { }

public enum EAttachState
{
    Absorb, // Can attach, but will destroy the object.
    Accept, // Can attach.
    Reject, // Can't attach.
    Replace, // Replaces the original, does not know what to do with the original after. Use delegates to solve this issue.
    Same, // It's the same attacher is currently is attached to.
    Switch, // When your object occupies an attacher and tries to attach to another occupied attacher, switch the 2 objects.
}

public abstract class AttacherUI<T> : MonoBehaviour where T : Component
{
    public T AttachedObject;
    public bool CanPlace = true;
    public bool CanSwitch = true;
    public Vector3 Offset = Vector2.zero;

    public virtual bool CompareIDs(int ID) { return false; }


    protected virtual void Awake() { }

    public virtual EAttachState GetAttachState(T objectToAttach)
    {
        if (!CanPlace)
            return EAttachState.Reject;

        if (AttachedObject.gameObject.activeInHierarchy)
        {
            if (!AttachedObject.IsDefault())
                if (!CanSwitch)
                    return EAttachState.Reject;
                else
                    return EAttachState.Replace;
        }
        return EAttachState.Accept;
    }
    

    public virtual EAttachState SolveAttach(T objectToAttach)
    {
        EAttachState EAS = GetAttachState(objectToAttach);
        Debug.Log("State: " + EAS);
        switch (EAS)
        {
            case EAttachState.Reject:
                Reject(objectToAttach);
                return EAS;
            case EAttachState.Replace:
                Replace(objectToAttach);
                break;
            case EAttachState.Accept:
                Attach(objectToAttach);
                break;
            case EAttachState.Switch:
                Switch(objectToAttach);
                break;
            case EAttachState.Absorb:
                Absorb(objectToAttach);
                break;
            case EAttachState.Same:
                Same(objectToAttach);
                break;
            default:
                break;
        }
        //Refresh();
        return EAS;
    }


    protected virtual void Attach(T objectToAttach)
    {
        AttachedObject = objectToAttach;
        AttachedObject.transform.position = transform.position + Offset;
    }
    protected virtual void Attach(Entity user, T objectToAttach)
    {
        AttachedObject = objectToAttach;
        AttachedObject.transform.position = transform.position + Offset;
    }
    public virtual void Replace(T objectToAttach) { Combine(objectToAttach); }
    public virtual void Switch(T objectToSwitch) { Attach(objectToSwitch); }
    public virtual void Absorb(T objectToAbsorb) { Attach(objectToAbsorb); }
    public virtual void Reject(T objectToReject) { }
    public virtual void Same(T objectToIgnore) { }
    public virtual void Combine(T objectToCombine) { }
    public virtual void Detach() { AttachedObject.gameObject.SetActive(false); }
    public virtual void Clean() { Detach(); }
    protected virtual void Refresh() { AttachedObject = default(T); }

    // Should receive inputs and requirements from different sources.
    protected virtual bool IsAllowed(T objectToAttach, RequirementCheck<T> requirementCheck = default(RequirementCheck<T>))
    {
        if (!CanSwitch && AttachedObject.IsDefault())
            return false;

        if (requirementCheck.IsDefault())
            return requirementCheck.Invoke(objectToAttach);

        return true;
    }


    public virtual EAttachState GetAttachState(Entity user, T objectToAttach) { return GetAttachState(objectToAttach); }
    public virtual EAttachState SolveAttach(Entity user, T objectToAttach)
    {
        EAttachState EAS = GetAttachState(user, objectToAttach);
        Debug.Log("State: " + EAS);
        switch (EAS)
        {
            case EAttachState.Reject:
                Reject(user, objectToAttach);
                return EAS;
            case EAttachState.Replace:
                Replace(user, objectToAttach);
                break;
            case EAttachState.Accept:
                Attach(user, objectToAttach);
                break;
            case EAttachState.Switch:
                Switch(user, objectToAttach);
                break;
            case EAttachState.Absorb:
                Absorb(user, objectToAttach);
                break;
            case EAttachState.Same:
                Same(user, objectToAttach);
                break;
            default:
                break;
        }
        //Refresh();
        return EAS;
    }
    public virtual EAttachState ManualAttach(Entity user, EAttachState EAS, T objectToAttach)
    {
        switch (EAS)
        {
            case EAttachState.Reject:
                Reject(user, objectToAttach);
                return EAS;
            case EAttachState.Replace:
                Replace(user, objectToAttach);
                break;
            case EAttachState.Accept:
                Attach(user, objectToAttach);
                break;
            case EAttachState.Switch:
                Switch(user, objectToAttach);
                break;
            case EAttachState.Absorb:
                Absorb(user, objectToAttach);
                break;
            case EAttachState.Same:
                Same(user, objectToAttach);
                break;
            default:
                break;
        }
        return EAS;
    }

    public virtual void Replace(Entity user, T objectToAttach) { Combine(user, objectToAttach); }
    public virtual void Switch(Entity user, T objectToSwitch) { Attach(objectToSwitch); }
    public virtual void Absorb(Entity user, T objectToAbsorb) { Attach(objectToAbsorb); }
    public virtual void Reject(Entity user, T objectToReject) { }
    public virtual void Same(Entity user, T objectToIgnore) { }
    public virtual void Combine(Entity user, T objectToCombine) { }
    public virtual void Detach(Entity user) { /*AttachedObject.gameObject.SetActive(false); */}









    public virtual void SameEffect() { }
    public virtual void AcceptEffect() { }
    public virtual void AbsorbEffect() { }
    public virtual void RejectEffect() { }
    public virtual void ReplaceEffect() { }
    public virtual void RestoreEffect() { }
    public virtual void CombineEffect() { }
}
