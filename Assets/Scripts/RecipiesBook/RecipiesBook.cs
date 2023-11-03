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

        _gridView.InitBookView(this, elementsStorage);
        _gridView.Deactivate();
        _recipiesView.Deactivate();
    }

    public void Open()
    {
        _gridView.Activate();
        _gridView.Fill(_elementsStorage.SortedOpenedElements);
    }

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
        _recipiesView.Fill(elementRenderer.Element);
        _recipiesView.Activate();
    }
}
