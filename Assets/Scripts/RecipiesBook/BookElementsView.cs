using System;
using UnityEngine;

public class BookElementsView : ElementsView
{
    [SerializeField] private BookElementRenderer _bookElementTemplate;
    [SerializeField] private RectTransform _container;
    [SerializeField] private UIButton _closeButton;

    private IElementClickHandler _elementClickHandler;

    public void Init(IElementClickHandler elementClickHandler)
    {
        _elementClickHandler = elementClickHandler;
        _closeButton.Init(Close);
        IsInitialized = true;
    }

    protected override void AddElement(Element element)
    {
        if (IsInitialized == false)
            throw new InvalidOperationException("Object is not initialized");

        var renderer = Instantiate(_bookElementTemplate, _container);
        renderer.Init(_elementClickHandler);
        renderer.Render(element);
        OpenedElementRenderers.Add(renderer);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
