using System;
using System.Collections.Generic;
using UnityEngine;

public class ElementsStorage : MonoBehaviour, IProgressHolder
{
    [SerializeField] private List<Element> _elements = new();
    private List<Element> _sortedOpenedElements = new();

    public IReadOnlyCollection<Element> SortedElements => _elements;
    public IReadOnlyCollection<Element> SortedOpenedElements => _sortedOpenedElements;
    public int MaxCount => _elements.Count;
    public int CurrentCount => _sortedOpenedElements.Count;

    public event Action<int> CurrentCountChanged;

    public void Init()
    {
        SortElements(_elements);

        foreach (var element in _elements)
        {
            if (element.IsOpened)
                _sortedOpenedElements.Add(element);

            element.Opened += OnElementOpened;
        }
        CurrentCountChanged?.Invoke(CurrentCount);
    }

    public void ResetOpenedElements()
    {
        _sortedOpenedElements.Clear();
    }

    private void OnElementOpened(Element element)
    {
        _sortedOpenedElements.Add(element);
        SortElements(_sortedOpenedElements);
        CurrentCountChanged?.Invoke(CurrentCount);
    }

    private void SortElements(List<Element> elements)
    {
        elements.Sort((a, b) => a.Lable.CompareTo(b.Lable));
    }

    public void OnValidate()
    {
        SortElements(_elements);
    }
}
