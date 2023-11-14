using System.Collections;
using UnityEngine;

public class CloneTask : Task
{
    [SerializeField] private GameField _gameField;

    protected override void BeginTask()
    {
    }

    protected override void SetHand(AnimatedHand animatedHand)
    {
        Settings.CoroutineObject.StartCoroutine(SetHandWithDelay(animatedHand));
    }

    private IEnumerator SetHandWithDelay(AnimatedHand animatedHand)
    {
        yield return new WaitForEndOfFrame();
        var elementsOnField = _gameField.ElementsOnField;

        if (elementsOnField != null && elementsOnField.Count > 0)
        {
            var elementRenderer = elementsOnField[0];
            Rect rendererRect = elementRenderer.GetComponent<RectTransform>().rect;
            float yMargin = rendererRect.size.y / 2;
            float rendererScale = elementRenderer.transform.lossyScale.x;
            animatedHand.SetPosition((Vector2)elementRenderer.transform.position + Vector2.down * yMargin * rendererScale);
            animatedHand.PlayClone();
        }
    }
}
