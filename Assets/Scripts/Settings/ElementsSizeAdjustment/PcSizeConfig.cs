using UnityEngine;

[CreateAssetMenu(fileName = "PcSizeConfig", menuName = "Settings/PcSizeConfig", order = 51)]
public class PcSizeConfig : ScriptableObject, IElementSizeConfig
{
    [SerializeField] private float _maxFontSize;
    [SerializeField] private float _openedElementHeight;
    [SerializeField] private Vector2 _mergeableElementSize;
    [SerializeField] private Vector2 _bookCellSize;

    public float MaxFontSize => _maxFontSize;
    public float OpenedElementHeight => _openedElementHeight;
    public Vector2 MergeableElementSize => _mergeableElementSize;
    public Vector2 BookCellSize => _bookCellSize;
}
