using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class GameInitialize : MonoBehaviour
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private MainOpenedElementsView _openedElementsView;
    [SerializeField] private AlphabeticalIndex _alphabeticalIndex;
    [SerializeField] private GameField _gameField;
    [SerializeField] private Menu _menu;
    [SerializeField] private UIButton _openRecipiesBookButton;
    [SerializeField] private Progress _progress;
    [SerializeField] private Score _score;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private AchievementsMenu _achievementsMenu;
    [SerializeField] private AchievementWindow _achievementWindow;
    [SerializeField] private TerminalElementWindow _terminalElementWindow;
    [SerializeField] private InterAdPanel _interAdPanel;
    [SerializeField] private TrainingWindow _trainingWindow;

    [SerializeField] private BookElementsView _bookGridView;
    [SerializeField] private RecipiesView _recipiesWithElementView;

    [SerializeField] private List<Element> _initialElements;

    [SerializeField] private Image _fadeImage;

    private Saver _saver;
    private RecipiesBook _recipiesBook;
    private ElementsMerger _elementsMerger;

    private IEnumerator Start()
    {
        _fadeImage.Activate();
#if UNITY_EDITOR
        Settings.CoroutineObject.StartCoroutine(Init());
        yield break;
#endif
        while (YandexGamesSdk.IsInitialized == false)
            yield return YandexGamesSdk.Initialize();

        GameAnalytics.Initialize();
        Settings.CoroutineObject.StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
#if !UNITY_EDITOR
        string systemLang = YandexGamesSdk.Environment.GetCurrentLang();
        LeanLocalization.SetCurrentLanguageAll(systemLang);
#endif
        _saver = Saver.Create(_elementsStorage, _score);

        while (_saver.IsReady == false)
            yield return null;

        OpenInitialElements();
        _menu.Init(this);
        _gameField.Init(_saver, _elementsStorage);
        _interAdPanel.Init(_saver);
        _progress.Init(_elementsStorage);
        _elementsMerger = new ElementsMerger();
        _openedElementsView.InitMainView(_gameField, _elementsMerger, _elementsStorage);
        _alphabeticalIndex.Init();

        _openRecipiesBookButton.AssignOnClickAction(onButtonClick: OpenRecipiesBook);

        _recipiesBook = new RecipiesBook(_elementsStorage, _bookGridView, _recipiesWithElementView);
        _leaderboardView.Init(_score, _saver);
        _achievementsMenu.Init(_score);
        _achievementWindow.Init(_achievementsMenu);
        _terminalElementWindow.Init(_elementsStorage, _saver);
        _trainingWindow.Init(_saver);

#if UNITY_EDITOR
        _fadeImage.Deactivate();
        yield break;
#endif
        if (_saver.IsAdAllowed)
        {
            StickyAd.Show();
            InterstitialAd.Show(onOpenCallback: () => Sound.Mute(), onCloseCallback: (_) => Sound.TurnOn());
        }
        else
        {
            StickyAd.Hide();
        }
        _fadeImage.Deactivate();
        YandexGamesSdk.GameReady();
        Metrics.SendEvent(MetricEvent.StartGame);
    }

    public void OpenAllElements()
    {
        foreach (var element in _elementsStorage.SortedElements)
            element.Open();

        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    public void RemoveSaves()
    {
        _fadeImage.Activate();
        _saver.RemoveSaves();
        _elementsStorage.ResetOpenedElements();
        OpenInitialElements();
        _gameField.Clear();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
        SceneManager.LoadScene(Settings.MainSceneName);
    }

    public void ResetProgress()
    {
        _fadeImage.Activate();

#if UNITY_EDITOR
        RemoveProgress();
        return;
#endif
        if (PlayerAccount.IsAuthorized)
            Billing.GetPurchasedProducts(onSuccessCallback: (response) =>
            {
                for (int i = 0; i < response.purchasedProducts.Length; i++)
                    Billing.ConsumeProduct(response.purchasedProducts[i].purchaseToken);

                RemoveProgress();
            });
        else
            RemoveProgress();
    }

    private void RemoveProgress()
    {
        _saver.ResetSaves();
        _elementsStorage.ResetOpenedElements();
        OpenInitialElements();
        _gameField.Clear();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
        _fadeImage.Deactivate();
    }

    private void OpenRecipiesBook()
    {
        _recipiesBook.Open();
    }

    private void OpenInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }
}
