using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Freethware.Effects;

public abstract class IconUI : MonoBehaviour
{
    public RectTransform RectTransform;

    protected virtual void Reset() { }
    public virtual Vector2 GetSize() { return RectTransform.sizeDelta; }

    public virtual void SetSize(Vector2 size) { }
    public virtual void AnimateSize(Vector2 size) { }
    public virtual void AnimateSize(Vector2 size, float duration) { }

    protected float alpha;
    public virtual void Conceal() { Conceal(0); }
    public virtual void Conceal(float a) { }
    public virtual void Reveal() { }

    protected virtual void UpdateRectSize() { }

    public virtual void SetPosition(Vector3 pos) { RectTransform.localPosition = pos; }
    public virtual void ChangePosition(Vector3 diff) { RectTransform.position += diff; }
    protected virtual void PositionChanged(Vector3 diff) { transform.SetAsLastSibling(); }

}
