using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Freethware.Effects;

public class ImageUI : IconUI {

    public Image Image;
    public float AnimationDuration = 1f;
    private SizeEffect sizeEffect;

    private void Awake()
    {
        sizeEffect = new SizeEffect();
    }

    [EasyButtons.Button("SetSize")]
    public void SetSizeButton()
    {
        AnimateSize(new Vector2(500, 500), 1);
    }

    [EasyButtons.Button("SetScale")]
    public void SetScaleButton()
    {

        if (sizeEffect == null)
            sizeEffect = new SizeEffect();
        sizeEffect.Play(RectTransform, .5f, 1);
    }

    public override void SetSize(Vector2 size) { RectTransform.sizeDelta = size; }
    public override void AnimateSize(Vector2 size) { sizeEffect.Play(RectTransform, size, AnimationDuration); }
    public override void AnimateSize(Vector2 size, float duration) { sizeEffect.Play(RectTransform, size, duration); }


    public override void Conceal(float a)
    {
        Color color = Image.color;
        alpha = color.a;
        color.a = a;
        Image.color = color;
    }
    public override void Reveal()
    {
        Color color = Image.color;
        color.a = alpha;
        Image.color = color;
    }

    protected override void Reset()
    {
        base.Reset();
        RectTransform = (RectTransform)transform;
        Image = GetComponent<Image>();
    }
}
