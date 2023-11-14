using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Training : MonoBehaviour
{
    [SerializeField] private TrainingPanel _trainingPanel;
    [SerializeField] private Task[] _tasks;

    private static Training _instance;
    private int _currentTask;
    private Saver _saver;

    public bool IsTraining { get; private set; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            throw new System.InvalidOperationException();

    }

    public void Init(Saver saver)
    {
        _saver = saver;
        _trainingPanel.CancelButtonClicked += ForceStopTraining;
    }

    public void Begin()
    {
        if (IsTraining)
            throw new System.InvalidOperationException();

        _currentTask = -1;
        BeginNextTask();
        IsTraining = true;
    }

    private void BeginNextTask()
    {
        if (_currentTask >= 0)
            _tasks[_currentTask].Completed -= BeginNextTask;

        if (_currentTask + 1 < _tasks.Length)
        {
            ResetTrainingPanel();
            _currentTask++;
            _tasks[_currentTask].Completed += BeginNextTask;
            _tasks[_currentTask].Begin(_trainingPanel);
        }
        else
        {
            StopTraining();
        }
    }

    private void ResetTrainingPanel()
    {
        _trainingPanel.SetContinueButtonActive(false);
        _trainingPanel.SetCancelButtonActive(true);
        _trainingPanel.HideFadePanel();
        _trainingPanel.SetGameInteractable(true);
    }

    private void StopTraining()
    {
        _saver.SetTrainingCompleted();
        SceneManager.LoadScene(Settings.MainSceneName);
        _trainingPanel.Deactivate();
        IsTraining = false;
    }

    private void ForceStopTraining()
    {
        _tasks[_currentTask].Completed -= BeginNextTask;
        _tasks[_currentTask].ForceComplete();
        _currentTask = _tasks.Length - 1;
        ResetTrainingPanel();
        _tasks[_currentTask].Completed += BeginNextTask;
        _tasks[_currentTask].Begin(_trainingPanel);
    }

    public static void SetElementOnGameField(Element element)
    {
        if (_instance._tasks[_instance._currentTask] is DragTask dragTask)
            dragTask.CheckDraggedElement(element);
    }

    public static void SetElementCreated(Element firstElement, Element secondElement)
    {
        if (_instance._tasks[_instance._currentTask] is MergeTask mergeTask)
            mergeTask.CheckDraggedElement(firstElement, secondElement);
    }

    public static void SetDoubleClick()
    {
        if (_instance._tasks[_instance._currentTask] is CloneTask cloneTask)
            cloneTask.ForceComplete();
    }
}
