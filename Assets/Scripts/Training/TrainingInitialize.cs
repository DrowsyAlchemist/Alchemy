using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class TrainingInitialize : MonoBehaviour
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private MainOpenedElementsView _openedElementsView;
    [SerializeField] private AlphabeticalIndex _alphabeticalIndex;
    [SerializeField] private GameField _gameField;
    [SerializeField] private Menu _menu;
    [SerializeField] private UIButton _openRecipiesBookButton;
    [SerializeField] private Score _score;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private AchievementsMenu _achievementsMenu;
    [SerializeField] private BookElementsView _bookGridView;
    [SerializeField] private RecipiesView _recipiesWithElementView;

    [SerializeField] private List<Element> _initialElements;

    [SerializeField] private Image _fadeImage;

    [SerializeField] private UIButton _nextTaskButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Training _training;

    private Saver _saver;
    private RecipiesBook _recipiesBook;
    private ElementsMerger _elementsMerger;

    private void Start()
    {
        Settings.CoroutineObject.StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        _fadeImage.Activate();
        yield return new WaitForEndOfFrame();
#if UNITY_EDITOR
        bool isPlayerAuthorized = false;
#else
        bool isPlayerAuthorized = PlayerAccount.IsAuthorized;

#endif
        _saver = Saver.Create(_elementsStorage, isPlayerAuthorized, _score, isTrainingMode: true);

        while (_saver.IsReady == false)
            yield return null;

        _score.Init(isPlayerAuthorized);
        OpenInitialElements();
        _elementsStorage.Init(_saver);
        _menu.Init(null); //
        _gameField.Init(_saver, _elementsStorage, isTrainingMode: true);
        _elementsMerger = new ElementsMerger();
        _openedElementsView.InitMainView(_gameField, _elementsMerger, _elementsStorage, trainingMode: true);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
        _alphabeticalIndex.Init();

        _openRecipiesBookButton.AssignOnClickAction(onButtonClick: OpenRecipiesBook);

        _recipiesBook = new RecipiesBook(_elementsStorage, _bookGridView, _recipiesWithElementView);
        _leaderboardView.Init(_score, _saver);
        _achievementsMenu.Init(_score);
        _training.Init(_saver);
        _training.Begin();
        _fadeImage.Deactivate();

#if UNITY_EDITOR
        _fadeImage.Deactivate();
        yield break;
#endif
        if (_saver.IsAdAllowed)
            StickyAd.Show();
        else
            StickyAd.Hide();

        string systemLang = YandexGamesSdk.Environment.GetCurrentLang();
        LeanLocalization.SetCurrentLanguageAll(systemLang);
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
