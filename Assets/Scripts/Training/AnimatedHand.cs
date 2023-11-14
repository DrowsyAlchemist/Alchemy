using UnityEngine;

public class AnimatedHand : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string PointDownAnimation = "PointDown";
    private const string TapAnimation = "Tap";
    private const string DragAndDropAnimation = "DragAndDrop";
    private const string CloneAnimation = "Clone";

    public void PlayPointDown()
    {
        gameObject.SetActive(true);
        _animator.Play(PointDownAnimation);
    }

    public void PlayTap()
    {
        gameObject.SetActive(true);
        _animator.Play(TapAnimation);
    }

    public void PlayDragAndDrop()
    {
        gameObject.SetActive(true);
        _animator.Play(DragAndDropAnimation);
    }

    public void PlayClone()
    {
        gameObject.SetActive(true);
        _animator.Play(CloneAnimation);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
}
