using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class YanElementWindow : MonoBehaviour
{
    [SerializeField] private Image _elementImage;
    [SerializeField] private TMP_Text _elementLable;

    private const string ShowAnimation = "ShowYanElement";
    private Animator _animator;

    public void Show(Element element)
    {
        _elementImage.sprite = element.Sprite;
        _elementLable.text = element.Lable;
        _animator ??= GetComponent<Animator>();
        _animator.Play(ShowAnimation);
    }
}
