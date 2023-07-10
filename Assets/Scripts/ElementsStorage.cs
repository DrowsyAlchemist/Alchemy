using System.Collections.Generic;
using UnityEngine;

public class ElementsStorage : MonoBehaviour
{
    [SerializeField] private List<Element> _elements = new();

    public IReadOnlyCollection<Element> SortedElements => _elements;
    public IReadOnlyCollection<Element> SortedOpenedElements => GetOpenedElements();

    private IReadOnlyCollection<Element> GetOpenedElements()
    {
        var result = new List<Element>();

        foreach (var element in _elements)
            if(element.IsOpened)
                result.Add(element);

        return result;
    }

    public void OnValidate()
    {
        _elements.Sort((a, b) => a.Lable.CompareTo(b.Lable));
    }
}
