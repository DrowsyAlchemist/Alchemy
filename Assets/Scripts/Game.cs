using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Game : MonoBehaviour, IMergeHandler
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private OpenedElementsView _openedElementsView;
    [SerializeField] private GameField _gameField;
    [SerializeField] private Button _resetButton;

    [SerializeField] private List<Element> _initialElements;

    private void Start()
    {
        OpedInitialElements();
        _resetButton.onClick.AddListener(ResetProgress);
        _openedElementsView.Init(_gameField, this);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    private void OnDestroy()
    {
        _resetButton.onClick.RemoveListener(ResetProgress);
    }

    private void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        _gameField.Clear();
        OpedInitialElements();
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
        _openedElementsView.Rerender(_elementsStorage.SortedOpenedElements);
    }

    private void OpedInitialElements()
    {
        foreach (var element in _initialElements)
            element.Open();
    }
}
