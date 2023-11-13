using UnityEngine;
using UnityEngine.UI;

public class ClickTask : Task
{
    [SerializeField] private Button _targetButton;
    [SerializeField] private Button[] _excessButtons;

    protected override void BeginTask()
    {
        foreach (var button in _excessButtons)
            button.interactable = false;

        _targetButton.AddListener(Complete);
    }

    protected override void OnComplete()
    {
        foreach (var button in _excessButtons)
            button.interactable = true;

        _targetButton.RemoveListener(Complete);
        base.OnComplete();
    }
}
