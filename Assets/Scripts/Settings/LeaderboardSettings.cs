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

    public string LeaderboardName => _leaderboardName;
    public int TopPlayersCount => _topPlayersCount;
    public int CompetingPlayersCount => _competingPlayersCount;
    public bool IncludeSelf => _includeSelf;
    public ProfilePictureSize ProfilePictureSize => _profilePictureSize;
}
