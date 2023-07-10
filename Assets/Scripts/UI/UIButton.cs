using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private Action _onClick;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void Init(Action onButtonClick)
    {
        _onClick = onButtonClick;
    }

    private void OnButtonClick()
    {
        _onClick();
    }
}
