public class MergeTask : Task
{
    public void CheckDraggedElement(Element firstElement, Element secondElement)
    {
        if (firstElement.Id.Equals("Fire") && secondElement.Id.Equals("Water")
            || firstElement.Id.Equals("Water") && secondElement.Id.Equals("Fire"))
            Complete();
    }

    protected override void BeginTask()
    {
    }
}
