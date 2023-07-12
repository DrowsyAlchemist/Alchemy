using UnityEngine;
using UnityEngine.UI;

public class BookElementRenderer : ElementRenderer
{
    [SerializeField] private Button _button;

    private IElementClickHandler _clickHandler;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    public void Init(IElementClickHandler clickHandler)
    {
        _clickHandler = clickHandler;
    }

    private void OnClick()
    {
        _clickHandler.OnBookTileElementClick(Element);
    }
}
