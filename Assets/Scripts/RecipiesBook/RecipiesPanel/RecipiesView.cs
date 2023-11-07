using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipiesView : MonoBehaviour
{
    [SerializeField] private RecipeRenderer _recipieRendererTemplate;
    [SerializeField] private RectTransform _recipiesWithElementContainer;
    [SerializeField] private RectTransform _creationRecipiesContainer;
    [SerializeField] private UIButton _closeButton;
    [SerializeField] private ScrollRect _scrollView;

    [SerializeField] private RectTransform _recipiesWithElementLable;
    [SerializeField] private RectTransform _creationRecipiesLable;

    private List<RecipeRenderer> _recipiesWithElementsRenderers = new();
    private List<RecipeRenderer> _creationRecipiesRenderers = new();

    private void Awake()
    {
        _closeButton.AssignOnClickAction(Close);
    }

    public void Fill(Element element)
    {
        element.SortRecipies();
        FillRecipiesWithElement(element);
        FillCreationRecipies(element);
        _scrollView.RerenderContent(Settings.CoroutineObject);
        _scrollView.RaiseContent();
        Metrics.SendEvent(MetricEvent.OpenRecipies);
    }

    private void FillRecipiesWithElement(Element element)
    {
        int i = 0;

        foreach (var recipie in element.Recipies)
        {
            if ((i + 1) > _recipiesWithElementsRenderers.Count)
                AddRecipieWithElement(element, recipie);
            else
                _recipiesWithElementsRenderers[i].Render(element, recipie);

            i++;
        }
        while (_recipiesWithElementsRenderers.Count > element.Recipies.Count)
        {
            Destroy(_recipiesWithElementsRenderers[i].gameObject);
            _recipiesWithElementsRenderers.RemoveAt(i);
        }
        _recipiesWithElementLable.gameObject.SetActive(_recipiesWithElementsRenderers.Count > 0);
    }

    private void FillCreationRecipies(Element element)
    {
        int i = 0;

        foreach (var recipie in element.CreationRecipies)
        {
            if ((i + 1) > _creationRecipiesRenderers.Count)
                AddCreationRecipie(element, recipie);
            else
                _creationRecipiesRenderers[i].RenderCreationRecipie(element, recipie);

            i++;
        }
        while (_creationRecipiesRenderers.Count > element.CreationRecipies.Count)
        {
            Destroy(_creationRecipiesRenderers[i].gameObject);
            _creationRecipiesRenderers.RemoveAt(i);
        }
        _creationRecipiesLable.gameObject.SetActive(_creationRecipiesRenderers.Count > 0);
    }

    private void AddRecipieWithElement(Element element, Recipe recipie)
    {
        var recipieRenderer = Instantiate(_recipieRendererTemplate, _recipiesWithElementContainer);
        recipieRenderer.Render(element, recipie);
        _recipiesWithElementsRenderers.Add(recipieRenderer);
    }

    private void AddCreationRecipie(Element element, CreationRecipie recipie)
    {
        var recipieRenderer = Instantiate(_recipieRendererTemplate, _creationRecipiesContainer);
        recipieRenderer.RenderCreationRecipie(element, recipie);
        _creationRecipiesRenderers.Add(recipieRenderer);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
