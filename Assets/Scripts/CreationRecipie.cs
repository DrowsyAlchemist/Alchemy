using System;
using UnityEngine;

[Serializable]
public sealed class CreationRecipie
{
    [SerializeField] private Element _firstElement;
    [SerializeField] private Element _secondElement;

    public Element FirstElement => _firstElement;
    public Element SecondElement => _secondElement;
    public int OpenedElementsCount
    {
        get
        {
            int i = 0;

            if (FirstElement.IsOpened)
                i++;

            if (SecondElement.IsOpened)
                i++;

            return i;
        }
    }

    public CreationRecipie(Element firstElement, Element secondElement)
    {
        _firstElement = firstElement;
        _secondElement = secondElement;
    }

    public override bool Equals(object obj)
    {
        var otherRecipe = obj as CreationRecipie;

        if (obj == null)
            return false;

        if (otherRecipe.FirstElement.Equals(_firstElement) && otherRecipe.SecondElement.Equals(_secondElement))
            return true;

        if (otherRecipe.FirstElement.Equals(_secondElement) && otherRecipe.SecondElement.Equals(_firstElement))
            return true;

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}