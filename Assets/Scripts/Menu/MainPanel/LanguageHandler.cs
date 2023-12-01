using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _loadingCanvas;
    [SerializeField] private LanguageButton[] _languageButtons;

    private Saver _saver;

    private void OnDestroy()
    {
        foreach (var button in _languageButtons)
            button.Clicked -= OnChangeLanguageButtonClick;
    }

    public void Init(Saver saver)
    {
        _saver = saver;

#if !UNITY_EDITOR
        string systemLang = YandexGamesSdk.Environment.GetCurrentLang();
#else
        string systemLang = LeanLocalization.GetFirstCurrentLanguage();
#endif
        string savedLang = saver.CurrentLanguage;
        string targetLang = string.IsNullOrEmpty(savedLang) ? systemLang : savedLang;
        LeanLocalization.SetCurrentLanguageAll(targetLang);
        _saver.SetLanguage(targetLang);

        if (_languageButtons == null || _languageButtons.Length == 0)
            return;

        foreach (var button in _languageButtons)
        {
            button.Init(targetLang);
            button.Clicked += OnChangeLanguageButtonClick;
        }
        if (targetLang.Equals("ru"))
            Metrics.SendEvent(MetricEvent.LngRu);
        else
            Metrics.SendEvent(MetricEvent.LngEn);
    }

    private void OnChangeLanguageButtonClick(string targetLang)
    {
        string currentLanguage = LeanLocalization.GetFirstCurrentLanguage();

        if (currentLanguage.Equals(targetLang))
            return;

        if (targetLang.Equals("ru") == false && targetLang.Equals("en") == false)
            throw new System.NotImplementedException();

        _loadingCanvas.Activate();
        _saver.SetLanguage(targetLang);
        SceneManager.LoadScene(Settings.MainSceneName);
    }
}
