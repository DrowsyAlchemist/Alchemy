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

    public event Action ElementOpened;

    private void Init()
    {
        _highlightedImage.gameObject.SetActive(false);
        _button.onClick.AddListener(OnClick);
        _button.Deactivate();
        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_isInitialized)
            _button.onClick.RemoveListener(OnClick);
    }

    public void AssignClickHandler(IElementClickHandler clickHandler)
    {
        _clickHandler = clickHandler;
        _clickHandler.ElementOpened += () => ElementOpened?.Invoke();
    }

    public void RenderOpened(Element element)
    {
        if (_isInitialized == false)
            Init();

        base.Render(element);
        _isInteractable = false;
        _button.Deactivate();
    }

    public void RenderCompletelyClosed(Element element)
    {
        if (_isInitialized == false)
            Init();

        base.RenderClosed(element);
        _isInteractable = false;
        _button.Deactivate();
    }

    public void RenderInteractable(Element element)
    {
        if (_isInitialized == false)
            Init();

        if (_clickHandler == null)
            throw new InvalidOperationException("ClickHandler is not assigned");

        base.Render(element);
        _isInteractable = true;
        _button.Activate();
    }

    public void RenderOpeningForAd(Element element)
    {
        if (_isInitialized == false)
            Init();

        if (_clickHandler == null)
            throw new InvalidOperationException("ClickHandler is not assigned");

        base.RenderManual(element, Settings.MonetizationSettings.AdSprite, Settings.MonetizationSettings.AdLable);
        _isInteractable = true;
        _button.Activate();
    }

    public void RenderOpeningForYan(Element element)
    {
        if (_isInitialized == false)
            Init();

        if (_clickHandler == null)
            throw new InvalidOperationException("ClickHandler is not assigned");

        base.RenderManual(element, Settings.MonetizationSettings.YanSprite, Settings.MonetizationSettings.YanLable);
        _isInteractable = true;
        _button.Activate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isInteractable)
            _highlightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isInteractable)
            _highlightedImage.gameObject.SetActive(false);
    }

    private void OnClick()
    {
        Sound.PlayClick();
        _clickHandler.OnElementClick(this);
        _highlightedImage.gameObject.SetActive(false);
    }
}
