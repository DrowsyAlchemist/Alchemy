using System.Collections;
using UnityEngine;

public class RecipieRenderer : MonoBehaviour
{
    [SerializeField] private BookElementRenderer _firstElementRenderer;
    [SerializeField] private BookElementRenderer _secondElementRenderer;
    [SerializeField] private BookElementRenderer _resultElementRenderer;

    public void Render(Element element, Recipe recipe)
    {
        _firstElementRenderer.Render(element);
        _secondElementRenderer.Render(recipe.SecondElement);
        _resultElementRenderer.Render(recipe.Result);
    }
}
