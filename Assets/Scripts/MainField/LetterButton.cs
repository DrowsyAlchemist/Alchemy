using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _letterText;

    public event Action<char> Clicked;

    public char Letter { get; private set; }

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void Render(char letter)
    {
        Letter = letter;
        _letterText.text = letter.ToString();
    }

    private void OnButtonClick()
    {
        Clicked.Invoke(Letter);
    }
}
