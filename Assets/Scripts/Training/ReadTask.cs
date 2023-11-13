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
}
