using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MusicOnOffButton : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);

        _image.sprite = (Sound.MusicIsOn) ? Sound.TurnedOnSprite : Sound.MuteSprite;
        Sound.ConditionChanged += OnSoundConditionChanged;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
        Sound.ConditionChanged -= OnSoundConditionChanged;
    }

    private void OnButtonClick()
    {
        if (Sound.MusicIsOn)
        {
            Sound.PauseMusic();
            _image.sprite = Sound.MuteSprite;
        }
        else
        {
            Sound.ResumeMusic();
            _image.sprite = Sound.TurnedOnSprite;
        }
    }

    private void OnSoundConditionChanged()
    {
        if (Sound.MusicIsOn)
            _image.sprite = Sound.TurnedOnSprite;
        else
            _image.sprite = Sound.MuteSprite;
    }
}
