using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ElementsMerger : IMergeHandler
{
    private MainOpenedElementsView _openedElementsView;
    private Score _score;
    private ElementsStorage _elementsStorage;

    public ElementsMerger(MainOpenedElementsView openedElementsView, Score score, ElementsStorage elementsStorage)
    {
        _openedElementsView = openedElementsView;
        _score = score;
        _elementsStorage = elementsStorage;
    }

    public void TryMergeElements(MergeableElementRenderer firstRenderer, MergeableElementRenderer secondRenderer)
    {
        var results = new List<Element>();

        foreach (var recipe in firstRenderer.Element.Recipies)
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
                var newRenderer = Object.Instantiate(firstRenderer, firstRenderer.transform.position, Quaternion.identity, firstRenderer.transform.parent);
                newRenderer.Render(results[i]);
            }
        }
        if (results.Count == 1)
            Object.Destroy(secondRenderer.gameObject);
    }

    private void OpenNewElement(Element element)
    {
        element.Open();
        _score.AddScore(Settings.GameSettings.PointsForOpenedElement);
        _openedElementsView.Fill(_elementsStorage.SortedOpenedElements);
    }
}
