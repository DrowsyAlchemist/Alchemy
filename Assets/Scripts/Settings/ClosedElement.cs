using UnityEngine;

[CreateAssetMenu(fileName = "ClosedElement", menuName = "Settings/ClosedElement", order = 51)]
public class ClosedElement : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _lable = "Закрыто";
    [SerializeField] private string _lableEn = "Closed";

    public Sprite Sprite => _sprite;
    public string Lable => _lable;
}