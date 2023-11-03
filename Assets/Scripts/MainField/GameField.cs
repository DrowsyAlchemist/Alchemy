using Agava.YandexGames;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private RectTransform _elementsContainer;
    [SerializeField] private UIButton _clearButton;
    [SerializeField] private UIButton _hideAdForYanButton;
    [SerializeField] private UIButton _openElementForYanButton;

    private const string OffAdProductId = "OffAd";
    private const string OpenElementForYanId = "OpenElement";
    private Saver _saver;
    private ElementsStorage _elementsStorage;
    private ElementForAdOpener _elementForAdOpener;

    public RectTransform ElementsContainer => _elementsContainer;

    public void Init(Saver saver, ElementsStorage elementsStorage)
    {
        _saver = saver;
        _elementsStorage = elementsStorage;
        _elementForAdOpener = new ElementForAdOpener();
        _clearButton.AssignOnClickAction(Clear);
        _hideAdForYanButton.AssignOnClickAction(HideStickyAd);
        _openElementForYanButton.AssignOnClickAction(OpenElementForAd);

        if (_saver.IsAdAllowed == false)
            _hideAdForYanButton.Deactivate();

        foreach (var element in _elementsStorage.SortedElements)
            element.Opened += CheckClosedElements;
    }

    private void OnDestroy()
    {
        foreach (var element in _elementsStorage.SortedElements)
            element.Opened -= CheckClosedElements;
    }

    public void Clear()
    {
        for (int i = 0; i < _elementsContainer.childCount; i++)
            Destroy(_elementsContainer.GetChild(i).gameObject);
    }

    private void CheckClosedElements(Element _)
    {
        if (_elementsStorage.SortedElements.Count == _elementsStorage.SortedOpenedElements.Count)
            _openElementForYanButton.Deactivate();
    }

    private void OpenElementForAd()
    {
#if UNITY_EDITOR
        List<Element> elementsForOpenening = FindElementsForOpening();
        Element elementForOpenenig = elementsForOpenening[Random.Range(0, elementsForOpenening.Count)];
        elementForOpenenig.Open();
        return;
#endif
        Billing.PurchaseProduct(OffAdProductId, onSuccessCallback: (response) =>
        {
            List<Element> elementsForOpenening = FindElementsForOpening();
            Element elementForOpenenig = elementsForOpenening[Random.Range(0, elementsForOpenening.Count)];
            elementForOpenenig.Open();
        });
    }

    private List<Element> FindElementsForOpening()
    {
        var elementsForOpening = new List<Element>();

        foreach (var element in _elementsStorage.SortedElements)
        {
            if (element.IsOpened == false)
            {
                foreach (var creationRecipie in element.CreationRecipies)
                {
                    if (creationRecipie.FirstElement.IsOpened && creationRecipie.SecondElement.IsOpened)
                    {
                        elementsForOpening.Add(element);
                        break;
                    }
                }
            }
        }
        return elementsForOpening;
    }

    private void HideStickyAd()
    {
        Billing.PurchaseProduct(OffAdProductId, onSuccessCallback: (response) =>
        {
            StickyAd.Hide();
            _saver.OffAd();
            _hideAdForYanButton.Deactivate();
        });
    }
}
