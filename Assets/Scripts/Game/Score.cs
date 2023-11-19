using Agava.YandexGames;
using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private ScoreRenderer[] _scoreRenderers;

    private const int DefaultScore = 0;

    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;

    public void Init(int bestScore, int currentScore)
    {
        foreach (var scoreRenderer in _scoreRenderers)
            scoreRenderer.Init(this);

        BestScore = bestScore;
        CurrentScore = currentScore;
        BestScoreChanged?.Invoke(BestScore);
        CurrentScoreChanged?.Invoke(CurrentScore);


        SetScoreToLeaderboard();
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
    }

    public void ResetCurrentScore()
    {
        CurrentScore = DefaultScore;
        CurrentScoreChanged?.Invoke(CurrentScore);
    }

    public void RemoveScore()
    {
        CurrentScore = DefaultScore;
        BestScore = DefaultScore;
        CurrentScoreChanged?.Invoke(CurrentScore);
        BestScoreChanged?.Invoke(BestScore);
        SetScoreToLeaderboard();
    }

    private void SetScoreToLeaderboard()
    {
#if UNITY_EDITOR
        return;
#endif
        Leaderboard.GetPlayerEntry(Settings.LeaderboardSettings.LeaderboardName, onSuccessCallback: (result) =>
        {
            if (result.score < BestScore)
                Leaderboard.SetScore(Settings.LeaderboardSettings.LeaderboardName, BestScore);
        });
    }
}
