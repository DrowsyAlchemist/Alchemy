using System;
using UnityEngine;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField] private Achievement[] _achievements;
    [SerializeField] private AchievementRenderer _achievementRendererTemplate;
    [SerializeField] private RectTransform _container;

    public event Action<Achievement> Achieved;     

    private void Start()
    {
        Metrics.SendEvent(MetricEvent.OpenAchievements);
    }

    public void Init(Score score)
    {
        foreach (var achievement in _achievements)
        {
            achievement.Init(score);
            achievement.Achieved += OnAchieved;
            var achievementRenderer = Instantiate(_achievementRendererTemplate, _container);
            achievementRenderer.Render(achievement);
        }
    }

    private void OnDestroy()
    {
        foreach (var achievement in _achievements)
            achievement.Achieved -= OnAchieved;
    }

    private void OnAchieved(Achievement achievement)
    {
        Achieved?.Invoke(achievement);
    }
}
