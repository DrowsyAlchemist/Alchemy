using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenedElementsView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private ElementRenderer _elementRendererTemplate;

    public void FillWithOpened(IReadOnlyCollection<Element> elements)
    {
        foreach (var element in elements)
        {
            if (element.IsOpened)
            {
                var renderer = Instantiate(_elementRendererTemplate, _container);
                renderer.Render(element);
            }

        }
    }
}
