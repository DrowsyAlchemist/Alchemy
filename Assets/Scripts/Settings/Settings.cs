using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private ElementsSettings _elementsSettings;
    [SerializeField] private MonetizationSettings _monetizationSettings;
    [SerializeField] private LeaderboardSettings _leaderboardSettings;
    [SerializeField] private MonoBehaviour _coroutineObject;

    public const string MainSceneName = "Main Scene";
    public const string TrainingSceneName = "Training Scene";
    private static Settings _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public static ElementsSettings Elements => _instance._elementsSettings;
    public static MonetizationSettings Monetization => _instance._monetizationSettings;
    public static LeaderboardSettings Leaderboard => _instance._leaderboardSettings;
    public static MonoBehaviour CoroutineObject => _instance._coroutineObject;
}
