using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using Freethware.Math;

public class ScalableLayoutGroup : MonoBehaviour {

    public RectTransform RectTransform;
    public bool ChangeSize = true;
    public bool OnlyUseActive = true;
    public bool UseUpdate = true;
    private void Reset()
    {
        RectTransform = (RectTransform)transform;
    }

    private void Awake()
    {
        //OnChildrenExcluded.AddListener(Test);
    }
    /*public void Test(List<RectTransform> list)
    {
        Debug.Log("Received event");
    }*/
    public UnityEvent<List<RectTransform>> OnChildrenExcluded;
    public UnityEvent OnSizeUpdate;

    public enum PivotPoint
    {
        UpperLeft,
        UpperCenter,
        UpperRight,
        CenterLeft,
        Center,
        CenterRight,
        LowerLeft,
        LowerCenter,
        LowerRight,
    }

    public PivotPoint Pivot = PivotPoint.Center;

    //protected List<X> ElementList = new List<X>();
    //protected List<Y> DataList = new List<Y>();
    [ReadOnlyDuringPlay]
    public Vector2 Spacing = new Vector2(10, 10);
    [ReadOnlyDuringPlay]
    public Vector2 Border = new Vector2(10, 10);

    //[SerializeField]
    //public IPooler<X> Pooler;

    private void Update()
    {
        if (UseUpdate)
            SolveScalableLayout();
    }

    private Vector2 newSize = Vector2.zero;
    private Vector2 previousSize;
    private bool SizeChanged = false;
    float timer = 0;
    [EasyButtons.Button("Solve")]
    public virtual void SolveScalableLayout()
    {

        SolveSize();
        RectTransform.sizeDelta = lastInfo.SolvedSize;
        /*timer += Time.deltaTime;

        if (RectTransform.sizeDelta.x >= lastInfo.SolvedSize.x)
        {
            timer = 0;
        }
        else
        if (RectTransform.sizeDelta != lastInfo.SolvedSize)
        {
            newSize.x = TweenEaseScrpt.Linear(timer, RectTransform.sizeDelta.x, lastInfo.SolvedSize.x, 1);
            newSize.y = TweenEaseScrpt.Linear(timer, RectTransform.sizeDelta.y, lastInfo.SolvedSize.y, 1);
            RectTransform.sizeDelta = newSize;
            lastInfo.CurSize = newSize;
            Debug.Log("Cur: " + RectTransform.sizeDelta);
            Debug.Log("New: "  + newSize);
        }*/



        SolvePositions(lastInfo);
    }
    public virtual void CopyScalableLayout(SizeInfo originalSizeInfo)
    {
        RectTransform[] rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();
        List<RectTransform> children = new List<RectTransform>();
        int length = rectTransforms.Length;
        for (int i = 1; i < length; i++)
        {
            if (rectTransforms[i].parent == transform && (!OnlyUseActive || rectTransforms[i].gameObject.activeInHierarchy))
                children.Add(rectTransforms[i]);

            // Not supporting multi layered layouts for now.
            //ScalableLayoutGroup SLG = rectTransforms[i].GetComponent<ScalableLayoutGroup>();
            //if (SLG != null)
            //    SLG.SolveScalableLayout();
        }
        originalSizeInfo.Children = children;
        SolvePositions(originalSizeInfo);
    }

    public struct SizeInfo
    {
        public SizeInfo(List<RectTransform> children, Vector2 solvedSize, Vector2 curSize, List<int> ElementWidthCount, List<float> ElementRowHeight)
        {
            Children = children;
            SolvedSize = solvedSize;
            CurSize = curSize;
            WidthCount = ElementWidthCount;
            RowHeight = ElementRowHeight;

            Scales = new List<Vector3>();
            int length = children.Count;
            for (int i = 0; i < length; i++)
                Scales.Add(children[i].localScale);
        }
        public List<RectTransform> Children;
        public List<Vector3> Scales;
        public Vector2 SolvedSize;
        public Vector2 CurSize;
        public List<int> WidthCount;
        public List<float> RowHeight;
    }
    protected SizeInfo lastInfo;
    public SizeInfo GetSizeInfo() { return lastInfo; }

    public Vector2 MaxSize, MinSize;
    protected int breakIndex = -1;

