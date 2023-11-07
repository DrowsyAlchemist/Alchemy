using GameAnalyticsSDK;

public class ElementForAdOpener : IElementClickHandler
{
    private readonly AdShower _adShower;

    public ElementForAdOpener()
    {
        _adShower = new AdShower();
    }

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        Metrics.SendEvent(MetricEvent.ClickOpenForAdButton);
        _adShower.ShowVideo(onRewarded: () => OnRewarded(elementRenderer));
    }

    private void OnRewarded(BookElementRenderer elementRenderer)
    {
        elementRenderer.RenderOpened(elementRenderer.Element);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "VideoAd", 2, "Element", "InBook");
        Metrics.SendEvent(MetricEvent.OpenElementForAd);
    }
}
