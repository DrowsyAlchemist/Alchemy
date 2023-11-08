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
        if (achievement.ScoreRequired == 5)
            Metrics.SendEvent(MetricEvent.MakeFirstElement);
        else if (achievement.ScoreRequired == 50)
            Metrics.SendEvent(MetricEvent.MakeFiftyElements);
        else if (achievement.ScoreRequired == 100)
            Metrics.SendEvent(MetricEvent.MakeOneHundredElements);
        else if (achievement.ScoreRequired == 200)
            Metrics.SendEvent(MetricEvent.MakeTwoHundredElements);
        else if (achievement.ScoreRequired == 300)
            Metrics.SendEvent(MetricEvent.MakeThreeHundredElements);
        else if (achievement.ScoreRequired == 400)
            Metrics.SendEvent(MetricEvent.MakeFourHundredElements);
        else if (achievement.ScoreRequired == 500)
            Metrics.SendEvent(MetricEvent.MakeFiveHundredElements);
        else if (achievement.ScoreRequired == 600)
            Metrics.SendEvent(MetricEvent.MakeSixHundredElements);

        Achieved?.Invoke(achievement);
    }
}
