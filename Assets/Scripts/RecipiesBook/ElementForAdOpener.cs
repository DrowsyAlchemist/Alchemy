public class ElementForAdOpener : IElementClickHandler
{
    private readonly AdShower _adShower;

    public ElementForAdOpener()
    {
        _adShower = new AdShower();
    }

    public void OnElementClick(ElementRenderer elementRenderer)
    {
        _adShower.ShowVideo(onRewarded: () => elementRenderer.Render(elementRenderer.Element));
    }
}
