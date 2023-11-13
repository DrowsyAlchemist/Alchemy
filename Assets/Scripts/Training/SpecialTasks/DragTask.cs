public class DragTask : Task
{
    public void CheckDraggedElement(Element element)
    {
        if (element.Id.Equals("Fire"))
            Complete();
    }

    protected override void BeginTask()
    {
        
    }
}
