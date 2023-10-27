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
        if (_isPlayerAuthorized)
        {
            PlayerAccount.SetCloudSaveData(_saveDataBuilder.ToString());
        }
        else
        {
            PlayerPrefs.SetString(SavesStorage, _saveDataBuilder.ToString());
            PlayerPrefs.Save();
        }
    }

    private void Load()
    {
        string saves = string.Empty;

        if (_isPlayerAuthorized)
            PlayerAccount.GetCloudSaveData(onSuccessCallback: (result) => saves = result, onErrorCallback: (error) => Debug.Log("Saves load error: " + error));
        else
            saves = PlayerPrefs.GetString(SavesStorage);

        if (string.IsNullOrEmpty(saves) == false)
            _saveDataBuilder.Append(saves);
    }
}
