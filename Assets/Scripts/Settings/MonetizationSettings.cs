using Lean.Localization;
using UnityEngine;

[CreateAssetMenu(fileName = "AdSettings", menuName = "Settings/AdSettings", order = 51)]
public class MonetizationSettings : ScriptableObject
{
    [SerializeField] private float _secondsBetweenInter = 100;

    [SerializeField] private Sprite _adSprite;
    [SerializeField] private string _adLable = "Посмотреть";
    [SerializeField] private string _adLableEn = "Peep";

    [SerializeField] private Sprite _yanSprite;
    [SerializeField] private string _yanLable = "Открыть";
    [SerializeField] private string _yanLableEn = "Open";

    public float SecondsBetweenInters => _secondsBetweenInter;

    public Sprite AdSprite => _adSprite;
    public string AdLable => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _adLable : _adLableEn;

    public Sprite YanSprite => _adSprite;
    public string YanLable => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? _yanLable : _yanLableEn;
}