using Agava.YandexGames;
using TMPro;
using UnityEngine;

public class InterAdPanel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _secondsBetweenAds = 10;
    [SerializeField] private float _initialTime;

    private const string ShowAnimation = "Show";
    private float _elapsedTime;

    private void Awake()
    {
        _elapsedTime = _initialTime;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > _secondsBetweenAds)
        {
            _elapsedTime = 0;
            _animator.Play(ShowAnimation);
        }
    }

    private void SetTimerString(string str)
    {
        _timerText.text = str;
    }

    private void ShowInter()
    {
#if UNITY_EDITOR
        return;
#endif
        InterstitialAd.Show();
    }
}
