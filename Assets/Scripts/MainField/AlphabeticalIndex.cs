using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabeticalIndex : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _lettersContainer;
    [SerializeField] private LetterButton _letterTemplate;

    private List<LetterButton> _letterButtons = new();

    private const float MinPosition = 0.1f;
    private const float ScrollDuration = 1;
    private Coroutine _coroutine;

    public void Init()
    {
        FillByLanguage();
    }

    private void OnDestroy()
    {
        foreach (var button in _letterButtons)
            button.Clicked -= OnButtonClick;
    }

    private void FillByLanguage()
    {
        if (LeanLocalization.GetFirstCurrentLanguage().Equals("ru"))
        {
            for (char symbol = 'À'; symbol <= 'ß'; symbol++)
                CreateLetter(symbol);
        }
        else
        {
            for (char symbol = 'A'; symbol <= 'Z'; symbol++)
                CreateLetter(symbol);
        }
    }

    private void CreateLetter(char symbol)
    {
        var letterButton = Instantiate(_letterTemplate, _lettersContainer);
        letterButton.Render(symbol);
        letterButton.Clicked += OnButtonClick;
        _letterButtons.Add(letterButton);
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
                if (hasElement.Element.Lable[0] >= symbol)
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

        float maxPosition = content.rect.height - (_scrollRect.transform as RectTransform).rect.height;

        if (maxPosition < 0)
            return;

        if (newYPosition < MinPosition)
            newYPosition = MinPosition;

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
