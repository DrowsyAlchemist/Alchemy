public sealed class RecipiesBook : IElementClickHandler
{
    private ElementsStorage _elementsStorage;
    private BookElementsView _gridView;
    private RecipiesWithElementView _recipiesWithElementView;

    public RecipiesBook(ElementsStorage elementsStorage, BookElementsView gridView, RecipiesWithElementView recipiesWithElementView)
    {
        _elementsStorage = elementsStorage;
        _gridView = gridView;
        _recipiesWithElementView = recipiesWithElementView;

        _gridView.Init(this);
        _gridView.gameObject.SetActive(false);
        _recipiesWithElementView.gameObject.SetActive(false);
    }

    public void Open()
    {
        _gridView.gameObject.SetActive(true);
        _gridView.Fill(_elementsStorage.SortedOpenedElements);
    }

    public void OnElementClick(ElementRenderer elementRenderer)
    {
        _recipiesWithElementView.Fill(elementRenderer.Element);
        _recipiesWithElementView.gameObject.SetActive(true);
    }
}
