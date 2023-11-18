using Agava.YandexGames;
using GameAnalyticsSDK;
using System;
using UnityEngine;

public class ElementForYanOpener : IElementClickHandler
{
    private ElementsStorage _elementsStorage;

    public event Action ElementOpened;

    public ElementForYanOpener(ElementsStorage elementsStorage)
    {
        _elementsStorage = elementsStorage;
    }

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        Metrics.SendEvent(MetricEvent.ClickBookBuyButton);
#if UNITY_EDITOR
        OnPurchased(elementRenderer);
        return;
#endif
        if (_elementsStorage.ElementsLeft <= Settings.MonetizationSettings.LastElementsCount)
        {
            Billing.PurchaseProduct(
              MonetizationSettings.OpenLastElementForYanId,
              onSuccessCallback: (_) =>
              {
                  OnPurchased(elementRenderer);
                  Metrics.SendEvent(MetricEvent.BuyLastElement);
                  GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 10, "LastElement", "InBook");
              });
        }
        else
        {
            Billing.PurchaseProduct(
            MonetizationSettings.OpenElementForYanId,
            onSuccessCallback: (_) =>
            {
                OnPurchased(elementRenderer);
                Metrics.SendEvent(MetricEvent.BuyBookElement);
                GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 2, "Element", "InBook");
            });
        }
    }

    private void OnPurchased(BookElementRenderer elementRenderer)
    {
        elementRenderer.Element.Open();
        Debug.Log("Element purchased");
        elementRenderer.RenderOpened(elementRenderer.Element);
        ElementOpened?.Invoke();
    }
}
