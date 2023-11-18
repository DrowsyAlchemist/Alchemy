using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TerminalElementWindow : MonoBehaviour
{
    private const string ShowAnimation = "ShowTerminalElement";
    private Animator _animator;
    private ElementsStorage _elementsStorage;
    private Saver _saver;

    private void OnDestroy()
    {
        _elementsStorage.ElementOpened -= OnElementOpened;
    }

    public void Init(ElementsStorage elementsStorage, Saver saver)
    {
        _animator = GetComponent<Animator>();
        _elementsStorage = elementsStorage;
        _saver = saver;

        if (saver.IsTerminalElementOpened)
        {
            Destroy(gameObject);
            return;
        }
        _elementsStorage.ElementOpened += OnElementOpened;
    }

    private void OnElementOpened(Element element)
    {
        if (element.Recipies.Count == 0)
        {
            _saver.OpenTerminateElement();
            _animator.Play(ShowAnimation);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
