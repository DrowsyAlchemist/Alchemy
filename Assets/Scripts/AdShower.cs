using Agava.YandexGames;
using System;
using UnityEngine;

public static class AdShower
{
    public static bool IsAdOpen { get; private set; }

    public static void ShowVideo(Action onRewarded)
    {
        if (onRewarded == null)
            throw new InvalidOperationException("Reward action is not assigned");
#if UNITY_EDITOR
        Debug.Log("ShowVideo");
        onRewarded.Invoke();
        return;
#endif
        VideoAd.Show(onOpenCallback: OnAdOpen, onRewardedCallback: onRewarded, onCloseCallback: OnAdClose, onErrorCallback: OnErrorDefault);
    }

    public static void ShowInter()
    {
#if UNITY_EDITOR
        Debug.Log("ShowInter");
        return;
#endif
        InterstitialAd.Show(onOpenCallback: OnAdOpen, onCloseCallback: (_) => OnAdClose(), onErrorCallback: OnErrorDefault);
    }

    private static void OnAdOpen()
    {
        Sound.Mute();
        IsAdOpen = true;
    }

    private static void OnAdClose()
    {
        Sound.TurnOn();
        IsAdOpen = false;
    }

    private static void OnErrorDefault(string error)
    {
        Debug.Log("ShowAd error: " + error);
    }
}
