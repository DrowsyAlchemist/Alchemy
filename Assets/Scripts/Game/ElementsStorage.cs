using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElementsStorage : MonoBehaviour, IProgressHolder
{
    [SerializeField] private List<Element> _elements = new();

    private const string OffAdProductId = "OffAd";
    //private static ElementsStorage _instance;
    private List<Element> _sortedOpenedElements = new();

    public int ElementsLeft => SortedElements.Count - SortedOpenedElements.Count;
    public IReadOnlyCollection<Element> SortedElements => _elements;
    public IReadOnlyCollection<Element> SortedOpenedElements => _sortedOpenedElements;
    public int MaxCount => _elements.Count;
    public int CurrentCount => _sortedOpenedElements.Count;

    public event Action<int> CurrentCountChanged;

    private void OnDestroy()
    {
        foreach (var element in _elements)
            element.Opened -= OnElementOpened;
    }

    public void Init(Saver saver)
    {
        // _instance = this;
        SortElements(_elements);

        foreach (var element in _elements)
        {
            if (element.IsOpened)
                _sortedOpenedElements.Add(element);

            element.Opened += OnElementOpened;
        }
        CurrentCountChanged?.Invoke(CurrentCount);

#if UNITY_EDITOR
        return;
#endif
        Billing.GetPurchasedProducts(onSuccessCallback: (response) =>
        {
            foreach (var product in response.purchasedProducts)
            {
                if (product.productID.Contains(OffAdProductId) && saver.IsAdAllowed)
                {
                    saver.OffAd();
                    continue;
                }
                Debug.Log("DeveloperPayload: " + product.developerPayload);
                var purchasedElement = _elements.FirstOrDefault((element) => element.Id.Equals(product.developerPayload));

                if (purchasedElement != null)
                {
                    purchasedElement.Open();
                    Debug.Log($"Element {product.developerPayload} is opened");
                }
                else
                {
                    Debug.Log($"Element {product.developerPayload} is not found");
                }
            }
        });
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
