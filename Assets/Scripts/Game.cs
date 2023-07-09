using UnityEngine;

public sealed class Game : MonoBehaviour, IMergeHandler
{
    public void TryMergeElements(ElementRenderer firstRenderer, ElementRenderer secondRenderer)
    {
        foreach (var recipe in firstRenderer.Element.Recipes)
            if (recipe.SecondElement.Equals(secondRenderer.Element))
                Merge(firstRenderer, secondRenderer, recipe.Result);
    }

    private void Merge(ElementRenderer firstRenderer, ElementRenderer secondRenderer, Element result)
    {
        var resultRenderer = Instantiate(firstRenderer, firstRenderer.transform.position, Quaternion.identity, firstRenderer.transform.parent);
        resultRenderer.Render(result, this);
        Destroy(firstRenderer.gameObject);
        Destroy(secondRenderer.gameObject);
    }
}
