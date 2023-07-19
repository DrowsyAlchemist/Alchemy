using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtentions
{
    public static void RaiseContent(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    public static void RerenderContent(this ScrollRect scrollRect, MonoBehaviour coroutineObject)
    {
        coroutineObject.StartCoroutine(Rerender(scrollRect));
    }

    private static IEnumerator Rerender(this ScrollRect scrollRect)
    {
        yield return new WaitForEndOfFrame();
        scrollRect.content.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        scrollRect.content.gameObject.SetActive(true);
    }
}
