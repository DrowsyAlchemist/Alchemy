using Agava.YandexGames;
using GameAnalyticsSDK;
using UnityEngine;

public class ElementForYanOpener : IElementClickHandler
{
    private const string OpenElementForYanId = "OpenElement";
    private const string OpenLastElementsForYanId = "OpenLastElement";
    private const int LastElementsCount = 20;

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        Metrics.SendEvent(MetricEvent.ClickBookBuyButton);
#if UNITY_EDITOR
        OnPurchased(elementRenderer);
        return;
#endif
      //  if (_elementsStorage.ElementsLeft <= LastElementsCount)
       // {
        //    Billing.PurchaseProduct(
         //       OpenLastElementsForYanId,
          //      onSuccessCallback: (_) =>
          //      {
           //         OnPurchased(elementRenderer);
            //        Metrics.SendEvent(MetricEvent.BuyLastElement);
            //        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 10, "LastElement", "InBook");
            //    });
       // }
      //  else
     //   {
            Billing.PurchaseProduct(
                OpenElementForYanId,
                onSuccessCallback: (_) =>
                {
                    OnPurchased(elementRenderer);
                    Metrics.SendEvent(MetricEvent.BuyBookElement);
                    GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Yan", 2, "Element", "InBook");
                });
     //   }
    }

    private void OnPurchased(BookElementRenderer elementRenderer)
    {
        elementRenderer.Element.Open();
        Debug.Log("Element purchased");
        elementRenderer.RenderOpened(elementRenderer.Element);
    }
}
