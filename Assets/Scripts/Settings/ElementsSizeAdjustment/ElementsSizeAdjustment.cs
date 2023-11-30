using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementsSizeAdjustment : MonoBehaviour
{
    [SerializeField] private PcSizeConfig _pcSizeConfig;
    [SerializeField] private MobileSizeConfig _mobileSizeConfig;

    [SerializeField] private OpenedElementRenderer _openedElementRendererTemplate;
    [SerializeField] private MergeableElementRenderer _mergeableElementRendererTemplate;
    [SerializeField] private BookElementRenderer _bookElementRendererTemplate;
    [SerializeField] private GridLayoutGroup _recipeBookGrid;

    public void AdjustForMobile()
    {
        Adjust(_mobileSizeConfig);
    }

    public void AdjustForPc()
    {
        Adjust(_pcSizeConfig);
    }

    private void Adjust(IElementSizeConfig sizeConfig)
    {
        AdjustOpenedElement(sizeConfig);
        AdjustMegreableElement(sizeConfig);
        AdjustBookGrid(sizeConfig);
        AdjustFontSize(sizeConfig);
    }

    private void AdjustOpenedElement(IElementSizeConfig sizeConfig)
    {
        var rectTransform = _openedElementRendererTemplate.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeConfig.OpenedElementHeight);
    }

    private void AdjustMegreableElement(IElementSizeConfig sizeConfig)
    {
        var rectTransform = _mergeableElementRendererTemplate.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeConfig.MergeableElementSize.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeConfig.MergeableElementSize.y);
    }

    private void AdjustBookGrid(IElementSizeConfig sizeConfig)
    {
        _recipeBookGrid.cellSize = sizeConfig.BookCellSize;
    }

    private void AdjustFontSize(IElementSizeConfig sizeConfig)
    {
        _openedElementRendererTemplate.SetFontMaxSize(sizeConfig.MaxFontSize);
        _mergeableElementRendererTemplate.SetFontMaxSize(sizeConfig.MaxFontSize);
        _bookElementRendererTemplate.SetFontMaxSize(sizeConfig.MaxFontSize);
    }
}
