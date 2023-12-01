using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _loadingCanvas;
    [SerializeField] private UIButton _languageButton;
    [SerializeField] private Image _lngImage;
    [SerializeField] private Sprite _ruSprite;
    [SerializeField] private Sprite _enSprite;

    private Saver _saver;

    public void Init(Saver saver)
    {
        _saver = saver;
        _languageButton.AssignOnClickAction(OnChangeLanguageButtonClick);

#if !UNITY_EDITOR
        string systemLang = YandexGamesSdk.Environment.GetCurrentLang();
#else
        string systemLang = LeanLocalization.GetFirstCurrentLanguage();
#endif
        string savedLang = saver.CurrentLanguage;
        string targetLang = string.IsNullOrEmpty(savedLang) ? systemLang : savedLang;

        _lngImage.sprite = targetLang.Equals("ru") ? _ruSprite : _enSprite;
        LeanLocalization.SetCurrentLanguageAll(targetLang);
        _saver.SetLanguage(targetLang);

        if (targetLang.Equals("ru"))
            Metrics.SendEvent(MetricEvent.LngRu);
        else
            Metrics.SendEvent(MetricEvent.LngEn);
    }

    private void OnChangeLanguageButtonClick()
    {
        _loadingCanvas.Activate();
        string currentLanguage = LeanLocalization.GetFirstCurrentLanguage();
        string targetlanguage = currentLanguage.Equals("ru") ? "en" : "ru";
        _saver.SetLanguage(targetlanguage);
        SceneManager.LoadScene(Settings.MainSceneName);
    }
}
