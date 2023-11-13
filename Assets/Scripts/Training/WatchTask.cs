using UnityEngine;

public class WatchTask : ReadTask
{
    [SerializeField] private RectTransform _highlightedObject;

    protected override void BeginTask()
    {
        TrainingPanel.SetHighlightedObject(_highlightedObject);
        base.BeginTask();
    }

    protected override void OnComplete()
    {
        base.OnComplete();
    }
}
