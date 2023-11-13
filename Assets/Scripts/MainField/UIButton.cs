using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    private Action _onClick;
    private bool _isInitialized;

    protected Button Button { get; private set; }

    public void Init()
    {
        if (_isInitialized == false)
        {
            Button = GetComponent<Button>();
            Button.AddListener(OnButtonClick);
        }
    }

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        Button?.RemoveListener(OnButtonClick);
    }

    public void AssignOnClickAction(Action onButtonClick)
    {
        _onClick += onButtonClick;
    }

    private void OnButtonClick()
    {
        _onClick?.Invoke();
    }
}
