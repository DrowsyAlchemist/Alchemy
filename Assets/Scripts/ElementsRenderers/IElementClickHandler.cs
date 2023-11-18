using System;

public interface IElementClickHandler
{
    public void OnElementClick(BookElementRenderer elementRenderer);

    public event Action ElementOpened;
}
