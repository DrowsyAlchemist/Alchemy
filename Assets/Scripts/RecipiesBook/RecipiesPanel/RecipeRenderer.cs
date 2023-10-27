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
            _secondElementRenderer.RenderOpened(element);
        else if (recipe.SecondElement.IsOpened)
            _secondElementRenderer.RenderInteractable(element);
        else
            _secondElementRenderer.RenderCompletelyClosed(element);

        if (recipe.Result.IsOpened)
            _resultElementRenderer.RenderOpened(element);
        else
            _resultElementRenderer.RenderCompletelyClosed(element);
    }

    public void RenderCreationRecipie(Element element, CreationRecipie creationRecipie)
    {
        if (element.IsOpened == false)
            throw new InvalidOperationException();

        if (creationRecipie.FirstElement.IsOpened)
        {
            _firstElementRenderer.RenderOpened(creationRecipie.FirstElement);
            _secondElementRenderer.RenderOpened(creationRecipie.SecondElement);
        }
        else
        {
            _firstElementRenderer.RenderCompletelyClosed(creationRecipie.FirstElement);
            _secondElementRenderer.RenderCompletelyClosed(creationRecipie.SecondElement);
        }
        _resultElementRenderer.RenderOpened(element);
    }
}
