using Agava.YandexGames;
using GameAnalyticsSDK;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private RectTransform _elementsContainer;
    [SerializeField] private UIButton _clearButton;
    [SerializeField] private UIButton _hideAdForYanButton;
    [SerializeField] private UIButton _openElementForYanButton;
    [SerializeField] private YanElementWindow _yanElementWindow;

    private const string OffAdProductId = "OffAd";
    private Saver _saver;
    private ElementsStorage _elementsStorage;

    public RectTransform ElementsContainer => _elementsContainer;
    public IReadOnlyList<ElementRenderer> ElementsOnField => _elementsContainer.GetComponentsInChildren<ElementRenderer>();

    public void Init(Saver saver, ElementsStorage elementsStorage, bool isTrainingMode = false)
    {
        _saver = saver;
        _elementsStorage = elementsStorage;
        _clearButton.AssignOnClickAction(Clear);

        if (isTrainingMode == false)
        {
            _hideAdForYanButton.AssignOnClickAction(OffAd);
            _openElementForYanButton.AssignOnClickAction(OpenElementForYan);
        }
        if (_saver.IsAdAllowed == false)
            _hideAdForYanButton.Deactivate();

        CheckClosedElements(null);
        _elementsStorage.ElementOpened += CheckClosedElements;
    }

    private void OnDestroy()
    {
        _elementsStorage.ElementOpened -= CheckClosedElements;
    }

    public void Clear()
    {
        for (int i = 0; i < _elementsContainer.childCount; i++)
            Destroy(_elementsContainer.GetChild(i).gameObject);
    }

    private void CheckClosedElements(Element _)
    {
        if (_elementsStorage.ElementsLeft == 0)
            _openElementForYanButton.Deactivate();
    }

    private void OpenElementForYan()
    {
        Metrics.SendEvent(MetricEvent.ClickFieldBuyButton);
        List<Element> elementsForOpenening = FindElementsForOpening();
        Element elementForOpenenig = elementsForOpenening[Random.Range(0, elementsForOpenening.Count)];
#if UNITY_EDITOR
        elementForOpenenig.Open();
        _yanElementWindow.Show(elementForOpenenig);
        return;
#endif
        if (_elementsStorage.ElementsLeft <= Settings.Monetization.LastElementsCount)
        {
            Billing.PurchaseProduct(
                MonetizationSettings.OpenLastElementForYanId,
                onSuccessCallback: (_) =>
                {
                    elementForOpenenig.Open();
                    _yanElementWindow.Show(elementForOpenenig);
                    Metrics.SendEvent(MetricEvent.BuyLastElement);
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 10, "LastElement", "InField");
                },
                developerPayload: elementForOpenenig.Id); ;
        }
        else
        {
            Billing.PurchaseProduct(
                MonetizationSettings.OpenElementForYanId,
                onSuccessCallback: (_) =>
                {
                    elementForOpenenig.Open();
                    _yanElementWindow.Show(elementForOpenenig);
                    Metrics.SendEvent(MetricEvent.BuyRandomElement);
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 2, "Element", "InField");
                },
                developerPayload: elementForOpenenig.Id);
        }
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

    private void OffAd()
    {
        Metrics.SendEvent(MetricEvent.ClickOffAd);
        Billing.PurchaseProduct(OffAdProductId, onSuccessCallback: (response) =>
        {
            StickyAd.Hide();
            _saver.OffAd();
            _hideAdForYanButton.Deactivate();
            Metrics.SendEvent(MetricEvent.BuyOffAd);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 20, "OffAd", "OffAd");
        });
    }
}
