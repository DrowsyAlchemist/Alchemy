using Agava.YandexGames;
using System;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public class Score : MonoBehaviour
{
    [SerializeField] private ScoreRenderer[] _scoreRenderers;

    private const string CurrentScoreStorage = "CurrentScore";

    private static Score _instance;
    private bool _isPlayerAuthorized;
    private PlayerExtraData _playerExtraData;

    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void Init(bool isPlayerAuthorized)
    {
        _isPlayerAuthorized = isPlayerAuthorized;

        if (isPlayerAuthorized)
            Leaderboard.GetPlayerEntry(Settings.LeaderboardSettings.LeaderboardName, GetScoreFromLeaderboard, OnLeaderboardError);
        else
            GetScoreFromPrefs();

        foreach (var scoreRenderer in _scoreRenderers)
            scoreRenderer.Init(this);
    }

    public void AddScore(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        CurrentScore += amount;
        CurrentScoreChanged.Invoke(CurrentScore);

        if (BestScore < CurrentScore)
        {
            BestScore = CurrentScore;
            BestScoreChanged.Invoke(BestScore);
        }

        SaveScore();
    }

    public void ResetCurrentScore()
    {
        CurrentScore = 0;
        CurrentScoreChanged?.Invoke(CurrentScore);
        SaveScore();
    }

    private void SaveScore()
    {
        if (_isPlayerAuthorized)
            SetScoreToLeaderboard();
        else
            SetScoreToPrefs();
    }

    private void SetScoreToLeaderboard()
    {
        _playerExtraData.CurrentScore = CurrentScore;
        string jsonData = JsonUtility.ToJson(_playerExtraData);
        Leaderboard.SetScore(Settings.LeaderboardSettings.LeaderboardName, BestScore, extraData: jsonData);
    }

    private void SetScoreToPrefs()
    {
        PlayerPrefs.SetInt(Settings.LeaderboardSettings.LeaderboardName, BestScore);
        PlayerPrefs.SetInt(CurrentScoreStorage, CurrentScore);
        PlayerPrefs.Save();
    }

    private void GetScoreFromLeaderboard(LeaderboardEntryResponse entry)
    {
        BestScore = entry.score;
        string jsonExtraData = entry.extraData;

        if (string.IsNullOrEmpty(jsonExtraData))
            OnLeaderboardError("jsonExtraData is null or empty");

        var playerExtraData = JsonUtility.FromJson<PlayerExtraData>(entry.extraData);

        if (playerExtraData == null)
            OnLeaderboardError("playerExtraData is null");

        _playerExtraData = playerExtraData ?? new PlayerExtraData();
        CurrentScore = _playerExtraData.CurrentScore;
    }

    private void OnLeaderboardError(string error)
    {
        Debug.Log("Leaderboard error: " + error);
    }

    private void GetScoreFromPrefs()
    {
        BestScore = PlayerPrefs.GetInt(Settings.LeaderboardSettings.LeaderboardName, 0);
        CurrentScore = PlayerPrefs.GetInt(CurrentScoreStorage, 0);
    }

    [Serializable]
    private class PlayerExtraData
    {
        public int CurrentScore;
    }
}
