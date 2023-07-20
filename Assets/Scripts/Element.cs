using Lean.Localization;
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
    public string Lable => GetLocalizedLable();
    public Sprite Sprite => _sprite;
    public IReadOnlyCollection<CreationRecipie> CreationRecipies => _creationRecipies;
    public IReadOnlyCollection<Recipe> Recipies => _recipesWithElement;
    public bool IsOpened => Saver.GetInstance().IsElementOpened(this);

    public event Action<Element> Opened;

    public void Open()
    {
        if (IsOpened == false)
            Opened?.Invoke(this);
    }

    public void SortRecipies()
    {
        _creationRecipies.Sort((a, b) => b.OpenedElementsCount - a.OpenedElementsCount);
        _recipesWithElement.Sort((a, b) => b.OpenedElementsCount - a.OpenedElementsCount);
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

    private string GetLocalizedLable()
    {
        switch (LeanLocalization.GetFirstCurrentLanguage())
        {
            case "en":
                return _id;
            case "ru":
                return _lable;
            case null:
                return _lable;
            default:
                Debug.Log(LeanLocalization.GetFirstCurrentLanguage());
                throw new NotImplementedException();
        }
    }
}
