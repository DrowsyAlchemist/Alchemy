using UnityEngine;

public static class ComponentExtentions
{
    public static void Activate(this Component component)
    {
        component.gameObject.SetActive(true);
    }

    public static void Deactivate(this Component component)
    {
        component.gameObject.SetActive(false);
    }
}
