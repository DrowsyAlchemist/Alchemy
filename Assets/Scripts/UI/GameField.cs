using UnityEngine;
using UnityEngine.UI;

public class GameField : MonoBehaviour
{
    [SerializeField] private Button _clearButton;

    public void Start()
    {
        _clearButton.onClick.AddListener(Clear);
    }

    private void OnDestroy()
    {
        _clearButton.onClick.RemoveListener(Clear);
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
