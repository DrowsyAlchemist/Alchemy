using Agava.YandexGames;
using System;
using UnityEngine;

public class AdShower
{
    public void ShowVideo(Action onRewarded)
    {
        if (onRewarded == null)
            throw new InvalidOperationException("Reward action is not assigned");
#if UNITY_EDITOR
        Debug.Log("ShowVideo");
        onRewarded.Invoke();
        return;
#endif
        VideoAd.Show(onOpenCallback: Sound.Mute, onRewardedCallback: onRewarded, onCloseCallback: Sound.TurnOn, onErrorCallback: OnErrorDefault);
    }

    public void ShowInter()
    {
#if UNITY_EDITOR
        Debug.Log("ShowInter");
        return;
#endif
        InterstitialAd.Show(onOpenCallback: Sound.Mute, onCloseCallback: (_) => Sound.TurnOn(), onErrorCallback: OnErrorDefault);
    }

    private void OnErrorDefault(string error)
    {
        Debug.Log("ShowAd error: " + error);
    }
}
