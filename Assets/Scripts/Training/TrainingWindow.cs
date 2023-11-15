using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingWindow : MonoBehaviour
{
    [SerializeField] private UIButton _startTrainingButton;
    [SerializeField] private UIButton _cancelTrainingButton;
    [SerializeField] private RectTransform _greetingPanel;
    [SerializeField] private Image _loadingImage;

    private Saver _saver;

    public void Init(Saver saver)
    {
        _saver = saver ?? throw new ArgumentNullException();

        if (_saver.IsTrainingCompleted == false)
        {
            _startTrainingButton.AssignOnClickAction(OnStartButtonClick);
            _cancelTrainingButton.AssignOnClickAction(OnCancelButtonClick);
            _greetingPanel.Activate();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnStartButtonClick()
    {
        _loadingImage.Activate();
        SceneManager.LoadScene(Settings.TrainingSceneName);
    }

    private void OnCancelButtonClick()
    {
        _saver.SetTrainingCompleted();
        Destroy(gameObject);
    }
}
