using UnityEngine;
using UnityEngine.Audio;
using Agava.WebUtility;
using System;
using UnityEditor;

public class Sound : MonoBehaviour
{
    [SerializeField] private bool _muteOnAwake;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private string _masterVolumeName = "MasterVolume";
    [SerializeField] private string _musicVolumeName = "MusicVolume";
    [SerializeField] private float _maxValue = 0;
    [SerializeField] private float _minValue = -80;

    [SerializeField] private Sprite _turnedOnSprite;
    [SerializeField] private Sprite _muteSprite;

    [SerializeField] private Sprite _turnedOnMusicSprite;
    [SerializeField] private Sprite _muteMusicSprite;

    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _mergeSound;

    private static Sound _instance;
    private const float ValuePower = 0.3f;
    private float _musicNormalizedVolume = 1;

    public static bool IsOn { get; private set; }
    public static bool MusicIsOn { get; private set; }
    public static Sprite TurnedOnSprite => _instance._turnedOnSprite;
    public static Sprite MuteSprite => _instance._muteSprite;
    public static Sprite TurnedOnMusicSprite => _instance._turnedOnMusicSprite;
    public static Sprite MuteMusicSprite => _instance._muteMusicSprite;
    public static AudioSource BackgroundMusic => _instance._backgroundMusic;
    public static AudioSource ClickSound => _instance._clickSound;
    public static AudioSource MergeSound => _instance._mergeSound;

    public static event Action ConditionChanged;

    private void Awake()
    {
        if (_instance == false)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (_muteOnAwake)
            Mute();
        else
            TurnOn();

        ResumeMusic();
        WebApplication.InBackgroundChangeEvent += OnBackgroundChanged;
        ConditionChanged?.Invoke();
    }

    private void OnDestroy()
    {
        WebApplication.InBackgroundChangeEvent -= OnBackgroundChanged;
    }

    public static void TurnOn()
    {
        _instance.TurnSoundOn();
        IsOn = true;
        ConditionChanged?.Invoke();
    }

    public static void Mute()
    {
        _instance.TurnSoundOff();
        IsOn = false;
        ConditionChanged?.Invoke();
    }

    public static void SetGeneralVolume(float normalizedValue)
    {
        _instance.SetVolume(_instance._masterVolumeName, normalizedValue);
    }

    public static void SetMusicVolume(float normalizedValue)
    {
        _instance._musicNormalizedVolume = normalizedValue;
        _instance.SetVolume(_instance._musicVolumeName, normalizedValue);
    }

    public static void PauseMusic()
    {
        _instance.SetVolume(_instance._musicVolumeName, 0);
        MusicIsOn = false;
        ConditionChanged?.Invoke();
    }

    public static void ResumeMusic()
    {
        _instance.SetVolume(_instance._musicVolumeName, _instance._musicNormalizedVolume);
        MusicIsOn = true;
        ConditionChanged?.Invoke();
    }

    public static void PlayMerge()
    {
        _instance._mergeSound.Play();
    }

    public static void PlayClick()
    {
        _instance._clickSound.Play();
    }

    private void OnBackgroundChanged(bool isOut)
    {
        if (isOut)
        {
            TurnSoundOff();
        }
        else
        {
            if (IsOn)
                TurnSoundOn();
        }
    }

    private void TurnSoundOn()
    {
        _mixer.SetFloat(_instance._masterVolumeName, _instance._maxValue);
    }

    private void TurnSoundOff()
    {
        _mixer.SetFloat(_instance._masterVolumeName, _instance._minValue);
    }

    private void SetVolume(string volumeName, float normalizedValue)
    {
        float poweredValue = Mathf.Pow(normalizedValue, ValuePower);
        _mixer.SetFloat(volumeName, Mathf.Lerp(_minValue, _maxValue, poweredValue));
    }
}