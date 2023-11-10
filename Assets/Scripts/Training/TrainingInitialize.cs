using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class TrainingInitialize : MonoBehaviour
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private MainOpenedElementsView _openedElementsView;
    [SerializeField] private AlphabeticalIndex _alphabeticalIndex;
    [SerializeField] private GameField _gameField;
    [SerializeField] private Score _score;

    [SerializeField] private List<Element> _initialElements;

    [SerializeField] private Image _fadeImage;

    [SerializeField] private UIButton _nextTaskButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private TaskText[] _tasks;

    private const string MainSceneName = "Main Scene";
    private static TrainingInitialize _instance;
    private Saver _saver;
    private ElementsMerger _elementsMerger;
    private int _currentTask;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            throw new InvalidOperationException();
    }

    private void Start()
    {
        _nextTaskButton.AssignOnClickAction(ShowNextTask);
        Settings.CoroutineObject.StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        _fadeImage.Activate();
#if UNITY_EDITOR
        bool isPlayerAuthorized = false;
#else
        bool isPlayerAuthorized = PlayerAccount.IsAuthorized;

#endif
        _saver = Saver.Create(_elementsStorage, isPlayerAuthorized);

        while (_saver.IsReady == false)
            yield return null;

        OpenInitialElements();
        _gameField.Init(_saver, _elementsStorage);
        _elementsMerger = new ElementsMerger(_openedElementsView, _score, _elementsStorage);
        _openedElementsView.InitMainView(_gameField, _elementsMerger, _elementsStorage, trainingMode: true);
        _openedElementsView.Fill(_initialElements);
        _alphabeticalIndex.Init();
        _clearButton.AddListener(OnClearButtonClick);
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

    public static void SetElementOnGameField(Element element)
    {
        if (element.Id.Equals("Fire"))
            _instance.ShowNextTask();
    }

    public static void SetElementCreated(Element firstElement, Element secondElement)
    {
        if (firstElement.Id.Equals("Fire") && secondElement.Id.Equals("Water")
            || firstElement.Id.Equals("Water") && secondElement.Id.Equals("Fire"))
            _instance.ShowNextTask();
    }

    public static void SetDoubleClick()
    {
        _instance.ShowNextTask();
    }

    private void OnClearButtonClick()
    {
        ShowNextTask();
        _clearButton.RemoveListener(OnClearButtonClick);
    }

    private void ShowNextTask()
    {
        if (_tasks.Length == _currentTask + 1)
        {
            SceneManager.LoadScene(MainSceneName);
            return;
        }

        _currentTask++;
        _taskText.text = _tasks[_currentTask].LocalizedText;

        if (_tasks[_currentTask].ObjectsToShow.Length > 0)
            foreach (var obj in _tasks[_currentTask].ObjectsToShow)
                obj.Activate();
    }

    private void OpenInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }

    [Serializable]
    private class TaskText
    {
        [SerializeField, TextArea(5, 10)] private string _textRu;
        [SerializeField, TextArea(5, 10)] private string _textEn;
        [SerializeField] private RectTransform[] _objectsToShow = { };

        public string LocalizedText => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _textRu : _textEn;
        public RectTransform[] ObjectsToShow => _objectsToShow;
    }
}
