using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _letterText;

    public event Action<char> Clicked;

    public char Letter => _letterText.text[0];

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Clicked.Invoke(Letter);
    }
}
