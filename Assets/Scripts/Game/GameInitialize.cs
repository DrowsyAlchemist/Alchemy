using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
#if UNITY_EDITOR
        bool isPlayerAuthorized = false;
#else
        bool isPlayerAuthorized = PlayerAccount.IsAuthorized;

#endif
        _saver = Saver.Create(_elementsStorage, isPlayerAuthorized);

        while (_saver.IsReady == false)
            yield return null;

        OpenInitialElements();
        _elementsStorage.Init(_saver);
        _menu.Init(this);
        _gameField.Init(_saver, _elementsStorage);
        _interAdPanel.Init(_saver);
        _progress.Init(_elementsStorage);
        _score.Init(isPlayerAuthorized);
        _elementsMerger = new ElementsMerger(_openedElementsView, _score, _elementsStorage);
        _openedElementsView.InitMainView(_gameField, _elementsMerger, _elementsStorage);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
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
        string systemLang = YandexGamesSdk.Environment.GetCurrentLang();
        LeanLocalization.SetCurrentLanguageAll(systemLang);
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

    public void ResetSaves()
    {
        ResetProgress();
        UnityEngine.PlayerPrefs.DeleteAll();
        UnityEngine.PlayerPrefs.Save();
    }

    public void ResetProgress()
    {
        _saver.ResetSaves();
        _score.ResetCurrentScore();
        _elementsStorage.ResetOpenedElements();
        OpenInitialElements();

        _gameField.Clear();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
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
