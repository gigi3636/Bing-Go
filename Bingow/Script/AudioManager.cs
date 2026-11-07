using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }

    [Export] private AudioStream[] _audioClips;
    [Export] private AudioStreamPlayer _musicPlayer;

    private List<AudioStreamPlayer> _sfxPlayers = new List<AudioStreamPlayer>();
    private const int MaxSfxPlayers = 8;

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }

        Instance = this;

        for (int i = 0; i < MaxSfxPlayers; i++)
        {
            AudioStreamPlayer player = new AudioStreamPlayer();
            AddChild(player);
            _sfxPlayers.Add(player);
        }
    }

    public void PlayMusic(string name)
    {
        AudioStream clip = _audioClips?.FirstOrDefault(c => c != null && c.ResourceName == name);
        if (clip != null && _musicPlayer != null)
        {
            _musicPlayer.Stream = clip;
            _musicPlayer.StreamPaused = false;
            _musicPlayer.Play();
        }
    }

    public void PauseMusic()
    {
        if (_musicPlayer != null)
        {
            _musicPlayer.StreamPaused = true;
        }
    }

    public void ReplayMusic()
    {
        if (_musicPlayer != null)
        {
            _musicPlayer.StreamPaused = false;
            _musicPlayer.Play(0);
        }
    }

    public void PlaySfx(string name)
    {
        AudioStream clip = _audioClips?.FirstOrDefault(c => c != null && System.IO.Path.GetFileNameWithoutExtension(c.ResourcePath) == name);

        if (clip != null)
        {
            AudioStreamPlayer playerToUse = _sfxPlayers.FirstOrDefault(p => !p.Playing);

            if (playerToUse == null)
            {
                playerToUse = _sfxPlayers[0];
            }

            playerToUse.Stream = clip;
            playerToUse.Play();

            _sfxPlayers.Remove(playerToUse);
            _sfxPlayers.Add(playerToUse);
        }
    }
}