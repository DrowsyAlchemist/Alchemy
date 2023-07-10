using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenedElementRenderer : ElementRenderer, IBeginDragHandler, IDragHandler
{
    private MergeableElementRenderer _mergeableElementRendererTemplate;
    private GameField _gameField;
    private IMergeHandler _mergeHandler;
    private bool _isInitialized;

    public void Init(MergeableElementRenderer mergeableElementRendererTemplate, GameField gameField, IMergeHandler mergeHandler)
    {
        _mergeableElementRendererTemplate = mergeableElementRendererTemplate ?? throw new ArgumentNullException();
        _gameField = gameField ?? throw new ArgumentNullException();
        _mergeHandler = mergeHandler ?? throw new ArgumentNullException();
        _isInitialized = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isInitialized == false)
            throw new System.InvalidOperationException("Object is not initialized");

        var mergeableElementRenderer = Instantiate(_mergeableElementRendererTemplate, transform.position, Quaternion.identity, _gameField.transform);
        mergeableElementRenderer.Init(_mergeHandler);
        mergeableElementRenderer.Render(Element);
        eventData.pointerDrag = mergeableElementRenderer.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
