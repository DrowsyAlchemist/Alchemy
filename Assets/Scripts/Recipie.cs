using System;
using UnityEngine;

[Serializable]
public sealed class Recipe
{
    [SerializeField] private Element _secondElement;
    [SerializeField] private Element _result;

    public Element SecondElement => _secondElement;
    public Element Result => _result;

    public Recipe(Element secondElement, Element result)
    {
        _secondElement = secondElement;
        _result = result;
    }

    public override bool Equals(object obj)
    {
        var otherRecipe = obj as Recipe;

        if (obj == null)
            return false;

        if (otherRecipe.SecondElement.Equals(_secondElement) == false)
            return false;

        if (otherRecipe.Result.Equals(_result))
            return true;
        else
            throw new ArgumentException();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}