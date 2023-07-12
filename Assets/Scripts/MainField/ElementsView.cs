using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementsView : MonoBehaviour
{
    protected List<ElementRenderer> OpenedElementRenderers = new();

    protected bool IsInitialized { get; set; }

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
}
