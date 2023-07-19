using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    private Action _onClick;
    protected Button Button { get; private set; }

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        Button?.RemoveListener(OnButtonClick);
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
