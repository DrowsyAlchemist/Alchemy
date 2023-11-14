using UnityEngine;

public class ReadTask : Task
{
    protected override void BeginTask()
    {
        TrainingPanel.SetContinueButtonActive(true);
        TrainingPanel.SetCancelButtonActive(false);
        TrainingPanel.SetGameInteractable(false);
        TrainingPanel.ContinueButtonClicked += Complete;
    }

    protected override void OnComplete()
    {
        TrainingPanel.ContinueButtonClicked -= Complete;
        base.OnComplete();
    }

    protected override void SetHand(AnimatedHand animatedHand)
    {
        animatedHand.SetPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        animatedHand.PlayTap();
    }
}
