using Agava.YandexGames;
using System;
using UnityEngine;

public class AdShower
{
    private readonly Action _onOpen;
    private readonly Action _onClose;
    private readonly Action<string> _onError;

    public AdShower(Action onopen = null, Action onClose = null, Action<string> onError = null)
    {
        _onOpen = onopen;
        _onClose = onClose;
        _onError = onError ?? OnErrorDefault;
    }

    public void ShowVideo(Action onRewarded)
    {
        if (onRewarded == null)
            throw new InvalidOperationException("Reward action is not assigned");
#if UNITY_EDITOR
        Debug.Log("ShowVideo");
        onRewarded.Invoke();
        return;
#endif
        VideoAd.Show(_onOpen, onRewarded, null, _onError);
    }

    public void ShowInter()
    {
#if UNITY_EDITOR
        Debug.Log("ShowInter");
        return;
#endif
        InterstitialAd.Show(_onOpen, null, _onError);
    }

    private void OnErrorDefault(string error)
    {
        Debug.Log("ShowAd error: " + error);
    }

    //private void OnCloseInter(bool _)
    //{
    //    _onClose?.Invoke();
    //}
}
