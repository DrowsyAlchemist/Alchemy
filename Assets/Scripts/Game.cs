using System.Collections.Generic;
using UnityEngine;

public sealed class Game : MonoBehaviour, IMergeHandler
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private OpenedElementsView _openedElementsView;
    [SerializeField] private GameField _gameField;
    [SerializeField] private UIButton _resetButton;
    [SerializeField] private ProgressRenderer _progressRenderer;

    [SerializeField] private List<Element> _initialElements;

    private Saver _saver;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _saver = Saver.Create(_elementsStorage);
        OpenInitialElements();
        _elementsStorage.Init();
        _progressRenderer.Init(_elementsStorage);
        _openedElementsView.Init(_gameField, this);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
        _resetButton.Init(onButtonClick: ResetProgress);
    }

    private void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        _gameField.Clear();
        OpenInitialElements();
        _elementsStorage.Init();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    public void TryMergeElements(MergeableElementRenderer firstRenderer, MergeableElementRenderer secondRenderer)
    {
        var results = new List<Element>();

        foreach (var recipe in firstRenderer.Element.Recipes)
            if (recipe.SecondElement.Equals(secondRenderer.Element))
                results.Add(recipe.Result);

        Merge(firstRenderer, secondRenderer, results);
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
        element.Open();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    private void OpenInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }
}
