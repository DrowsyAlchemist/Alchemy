using Agava.YandexGames;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Game : MonoBehaviour, IMergeHandler
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private MainOpenedElementsView _openedElementsView;
    [SerializeField] private GameField _gameField;
    [SerializeField] private UIButton _resetButton;
    [SerializeField] private UIButton _openRecipiesBookButton;
    [SerializeField] private ProgressRenderer _progressRenderer;
    [SerializeField] private ScoreRenderer _scoreRenderer;

    [SerializeField] private BookElementsView _bookGridView;
    [SerializeField] private RecipiesView _recipiesWithElementView;

    [SerializeField] private List<Element> _initialElements;

    [SerializeField] private Settings _settings;

    private const int PointsByOpenElement = 5;
    private static Game _instance;
    private Saver _saver;
    private RecipiesBook _recipiesBook;
    private Score _score;

    public static MonoBehaviour CoroutineObject => _instance;
    public static Settings Settings => _instance._settings;

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

    public void OpenAllElements()
    {
        foreach (var element in _elementsStorage.SortedElements)
            element.Open();

        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
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
        _progressRenderer.Init(_elementsStorage);
        _openedElementsView.Init(_gameField, this);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);

        _resetButton.AssignOnClickAction(onButtonClick: ResetProgress);
        _openRecipiesBookButton.AssignOnClickAction(onButtonClick: OpenRecipiesBook);

        _recipiesBook = new RecipiesBook(_elementsStorage, _bookGridView, _recipiesWithElementView);

        _score = new Score(isPlayerAuthorized);
        _scoreRenderer.Init(_score);
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
        _score.AddScore(PointsByOpenElement);
        element.Open();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    private void OpenInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }
}
