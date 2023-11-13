using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPanel : MonoBehaviour
{
    [SerializeField] private Image _fadePanel;
    [SerializeField] private Image _raycastTarget;
    [SerializeField] private RectTransform _notePanel;
    [SerializeField] private TMP_Text _noteText;
    [SerializeField] private UIButton _continueButton;
    [SerializeField] private UIButton _cancelButton;

    [SerializeField] Vector2 _panelLeftPosition;
    [SerializeField] Vector2 _panelRightPosition;

    public event Action ContinueButtonClicked;
    public event Action CancelButtonClicked;

    private void Awake()
    {
        _continueButton.AssignOnClickAction(() => ContinueButtonClicked?.Invoke());
        _cancelButton.AssignOnClickAction(() => CancelButtonClicked?.Invoke());
    }

    public void SetNote(string note)
    {
        _noteText.text = note;
    }

    public void HideFadePanel()
    {
        _fadePanel.Deactivate();
    }

    public void SetHighlightedObject(RectTransform highlightedObject)
    {
        transform.SetParent(highlightedObject.parent);
        transform.SetSiblingIndex(highlightedObject.parent.childCount - 1);
        highlightedObject.SetSiblingIndex(highlightedObject.parent.childCount - 1);
        _fadePanel.Activate();
    }

    public void SetGameInteractable(bool value)
    {
        _raycastTarget.SetActive(value == false);
    }

    public void SetInLeftCorner()
    {
        _notePanel.anchorMax = Vector2.up;
        _notePanel.anchorMin = Vector2.up;
        _notePanel.anchoredPosition = _panelLeftPosition;
    }

    public void SetInRightCorner()
    {
        _notePanel.anchorMax = Vector2.one;
        _notePanel.anchorMin = Vector2.one;
        _notePanel.anchoredPosition = _panelRightPosition;
    }

    public void SetContinueButtonActive(bool value)
    {
        _continueButton.SetActive(value);
    }

    public void SetCancelButtonActive(bool value)
    {
        _cancelButton.SetActive(value);
    }

    private void OnValidate()
    {
       // if (_notePanel.anchorMax == Vector2.up)
        //    SetInLeftCorner();
      //  else
         //   SetInRightCorner();
    }
}
