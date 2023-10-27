using System;
using UnityEngine;
using UnityEngine.UI;

public class BookElementsView : ElementsView
{
    [SerializeField] private BookElementRenderer _bookElementTemplate;
    [SerializeField] private RectTransform _container;
    [SerializeField] private UIButton _closeButton;
    [SerializeField] private ScrollRect _scrollView;

    private IElementClickHandler _elementClickHandler;

    private void OnEnable()
    {
        _scrollView.normalizedPosition = new Vector2(0, 1);
    }

    public void Init(IElementClickHandler elementClickHandler)
    {
        _elementClickHandler = elementClickHandler;
        _closeButton.AssignOnClickAction(Close);
        IsInitialized = true;
    }

    protected override void AddElement(Element element)
    {
        if (IsInitialized == false)
            throw new InvalidOperationException("Object is not initialized");

        var renderer = Instantiate(_bookElementTemplate, _container);
        renderer.AssignClickHandler(_elementClickHandler);
        renderer.RenderInteractable(element);
        OpenedElementRenderers.Add(renderer);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
