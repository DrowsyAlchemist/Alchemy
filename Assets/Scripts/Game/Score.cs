using Agava.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public class Score : MonoBehaviour
{
    [SerializeField] private ScoreRenderer[] _scoreRenderers;

    private const string CurrentScoreStorage = "CurrentScore";
    private const int DefaultScore = 0;
    private bool _isPlayerAuthorized;
    private PlayerExtraData _playerExtraData;
    private Queue<Action> _saveActions = new();
    private Coroutine _saveCoroutine;
    private Saver _saver;

    public bool IsReady { get; private set; } = true;
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;

    public void Init(int bestScore, int currentScore)
    {
        foreach (var scoreRenderer in _scoreRenderers)
            scoreRenderer.Init(this);

        //_isPlayerAuthorized = isPlayerAuthorized;
        BestScore = bestScore;
        CurrentScore = currentScore;
        BestScoreChanged?.Invoke(BestScore);
        CurrentScoreChanged?.Invoke(CurrentScore);
        /*
        try
        {
            if (isPlayerAuthorized)
            {
                _saveActions.Enqueue(() => Leaderboard.GetPlayerEntry(Settings.LeaderboardSettings.LeaderboardName, GetScoreFromLeaderboard, OnLeaderboardError));
                StartCoroutine(SaveWithDelay());
            }
            else
            {
                GetScoreFromPrefs();
            }
        }
        catch (Exception e)
        {
            Debug.Log("ScoreInitError: " + e.Message + e.StackTrace);
            _playerExtraData = new PlayerExtraData();
        }
        */
    }

    public void AddScore(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        CurrentScore += amount;
        CurrentScoreChanged?.Invoke(CurrentScore);

        if (BestScore < CurrentScore)
        {
            BestScore = CurrentScore;
            BestScoreChanged?.Invoke(BestScore);
            SetScoreToLeaderboard();
        }
        // SaveScore();
    }

    public void ResetCurrentScore()
    {
        CurrentScore = DefaultScore;
        CurrentScoreChanged?.Invoke(CurrentScore);
        // SaveScore();
    }

    public void RemoveScore()
    {
        CurrentScore = DefaultScore;
        BestScore = DefaultScore;
        // SaveScore();
        CurrentScoreChanged?.Invoke(CurrentScore);
        BestScoreChanged?.Invoke(BestScore);
        SetScoreToLeaderboard();
    }

    private void SaveScore()
    {
        if (_isPlayerAuthorized)
            // SetScoreToLeaderboard();
            SetScoreToCloud();
        else
            SetScoreToPrefs();
    }

    private void SetScoreToCloud()
    {

    }

    private void SetScoreToLeaderboard()
    {
#if UNITY_EDITOR
        return;
#endif
        Leaderboard.SetScore(Settings.LeaderboardSettings.LeaderboardName, BestScore);
    }

    private IEnumerator SaveWithDelay()
    {
        IsReady = false;
        var waitForSeconds = new WaitForSecondsRealtime(1.1f);

        while (_saveActions.Count > 0)
        {
            _saveActions.Dequeue()?.Invoke();
            yield return waitForSeconds;
        }
        IsReady = true;
    }

    private void SetScoreToPrefs()
    {
        PlayerPrefs.SetInt(Settings.LeaderboardSettings.LeaderboardName, BestScore);
        PlayerPrefs.SetInt(CurrentScoreStorage, CurrentScore);
        PlayerPrefs.Save();
    }

    private void GetScoreFromLeaderboard(LeaderboardEntryResponse entry)
    {
        if (entry != null)
        {
            BestScore = entry.score;
            string jsonExtraData = entry.extraData;

            if (string.IsNullOrEmpty(jsonExtraData))
                OnLeaderboardError("jsonExtraData is null or empty");

            var playerExtraData = JsonUtility.FromJson<PlayerExtraData>(entry.extraData);

            if (playerExtraData == null)
                OnLeaderboardError("playerExtraData is null");

            _playerExtraData = playerExtraData;
        }
        _playerExtraData ??= new PlayerExtraData();
        CurrentScore = _playerExtraData.CurrentScore;
        CurrentScoreChanged?.Invoke(CurrentScore);
        BestScoreChanged?.Invoke(BestScore);
    }

    private void OnLeaderboardError(string error)
    {
        Debug.Log("Leaderboard error: " + error);
        _playerExtraData = new PlayerExtraData();
    }

    private void GetScoreFromPrefs()
    {
        BestScore = PlayerPrefs.GetInt(Settings.LeaderboardSettings.LeaderboardName);
        CurrentScore = PlayerPrefs.GetInt(CurrentScoreStorage);
        CurrentScoreChanged?.Invoke(CurrentScore);
        BestScoreChanged?.Invoke(BestScore);
    }

    [Serializable]
    private class PlayerExtraData
    {
        public int CurrentScore;
    }
}
