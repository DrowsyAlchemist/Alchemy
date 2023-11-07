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

    protected ElementsStorage ElementsStorage { get; private set; }
    protected Score Score { get; private set; }
    public bool IsAchieved => CheckAchieved(Score);

    public Sprite Icon => _icon;
    public string Lable => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _lableRu : _lableEn;
    public string Description => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _descriptionRu : _descriptionEn;

    public event Action Achieved;

    public void Init(Score score)
    {
        Score = score;
        score.BestScoreChanged += OnBestScoreChanged;
    }

    private void OnBestScoreChanged(int _)
    {
        if (CheckAchieved(Score))
        {
            Score.BestScoreChanged -= OnBestScoreChanged;
            Achieved?.Invoke();
        }
    }

    private bool CheckAchieved(Score score)
    {
        return score.BestScore >= _scoreRequired;
    }
}
