using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ElementsMerger : IMergeHandler
{
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
        Action playSound = null;

        for (var i = 0; i < results.Count; i++)
        {
            if (results[i].IsOpened == false)
            {
                results[i].Open();
                playSound = Sound.PlayOpenElement;
            }
            if (i == 0)
            {
                firstRenderer.Render(results[0]);
                playSound ??= Sound.PlayCreate;
            }
            if (i == 1)
                secondRenderer.Render(results[1]);

            if (i > 1)
            {
                var newRenderer = Object.Instantiate(firstRenderer, firstRenderer.transform.position, Quaternion.identity, firstRenderer.transform.parent);
                newRenderer.Render(results[i]);
            }
        }
        playSound?.Invoke();

        if (results.Count == 1)
            Object.Destroy(secondRenderer.gameObject);
    }
}
