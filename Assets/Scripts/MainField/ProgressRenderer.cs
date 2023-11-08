using Lean.Localization;
using TMPro;
using UnityEngine;

public class ProgressRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private int _maxCount;
    private IProgressHolder _currentCountHolder;

    public void Init(IProgressHolder progressHolder)
    {
        _maxCount = progressHolder.MaxCount;
        _currentCountHolder = progressHolder;
        _currentCountHolder.CurrentCountChanged += RenderCount;
        RenderCount(_currentCountHolder.CurrentCount);
    }

    private void RenderCount(int currentProgress)
    {
        if (LeanLocalization.GetFirstCurrentLanguage().Equals("ru"))
            _text.text = $"Открыто {currentProgress} из {_maxCount}";
        else
            _text.text = $"{currentProgress} out of {_maxCount} are open";
    }
}
