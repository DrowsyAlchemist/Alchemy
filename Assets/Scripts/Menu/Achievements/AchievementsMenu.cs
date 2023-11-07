using UnityEngine;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField] private Achievement[] _achievements;
    [SerializeField] private AchievementRenderer _achievementRendererTemplate;
    [SerializeField] private RectTransform _container;

    public void Init(Score score)
    {
        foreach (var achievement in _achievements)
        {
            achievement.Init(score);
            var achievementRenderer = Instantiate(_achievementRendererTemplate, _container);
            achievementRenderer.Render(achievement);
        }
    }
}
