using Godot;

// Autor : Dylan Lupon
// Date : 06 / 02 / 2024
public class Settings : Control
{
    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // VARIABLES

    // SIGNALS
    private const string SIGNAL_VALUE_CHANGED = "value_changed";

    // AUDIO
    [Export] private NodePath matserSliderPath;
    [Export] private NodePath musicSliderPath;
    [Export] private NodePath soundSliderPath;
    private HSlider masterSlider;
    private HSlider musicSlider;
    private HSlider soundSlider;

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // INITIALIZATION

    public override void _Ready()
    {
        SetNodes();
        ConnectSignals();
    }

    private void SetNodes()
    {
        masterSlider = (HSlider)GetNode(matserSliderPath);
        musicSlider = (HSlider)GetNode(musicSliderPath);
        soundSlider = (HSlider)GetNode(soundSliderPath);
    }

    private void ConnectSignals()
    {
        masterSlider.Connect(SIGNAL_VALUE_CHANGED, this, nameof(SetVolumeMaster));
        musicSlider.Connect(SIGNAL_VALUE_CHANGED, this, nameof(SetVolumeMusic));
        soundSlider.Connect(SIGNAL_VALUE_CHANGED, this, nameof(SetVolumeSound));
    }

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // VOLUMES

    private void SetVolume(SoundManager.Bus pBus, float pVolume) => AudioServer.SetBusVolumeDb((int)pBus, pVolume);

    private void SetVolumeMaster(float pValue)
    {
        SetVolume(SoundManager.Bus.Master, pValue);
    }


    private void SetVolumeMusic(float pValue)
    {
        SetVolume(SoundManager.Bus.Music, pValue);
    }

    private void SetVolumeSound(float pValue)
    {
        SetVolume(SoundManager.Bus.SoundEffect, pValue);
    }
}
