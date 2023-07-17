public sealed class RecipiesBook : IElementClickHandler
{
    private ElementsStorage _elementsStorage;
    private BookElementsView _gridView;
    private RecipiesView _recipiesView;

    public RecipiesBook(ElementsStorage elementsStorage, BookElementsView gridView, RecipiesView recipiesView)
    {
        _elementsStorage = elementsStorage;
        _gridView = gridView;
        _recipiesView = recipiesView;

        _gridView.Init(this);
        _gridView.gameObject.SetActive(false);
        _recipiesView.gameObject.SetActive(false);
    }

    public void Open()
    {
        _gridView.gameObject.SetActive(true);
        _gridView.Fill(_elementsStorage.SortedOpenedElements);
    }

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        _recipiesView.Fill(elementRenderer.Element);
        _recipiesView.gameObject.SetActive(true);
    }
}
