using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Element", order = 51)]
public sealed class Element : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _lable;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<CreationRecipie> _creationRecipies = new();
    [SerializeField] private List<Recipe> _recipesWithElement = new();

    public string Id => _id;
    public string Lable => _lable;
    public Sprite Sprite => _sprite;
    public IReadOnlyCollection<CreationRecipie> CreationRecipies => _creationRecipies;
    public IReadOnlyCollection<Recipe> Recipes => _recipesWithElement;
    public bool IsOpened => Saver.GetInstance().IsElementOpened(this);

    public event Action<Element> Opened;

    public void Open()
    {
        if (IsOpened == false)
            Opened?.Invoke(this);
    }

    public void CheckAndAddRecipe(Element secondElement, Element result)
    {
        if (secondElement == null)
            throw new ArgumentNullException(nameof(secondElement));

        if (result == null)
            throw new ArgumentNullException(nameof(result));

        var recipieToCheck = new Recipe(secondElement, result);

        if (HasRecipie(recipieToCheck) == false)
            _recipesWithElement.Add(recipieToCheck);
    }

    public void RemoveRecipie(Recipe recipeWithElement)
    {
        if (recipeWithElement == null)
            throw new ArgumentNullException(nameof(recipeWithElement));

        if (HasRecipie(recipeWithElement))
            _recipesWithElement.Remove(recipeWithElement);
    }

    public bool HasRecipie(Recipe recipieToCheck)
    {
        foreach (var elementRecipie in _recipesWithElement)
            if (elementRecipie.Equals(recipieToCheck))
                return true;

        return false;
    }

    public bool HasCreationRecipie(Element firstElement, Element secondElement)
    {
        var recipieToCheck = new CreationRecipie(firstElement, secondElement);

        foreach (var creationRecipie in _creationRecipies)
            if (creationRecipie.Equals(recipieToCheck))
                return true;

        return false;
    }
}
