using Agava.YandexGames;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardSettings", menuName = "Settings/LeaderboardSettings", order = 51)]
public class LeaderboardSettings : ScriptableObject
{
    [SerializeField] private string _leaderboardName = "AlchemyLeaderboard";
    [SerializeField] private int _topPlayersCount = 5;
    [SerializeField] private int _competingPlayersCount = 5;
    [SerializeField] private bool _includeSelf = true;
    [SerializeField] private ProfilePictureSize _profilePictureSize = ProfilePictureSize.medium;

    [SerializeField] private string _defaultNameRu = "Неизвестный Алхимик";
    [SerializeField] private string _defaultNameEn = "Unknown Alchemist";
    [SerializeField] private string _defaulRank = "???";
    [SerializeField] private Texture2D _defaultAvatar;

    public string LeaderboardName => _leaderboardName;
    public int TopPlayersCount => _topPlayersCount;
    public int CompetingPlayersCount => _competingPlayersCount;
    public bool IncludeSelf => _includeSelf;
    public ProfilePictureSize ProfilePictureSize => _profilePictureSize;
    public string DefaultNameRu => _defaultNameRu;
    public string DefaultNameEn => _defaultNameEn;
    public string DefaultRank => _defaulRank;
    public Texture2D DefaultAvatar => _defaultAvatar;
}
