using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookElementRenderer : ElementRenderer, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _highlightedImage;

    private IElementClickHandler _clickHandler;
    private bool _isInitialized;
    private bool _isInteractable;

    private void Init()
    {
        _highlightedImage.gameObject.SetActive(false);
        _button.onClick.AddListener(OnClick);
        _button.interactable = false;
    }

    private void OnDestroy()
    {
        if (_isInitialized)
            _button.onClick.RemoveListener(OnClick);
    }

    public void AssignClickHandler(IElementClickHandler clickHandler)
    {
        _clickHandler = clickHandler;
    }

    public void Render(Element element, bool isInteractable, bool isClosed = false)
    {
        if (_isInitialized == false)
            Init();

        if (isClosed)
            base.RenderClosed(element);
        else
            base.Render(element);

        if (isInteractable && _clickHandler == null)
            throw new InvalidOperationException("ClickHandler is not assigned");

        _isInteractable = isInteractable;
        _button.interactable = isInteractable;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_isInteractable)
            _highlightedImage.gameObject.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (_isInteractable)
            _highlightedImage.gameObject.SetActive(false);
    }

    private void OnClick()
    {
        _clickHandler.OnElementClick(this);
    }
}
