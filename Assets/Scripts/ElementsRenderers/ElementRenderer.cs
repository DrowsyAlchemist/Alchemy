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

        _lableText.color = (Element.Recipies.Count == 0) ? Color.yellow : Color.white;
    }
}
