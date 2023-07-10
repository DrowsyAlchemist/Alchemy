using System.Collections.Generic;
using UnityEngine;

public class ElementsStorage : MonoBehaviour
{
    [SerializeField] private List<Element> _elements = new();

    public IReadOnlyCollection<Element> Elements => _elements;

    public void OnValidate()
    {
        _elements.Sort((a, b) => a.Lable.CompareTo(b.Lable));
    }
}
