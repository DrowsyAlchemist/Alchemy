using UnityEngine;

public class MergeTask : Task
{
    [SerializeField] private MainOpenedElementsView _elementsView;

    public void CheckDraggedElement(Element firstElement, Element secondElement)
    {
        if (firstElement.Id.Equals("Fire") && secondElement.Id.Equals("Water")
            || firstElement.Id.Equals("Water") && secondElement.Id.Equals("Fire"))
            Complete();
    }

    protected override void BeginTask()
    {
    }
    
    protected override void SetHand(AnimatedHand animatedHand)
    {
        foreach (var renderer in _elementsView.ElementRenderers)
            if (renderer.Element.Id.Equals("Water"))
                SetHandAtRendererCentre(animatedHand, renderer);

        animatedHand.PlayDragAndDrop();
    }

    private void SetHandAtRendererCentre(AnimatedHand animatedHand, ElementRenderer elementRenderer)
    {
        float rendererScale = elementRenderer.transform.lossyScale.x;
        Rect rendererRect = elementRenderer.GetComponent<RectTransform>().rect;
        // Vector3 margin = rendererScale * new Vector2(rendererRect.size.x / 2, rendererRect.size.y / 2);
        animatedHand.SetPosition((Vector2)elementRenderer.transform.position);
    }
}
