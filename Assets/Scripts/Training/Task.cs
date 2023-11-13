using Lean.Localization;
using System;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private bool _isNotePanelInRightCorner;
    [SerializeField] private string _localizationPhrase;
    [SerializeField] private RectTransform[] _objectsToShow;

    public bool IsCompleted { get; private set; } = false;
    protected TrainingPanel TrainingPanel { get; private set; }

    public event Action Completed;

    public void Begin(TrainingPanel trainingPanel)
    {
        TrainingPanel = trainingPanel;

       // if (_isNotePanelInRightCorner)
       //     trainingPanel.SetInRightCorner();
       // else
       //     trainingPanel.SetInLeftCorner();

        if (_objectsToShow.Length > 0)
            foreach (var obj in _objectsToShow)
                obj.Activate();

        string localizedNote = LeanLocalization.GetTranslationText(_localizationPhrase);
        trainingPanel.SetNote(localizedNote);
        trainingPanel.transform.SetParent(_parent);
        trainingPanel.transform.SetSiblingIndex(_parent.childCount - 1);
        trainingPanel.Activate();
        BeginTask();
    }

    public void ForceComplete()
    {
        Complete();
    }

    protected abstract void BeginTask();

    protected virtual void OnComplete()
    {
    }

    protected void Complete()
    {
        OnComplete();
        IsCompleted = true;
        Completed?.Invoke();
    }
}
