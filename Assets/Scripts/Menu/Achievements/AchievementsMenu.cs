using UnityEngine;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField] private Achievement[] _achievements;
    [SerializeField] private AchievementRenderer _achievementRendererTemplate;
    [SerializeField] private RectTransform _container;

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

    private void OnAchieved(int newScore)
    {
        if (newScore == 5)
            Metrics.SendEvent(MetricEvent.MakeFirstElement);
        else if (newScore == 50)
            Metrics.SendEvent(MetricEvent.MakeFiftyElements);
        else if (newScore == 100)
            Metrics.SendEvent(MetricEvent.MakeOneHundredElements);
        else if (newScore == 200)
            Metrics.SendEvent(MetricEvent.MakeTwoHundredElements);
        else if (newScore == 300)
            Metrics.SendEvent(MetricEvent.MakeThreeHundredElements);
        else if (newScore == 400)
            Metrics.SendEvent(MetricEvent.MakeFourHundredElements);
        else if (newScore == 500)
            Metrics.SendEvent(MetricEvent.MakeFiveHundredElements);
        else if (newScore == 600)
            Metrics.SendEvent(MetricEvent.MakeSixHundredElements);
    }
}
