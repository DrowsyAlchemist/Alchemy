using System;
using UnityEngine;

public class MainOpenedElementsView : ElementsView
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private OpenedElementRenderer _openedElementRendererTemplate;
    [SerializeField] private MergeableElementRenderer _mergeableElementRendererTemplate;

    private GameField _gameField;
    private IMergeHandler _mergeHandler;

    public void InitMainView(GameField gameField, IMergeHandler mergeHandler, ElementsStorage elementsStorage)
    {
        base.Init(elementsStorage);
        _gameField = gameField ?? throw new ArgumentNullException();
        _mergeHandler = mergeHandler ?? throw new ArgumentNullException();
        IsInitialized = true;
    }

    protected override void AddElement(Element element)
    {
        if (IsInitialized == false)
            throw new InvalidOperationException("Object is not initialized");

        var renderer = Instantiate(_openedElementRendererTemplate, _container);
        renderer.Render(element);
        renderer.Init(_mergeableElementRendererTemplate, _gameField, _mergeHandler);
        OpenedElementRenderers.Add(renderer);
    }
}
