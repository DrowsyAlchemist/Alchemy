using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private ClosedElement _closedElement;
    [SerializeField] private MonetizationSettings _monetizationSettings;
    [SerializeField] private LeaderboardSettings _leaderboardSettings;
    [SerializeField] private MonoBehaviour _coroutineObject;

    private static Settings _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public static GameSettings GameSettings => _instance._gameSettings;
    public static ClosedElement ClosedElement => _instance._closedElement;
    public static MonetizationSettings MonetizationSettings => _instance._monetizationSettings;
    public static LeaderboardSettings LeaderboardSettings => _instance._leaderboardSettings;
    public static MonoBehaviour CoroutineObject => _instance._coroutineObject;
}
