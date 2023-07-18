using System;
using UnityEngine;

[Serializable]
public sealed class Recipe
{
    [SerializeField] private Element _secondElement;
    [SerializeField] private Element _result;

    public Element SecondElement => _secondElement;
    public Element Result => _result;
    public int OpenedElementsCount
    {
        get
        {
            int i = 0;

            if (SecondElement.IsOpened)
                i++;

            if (Result.IsOpened)
                i++;

            return i;
        }
    }

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

        try
        {
            if (otherRecipe.SecondElement.Equals(_secondElement) == false)
                return false;
        }
        catch
        {
            var second = SecondElement?.Id ?? "null";
            var other = otherRecipe.SecondElement?.Id ?? "null";
            var result = Result?.Id ?? "null";
            throw new InvalidOperationException("Second: " + second + " OtherSecond: " + other + " Result: " + result);
        }

        return otherRecipe.Result.Equals(_result);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}