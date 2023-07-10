using System.Text;
using UnityEngine;

public class Saver
{
    private const string SavesStorage = "Saves";

    private static Saver _instance;

    private ElementsStorage _elementsStorage;
    private StringBuilder _saveDataBuilder = new StringBuilder();

    private Saver(ElementsStorage elementsStorage)
    {
        Load();
        _elementsStorage = elementsStorage;

        foreach (var element in _elementsStorage.SortedElements)
            if (IsElementOpened(element) == false)
                element.Opened += OnElementOpened;
    }

    public static Saver Create(ElementsStorage elementsStorage)
    {
        if (_instance == null)
            _instance = new Saver(elementsStorage);

        return _instance;
    }

    public static Saver GetInstance()
    {
        if (_instance == null)
            throw new System.InvalidOperationException();

        return _instance;
    }

    public bool IsElementOpened(Element element)
    {
        return _saveDataBuilder.ToString().Contains(element.Id);
    }

    public void ResetSaves()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
#endif
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
#if UNITY_EDITOR
        PlayerPrefs.SetString(SavesStorage, _saveDataBuilder.ToString());
        PlayerPrefs.Save();
#endif
    }

    private void Load()
    {
#if UNITY_EDITOR
        string saves = PlayerPrefs.GetString(SavesStorage);

        if (string.IsNullOrEmpty(saves) == false)
            _saveDataBuilder.Append(saves);
#endif
    }
}
