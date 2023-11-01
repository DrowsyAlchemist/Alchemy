using Agava.YandexGames;
using System;
using System.Text;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public class Saver
{
    private const string SavesStorage = "Saves";
    private const string StickyAdName = "StickyAd";
    private const char SavesDevideSymbol = '0';

    private static Saver _instance;

    private readonly bool _isPlayerAuthorized;
    private readonly ElementsStorage _elementsStorage;
    private readonly StringBuilder _saveDataBuilder = new();

    public bool IsReady { get; private set; } = false;
    public bool IsStickyAdAllowed => _saveDataBuilder.ToString().Contains(StickyAdName) == false;

    private Saver(ElementsStorage elementsStorage, bool isPlayerAuthorized)
    {
        IsReady = false;
        _isPlayerAuthorized = isPlayerAuthorized;
        _elementsStorage = elementsStorage;

        foreach (var element in _elementsStorage.SortedElements)
            element.Opened += OnElementOpened;

        Load();
    }

    public static Saver Create(ElementsStorage elementsStorage, bool isPlayerAuthorizedge)
    {
        if (_instance == null)
            _instance = new Saver(elementsStorage, isPlayerAuthorizedge);

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

    public void ResetSaves()
    {
        _saveDataBuilder.Clear();
        Save();
    }

    private void OnElementOpened(Element element)
    {
        if (IsElementOpened(element) == false)
        {
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

    private void Load()
    {
        if (_isPlayerAuthorized)
            PlayerAccount.GetCloudSaveData(onSuccessCallback: (result) => SetLoadedData(result), onErrorCallback: (error) => Debug.Log("Saves load error: " + error));
        else
            SetLoadedData(PlayerPrefs.GetString(SavesStorage));
    }

    private void SetLoadedData(string jsonData)
    {
        var savedElements = JsonUtility.FromJson<SavedElements>(jsonData);

        if (savedElements == null)
            Debug.Log("savedElements is null");
        else
            Debug.Log("Savedelements: " + savedElements.Elements);

        if (string.IsNullOrEmpty(jsonData) == false)
            _saveDataBuilder.Append(savedElements.Elements);
        else
            Debug.Log("jsonData is null or empty");

        IsReady = true;
    }

    [Serializable]
    private class SavedElements
    {
        public string Elements;
    }
}
