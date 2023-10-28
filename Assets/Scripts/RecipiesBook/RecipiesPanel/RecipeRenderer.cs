using System;
using UnityEngine;

public class RecipeRenderer : MonoBehaviour
{
    [SerializeField] private BookElementRenderer _firstElementRenderer;
    [SerializeField] private BookElementRenderer _secondElementRenderer;
    [SerializeField] private BookElementRenderer _resultElementRenderer;

    private bool _isInitialized;

    private void Init()
    {
        _secondElementRenderer.AssignClickHandler(new ElementForAdOpener());
        _resultElementRenderer.AssignClickHandler(new ElementForYanOpener());
        _isInitialized = true;
    }

    public void Render(Element element, Recipe recipe)
    {
        if (_isInitialized == false)
            Init();

        if (element.IsOpened)
            _firstElementRenderer.RenderOpened(element);
        else
            _firstElementRenderer.RenderCompletelyClosed(element);

        if (recipe.SecondElement.IsOpened && recipe.Result.IsOpened)
            _secondElementRenderer.RenderOpened(recipe.SecondElement);
        else if (recipe.SecondElement.IsOpened)
            _secondElementRenderer.RenderOpeningForAd(recipe.SecondElement);
        else
            _secondElementRenderer.RenderCompletelyClosed(recipe.SecondElement);

        if (recipe.Result.IsOpened)
            _resultElementRenderer.RenderOpened(recipe.Result);
        else if (recipe.SecondElement.IsOpened)
            _resultElementRenderer.RenderOpeningForYan(recipe.Result);
        else
            _resultElementRenderer.RenderCompletelyClosed(recipe.Result);
    }

    public void RenderCreationRecipie(Element element, CreationRecipie creationRecipie)
    {
        if (element.IsOpened == false)
            throw new InvalidOperationException();

        if (creationRecipie.FirstElement.IsOpened)
            _firstElementRenderer.RenderOpened(creationRecipie.FirstElement);
        else
            _firstElementRenderer.RenderCompletelyClosed(creationRecipie.FirstElement);

        if (creationRecipie.SecondElement.IsOpened)
            _secondElementRenderer.RenderOpened(creationRecipie.SecondElement);
        else
            _secondElementRenderer.RenderCompletelyClosed(creationRecipie.SecondElement);

        _resultElementRenderer.RenderOpened(element);
    }
}
