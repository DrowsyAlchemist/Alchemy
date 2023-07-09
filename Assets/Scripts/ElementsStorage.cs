using System.Collections.Generic;
using UnityEngine;

public class ElementsStorage : MonoBehaviour
{
    [SerializeField] private List<Element> _elements = new();

    public IReadOnlyCollection<Element> Elements => _elements;
}