    //List<RectTransform> children = new List<RectTransform>();
    public virtual void SolveSize()
    {     
        RectTransform[] rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();
        List<int> WidthCount = new List<int>();
        List<float> RowHeight = new List<float>();

        // Filters out all the relevant children.
        List<RectTransform> children = new List<RectTransform>();
        int length = rectTransforms.Length;
        for (int i = 1; i < length; i++)
        {
            if (rectTransforms[i].parent == transform && (!OnlyUseActive || rectTransforms[i].gameObject.activeInHierarchy))
                children.Add(rectTransforms[i]);

            ScalableLayoutGroup SLG = rectTransforms[i].GetComponent<ScalableLayoutGroup>();
            if (SLG != null)
                SLG.SolveScalableLayout();
        }

        breakIndex = -1;
        Vector2 currentSize = new Vector2(0, 0);
        Vector2 solvedSize = Vector2.zero;
        Vector2 ElementSize = Vector2.zero;
        WidthCount.Add(0);
        RowHeight.Add(0f);

        length = children.Count;
        for (int i = 0; i < length; i++)
        {            
            ElementSize.x = Mathf.Abs(children[i].sizeDelta.x * children[i].localScale.x);
            ElementSize.y = Mathf.Abs(children[i].sizeDelta.y * children[i].localScale.y);
            // Should make exceptions for when stuff is resizing. Cap if it can be resized, etc.
            float space = (WidthCount[WidthCount.Count - 1] == 0) ? 0 : Spacing.x;

            if (MaxSize.x <= 0 || currentSize.x + ElementSize.x + space <= MaxSize.x - (Border.x * 2))
            {
                currentSize.x += ElementSize.x + space;
                WidthCount[WidthCount.Count - 1]++;
                if (solvedSize.x < currentSize.x)
                    solvedSize.x = currentSize.x;

                if (currentSize.y < ElementSize.y)
                    if (MaxSize.y <= 0 || ElementSize.y < MaxSize.y - (Border.y * 2))
                        currentSize.y = ElementSize.y;
                    else
                        currentSize.y = MaxSize.y;

                RowHeight[RowHeight.Count - 1] = currentSize.y;

                if (i == length - 1)
                {
                    solvedSize.y += currentSize.y + ((RowHeight.Count - 1) * Spacing.y);                
                    lastInfo = new SizeInfo(children, solvedSize, RectTransform.sizeDelta, WidthCount, RowHeight);
                }
                continue;
            }

            i--;
            solvedSize.y += currentSize.y + ((RowHeight.Count - 1) * Spacing.y);
            if (MaxSize.y <= 0 || solvedSize.y + ElementSize.y <= MaxSize.y - (Border.y * 2))
            {
                currentSize.x = 0;
                RowHeight.Add(currentSize.y);
                WidthCount.Add(0);
            }
            else
            {
                breakIndex = i;


                List<RectTransform> ExcludedList = new List<RectTransform>();
                for (int e = length - 1; e > breakIndex; e--)
                {
                    ExcludedList.Add(children[e]);
                    children[e].gameObject.SetActive(false);
                    children.RemoveAt(e);
                }
                //OnChildrenExcluded.Invoke(ExcludedList);
            

                Debug.Log("Allowed Elements: " + (i + 1));
                if (lastInfo.Children == children && lastInfo.Children.Count == children.Count)
                {
                    
                    solvedSize = lastInfo.SolvedSize;
                    WidthCount = lastInfo.WidthCount;
                    RowHeight = lastInfo.RowHeight;
                    RectTransform.sizeDelta = solvedSize;
                    return;
                }
                break;
            }

        }

        solvedSize.x += Border.x * 2;
        solvedSize.y += Border.y * 2;

        if (MinSize.x > solvedSize.x)
            solvedSize.x = MinSize.x;
        if (MinSize.y > solvedSize.y)
            solvedSize.y = MinSize.y;

        //if (ChangeSize)
        //    RectTransform.sizeDelta = solvedSize;
        lastInfo = new SizeInfo(children, solvedSize, RectTransform.sizeDelta, WidthCount, RowHeight);
        if (!OnSizeUpdate.IsDefault())
            OnSizeUpdate.Invoke();
    }

    float timePassed = 0;
    void UpdateSize()
    {
        // time, start, difference, duration
        //TweenEaseScrpt.EaseInOutQuad(timePassed, )
    }

    public virtual void SolvePositions(SizeInfo sizeInfo)
    {
        Vector2 localPosition = Vector2.zero;

        Vector2 currentSize = sizeInfo.CurSize;
            
            //RectTransform.sizeDelta;
        Vector2 ElementSize = Vector2.zero;

        float startWidth = (-currentSize.x / 2) + Border.x;
        float startHeight = (currentSize.y / 2) - Border.y;

        //float startWidth = (RowHeight.Count > 1) ? (-currentSize.x / 2) + Border.x : 0;
        //float startHeight = (WidthCount.Count > 1) ? startWidth : ElementList[WidthCount[0]].GetSize().x / 2;

        Vector2 solvedSize = new Vector2(startWidth, startHeight);
        int index = 0;
        for (int h = 0; h < sizeInfo.RowHeight.Count; h++)
        {
            localPosition.y = (sizeInfo.RowHeight.Count == 1) ? 0 : solvedSize.y - sizeInfo.RowHeight[h] / 2;
            solvedSize.x = startWidth;
            for (int w = 0; w < sizeInfo.WidthCount[h]; w++)
            {
                ElementSize.x = Mathf.Abs(sizeInfo.Children[index].sizeDelta.x * sizeInfo.Scales[index].x);
                ElementSize.y = Mathf.Abs(sizeInfo.Children[index].sizeDelta.y * sizeInfo.Scales[index].y);

                localPosition.x = (sizeInfo.WidthCount[h] == 1 && h == 0) ? 0 : solvedSize.x + ElementSize.x / 2;
                sizeInfo.Children[index].localPosition = localPosition; //.DOLocalMove(localPosition , 1f);
                solvedSize.x += ElementSize.x + Spacing.x;
                index++;
            }
            solvedSize.y -= (sizeInfo.RowHeight[h] + Spacing.y);
        }
    }
}
