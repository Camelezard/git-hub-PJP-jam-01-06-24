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

    // BUTTONS
    [Export] private NodePath quitButtonPath;
    private Button quitButton;


    // INIT ANIMATION
    private Tween animation = new Tween();

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // INITIALIZATION

    public override void _Ready()
    {
        SetNodes();
        ConnectSignals();
        InitAnimation();
    }

    private void SetNodes()
    {
        masterSlider = (HSlider)GetNode(matserSliderPath);
        musicSlider = (HSlider)GetNode(musicSliderPath);
        soundSlider = (HSlider)GetNode(soundSliderPath);
        quitButton = (Button)GetNode(quitButtonPath);
        AddChild(animation);
    }

    private void ConnectSignals()
    {
        masterSlider.Connect(SIGNAL_VALUE_CHANGED, this, nameof(SetVolumeMaster));
        musicSlider.Connect(SIGNAL_VALUE_CHANGED, this, nameof(SetVolumeMusic));
        soundSlider.Connect(SIGNAL_VALUE_CHANGED, this, nameof(SetVolumeSound));
        quitButton.Connect("button_down", this, nameof(QuitAnimation));
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
        SetVolume(SoundManager.Bus.SoundEffects, pValue);
        SoundManager.GetInstance().Play(SoundManager.SoundType.WATER);
    }

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // QUIT

    private void QuitPressed()
    {
        QueueFree();
    }

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // ANIMATNIOS

    private void InitAnimation()
    {
        RectPivotOffset = RectSize * .5f;

        animation.InterpolateProperty
            (
                this,
                "modulate",
                new Color(0, 0, 0, 0),
                Colors.White,
                1.5f
            );

        animation.InterpolateProperty
            (
                this,
                "rect_rotation",
                45 - 90f * GD.Randf(),
                0,
                1.5f,
                Tween.TransitionType.Expo,
                Tween.EaseType.Out
            );

        animation.InterpolateProperty
            (
                this,
                "rect_position",
                RectPosition + Vector2.Up * 100f,
                RectPosition,
                1.5f,
                Tween.TransitionType.Expo,
                Tween.EaseType.Out
            );

        Modulate = new Color(0, 0, 0, 0);

        animation.Start();
    }

    private void QuitAnimation()
    {
        SoundManager.GetInstance().Play(SoundManager.SoundType.BUTTON_CLICK);
        RectPivotOffset = RectSize * .5f;

        animation.InterpolateProperty
            (
                this,
                "modulate",
                Colors.White,
                new Color(0, 0, 0, 0),
                1.5f
            );

        animation.InterpolateProperty
            (
                this,
                "rect_rotation",
                0,
                45 - 90f * GD.Randf(),
                1.5f,
                Tween.TransitionType.Expo,
                Tween.EaseType.In
            );

        animation.InterpolateProperty
            (
                this,
                "rect_position",
                RectPosition,
                RectPosition + Vector2.Up * 100f,
                1.5f,
                Tween.TransitionType.Expo,
                Tween.EaseType.In
            );

        Modulate = new Color(0, 0, 0, 0);

        animation.InterpolateCallback(this, 1.5f, nameof(QuitPressed));

        animation.Start();
    }
}
