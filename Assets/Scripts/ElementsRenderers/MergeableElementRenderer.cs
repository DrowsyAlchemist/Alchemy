using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeableElementRenderer : ElementRenderer, IEndDragHandler, IDragHandler, IPointerClickHandler, IBeginDragHandler
{
    private const float DoubleClickTime = 0.3f;
    private const float XCloneShift = 20;
    private const float YCloneShift = 20;

    private IMergeHandler _mergeHandler;
    private float _lastClickTime;
    private bool _trainingMode;
    private Vector3 _deltaPosition;

    public void Init(IMergeHandler mergeHandler, bool trainingMode = false)
    {
        _mergeHandler = mergeHandler;
        _trainingMode = trainingMode;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _deltaPosition = (Vector3)eventData.position - transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector3)eventData.position - _deltaPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool needDestroy = true;

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out GameField _))
            {
                needDestroy = false;

                if (_trainingMode)
                    Training.SetElementOnGameField(Element);
            }

            if (result.gameObject.TryGetComponent(out MergeableElementRenderer otherElementRenderer))
            {
                if (otherElementRenderer != this)
                {
                    if (_trainingMode)
                        Training.SetElementCreated(Element, otherElementRenderer.Element);

                    _mergeHandler.TryMergeElements(this, otherElementRenderer);
                    needDestroy = false;
                    break;
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
        clone.Init(_mergeHandler, _trainingMode);
        Sound.PlayCreate();

        if (_trainingMode)
            Training.SetDoubleClick();
    }
}
