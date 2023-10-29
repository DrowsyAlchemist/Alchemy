using Agava.YandexGames;
using System;
using System.Text;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public class Saver
{
    private const string SavesStorage = "Saves";

    private static Saver _instance;

    private readonly bool _isPlayerAuthorized;
    private readonly ElementsStorage _elementsStorage;
    private readonly StringBuilder _saveDataBuilder = new();

    private Saver(ElementsStorage elementsStorage, bool isPlayerAuthorized)
    {
        _isPlayerAuthorized = isPlayerAuthorized;
        _elementsStorage = elementsStorage;
        Load();

        foreach (var element in _elementsStorage.SortedElements)
            element.Opened += OnElementOpened;
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
        return _saveDataBuilder.ToString().Contains(element.Id);
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
            _saveDataBuilder.Append(element.Id);
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
        }
        else
        {
            PlayerPrefs.SetString(SavesStorage, jsonData);
            PlayerPrefs.Save();
        }
    }

    private void Load()
    {
        string jsonSaves = string.Empty;

        if (_isPlayerAuthorized)
            PlayerAccount.GetCloudSaveData(onSuccessCallback: (result) => jsonSaves = result, onErrorCallback: (error) => Debug.Log("Saves load error: " + error));
        else
            jsonSaves = PlayerPrefs.GetString(SavesStorage);

        try
        {

            var savedElements = JsonUtility.FromJson<SavedElements>(jsonSaves);

            if (savedElements == null)
                Debug.Log("savedElements is null");
            else
                Debug.Log("Savedelements: " + savedElements.Elements);

            if (string.IsNullOrEmpty(jsonSaves) == false)
                _saveDataBuilder.Append(savedElements.Elements);
            else
                Debug.Log("jsonData is null or empty");
        }
        catch (Exception e)
        {
            Debug.Log("CATCHED. Exeption: " + e.Message + e.StackTrace);
        }
    }

    [Serializable]
    private class SavedElements
    {
        public string Elements;
    }
}
