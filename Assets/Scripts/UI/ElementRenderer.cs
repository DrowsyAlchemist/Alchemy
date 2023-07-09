using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementRenderer : MonoBehaviour, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _lableText;

    [SerializeField] private Element _element;
    [SerializeField] private Game _game;

    private IMergeHandler _mergeHandler;

    public Element Element { get; private set; }

    private void Awake()
    {
        Render(_element, _game);
    }

    public void Render(Element element, IMergeHandler mergeHandler)
    {
        Element = element;
        _mergeHandler = mergeHandler;
        _image.sprite = element.Sprite;
        _lableText.text = element.Lable;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
            if (result.gameObject.TryGetComponent(out ElementRenderer otherElementRenderer))
                if (otherElementRenderer != this)
                    _mergeHandler.TryMergeElements(this, otherElementRenderer);
    }
}
