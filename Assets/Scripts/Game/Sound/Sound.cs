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
    [SerializeField] private float _defaultNormalizedValue = 0.5f;

    [SerializeField] private Sprite _turnedOnSprite;
    [SerializeField] private Sprite _muteSprite;

    [SerializeField] private Sprite _turnedOnMusicSprite;
    [SerializeField] private Sprite _muteMusicSprite;

    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _elementCreatedSound;
    [SerializeField] private AudioSource _elementOpenedSound;

    private static Sound _instance;
    private const float ValuePower = 0.3f;
    private static float _currentGeneralNormalizedVolume = 1;
    private float _musicNormalizedVolume = 1;

    public static bool IsOn { get; private set; }
    public static bool MusicIsOn { get; private set; }
    public static Sprite TurnedOnSprite => _instance._turnedOnSprite;
    public static Sprite MuteSprite => _instance._muteSprite;
    public static Sprite TurnedOnMusicSprite => _instance._turnedOnMusicSprite;
    public static Sprite MuteMusicSprite => _instance._muteMusicSprite;
    public static float CurrentGeneralNormalizedVolume => _currentGeneralNormalizedVolume;
    public static float DefaultNormalizedVolume => _instance._defaultNormalizedValue;

    public static event Action ConditionChanged;

    private void Awake()
    {
        if (_instance == false)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        WebApplication.InBackgroundChangeEvent -= _instance.OnBackgroundChanged;
    }

    public static void Init(bool isVolumeOn, bool isMusicOn, float normalizedVolume)
    {
        SetGeneralVolume(normalizedVolume);

        if (isVolumeOn)
            TurnOn();
        else
            Mute();

        if (isMusicOn)
            ResumeMusic();
        else
            PauseMusic();

        WebApplication.InBackgroundChangeEvent += _instance.OnBackgroundChanged;
        ConditionChanged?.Invoke();
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
        _currentGeneralNormalizedVolume = normalizedValue;
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

    public static void PlayCreate()
    {
        _instance._elementCreatedSound.Play();
    }

    public static void PlayOpenElement()
    {
        _instance._elementOpenedSound.Play();
    }

    public static void PlayClick()
    {
        _instance._clickSound.Play();
    }

    private static void OnListener()
    {
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }

    private static void OffListener()
    {
        AudioListener.pause = true;
        AudioListener.volume = 0;
    }

    private void OnBackgroundChanged(bool isOut)
    {
        if (isOut)
            OffListener();
        else
            OnListener();
    }

    private void TurnSoundOn()
    {
        SetVolume(_instance._masterVolumeName, CurrentGeneralNormalizedVolume);
    }

    private void TurnSoundOff()
    {
        SetVolume(_instance._masterVolumeName, 0);
    }

    private void SetVolume(string volumeName, float normalizedValue)
    {
        float poweredValue = Mathf.Pow(normalizedValue, ValuePower);
        _mixer.SetFloat(volumeName, Mathf.Lerp(_minValue, _maxValue, poweredValue));
    }
}