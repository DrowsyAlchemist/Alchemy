using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeableElementRenderer : ElementRenderer, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private const float DoubleClickTime = 0.3f;
    private const float XCloneShift = 20;
    private const float YCloneShift = 20;

    private IMergeHandler _mergeHandler;
    private float _lastClickTime;

    public void Init(IMergeHandler mergeHandler)
    {
        _mergeHandler = mergeHandler;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool needDestroy = true;

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out GameField _))
                needDestroy = false;

            if (result.gameObject.TryGetComponent(out MergeableElementRenderer otherElementRenderer))
            {
                if (otherElementRenderer != this)
                {
                    _mergeHandler.TryMergeElements(this, otherElementRenderer);
                    needDestroy = false;
                }
            }
        }
        if (needDestroy)
            Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTimeClick = eventData.clickTime;

        if (Mathf.Abs(currentTimeClick - _lastClickTime) < DoubleClickTime)
            Clone();

        _lastClickTime = currentTimeClick;
    }

    private void Clone()
    {
        var clone = Instantiate(this, transform.position + new Vector3(XCloneShift, YCloneShift, 0), Quaternion.identity, transform.parent);
        clone.Render(Element);
        clone.Init(_mergeHandler);
    }
}
