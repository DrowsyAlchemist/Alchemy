using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtentions
{
    public static void AddListener(this Button button, UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public static void RemoveListener(this Button button, UnityAction action)
    {
        button.onClick.RemoveListener(action);
    }
}
