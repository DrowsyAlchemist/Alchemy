using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingWindow : MonoBehaviour
{
    [SerializeField] private UIButton _startTrainingButton;
    [SerializeField] private UIButton _cancelTrainingButton;
    [SerializeField] private RectTransform _greetingPanel;

    private const string TrainingSceneName = "Training Scene";
    private Saver _saver;

    public void Init(Saver saver)
    {
        _saver = saver ?? throw new ArgumentNullException();

        if (_saver.IsFirstGame)
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
        SceneManager.LoadScene(TrainingSceneName);
    }

    private void OnCancelButtonClick()
    {
        _saver.SetNotFirstGameFlag();
        Destroy(gameObject);
    }
}
