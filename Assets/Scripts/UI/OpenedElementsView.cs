using System;
using System.Collections.Generic;
using UnityEngine;

public class OpenedElementsView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private OpenedElementRenderer _openedElementRendererTemplate;
    [SerializeField] private MergeableElementRenderer _mergeableElementRendererTemplate;

    private List<OpenedElementRenderer> _openedElementRenderers = new();
    private GameField _gameField;
    private IMergeHandler _mergeHandler;
    private bool _initialized = false;

    public void Init(GameField gameField, IMergeHandler mergeHandler)
    {
        _gameField = gameField ?? throw new ArgumentNullException();
        _mergeHandler = mergeHandler ?? throw new ArgumentNullException();
        _initialized = true;
    }

    public void Fill(IReadOnlyCollection<Element> elements)
    {
        if (_initialized == false)
            throw new InvalidOperationException("Object is not initialized");

        int i = 0;

        foreach (var element in elements)
        {
            if ((i + 1) > _openedElementRenderers.Count)
                AddElement(element);
            else
                _openedElementRenderers[i].Render(element);

            i++;
        }
        while (_openedElementRenderers.Count > elements.Count)
        {
            Destroy(_openedElementRenderers[i].gameObject);
            _openedElementRenderers.RemoveAt(i);
        }
    }

    private void AddElement(Element element)
    {
        if (_initialized == false)
            throw new InvalidOperationException("Object is not initialized");

        var renderer = Instantiate(_openedElementRendererTemplate, _container);
        renderer.Render(element);
        renderer.Init(_mergeableElementRendererTemplate, _gameField, _mergeHandler);
        _openedElementRenderers.Add(renderer);
    }
}
