using UnityEngine;

public interface IElementSizeConfig
{
    float MaxFontSize { get; }
    float OpenedElementHeight { get; }
    Vector2 MergeableElementSize { get; }
    Vector2 BookCellSize { get; }
}
