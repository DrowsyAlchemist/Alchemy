using UnityEngine;

public class RecipieRenderer : MonoBehaviour
{
    [SerializeField] private BookElementRenderer _firstElementRenderer;
    [SerializeField] private BookElementRenderer _secondElementRenderer;
    [SerializeField] private BookElementRenderer _resultElementRenderer;

    public void Render(Element element, Recipe recipe)
    {
        if (element.IsOpened)
            _firstElementRenderer.Render(element);
        else
            _firstElementRenderer.Render(Game.ClosedElement);

        if (recipe.SecondElement.IsOpened && recipe.Result.IsOpened)
            _secondElementRenderer.Render(recipe.SecondElement);
        else
            _secondElementRenderer.Render(Game.ClosedElement);

        if (recipe.Result.IsOpened)
            _resultElementRenderer.Render(recipe.Result);
        else
            _resultElementRenderer.Render(Game.ClosedElement);
    }
}
