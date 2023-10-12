using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ElementRenderer : MonoBehaviour, IHasElement
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _lableText;

    public Element Element { get; private set; }

    public void Render(Element element)
    {
        Element = element;
        _image.sprite = element.Sprite;
        _lableText.text = element.Lable;
        _lableText.color = HasLockedRecipies(element) ? Color.white : Color.yellow;
    }

    protected void RenderClosed(Element element)
    {
        Element = element;
        _image.sprite = Settings.ClosedElement.Sprite;
        _lableText.text = Settings.ClosedElement.Lable;
        _lableText.color = Color.white;
    }

    protected void RenderManual(Element element, Sprite sprite, string lable)
    {
        Element = element;
        _image.sprite = sprite;
        _lableText.text = lable;
        _lableText.color = Color.white;
    }

    private bool HasLockedRecipies(Element element)
    {
        foreach (var recipie in Element.Recipies)
            if (recipie.Result.IsOpened == false)
                return true;

        return false;
    }
}
