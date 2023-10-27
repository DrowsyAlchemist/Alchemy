using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameInitialize : MonoBehaviour, IMergeHandler
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

    [SerializeField] private BookElementsView _bookGridView;
    [SerializeField] private RecipiesView _recipiesWithElementView;

    [SerializeField] private List<Element> _initialElements;

    private static GameInitialize _instance;
    private Saver _saver;
    private RecipiesBook _recipiesBook;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        Init();
        yield break;
#endif
        while (YandexGamesSdk.IsInitialized == false)
            yield return YandexGamesSdk.Initialize();

        InterstitialAd.Show(onOpenCallback: () => Sound.Mute(), onCloseCallback: (_) => Sound.TurnOn());
        GameAnalytics.Initialize();
        Init();
    }

    private void Init()
    {
#if UNITY_EDITOR
        bool isPlayerAuthorized = false;
#else
        bool isPlayerAuthorized = PlayerAccount.IsAuthorized;

#endif
        _saver = Saver.Create(_elementsStorage, isPlayerAuthorized);
        OpenInitialElements();
        _elementsStorage.Init();
        _menu.Init();
        _progress.Init(_elementsStorage);
        _openedElementsView.Init(_gameField, this);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
        _alphabeticalIndex.Init();

        _resetButton.AssignOnClickAction(onButtonClick: ResetProgress);
        _openRecipiesBookButton.AssignOnClickAction(onButtonClick: OpenRecipiesBook);

        _recipiesBook = new RecipiesBook(_elementsStorage, _bookGridView, _recipiesWithElementView);
        _score.Init(isPlayerAuthorized);
        _leaderboardView.Init(isPlayerAuthorized, _score);
        YandexGamesSdk.GameReady();

#if UNITY_EDITOR
        return;
#endif
        string systemLang = YandexGamesSdk.Environment.GetCurrentLang();
        LeanLocalization.SetCurrentLanguageAll(systemLang);
    }

    public void OpenAllElements()
    {
        foreach (var element in _elementsStorage.SortedElements)
            element.Open();

        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
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

    public void TryMergeElements(MergeableElementRenderer firstRenderer, MergeableElementRenderer secondRenderer)
    {
        var results = new List<Element>();

        foreach (var recipe in firstRenderer.Element.Recipies)
            if (recipe.SecondElement.Equals(secondRenderer.Element))
                results.Add(recipe.Result);

        Merge(firstRenderer, secondRenderer, results);
    }

    public void OpenRecipiesBook()
    {
        _recipiesBook.Open();
    }

    private void Merge(MergeableElementRenderer firstRenderer, MergeableElementRenderer secondRenderer, List<Element> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
            if (results[i].IsOpened == false)
                OpenNewElement(results[i]);

            if (i == 0)
                firstRenderer.Render(results[0]);

            if (i == 1)
                secondRenderer.Render(results[1]);

            if (i > 1)
            {
                var newRenderer = Instantiate(firstRenderer, firstRenderer.transform.position, Quaternion.identity, firstRenderer.transform.parent);
                newRenderer.Render(results[i]);
            }
        }
        if (results.Count == 1)
            Destroy(secondRenderer.gameObject);
    }

    private void OpenNewElement(Element element)
    {
        _score.AddScore(Settings.GameSettings.PointsForOpenedElement);
        element.Open();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    private void OpenInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }
}
