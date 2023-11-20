using Agava.YandexMetrica;
using UnityEngine;

public static class Metrics
{
    public static void SendEvent(MetricEvent metricEvent, string eventDataJson = null)
    {
#if UNITY_EDITOR
        return;
#endif
        string metricEventStr = null;

        switch (metricEvent)
        {
            case MetricEvent.StartGame:
                metricEventStr = "startGame";
                break;
            case MetricEvent.MakeFirstElement:
                metricEventStr = "makeFirstElement";
                break;
            case MetricEvent.MakeFiftyElements:
                metricEventStr = "makeFiftyElements";
                break;
            case MetricEvent.MakeOneHundredElements:
                metricEventStr = "makeOneHundredElements";
                break;
            case MetricEvent.MakeTwoHundredElements:
                metricEventStr = "makeTwoHundredElements";
                break;
            case MetricEvent.MakeThreeHundredElements:
                metricEventStr = "makeThreeHundredElements";
                break;
            case MetricEvent.MakeFourHundredElements:
                metricEventStr = "makeFourHundredElements";
                break;
            case MetricEvent.MakeFiveHundredElements:
                metricEventStr = "makeFiveHundredElements";
                break;
            case MetricEvent.MakeSixHundredElements:
                metricEventStr = "makeSixHundredElements";
                break;
            case MetricEvent.ClickFieldBuyButton:
                metricEventStr = "clickFieldBuyButton";
                break;
            case MetricEvent.BuyRandomElement:
                metricEventStr = "buyRandomElement";
                break;
            case MetricEvent.ClickBookBuyButton:
                metricEventStr = "clickBookBuyButton";
                break;
            case MetricEvent.BuyBookElement:
                metricEventStr = "buyBookElement";
                break;
            case MetricEvent.BuyLastElement:
                metricEventStr = "buyLastElement";
                break;
            case MetricEvent.ClickOffAd:
                metricEventStr = "clickOffAd";
                break;
            case MetricEvent.BuyOffAd:
                metricEventStr = "buyOffAd";
                break;
            case MetricEvent.ClickOpenForAdButton:
                metricEventStr = "clickAdButton";
                break;
            case MetricEvent.OpenElementForAd:
                metricEventStr = "openElementForAd";
                break;
            case MetricEvent.OpenLeaderboard:
                metricEventStr = "openLeaderboard";
                break;
            case MetricEvent.OpenAchievements:
                metricEventStr = "openAchievements";
                break;
            case MetricEvent.OpenBook:
                metricEventStr = "openBook";
                break;
            case MetricEvent.OpenRecipies:
                metricEventStr = "openRecipies";
                break;
            case MetricEvent.LngEn:
                metricEventStr = "lngEn";
                break;
            case MetricEvent.LngRu:
                metricEventStr = "lngRu";
                break;
        }
        YandexMetrica.Send(metricEventStr, eventDataJson);
    }
}