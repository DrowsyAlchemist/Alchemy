using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementRenderer : MonoBehaviour, IHasElement
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _lableText;

    public Element Element { get; private set; }

    public void Render(Element element)
    {
        Element = element;
        _image.sprite = element.Sprite;
        _lableText.text = element.Lable;
        _lableText.color = HasRecipies() ? Settings.Elements.DefaultElementColor : Settings.Elements.ElementWithoutRecipiesColor;
    }

    public void SetFontMaxSize(float maxSize)
    {
        _lableText.fontSizeMax = maxSize;
    }

    protected void RenderClosed(Element element)
    {
        Element = element;
        _image.sprite = Settings.Elements.ClosedElement.Sprite;
        _lableText.text = Settings.Elements.ClosedElement.Lable;
        _lableText.color = Settings.Elements.DefaultElementColor;
    }

    protected void RenderManual(Element element, Sprite sprite, string lable)
    {
        Element = element;
        _image.sprite = sprite;
        _lableText.text = lable;
        _lableText.color = Settings.Elements.DefaultElementColor;
    }

    private bool HasRecipies()
    {
        return Element.Recipies.Count > 0;
    }
}
