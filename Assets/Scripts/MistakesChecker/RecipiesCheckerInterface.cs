using System.Collections.Generic;
using UnityEngine;

namespace RecipiesChecker
{
    public class RecipiesCheckerInterface : MonoBehaviour
    {
        [SerializeField] private List<Element> _elements;

        private void Awake()
        {
            Destroy(gameObject);
        }

        public void RemoveExcessRecipies()
        {
            var recipiesToRemove = new List<Recipe>();

            foreach (var element in _elements)
            {
                foreach (var recipieWithElement in element.Recipes)
                {
                    var createdElement = recipieWithElement.Result;

                    if (createdElement.HasCreationRecipie(element, recipieWithElement.SecondElement) == false)
                        recipiesToRemove.Add(recipieWithElement);
                }
                foreach (var recipieToRemove in recipiesToRemove)
                    element.RemoveRecipie(recipieToRemove);

                recipiesToRemove.Clear();
            }
        }

        public void AddRecipiesByCreationRecipieForAll()
        {
            foreach (var element in _elements)
                foreach (var creationRecipie in element.CreationRecipies)
                    AddRecipies(creationRecipie, result: element); ;
        }

        private void AddRecipies(CreationRecipie creationRecipie, Element result)
        {
            creationRecipie.FirstElement.CheckAndAddRecipe(creationRecipie.SecondElement, result);
            creationRecipie.SecondElement.CheckAndAddRecipe(creationRecipie.FirstElement, result);
        }
    }
}