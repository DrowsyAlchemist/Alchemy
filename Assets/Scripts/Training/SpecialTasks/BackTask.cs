public class BackTask : ClickTask
{
    protected override void BeginTask()
    {
        TrainingPanel.Deactivate();
        base.BeginTask();
    }
}
