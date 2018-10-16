using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

/// <summary>
/// Script that enables the animation of an object that contains a PhraseAnimatedImage component. Only works in editor.
/// </summary>
public class AnimateImage : MonoBehaviour
{
    public AnimationState AnimState = AnimationState.Idle;
    [HideInInspector]
    public Image image;
    //[HideInInspector]
    public Sprite[] sprites;

    protected IEnumerator AnimationCoroutine;

    public float FrameDuration = .25f;
    protected virtual void Awake()
    {
        Init();
    }
    protected virtual void Init()
    {
        if (image != null)
            return;

        image = gameObject.SearchComponent<Image>();
        UpdateAnimation();
    }
    protected virtual void Update() { }
    protected virtual void OnEnable() { UpdateAnimation(); }
    protected virtual void OnDisable() { Stop(); }
    protected virtual void Reset() { Init(); }

    public virtual void UpdateAnimation() { image.sprite = sprites[curIndex]; }
    public virtual void UpdateAnimation(Sprite sprite) { }
    public virtual void UpdateAnimation(Sprite[] sprites)
    {
        this.sprites = sprites;
        image.sprite = sprites[0];
    }

    public virtual void Play()
    {
        if (AnimationCoroutine != null)
        {
            if (AnimState == AnimationState.Pause)
            {
                StartCoroutine(AnimationCoroutine);
                AnimState = AnimationState.Play;
                return;
            }

            Debug.LogWarning("Animation is already playing.");
            return;
        }
        if (sprites != null)
        {
            //Debug.Log("Amount of sprites to play: " + sprites.Length);
            if (sprites.Length > 1)
            {
                if (curIndex >= sprites.Length)
                    curIndex = 0;
                AnimationCoroutine = Animation();
                StartCoroutine(AnimationCoroutine);
                AnimState = AnimationState.Play;
            }
        }
    }
    public virtual void Stop()
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
            AnimationCoroutine = null;
            curIndex = 0;
            AnimState = AnimationState.Idle;
        }
    }

 
    public virtual void Pause()
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
            AnimState = AnimationState.Pause;
        }
    }

    public virtual int GetCurrentIndex() { return curIndex; }
    protected int curIndex = 0;
    protected virtual IEnumerator Animation()
    {
        while (gameObject.activeInHierarchy)
        {
            SetSprite(sprites[curIndex]);

            curIndex++;
            if (curIndex >= sprites.Length)
                curIndex = 0;
            yield return new WaitForSeconds(FrameDuration);
        }
    }

    protected virtual void SetSprite(Sprite sprite) { image.sprite = sprite; }
}


