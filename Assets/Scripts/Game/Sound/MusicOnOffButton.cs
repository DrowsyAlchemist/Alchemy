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

        _image.sprite = (Sound.MusicIsOn) ? Sound.TurnedOnMusicSprite : Sound.MuteMusicSprite;
        Sound.ConditionChanged += OnSoundConditionChanged;
    }

    private void OnDestroy()
    {
        _button?.onClick.RemoveListener(OnButtonClick);
        Sound.ConditionChanged -= OnSoundConditionChanged;
    }

    private void OnButtonClick()
    {
        if (Sound.MusicIsOn)
        {
            Sound.PauseMusic();
            _image.sprite = Sound.MuteMusicSprite;
        }
        else
        {
            Sound.ResumeMusic();
            _image.sprite = Sound.TurnedOnMusicSprite;
        }
    }

    private void OnSoundConditionChanged()
    {
        if (Sound.MusicIsOn)
            _image.sprite = Sound.TurnedOnMusicSprite;
        else
            _image.sprite = Sound.MuteMusicSprite;
    }
}
