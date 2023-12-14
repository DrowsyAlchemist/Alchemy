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
        if (_scoreRenderers != null && _scoreRenderers.Length > 0)
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
        if (BestScore == 5)
            Metrics.SendEvent(MetricEvent.MakeFirstElement);
        else if (BestScore == 50)
            Metrics.SendEvent(MetricEvent.MakeFiftyElements);
        else if (BestScore == 100)
            Metrics.SendEvent(MetricEvent.MakeOneHundredElements);
        else if (BestScore == 200)
            Metrics.SendEvent(MetricEvent.MakeTwoHundredElements);
        else if (BestScore == 300)
            Metrics.SendEvent(MetricEvent.MakeThreeHundredElements);
        else if (BestScore == 400)
            Metrics.SendEvent(MetricEvent.MakeFourHundredElements);
        else if (BestScore == 500)
            Metrics.SendEvent(MetricEvent.MakeFiveHundredElements);
        else if (BestScore == 600)
            Metrics.SendEvent(MetricEvent.MakeSixHundredElements);
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
        Leaderboard.GetPlayerEntry(Settings.Leaderboard.LeaderboardName, onSuccessCallback: (result) =>
        {
            if (result.score < BestScore)
                Leaderboard.SetScore(Settings.Leaderboard.LeaderboardName, BestScore);
        });
    }
}
