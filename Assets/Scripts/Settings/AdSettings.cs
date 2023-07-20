using UnityEngine;

[CreateAssetMenu(fileName = "AdSettings", menuName = "Settings/AdSettings", order = 51)]
public class AdSettings : ScriptableObject
{
    [SerializeField] private Sprite _adSprite;
    [SerializeField] private string _adLable = "Посмотреть";
    [SerializeField] private string _adLableEn = "Peep";

    public Sprite AdSprite => _adSprite;
    public string AdLable => _adLableEn;
}