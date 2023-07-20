using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 51)]
public class GameSettings : ScriptableObject
{
    [SerializeField] private int _pointsForOpenedElements;

    public int PointsForOpenedElement => _pointsForOpenedElements;
}