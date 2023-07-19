using System;
using UnityEngine;

public class MenuButton : UIButton
{
    [SerializeField] private RectTransform _openedPanel;

    private bool _isInitialized;

    public Action<MenuButton> Clicked;

    private void Init()
    {
        base.AssignOnClickAction(OnClick);
        _isInitialized = true;
    }

    public void OpenPanel()
    {
        if (_isInitialized == false)
            Init();

        Button.interactable = false;
        _openedPanel.Activate();
    }

    public void ClosePanel()
    {
        if (_isInitialized == false)
            Init();

        Button.interactable = true;
        _openedPanel.Deactivate();
    }

    private void OnClick()
    {
        Clicked?.Invoke(this);
    }
}