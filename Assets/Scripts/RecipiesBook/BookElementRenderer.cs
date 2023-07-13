using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookElementRenderer : ElementRenderer, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _highlightedImage;

    private IElementClickHandler _clickHandler;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
        _highlightedImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    public void Init(IElementClickHandler clickHandler)
    {
        _clickHandler = clickHandler;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlightedImage.gameObject.SetActive(false);
    }

    private void OnClick()
    {
        _clickHandler.OnBookTileElementClick(Element);
    }
}
