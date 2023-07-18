public class ElementForAdOpener : IElementClickHandler
{
    private readonly AdShower _adShower;

    public ElementForAdOpener()
    {
        _adShower = new AdShower();
    }

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        _adShower.ShowVideo(onRewarded: () => OnRewarded(elementRenderer));
    }

    private void OnRewarded(BookElementRenderer elementRenderer)
    {
        elementRenderer.Render(elementRenderer.Element, isInteractable: false, isClosed: false);
    }
}
