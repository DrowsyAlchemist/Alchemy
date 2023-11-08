using Agava.YandexGames;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    private const string DefaultNameRu = "Инкогнито";
    private const string DefaultNameEn = "Incognito";
    private const string DefaulRank = "???";

    protected string LocalizedDefaultName => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? DefaultNameRu : DefaultNameEn;
    public void Render(LeaderboardEntryResponse entry)
    {
        _rank.text = entry.rank.ToString();
        _name.text = PlayerAccount.HasPersonalProfileDataPermission ? entry.player.publicName : LocalizedDefaultName;
        _score.text = entry.score.ToString();
    }

    public void Render(int score)
    {
        _rank.text = DefaulRank;
        _name.text = LocalizedDefaultName;
        _score.text = score.ToString();
    }
}
