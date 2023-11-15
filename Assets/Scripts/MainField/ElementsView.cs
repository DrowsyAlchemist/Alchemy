using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementsView : MonoBehaviour
{
    private ElementsStorage _elementsStorage;
    protected List<ElementRenderer> OpenedElementRenderers = new();

    public IReadOnlyList<ElementRenderer> ElementRenderers => OpenedElementRenderers;
    protected bool IsInitialized { get; set; }

    public void Init(ElementsStorage elementsStorage)
    {
        OpenedElementRenderers = new();
        _elementsStorage = elementsStorage;

        foreach (var element in elementsStorage.SortedElements)
            element.Opened += OnElementOpened;
    }

    private void OnDestroy()
    {
        foreach (var element in _elementsStorage.SortedElements)
            element.Opened -= OnElementOpened;
    }

    public void Fill(IReadOnlyCollection<Element> elements)
    {
        if (IsInitialized == false)
            throw new InvalidOperationException("Object is not initialized");

        int i = 0;

        foreach (var element in elements)
        {
            if ((i + 1) > OpenedElementRenderers.Count)
                AddElement(element);
            else
                OpenedElementRenderers[i].Render(element);

            i++;
        }
        while (OpenedElementRenderers.Count > elements.Count)
        {
            Destroy(OpenedElementRenderers[i].gameObject);
            OpenedElementRenderers.RemoveAt(i);
        }
    }

    protected abstract void AddElement(Element element);

    private void OnElementOpened(Element _)
    {
        Fill(_elementsStorage.SortedOpenedElements);
    }
}
