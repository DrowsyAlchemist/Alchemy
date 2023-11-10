using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private UIButton _trainingButton;
    [SerializeField] private UIButton _feedbackButton;
    [SerializeField] private UIButton _resetProgressButton;

    private const string TrainingSceneName = "Training Scene";
    private GameInitialize _gameInitialize;

    public void Init(GameInitialize gameInitialize)
    {
        _trainingButton.AssignOnClickAction(OnTrainingButtonClick);
        _feedbackButton.AssignOnClickAction(OnFeedbackButtonClick);
        _resetProgressButton.AssignOnClickAction(OnResetButtonClick);
    }

    private void OnTrainingButtonClick()
    {
        SceneManager.LoadScene(TrainingSceneName);
    }

    private void OnFeedbackButtonClick()
    {
        Feedback.RequestFeedbackFromSDK();
    }

    private void OnResetButtonClick()
    {
        _gameInitialize.ResetProgress();
    }
}
