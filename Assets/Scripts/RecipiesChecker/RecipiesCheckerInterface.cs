using System;
using System.Collections.Generic;
using UnityEngine;

namespace RecipiesChecker
{
    public sealed class RecipiesCheckerInterface : MonoBehaviour
    {
        [SerializeField] private ElementsStorage _elementsStorage;

        private void Awake()
        {
            Destroy(gameObject);
        }

        public void RemoveExcessRecipies()
        {
            var recipiesToRemove = new List<Recipe>();

            foreach (var element in _elementsStorage.SortedElements)
            {
                foreach (var recipieWithElement in element.Recipies)
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

        public void RemoveAllRecipies()
        {
            var recipiesToRemove = new List<Recipe>();

            foreach (var element in _elementsStorage.SortedElements)
            {
                foreach (var recipieWithElement in element.Recipies)
                    recipiesToRemove.Add(recipieWithElement);

                foreach (var recipieToRemove in recipiesToRemove)
                    element.RemoveRecipie(recipieToRemove);

                recipiesToRemove.Clear();
            }
        }

        public void AddRecipiesByCreationRecipieForAll()
        {
            foreach (var element in _elementsStorage.SortedElements)
                foreach (var creationRecipie in element.CreationRecipies)
                    AddRecipies(creationRecipie, result: element); ;
        }

        private void AddRecipies(CreationRecipie creationRecipie, Element result)
        {
            try
            {
                creationRecipie.FirstElement.CheckAndAddRecipe(creationRecipie.SecondElement, result);
                creationRecipie.SecondElement.CheckAndAddRecipe(creationRecipie.FirstElement, result);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Result: " + result.Id);
            }
        }
    }
}