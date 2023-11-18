using Agava.WebUtility;

public class GeneralVolumeSlider : UISlider
{
    private void OnEnable()
    {
        Slider.value = Sound.CurrentGeneralNormalizedVolume;
    }

    private void Start()
    {
        WebApplication.InBackgroundChangeEvent += (_) => OnValueChanged(Slider.value);
    }

    protected override void OnValueChanged(float value)
    {
        Sound.SetGeneralVolume(value);
    }
}
