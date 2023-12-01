using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ClickTask : Task
{
    [SerializeField] private Button _targetButton;
    [SerializeField] private Button[] _excessButtons;

    private const float MarginFromButtonModifier = 1.7f;

    protected Button TargetButton => _targetButton;
    protected float ButtonScale => _targetButton.transform.lossyScale.x;

    protected override void BeginTask()
    {
        foreach (var button in _excessButtons)
            button.interactable = false;

        _targetButton.AddListener(Complete);
    }

    protected override void SetHand(AnimatedHand animatedHand)
    {
        Vector2 buttonSize = _targetButton.GetComponent<RectTransform>().rect.size;
        Vector2 margin = ButtonScale * new Vector2(buttonSize.x / 3, MarginFromButtonModifier * buttonSize.y);
        animatedHand.SetPosition((Vector2)_targetButton.transform.position + margin);
        animatedHand.PlayPointDown();
    }

    protected override void OnComplete()
    {
        foreach (var button in _excessButtons)
            button.interactable = true;

        _targetButton.RemoveListener(Complete);
        base.OnComplete();
    }
}
