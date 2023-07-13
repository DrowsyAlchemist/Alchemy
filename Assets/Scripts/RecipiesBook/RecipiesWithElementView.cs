using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipiesWithElementView : MonoBehaviour
{
    [SerializeField] private RecipieRenderer _recipieRendererTemplate;
    [SerializeField] private RectTransform _container;
    [SerializeField] private UIButton _closeButton;
    [SerializeField] private ScrollRect _scrollView;

    private List<RecipieRenderer> _recipieRenderers = new();

    private void Awake()
    {
        _closeButton.Init(Close);
    }

    private void OnEnable()
    {
        _scrollView.normalizedPosition = new Vector2(0, 1);
    }

    public void Fill(Element element)
    {
        int i = 0;

        foreach (var recipie in element.Recipies)
        {
            if ((i + 1) > _recipieRenderers.Count)
                AddRecipie(element, recipie);
            else
                _recipieRenderers[i].Render(element, recipie);

            i++;
        }
        while (_recipieRenderers.Count > element.Recipies.Count)
        {
            Destroy(_recipieRenderers[i].gameObject);
            _recipieRenderers.RemoveAt(i);
        }
    }

    private void AddRecipie(Element element, Recipe recipie)
    {
        var recipieRenderer = Instantiate(_recipieRendererTemplate, _container);
        recipieRenderer.Render(element, recipie);
        _recipieRenderers.Add(recipieRenderer);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
