using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 51)]
public class ElementsSettings : ScriptableObject
{
    [SerializeField] private int _pointsForOpenedElements;
    [SerializeField] private Color _defaultElementColor = Color.white;
    [SerializeField] private Color _elementWithoutRecipiesColor;
    [SerializeField] private ClosedElement _closedElement;

    public int PointsForOpenedElement => _pointsForOpenedElements;
    public Color DefaultElementColor => _defaultElementColor;
    public Color ElementWithoutRecipiesColor => _elementWithoutRecipiesColor;
    public ClosedElement ClosedElement => _closedElement;
}