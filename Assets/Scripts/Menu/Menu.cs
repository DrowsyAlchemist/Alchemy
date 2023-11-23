using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private RectTransform _menuPanel;
    [SerializeField] private MainMenuPanel _mainMenuPanel;
    [SerializeField] private MenuButton[] _menuButtons;

    private MenuButton _currentButton;

    public MainMenuPanel MainMenuPanel => _mainMenuPanel;

    public void Init(GameInitialize gameInitialize)
    {
        _mainMenuPanel.Init(gameInitialize);

        foreach (var button in _menuButtons)
        {
            button.Clicked += OnButtonClick;
            button.ClosePanel();
        }
        OnButtonClick(_menuButtons[0]);
        Close();
    }

    public void Open()
    {
        _menuPanel.Activate();
    }

    public void OpenAchievements()
    {
        OnButtonClick(_menuButtons[2]);
        Open();
    }

    public void Close()
    {
        _menuPanel.Deactivate();
    }

    private void OnButtonClick(MenuButton menuButton)
    {
        _currentButton?.ClosePanel();
        menuButton.OpenPanel();
        _currentButton = menuButton;
    }
}
