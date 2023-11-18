using Lean.Localization;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement", order = 51)]
public class Achievement : ScriptableObject
{
    [SerializeField] private int _scoreRequired;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _lableEn;
    [SerializeField] private string _lableRu;
    [SerializeField, TextArea(5, 10)] private string _descriptionEn;
    [SerializeField, TextArea(5, 10)] private string _descriptionRu;

    public bool IsAchieved { get; private set; }
    public int ScoreRequired => _scoreRequired;
    protected ElementsStorage ElementsStorage { get; private set; }
    protected Score Score { get; private set; }

    public Sprite Icon => _icon;
    public string Lable => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _lableRu : _lableEn;
    public string Description => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _descriptionRu : _descriptionEn;

    public event Action<Achievement> Achieved;

    public void Init(Score score)
    {
        Score = score;
        IsAchieved = CheckAchieved(score);

        if (IsAchieved == false)
            score.BestScoreChanged += OnBestScoreChanged;
    }

    private void OnBestScoreChanged(int score)
    {
        if (score == _scoreRequired)
        {
            Score.BestScoreChanged -= OnBestScoreChanged;
            IsAchieved = true;
            Achieved?.Invoke(this);
        }
    }

    private bool CheckAchieved(Score score)
    {
        return score.BestScore >= _scoreRequired;
    }
}
