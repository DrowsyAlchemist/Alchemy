using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] private UIButton _button;
    [SerializeField] private string _language;
    [SerializeField] private Image _highlightedImage;

    public string Language => _language;

    public event Action<string> Clicked;

    public void Init(string currentLang)
    {
        _button.Init();
        _button.AssignOnClickAction(() => Clicked?.Invoke(Language));
        SetHighlighted(currentLang.Equals(Language));
    }

    private void SetHighlighted(bool value)
    {
        _highlightedImage.SetActive(value);
    }
}
