using UnityEngine;

public class Game : MonoBehaviour
{
    private void OnMerge(Element firstElement, Element secondElement)
    {
        foreach (var recipe in firstElement.Recipes)
            if (recipe.SecondElement.Equals(secondElement))
                Debug.Log("Result: " + recipe.Result);
    }
}
