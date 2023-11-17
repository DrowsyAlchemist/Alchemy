using UnityEngine;

[RequireComponent(typeof(Animator))]
public class YanElementWindow : MonoBehaviour
{
    [SerializeField] private ElementRenderer _elementRenderer;

    private const string ShowAnimation = "ShowYanElement";
    private Animator _animator;

    public void Show(Element element)
    {
        _elementRenderer.Render(element);
        _animator ??= GetComponent<Animator>();
        _animator.Play(ShowAnimation);
    }
}
