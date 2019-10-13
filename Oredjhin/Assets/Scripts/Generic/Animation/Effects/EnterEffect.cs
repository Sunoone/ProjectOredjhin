using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Effects
{
    public class EnterEffect : MonoBehaviour
    {
        public Vector3 fullScale = new Vector3(-1, 1, 1);
        public ScaleEffect SE;
        protected virtual void Reset() { }
        protected virtual void Start()
        {
            SE = new ScaleEffect();
            //gameObject.SetActive(false);
        }
        public virtual void SetImage(Sprite sprite) { }
        public virtual void SetSize(Vector3 size) { }
        public virtual void SetColor(Color color, float alpha) { }
        
        //public virtual void ScaleDown(Vector3 )

        public virtual void OnEnable()
        {
            Enable();
        }

        public virtual void Enable() { Enable(Vector3.zero); }
        public virtual void Enable(Vector3 scale)
        {
            if (SE == null)
                SE = new ScaleEffect();

            if (scale == Vector3.zero)
                scale = fullScale;

            transform.SetAsFirstSibling();
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            SE.Play(transform, scale, 0.2f);
        }

        public virtual void Disable()
        {
            if (SE == null)
                SE = new ScaleEffect();

            //Debug.LogWarning("Stop is being called");
            SE.Play(transform, 0, 0.2f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}