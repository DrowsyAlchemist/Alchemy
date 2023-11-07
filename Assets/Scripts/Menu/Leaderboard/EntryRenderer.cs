using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    private const string DefaultName = "Инкогнито";
    private const string DefaulRank = "???";

    public void Render(LeaderboardEntryResponse entry)
    {
        _rank.text = entry.rank.ToString();
        _name.text = PlayerAccount.HasPersonalProfileDataPermission ? entry.player.publicName : DefaultName;
        _score.text = entry.score.ToString();
    }

    public void Render(int score)
    {
        _rank.text = DefaulRank;
        _name.text = DefaultName;
        _score.text = score.ToString();
    }
}
