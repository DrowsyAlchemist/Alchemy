using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private RectTransform _menuPanel;
    [SerializeField] private MenuButton[] _menuButtons;

    private MenuButton _currentButton;

    private void Awake()
    {
        foreach (var button in _menuButtons)
        {
            button.Clicked += OnButtonClick;
            button.ClosePanel();
        }
        Close();
    }

    private void OnEnable()
    {
        OnButtonClick(_menuButtons[0]);
    }

    public void Open()
    {
        _menuPanel.gameObject.SetActive(true);
    }

    public void Close()
    {
        _menuPanel.gameObject.SetActive(false);
    }

    private void OnButtonClick(MenuButton menuButton)
    {
        _currentButton?.ClosePanel();
        menuButton.OpenPanel();
        _currentButton = menuButton;
    }
}
