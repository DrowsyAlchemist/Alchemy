using UnityEngine;

[CreateAssetMenu(fileName = "MobileSizeConfig", menuName = "Settings/MobileSizeConfig", order = 51)]
public class MobileSizeConfig : ScriptableObject, IElementSizeConfig
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
