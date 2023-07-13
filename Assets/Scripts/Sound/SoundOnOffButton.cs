using UnityEngine;
using UnityEngine.UI;

public class SoundOnOffButton : MonoBehaviour
{
    [SerializeField] private Image _image;

    protected void Start()
    {
        _image.sprite = (Sound.IsOn) ? Sound.TurnedOnSprite : Sound.MuteSprite;
        Sound.ConditionChanged += OnSoundConditionChanged;
    }

    protected void OnDestroy()
    {
        Sound.ConditionChanged -= OnSoundConditionChanged;
    }

    protected void OnButtonClick()
    {
        if (Sound.IsOn)
        {
            Sound.Mute();
            _image.sprite = Sound.MuteSprite;
        }
        else
        {
            Sound.ClickSound.Play();
            Sound.TurnOn();
            _image.sprite = Sound.TurnedOnSprite;
        }
    }

    private void OnSoundConditionChanged(bool isOn)
    {
        if (Sound.IsOn)
            _image.sprite = Sound.TurnedOnSprite;
        else
            _image.sprite = Sound.MuteSprite;
    }
}