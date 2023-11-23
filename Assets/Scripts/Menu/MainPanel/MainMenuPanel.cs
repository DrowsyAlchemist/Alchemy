using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private UIButton _trainingButton;
    [SerializeField] private UIButton _feedbackButton;
    [SerializeField] private UIButton _resetProgressButton;

    private GameInitialize _gameInitialize;

    public event Action Closed;

    private void OnDisable()
    {
        Closed?.Invoke();
    }

    public void Init(GameInitialize gameInitialize)
    {
        _gameInitialize = gameInitialize;
        _trainingButton.AssignOnClickAction(OnTrainingButtonClick);
        _feedbackButton.AssignOnClickAction(OnFeedbackButtonClick);
        _resetProgressButton.AssignOnClickAction(OnResetButtonClick);

#if UNITY_EDITOR
        return;
#endif
        if (PlayerAccount.IsAuthorized == false)
            _feedbackButton.Deactivate();
    }

    private void OnTrainingButtonClick()
    {
        SceneManager.LoadScene(Settings.TrainingSceneName);
    }

    private void OnFeedbackButtonClick()
    {
        _feedbackButton.SetInteractable(false);
        Feedback.RequestFeedbackFromSDK();
    }

    private void OnResetButtonClick()
    {
        _gameInitialize.ResetProgress();
    }
}
