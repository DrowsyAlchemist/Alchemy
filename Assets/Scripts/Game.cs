using System.Collections.Generic;
using UnityEngine;

public sealed class Game : MonoBehaviour, IMergeHandler
{
    public void TryMergeElements(ElementRenderer firstRenderer, ElementRenderer secondRenderer)
    {
        var results = new List<Element>();

        foreach (var recipe in firstRenderer.Element.Recipes)
            if (recipe.SecondElement.Equals(secondRenderer.Element))
                results.Add(recipe.Result);

        Merge(firstRenderer, secondRenderer, results);
    }

    private void Merge(ElementRenderer firstRenderer, ElementRenderer secondRenderer, List<Element> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
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
            Destroy(secondRenderer);
    }
}
