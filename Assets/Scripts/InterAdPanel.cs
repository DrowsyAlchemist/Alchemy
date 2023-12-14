using Agava.YandexGames;
using GameAnalyticsSDK;
using TMPro;
using UnityEngine;

public class InterAdPanel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _initialTime;

    private const float InterExtraSeconds = 30;
    private const string ShowAnimation = "Show";
    private Saver _saver;
    private float _elapsedTime;

    private void Awake()
    {
        _elapsedTime = _initialTime;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > Settings.Monetization.SecondsBetweenInters)
        {
            _elapsedTime = 0;

            if (_saver.IsAdAllowed)
                _animator.Play(ShowAnimation);
            else
                Destroy(gameObject);
        }
    }

    public void Init(Saver saver)
    {
        _saver = saver;

        if (_saver.IsAdAllowed == false)
            Destroy(gameObject);
    }

    private void SetTimerString(string str)
    {
        _timerText.text = str;
    }

    private void ShowInter()
    {
        if (AdShower.IsAdOpen)
        {
            _elapsedTime = Settings.Monetization.SecondsBetweenInters - InterExtraSeconds;
        }
        else
        {
            AdShower.ShowInter();
            _elapsedTime = 0;
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "InterAd", 1, "InterAd", "InterAd");
        }
    }
}
