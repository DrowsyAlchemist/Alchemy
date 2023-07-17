using UnityEngine;

public class RecipieRenderer : MonoBehaviour
{
    [SerializeField] private BookElementRenderer _firstElementRenderer;
    [SerializeField] private BookElementRenderer _secondElementRenderer;
    [SerializeField] private BookElementRenderer _resultElementRenderer;

    private bool _isInitialized;

    private void Init()
    {
        _secondElementRenderer.AssignClickHandler(new ElementForAdOpener());
        _isInitialized = true;
    }

    public void Render(Element element, Recipe recipe)
    {
        if (_isInitialized == false)
            Init();

        if (element.IsOpened)
            _firstElementRenderer.Render(element, isInteractable: false, isClosed: false);
        else
            _firstElementRenderer.Render(element, isInteractable: false, isClosed: true);

        if (recipe.SecondElement.IsOpened && recipe.Result.IsOpened)
            _secondElementRenderer.Render(recipe.SecondElement, isInteractable: false, isClosed: false);
        else if (recipe.SecondElement.IsOpened)
            _secondElementRenderer.Render(recipe.SecondElement, isInteractable: true, isClosed: true);
        else
            _secondElementRenderer.Render(recipe.SecondElement, isInteractable: false, isClosed: true);

        if (recipe.Result.IsOpened)
            _resultElementRenderer.Render(recipe.Result, isInteractable: false, isClosed: false);
        else
            _resultElementRenderer.Render(Game.ClosedElement, isInteractable: false, isClosed: true);
    }
}
