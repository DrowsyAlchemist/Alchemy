using GameAnalyticsSDK;
using System;

public class ElementForAdOpener : IElementClickHandler
{
    public event Action ElementOpened;

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        Metrics.SendEvent(MetricEvent.ClickOpenForAdButton);
        AdShower.ShowVideo(onRewarded: () => OnRewarded(elementRenderer));
    }

    private void OnRewarded(BookElementRenderer elementRenderer)
    {
        elementRenderer.RenderOpened(elementRenderer.Element);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "VideoAd", 2, "Element", "InBook");
        Metrics.SendEvent(MetricEvent.OpenElementForAd);
        ElementOpened?.Invoke();
    }
}
