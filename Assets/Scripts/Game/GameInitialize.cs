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
    [SerializeField] private UIButton _resetButton;
    [SerializeField] private UIButton _openRecipiesBookButton;
    [SerializeField] private Progress _progress;
    [SerializeField] private Score _score;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private InterAdPanel _interAdPanel;

    [SerializeField] private BookElementsView _bookGridView;
    [SerializeField] private RecipiesView _recipiesWithElementView;

    [SerializeField] private List<Element> _initialElements;

    [SerializeField] private Image _fadeImage;

    private static GameInitialize _instance;
    private Saver _saver;
    private RecipiesBook _recipiesBook;
    private ElementsMerger _elementsMerger;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
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
        _menu.Init();
        _gameField.Init(_saver);
        _interAdPanel.Init(_saver);
        _progress.Init(_elementsStorage);
        _score.Init(isPlayerAuthorized);
        _elementsMerger = new ElementsMerger(_openedElementsView, _score, _elementsStorage);
        _openedElementsView.Init(_gameField, _elementsMerger);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
        _alphabeticalIndex.Init();

        _resetButton.AssignOnClickAction(onButtonClick: ResetProgress);
        _openRecipiesBookButton.AssignOnClickAction(onButtonClick: OpenRecipiesBook);

        _recipiesBook = new RecipiesBook(_elementsStorage, _bookGridView, _recipiesWithElementView);
        _leaderboardView.Init(isPlayerAuthorized, _score, _saver);

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
    }

    public void OpenAllElements()
    {
        foreach (var element in _elementsStorage.SortedElements)
            element.Open();

        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    private void OpenRecipiesBook()
    {
        _recipiesBook.Open();
    }

    private void ResetProgress()
    {
        _saver.ResetSaves();
        _score.ResetCurrentScore();
        _elementsStorage.ResetOpenedElements();
        OpenInitialElements();

        _gameField.Clear();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    private void OpenInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }
}
