using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MergeableElementRenderer : ElementRenderer, IEndDragHandler, IDragHandler
{
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
            if (result.gameObject.TryGetComponent(out OpenedElementsView _))
            {
                Destroy(gameObject);
                return;
            }
            if (result.gameObject.TryGetComponent(out MergeableElementRenderer otherElementRenderer))
                if (otherElementRenderer != this)
                    _mergeHandler.TryMergeElements(this, otherElementRenderer);
        }
    }
}
