using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabeticalIndex : MonoBehaviour
{
    [SerializeField] private List<LetterButton> _letterButtons;
    [SerializeField] private ScrollRect _scrollRect;

    private Coroutine _coroutine;
    private const float ScrollDuration = 1;

    private void Awake()
    {
        foreach (var button in _letterButtons)
            button.Clicked += OnButtonClick;
    }

    private void OnDestroy()
    {
        foreach (var button in _letterButtons)
            button.Clicked -= OnButtonClick;
    }

    private void OnButtonClick(char symbol)
    {
        for (int i = 0; i < _scrollRect.content.childCount; i++)
        {
            RectTransform target = _scrollRect.content.GetChild(i) as RectTransform;

            if (target == null)
                throw new InvalidOperationException();

            if (target.TryGetComponent(out IHasElement hasElement))
            {
                if (hasElement.Element.Lable.StartsWith(symbol))
                {
                    MoveContentToTarget(_scrollRect.content, target);
                    return;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    private void MoveContentToTarget(RectTransform content, RectTransform target)
    {
        Vector2 targetPosition = (Vector2)target.position + (target.rect.size / 4);

        float newYPosition = _scrollRect.transform.InverseTransformPoint(content.position).y
            - _scrollRect.transform.InverseTransformPoint(targetPosition).y;

        float minPosition = 0.1f;
        float maxPosition = content.rect.height - (_scrollRect.transform as RectTransform).rect.height;

        if (newYPosition < minPosition)
            newYPosition = minPosition;

        if (newYPosition > maxPosition)
            newYPosition = maxPosition;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(MoveToTarget(content, newYPosition));
    }

    private IEnumerator MoveToTarget(RectTransform content, float yPositionTarget)
    {
        float elapsedTime = 0;
        _scrollRect.StopMovement();

        while (elapsedTime < ScrollDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            float t = elapsedTime / ScrollDuration;
            content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, new Vector2(content.anchoredPosition.x, yPositionTarget), t);
            yield return new WaitForFixedUpdate();
        }
    }
}
