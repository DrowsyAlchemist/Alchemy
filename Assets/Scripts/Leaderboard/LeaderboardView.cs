using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private EntryRenderer _entryRendererTemplate;
    [SerializeField] private RectTransform _container;
    [SerializeField] private EntryRenderer _playerRenderer;

    private bool _isInitialized;
    private bool _isPlayerAuthorized;
    private Score _score;
    private List<EntryRenderer> _entryRenderers = new();

    private void OnEnable()
    {
        if (_isInitialized)
            RenderLeaders();
    }

    public void Init(bool isPlayerAuthorized, Score score)
    {
        _isPlayerAuthorized = isPlayerAuthorized;
        _score = score ?? throw new ArgumentNullException();
        _isInitialized = true;
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
        Leaderboard.GetPlayerEntry(Game.LeaderboardName,
            onSuccessCallback: (entry) => _playerRenderer.Render(entry),
            onErrorCallback: (error) => _playerRenderer.Render(_score.BestScore));

        Leaderboard.GetEntries(Game.LeaderboardName,
            onSuccessCallback: (result) =>
            {
                int i = 0;

                foreach (var entry in result.entries)
                {
                    if (_entryRenderers.Count < i + 1)
                    {
                        var entryRenderer = Instantiate(_entryRendererTemplate, _container);
                        _entryRenderers.Add(entryRenderer);
                        entryRenderer.Render(entry);
                    }
                    _entryRenderers[i].Render(entry);
                    i++;
                }
            }
        );
    }
}
