using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundOnOffButton : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);

        _image.sprite = (Sound.IsOn) ? Sound.TurnedOnSprite : Sound.MuteSprite;
        Sound.ConditionChanged += OnSoundConditionChanged;
    }

    private void OnDestroy()
    {
        _button?.onClick.RemoveListener(OnButtonClick);
        Sound.ConditionChanged -= OnSoundConditionChanged;
    }

    private void OnButtonClick()
    {
        if (Sound.IsOn)
        {
            Sound.Mute();
            _image.sprite = Sound.MuteSprite;
        }
        else
        {
            Sound.TurnOn();
            _image.sprite = Sound.TurnedOnSprite;
        }
    }

    private void OnSoundConditionChanged()
    {
        if (Sound.IsOn)
            _image.sprite = Sound.TurnedOnSprite;
        else
            _image.sprite = Sound.MuteSprite;
    }
}