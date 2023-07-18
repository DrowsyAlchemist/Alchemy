using UnityEngine;

public class Progress : MonoBehaviour
{
    [SerializeField] private ProgressRenderer[] _progressRenderers;

    private static Progress _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void Init(IProgressHolder progressHolder)
    {
        foreach (var progressRenderer in _progressRenderers)
            progressRenderer.Init(progressHolder);
    }
}
