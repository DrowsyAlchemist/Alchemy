using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private UIButton _clearButton;

    public void Awake()
    {
        _clearButton.AssignOnClickAction(Clear);
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
