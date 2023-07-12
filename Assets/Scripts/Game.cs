using System.Collections.Generic;
using UnityEngine;

public sealed class Game : MonoBehaviour, IMergeHandler
{
    [SerializeField] private ElementsStorage _elementsStorage;
    [SerializeField] private MainOpenedElementsView _openedElementsView;
    [SerializeField] private GameField _gameField;
    [SerializeField] private UIButton _resetButton;
    [SerializeField] private UIButton _openRecipiesBookButton;
    [SerializeField] private ProgressRenderer _progressRenderer;

    [SerializeField] private BookElementsView _bookGridView;
    [SerializeField] private RecipiesWithElementView _recipiesWithElementView;

    [SerializeField] private List<Element> _initialElements;

    private Saver _saver;
    private RecipiesBook _recipiesBook;

    private void Start()
    {
        Init();
    }

    public void OpenAllElements()
    {
#if UNITY_EDITOR
        foreach (var element in _elementsStorage.SortedElements)
            element.Open();

        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
#endif
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
        _openRecipiesBookButton.Init(onButtonClick: OpenRecipiesBook);

        _recipiesBook = new RecipiesBook(_elementsStorage, _bookGridView, _recipiesWithElementView);
    }

    private void ResetProgress()
    {
        _saver.ResetSaves();
        _elementsStorage.ResetOpenedElements();
        OpenInitialElements();

        _gameField.Clear();
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }

    public void TryMergeElements(MergeableElementRenderer firstRenderer, MergeableElementRenderer secondRenderer)
    {
        var results = new List<Element>();

        foreach (var recipe in firstRenderer.Element.Recipies)
            if (recipe.SecondElement.Equals(secondRenderer.Element))
                results.Add(recipe.Result);

        Merge(firstRenderer, secondRenderer, results);
    }

    public void OpenRecipiesBook()
    {
        _recipiesBook.Open();
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
