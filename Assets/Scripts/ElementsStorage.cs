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
        _sortedOpenedElements.Clear();

        foreach (var element in _elements)
        {
            if (element.IsOpened)
                _sortedOpenedElements.Add(element);
            else
                element.Opened += OnElementOpened;
        }
        CurrentCountChanged?.Invoke(CurrentCount);
    }

    private void OnElementOpened(Element element)
    {
        _sortedOpenedElements.Add(element);
        CurrentCountChanged?.Invoke(CurrentCount);
    }

    public void OnValidate()
    {
        _elements.Sort((a, b) => a.Lable.CompareTo(b.Lable));
    }
}
