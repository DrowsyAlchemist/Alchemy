using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeableElementRenderer : ElementRenderer, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private const float XCloneShift = 20;
    private const float YCloneShift = 20;

    private IMergeHandler _mergeHandler;

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

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out MainOpenedElementsView _))
            {
                Destroy(gameObject);
                return;
            }
            if (result.gameObject.TryGetComponent(out MergeableElementRenderer otherElementRenderer))
                if (otherElementRenderer != this)
                    _mergeHandler.TryMergeElements(this, otherElementRenderer);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
            Clone();
    }

    private void Clone()
    {
        var clone = Instantiate(this, transform.position + new Vector3(XCloneShift, YCloneShift, 0), Quaternion.identity, transform.parent);
        clone.Render(Element);
        clone.Init(_mergeHandler);
    }
}
