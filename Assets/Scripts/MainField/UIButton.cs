using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    private Button _button;
    private Action _onClick;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void AssignOnClickAction(Action onButtonClick)
    {
        _onClick = onButtonClick;
    }

    private void OnButtonClick()
    {
        _onClick();
    }
}
