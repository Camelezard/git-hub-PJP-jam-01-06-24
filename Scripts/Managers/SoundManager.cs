using Com.IsartDigital.ProjectName;
using Godot;
using System.Collections.Generic;

// Author : Dylan Lupon
// Date : 06 / 02 / 2024
public class SoundManager : Node2D
{
    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // SINGLETON

    private static SoundManager instance = null;
    public static SoundManager GetInstance() => instance;
    private void SetSingleton() { if (instance != null) { QueueFree(); return; } instance = this; }

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // SIGNALS

    private const string SIGNAL_AUDIO_FINISHED = "finished";

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // VARIABLES

    // BUS
    public enum Bus
    {
        Master = 0,
        Music = 1,
        SoundEffect = 2
    }

    // TYPES
    public enum SoundType
    {
        BUTTON_CLICK,
        PLACE_TILE,
        BLACKHOLE_ASPIRATION

    }
    public enum MusicType
    {
        MAIN_MENU,
        IN_GAME,
    }

    // SOUND PROPERTIES
    private const string PROPERTY_VOLUME_DB = "volume_db";
    private const float VOLUME_DB_MIN = -80f;
    private const float VOLUME_DB_DEFAULT = 0f;

    // SOUNDS EFFECTS
    [Export] private int soundPlayerCount = 10;
    [Export] private List<AudioStream> allSounds;
    private List<AudioStreamPlayer> soundPlayers = new List<AudioStreamPlayer>();
    private int currentSoundPlayerIndex = 0;

    // MUSICS
    [Export] private int musicPlayerCount = 2;
    [Export] private List<AudioStream> allMusics;
    private List<AudioStreamPlayer> musicPlayers = new List<AudioStreamPlayer>();
    private int currentMusicPlayerIndex = 0;

    // TRANSITIONS
    public const float TRANSITION_LONG_DURATION = 10f;
    public const float TRANSITION_NORMAL_DURATION = 5f;
    public const float TRANSITION_SMALL_DURATION = 2.5f;
    private const float TRANSITION_DEFAULT_DURATION = 0f;
    private Tween transitionTween = new Tween();

    // INIT PROPERTIES
    private const float MAIN_MENU_FADE_IN = 0f;

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // INITIALIZATION

    public override void _Ready()
    {
        SetSingleton();
        SetUpPlayersProperties();
        Play(MusicType.MAIN_MENU, MAIN_MENU_FADE_IN);
    }

    private void SetUpPlayersProperties()
    {
        // Init
        AudioStreamPlayer lCurrentPlayer;
        
        // Create Sound Effect Players
        for (int lCurrentSoundPlayerIndex = 0; lCurrentSoundPlayerIndex < soundPlayerCount; lCurrentSoundPlayerIndex++)
            // Init
            lCurrentPlayer = CreatePlayer(soundPlayers, Bus.SoundEffect);

        // Create Musics Players
        for (int lCurrentMusicPlayerIndex = 0; lCurrentMusicPlayerIndex < musicPlayerCount; lCurrentMusicPlayerIndex++)
        {
            // Init
            lCurrentPlayer = CreatePlayer(musicPlayers, Bus.Music);
            // Set Signals
            lCurrentPlayer.Connect(SIGNAL_AUDIO_FINISHED, this, nameof(ReplaySound));
        }

        AddChild(transitionTween);
    }

    private AudioStreamPlayer CreatePlayer(List<AudioStreamPlayer> pContainer, Bus pBus)
    {
        // Init
        AudioStreamPlayer lCurrentPlayer = new AudioStreamPlayer();
        // Add To Container
        pContainer.Add(lCurrentPlayer);
        AddChild(lCurrentPlayer);
        // Set Bus
        lCurrentPlayer.Bus = pBus.ToString();

        return lCurrentPlayer;
    }


    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // PLAY

    private AudioStreamPlayer Play(AudioStream pSound, bool pIsLooping, AudioStreamPlayer pPlayer)
    {
        pPlayer.Stream = pSound;
        pPlayer.Play();
        return pPlayer;
    }

    private void ReplaySound() { foreach (AudioStreamPlayer lCurrentPlayer in musicPlayers) lCurrentPlayer.Play(); }

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // SOUNDS

    /// <summary>
    /// Play sound effect.
    /// </summary>
    /// <param name="pType">Sound effct to play.</param>
    public void Play(SoundType pType)
    {
        Play(allSounds[(int)pType], false, soundPlayers[currentSoundPlayerIndex]);
        currentSoundPlayerIndex = (int)(++currentSoundPlayerIndex % soundPlayers.Count);
    }

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // MUSICS


    /// <summary>
    /// Play music. It will loop.
    /// </summary>
    /// <param name="pType">>Music to play.</param>
    /// <param name="pTransitionDuration">>Fade in duration.</param>
    public void Play(MusicType pType, float pTransitionDuration = TRANSITION_DEFAULT_DURATION)
    {
        // Init Audio Properties
        AudioStreamPlayer lOldPlayer = musicPlayers[currentMusicPlayerIndex];
        currentMusicPlayerIndex = (int)(++currentMusicPlayerIndex % musicPlayers.Count);
        // Get The Music
        AudioStreamPlayer lNewPlayer = Play(allMusics[(int)pType], true, musicPlayers[currentMusicPlayerIndex]);
        // Play Transition
        transitionTween.InterpolateProperty(lOldPlayer, PROPERTY_VOLUME_DB, lOldPlayer.VolumeDb, VOLUME_DB_MIN, pTransitionDuration);
        transitionTween.InterpolateProperty(lNewPlayer, PROPERTY_VOLUME_DB, VOLUME_DB_MIN, VOLUME_DB_DEFAULT, pTransitionDuration);
        lNewPlayer.VolumeDb = VOLUME_DB_MIN;
        transitionTween.Start();
    }
}