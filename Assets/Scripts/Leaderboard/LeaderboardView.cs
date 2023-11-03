using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private RectTransform _authorizePanel;
    [SerializeField] private UIButton _authorizeButton;

    [SerializeField] private EntryRenderer _entryRendererTemplate;
    [SerializeField] private EntryRenderer _playerRenderer;
    [SerializeField] private RectTransform _container;

    private bool _isInitialized;
    private bool _isPlayerAuthorized;
    private Score _score;
    private Saver _saver;
    private List<EntryRenderer> _entryRenderers = new();

    private void OnEnable()
    {
        if (_isInitialized)
        {
#if UNITY_EDITOR
            RenderPlayerOnly();
            return;
#endif
            if (PlayerAccount.IsAuthorized)
                RenderLeaders();
            else
                RenderPlayerOnly();
        }
    }

    public void Init(bool isPlayerAuthorized, Score score, Saver saver)
    {
        /*
        _isPlayerAuthorized = isPlayerAuthorized;
        _score = score ?? throw new ArgumentNullException();
        _saver = saver ?? throw new ArgumentNullException();
        _authorizeButton.AssignOnClickAction(OnAuthorizationButtonClick);
        _isInitialized = true;
        */
    }

    public void RenderLeaders()
    {
        if (_isInitialized == false)
            throw new InvalidOperationException("Leaderboard is not initialized");

        if (_isPlayerAuthorized == false)
        {
            _playerRenderer.Render(_score.BestScore);
            return;
        }
        Leaderboard.GetPlayerEntry(Settings.LeaderboardSettings.LeaderboardName,
            onSuccessCallback: (entry) => _playerRenderer.Render(entry),
            onErrorCallback: (error) => _playerRenderer.Render(_score.BestScore));

        Leaderboard.GetEntries(Settings.LeaderboardSettings.LeaderboardName,
            onSuccessCallback: (result) =>
            {
                int i = 0;

                foreach (var entry in result.entries)
                {
                    if (_entryRenderers.Count < i + 1)
                    {
                        var entryRenderer = Instantiate(_entryRendererTemplate, _container);
                        _entryRenderers.Add(entryRenderer);
                    }
                    _entryRenderers[i].Render(entry);
                    i++;
                }
            },
            onErrorCallback: (error) => Debug.Log("Leaderboard error: " + error),
            topPlayersCount: Settings.LeaderboardSettings.TopPlayersCount,
            competingPlayersCount: Settings.LeaderboardSettings.CompetingPlayersCount,
            includeSelf: Settings.LeaderboardSettings.IncludeSelf,
            pictureSize: Settings.LeaderboardSettings.ProfilePictureSize
        );
    }

    private void OnAuthorizationButtonClick()
    {
        PlayerAccount.Authorize(
            onSuccessCallback: OnAuthorized,
            onErrorCallback: (error) => Debug.Log("Authorization error: " + error));
    }

    private void OnAuthorized()
    {
        if (PlayerAccount.HasPersonalProfileDataPermission)
        {
            _saver.Load();
            RenderLeaders();
        }
        else
        {
            PlayerAccount.RequestPersonalProfileDataPermission(
                onSuccessCallback: () =>
                {
                    RenderLeaders();
                    _saver.Load();
                },
                onErrorCallback: (error) =>
                {
                    Debug.Log("RequestPersonalProfileDataPermission error: " + error);
                    RenderLeaders();
                });
        }
    }

    private void RenderPlayerOnly()
    {
        _authorizePanel.Activate();
        _playerRenderer.Render(_score.BestScore);
    }
}
