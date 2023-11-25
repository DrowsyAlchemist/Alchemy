using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementRenderer : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _lableText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _fadeColor;

    private Achievement _achievement;

    private void OnDestroy()
    {
        if (_achievement != null)
            _achievement.Achieved -= OnAchieved;
    }

    public void Render(Achievement achievement)
    {
        _achievement = achievement ?? throw new System.ArgumentNullException();
        _iconImage.sprite = achievement.Icon;
        _lableText.text = achievement.Lable;
        _descriptionText.text = achievement.Description;

        if (achievement.IsAchieved)
        {
            UnfadeIcon();
        }
        else
        {
            FadeIcon();
            achievement.Achieved += OnAchieved;
        }
    }

    private void OnAchieved(Achievement _)
    {
        _achievement.Achieved -= OnAchieved;
        UnfadeIcon();
    }

    private void FadeIcon()
    {
        _iconImage.color = _fadeColor;
    }

    private void UnfadeIcon()
    {
        if (this != null)
        {
            _iconImage.color = _normalColor;
        }
    }
}
