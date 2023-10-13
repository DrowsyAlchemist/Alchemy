using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private UIButton _clearButton;
    [SerializeField] private RectTransform _elementsContainer;

    public RectTransform ElementsContainer => _elementsContainer;

    public void Awake()
    {
        _clearButton.AssignOnClickAction(Clear);
    }

    public void Clear()
    {
        for (int i = 0; i < _elementsContainer.childCount; i++)
            Destroy(_elementsContainer.GetChild(i).gameObject);
    }
}
