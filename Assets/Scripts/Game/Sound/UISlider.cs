using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class UISlider : MonoBehaviour
{
    protected Slider Slider { get; private set; }

    private void Awake()
    {
        Slider = GetComponent<Slider>();
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy()
    {
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    protected abstract void OnValueChanged(float value);
}
