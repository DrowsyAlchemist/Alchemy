using TMPro;
using UnityEngine;

public class ScoreRenderer : MonoBehaviour
{
   // [SerializeField] private TMP_Text _currentScoreText;
    [SerializeField] private TMP_Text _bestScoreText;

    private Score _score;

    private void OnDestroy()
    {
      //  _score.CurrentScoreChanged -= OnScoreChanged;
        _score.BestScoreChanged -= OnBestScoreChanged;
    }

    public void Init(Score score)
    {
        _score = score;
       // OnScoreChanged(_score.CurrentScore);
        OnBestScoreChanged(_score.BestScore);
       // _score.CurrentScoreChanged += OnScoreChanged;
        _score.BestScoreChanged += OnBestScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
       // _currentScoreText.text = score.ToString();
    }

    private void OnBestScoreChanged(int bestScore)
    {
        _bestScoreText.text = bestScore.ToString();
    }
}
