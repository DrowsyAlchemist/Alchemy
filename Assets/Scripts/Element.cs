using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Element", order = 51)]
public class Element : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<Recipe> _recipes;

    public string Id => _id;
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public IReadOnlyCollection<Recipe> Recipes => _recipes;

    public void AddRecipe(Recipe recipe)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));

        _recipes.Add(recipe);
    }

    [Serializable]
    public class Recipe
    {
        public Element SecondElement;
        public Element Result;
    }
}
