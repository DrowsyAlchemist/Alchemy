using Agava.YandexGames;
using TMPro;
using UnityEngine;

public class InterAdPanel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _initialTime;

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

        if (_elapsedTime > Settings.MonetizationSettings.SecondsBetweenInters)
        {
            _elapsedTime = 0;
            _animator.Play(ShowAnimation);
        }
    }

    public void Init(Saver saver)
    {
        _saver = saver;
    }

    private void SetTimerString(string str)
    {
        _timerText.text = str;
    }

    private void ShowInter()
    {
        if (_saver.IsAdAllowed)
            InterstitialAd.Show();
    }
}
