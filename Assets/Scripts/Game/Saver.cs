using Agava.YandexGames;
using System;
using System.Text;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public class Saver
{
    private const string SavesStorage = "Saves";
    private const string StickyAdName = "StickyAd";
    private const string TerminalElementName = "Terminal1";
    private const string TrainingCompletedName = "TrainingCompleted1";
    private const char SavesDevideSymbol = '0';

    private static Saver _instance;

    private readonly bool _isPlayerAuthorized;
    private readonly ElementsStorage _elementsStorage;
    private readonly StringBuilder _saveDataBuilder = new();
    private Score _score;
    private bool _isTrainingMode;

    public bool IsReady { get; private set; } = false;
    public bool IsAdAllowed => _saveDataBuilder.ToString().Contains(StickyAdName) == false;
    public bool IsTerminalElementOpened => _saveDataBuilder.ToString().Contains(TerminalElementName);
    public bool IsTrainingCompleted => _saveDataBuilder.ToString().Contains(TrainingCompletedName) == false;

    private Saver(ElementsStorage elementsStorage, bool isPlayerAuthorized, Score score, bool isTrainingMode)
    {
        IsReady = false;
        _isTrainingMode = isTrainingMode;
        _isPlayerAuthorized = isPlayerAuthorized;
        _elementsStorage = elementsStorage;
        _score = score;

        foreach (var element in _elementsStorage.SortedElements)
            element.Opened += OnElementOpened;

        Load();
    }

    public static Saver Create(ElementsStorage elementsStorage, bool isPlayerAuthorizedge, Score score, bool isTrainingMode = false)
    {
        if (_instance == null)
            _instance = new Saver(elementsStorage, isPlayerAuthorizedge, score, isTrainingMode);

        return _instance;
    }

    public static Saver GetInstance()
    {
        if (_instance == null)
            throw new InvalidOperationException();

        return _instance;
    }

    public bool IsElementOpened(Element element)
    {
        return _saveDataBuilder.ToString().Contains(element.Id + SavesDevideSymbol);
    }

    public void OffAd()
    {
        _saveDataBuilder.Append(StickyAdName);
        Save();
    }

    public void OpenTerminateElement()
    {
        _saveDataBuilder.Append(TerminalElementName);
        Save();
    }

    public void SetTrainingCompleted()
    {
        _saveDataBuilder.Append(TrainingCompletedName);
        Save();
    }

    public void ResetSaves()
    {
        _saveDataBuilder.Clear();
        Save();
    }

    public void Load()
    {
        if (_isTrainingMode)
        {
            IsReady = true;
            return;
        }
        if (_isPlayerAuthorized)
            PlayerAccount.GetCloudSaveData(onSuccessCallback: (result) => SetLoadedData(result), onErrorCallback: (error) => Debug.Log("Saves load error: " + error));
        else
            SetLoadedData(PlayerPrefs.GetString(SavesStorage));
    }

    private void OnElementOpened(Element element)
    {
        if (IsElementOpened(element) == false)
        {
            _score.AddScore(Settings.Elements.PointsForOpenedElement);
            _saveDataBuilder.Append(element.Id + SavesDevideSymbol);
            Save();
        }
    }

    private void Save()
    {
        var saves = new SavedElements();
        saves.Elements = _saveDataBuilder.ToString();
        string jsonData = JsonUtility.ToJson(saves);

        if (_isPlayerAuthorized)
        {
            PlayerAccount.SetCloudSaveData(jsonData);
            Debug.Log("Saved: " + jsonData);
        }
        else
        {
            PlayerPrefs.SetString(SavesStorage, jsonData);
            PlayerPrefs.Save();
        }
    }

    private void SetLoadedData(string jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.Log("jsonData is null or empty");
            IsReady = true;
            return;
        }
        var savedElements = JsonUtility.FromJson<SavedElements>(jsonData);

        if (savedElements == null)
            Debug.Log("savedElements is null");
        else
            Debug.Log("Savedelements: " + savedElements.Elements);

        _saveDataBuilder.Append(savedElements.Elements);
        Save();
        IsReady = true;
    }

    [Serializable]
    private class SavedElements
    {
        public string Elements;
    }
}
