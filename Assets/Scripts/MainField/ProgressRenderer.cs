using Lean.Localization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ProgressRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private const string OpenElementAnimation = "OpenElement";
    private Animator _animator;
    private int _maxCount;
    private IProgressHolder _currentCountHolder;

    public void Init(IProgressHolder progressHolder)
    {
        _animator = GetComponent<Animator>();
        _maxCount = progressHolder.MaxCount;
        _currentCountHolder = progressHolder;
        _currentCountHolder.CurrentCountChanged += RenderCount;
        RenderCount(_currentCountHolder.CurrentCount);
    }

    private void OnDestroy()
    {
        _currentCountHolder.CurrentCountChanged -= RenderCount;
    }

    private void RenderCount(int currentProgress)
    {
        _text.text = currentProgress + " / " + _maxCount;
        //if (LeanLocalization.GetFirstCurrentLanguage().Equals("ru"))
        //    _text.text = $"Открыто {currentProgress} из {_maxCount}";
        //else
        //    _text = $"{currentProgress} out of {_maxCount} are open";
        _animator.Play(OpenElementAnimation);
    }
}
