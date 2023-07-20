using System.Collections;
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
        _text.text = "Открыто " + currentProgress + " из " + _maxCount;
    }
}
