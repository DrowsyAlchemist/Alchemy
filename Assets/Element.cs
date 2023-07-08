using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Element", order = 51)]
public class Element : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private List<Recipe> _recipes;

    public string Id => _id;
    public string Name => _name;
    public IReadOnlyCollection<Recipe> Recipes => _recipes;


    [Serializable]
    public class Recipe
    {
        public List<Element> Elements;
    }
}
