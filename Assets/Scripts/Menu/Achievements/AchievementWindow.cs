using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AchievementWindow : MonoBehaviour
{
    [SerializeField] private AchievementRenderer _achievementRenderer;

    private const string ShowAnimation = "Show";
    private Animator _animator;
    private AchievementsMenu _achievementsMenu;

    public void Init(AchievementsMenu achievementsMenu)
    {
        _achievementsMenu = achievementsMenu ?? throw new ArgumentNullException();
        _achievementsMenu.Achieved += OnAchieved;
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        _achievementsMenu.Achieved -= OnAchieved;
    }

    private void OnAchieved(Achievement achievement)
    {
        _achievementRenderer.Render(achievement);
        _animator.Play(ShowAnimation);
    }
}
