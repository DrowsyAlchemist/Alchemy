using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EntryRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private RawImage _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    protected string LocalizedDefaultName => LeanLocalization.GetFirstCurrentLanguage().Equals("ru") ? Settings.Leaderboard.DefaultNameRu : Settings.Leaderboard.DefaultNameEn;

    public void Render(LeaderboardEntryResponse entry)
    {
        _rank.text = entry.rank.ToString();
        _score.text = entry.score.ToString();

        string playerName = entry.player.publicName;
        _name.text = string.IsNullOrEmpty(playerName) ? LocalizedDefaultName : playerName;

        string playerAvatarUrl = entry.player.profilePicture;
        Settings.CoroutineObject.StartCoroutine(DownloadPlayerAvatar(playerAvatarUrl));
    }

    public void Render(int score)
    {
        _rank.text = Settings.Leaderboard.DefaultRank;
        _name.text = LocalizedDefaultName;
        _score.text = score.ToString();
        _image.texture = Settings.Leaderboard.DefaultAvatar;
    }

    private IEnumerator DownloadPlayerAvatar(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            _image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        else
            _image.texture = Settings.Leaderboard.DefaultAvatar;
    }
}
