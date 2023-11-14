using UnityEngine;

public class BackTask : ClickTask
{
    [SerializeField] private float _handXPosition = 160;

    protected override void BeginTask()
    {
        TrainingPanel.NotePanel.Deactivate();
        base.BeginTask();
    }

    protected override void SetHand(AnimatedHand animatedHand)
    {
        animatedHand.SetPosition(new Vector2(_handXPosition, Screen.height / 2));
        animatedHand.PlayTap();
    }

    protected override void OnComplete()
    {
        TrainingPanel.NotePanel.Activate();
        base.OnComplete();
    }
}
