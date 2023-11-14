using System.Collections;
using UnityEngine;

public class DragTask : Task
{
    [SerializeField] private MainOpenedElementsView _elementsView;

    public void CheckDraggedElement(Element element)
    {
        if (element.Id.Equals("Fire"))
            Complete();
        else
            SetHand(TrainingPanel.AnimatedHand);
    }

    protected override void BeginTask()
    {
    }

    protected override void SetHand(AnimatedHand animatedHand)
    {
        foreach (var renderer in _elementsView.ElementRenderers)
            if (renderer.Element.Id.Equals("Fire"))
                Settings.CoroutineObject.StartCoroutine(SetHandWithDelay(animatedHand, renderer));
    }

    private IEnumerator SetHandWithDelay(AnimatedHand animatedHand, ElementRenderer elementRenderer)
    {
        yield return new WaitForEndOfFrame();
        Rect rendererRect = elementRenderer.GetComponent<RectTransform>().rect;
        float yMargin = rendererRect.size.y / 2;
        float rendererScale = elementRenderer.transform.lossyScale.x;
        animatedHand.SetPosition((Vector2)elementRenderer.transform.position + Vector2.down * yMargin * rendererScale);
        animatedHand.PlayDragAndDrop();
    }
}
