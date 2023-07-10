using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ElementRenderer : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _lableText;

    [SerializeField] private Element _element;

    public Element Element => _element;

    public void Render(Element element)
    {
        _element = element;
        _image.sprite = element.Sprite;
        _lableText.text = element.Lable;
    }
}
