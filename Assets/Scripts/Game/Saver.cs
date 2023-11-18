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
    private const char SavesDevideSymbol = '0';
    private const char TrainingSavesDevideSymbol = '1';

    private static Saver _instance;

    private readonly bool _isPlayerAuthorized;
    private readonly ElementsStorage _elementsStorage;
    private readonly StringBuilder _saveDataBuilder = new();
    private Score _score;
    private bool _isTrainingMode;

    public bool IsReady { get; private set; } = false;
    public bool IsAdAllowed => _saveDataBuilder.ToString().Contains(StickyAdName) == false;
    public bool IsTerminalElementOpened => _saveDataBuilder.ToString().Contains(TerminalElementName);
    public bool IsTrainingCompleted { get; private set; } = false;

    private Saver(ElementsStorage elementsStorage, bool isPlayerAuthorized, Score score, bool isTrainingMode)
    {
        IsReady = false;
        _isTrainingMode = isTrainingMode;
        _isPlayerAuthorized = isPlayerAuthorized;
        _elementsStorage = elementsStorage;
        _score = score;

        _elementsStorage.ElementOpened += OnElementOpened;

        Load();
    }

    public static Saver Create(ElementsStorage elementsStorage, bool isPlayerAuthorizedge, Score score, bool isTrainingMode = false)
    {
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
        char devider = _isTrainingMode ? TrainingSavesDevideSymbol : SavesDevideSymbol;
        return _saveDataBuilder.ToString().Contains(element.Id + devider);
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
        if (IsTrainingCompleted == false)
        {
            IsTrainingCompleted = true;
            Save();
        }
    }

    public void ResetSaves()
    {
        _saveDataBuilder.Clear();
        _score.ResetCurrentScore();
        Save();
    }

    public void RemoveSaves()
    {
        _saveDataBuilder.Clear();
        _score.RemoveScore();
        IsTrainingCompleted = false;
        Save();
    }

    public void Load()
    {
        if (_isPlayerAuthorized)
            PlayerAccount.GetCloudSaveData(onSuccessCallback: (result) => SetLoadedData(result), onErrorCallback: (error) => Debug.Log("Saves load error: " + error));
        else
            SetLoadedData(PlayerPrefs.GetString(SavesStorage));
    }

    private void OnElementOpened(Element element)
    {
        if (this == null)
            return;

        if (IsElementOpened(element) == false)
        {
            if (_saveDataBuilder.ToString().Contains(element.Id + SavesDevideSymbol) == false)
                _score.AddScore(Settings.Elements.PointsForOpenedElement);

            _saveDataBuilder.Append(element.Id + SavesDevideSymbol);

            if (_isTrainingMode)
                _saveDataBuilder.Append(element.Id + TrainingSavesDevideSymbol);

            Save();
        }
    }

    private void Save()
    {
        var saves = new Saves();
        saves.Elements = _saveDataBuilder.ToString();
        saves.CurrentScore = _score.CurrentScore;
        saves.BestScore = _score.BestScore;
        saves.IsTrainingCompleted = IsTrainingCompleted;
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
        var saves = JsonUtility.FromJson<Saves>(jsonData);

        if (saves == null)
            Debug.Log("savesElements is null");
        else
            Debug.Log("SavedElements: " + saves.Elements);

        _saveDataBuilder.Append(saves.Elements);
        _score.Init(saves.BestScore, saves.CurrentScore);
        IsTrainingCompleted = saves.IsTrainingCompleted;
        Save();
        IsReady = true;
    }

    [Serializable]
    private class Saves
    {
        public string Elements;
        public int BestScore;
        public int CurrentScore;
        public bool IsTrainingCompleted;
    }
}
