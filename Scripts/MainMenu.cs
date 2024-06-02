using Godot;
using System;
using Com.IsartDigital.ProjectName.Game;
using System.Collections.Generic;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName {
	
	public class MainMenu : Control
	{
        [Export] private NodePath PlayButtonPath;
        [Export] private NodePath SettingButtonPath;
        [Export] private NodePath QuitButtonPath;

        private Button playButton;
        private Button settingButton;
        private Button quitButton;

        public Button PlayButton { get => playButton; }
        public Button SettingButton { get => settingButton;  }
        public Button QuitButton { get => quitButton; }

        public static MainMenu instance;

        // ANIMATIONS
        // Settings
        private const string PROPERTY_MODULATE = "modulate";
        private const string PROPERTY_RECT_SCALE = "rect_scale";
        private const string PROPERTY_RECT_GLOBAL_POSITION = "rect_global_position";
        private const string PROPERTY_RECT_ROTATION = "rect_rotation";
        private const float ANIMATION_DELAY_MAX = .5f;
        private const float ANIMATION_DURATION = 1.25f;
        private const float ANIMATION_VORTEX_END_ROTATION = -720f;
        private const float ANIMATION_BLACK_HOLE_DURATION = 2f;
        private const float ANIMATION_BLACK_HOLE_END_SCALE = 1.05f;
        private const float ANIMATION_BLACK_HOLE_END_SCALE_B = 5f;
        private const float ANIMATION_HIDE_DURATION = 1f;

        // Properties
        [Export] private NodePath vortexPath;
        [Export] private NodePath blackHolePath;
        [Export] private NodePath singularityPath;
        [Export] private List<NodePath> allAnimatedObjectPath;
        private List<Control> allAnimatedObject = new List<Control>();
        private Control vortex;
        private Control blackHole;
        private Control singularity;
        private Tween animation = new Tween();
        private Tween blackHoleAnimation = new Tween();


        static public MainMenu GetInstance()
        {
            if (instance == null) instance = new MainMenu();
            return instance;
        }

        public override void _Ready()
		{
			playButton = GetNode<Button>(PlayButtonPath);
			settingButton = GetNode<Button>(SettingButtonPath);
			quitButton = GetNode<Button>(QuitButtonPath);

            playButton.Connect("pressed", this, nameof(AnimationPlayButton));
            settingButton.Connect("pressed", this, nameof(SettingPressed));
            quitButton.Connect("pressed", this, nameof(QuitPressed));
            settingButton.Connect("pressed", this, nameof(PlayClick));
            quitButton.Connect("pressed", this, nameof(PlayClick));

            SetAnimationProperties();
        }


        private void PlayPressed()
        {
            Main.instance.startLevelOne();
        }

        private void HideAll() => Hide();

        private void SettingPressed()
        {

        }

        private void QuitPressed()
        {
            GetTree().Quit();
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Sounds

        private void PlayClick() => SoundManager.GetInstance().Play(SoundManager.SoundType.BUTTON_CLICK);

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // ANIMATIONS

        // PLAY
        private void SetAnimationProperties()
        {
            GD.Randomize();
            AddChild(animation);
            AddChild(blackHoleAnimation);
            singularity = (Control)GetNode(singularityPath);
            blackHole = (Control)GetNode(blackHolePath);
            vortex = (Control)GetNode(vortexPath);
            vortex.RectPivotOffset = vortex.RectSize * .5f;
            blackHole.RectPivotOffset = blackHole.RectSize * .5f;
            foreach (NodePath lCurrentObjectPath in allAnimatedObjectPath)
                allAnimatedObject.Add((Control)GetNode(lCurrentObjectPath));

            AnimationBlackHole();
        }

        private void AnimationBlackHole()
        {
            blackHoleAnimation.InterpolateProperty
                (
                blackHole,
                PROPERTY_RECT_SCALE,
                blackHole.RectScale,
                blackHole.RectScale * ANIMATION_BLACK_HOLE_END_SCALE,
                ANIMATION_BLACK_HOLE_DURATION * .5f,
                Tween.TransitionType.Back,
                Tween.EaseType.InOut
                );
            blackHoleAnimation.InterpolateProperty
                (
                blackHole,
                PROPERTY_RECT_SCALE,
                blackHole.RectScale * ANIMATION_BLACK_HOLE_END_SCALE,
                blackHole.RectScale,
                ANIMATION_BLACK_HOLE_DURATION * .5f,
                Tween.TransitionType.Back,
                Tween.EaseType.InOut,
                delay: ANIMATION_BLACK_HOLE_DURATION * .5f
                );

            blackHoleAnimation.InterpolateProperty
                (
                vortex,
                PROPERTY_RECT_ROTATION,
                0,
                ANIMATION_VORTEX_END_ROTATION * .5f,
                ANIMATION_BLACK_HOLE_DURATION
                );

            blackHoleAnimation.InterpolateCallback(this, ANIMATION_BLACK_HOLE_DURATION, nameof(AnimationBlackHole));
            blackHoleAnimation.Start();
        }

        private void AnimationPlayButton()
        {

            SoundManager.GetInstance().Play(SoundManager.SoundType.BLACKHOLE_ASPIRATION);

            foreach (Control lCurrentObject in allAnimatedObject)
                AnimationToBlackHole(lCurrentObject);

            blackHoleAnimation.QueueFree();

            animation.InterpolateProperty
                (
                    vortex,
                    PROPERTY_RECT_ROTATION,
                    vortex.RectRotation,
                    vortex.RectRotation + ANIMATION_VORTEX_END_ROTATION,
                    ANIMATION_DURATION + ANIMATION_DELAY_MAX,
                    Tween.TransitionType.Sine,
                    Tween.EaseType.Out
                );

            animation.InterpolateProperty
               (
                   blackHole,
                   PROPERTY_RECT_SCALE,
                   blackHole.RectScale,
                   blackHole.RectScale * ANIMATION_BLACK_HOLE_END_SCALE_B,
                   ANIMATION_DURATION,
                   Tween.TransitionType.Sine,
                   Tween.EaseType.InOut,
                   delay: ANIMATION_DELAY_MAX
               );

            animation.InterpolateCallback(this, ANIMATION_DURATION + ANIMATION_DELAY_MAX, nameof(PlayPressed));

            animation.InterpolateProperty
                (
                this,
                PROPERTY_MODULATE,
                Modulate,
                new Color(Modulate, 0f),
                ANIMATION_HIDE_DURATION,
                delay: ANIMATION_DURATION
                );

            animation.InterpolateCallback(this, ANIMATION_DURATION + ANIMATION_DELAY_MAX + ANIMATION_HIDE_DURATION, nameof(HideAll));

            animation.Start();

            SoundManager.GetInstance().Play(SoundManager.MusicType.IN_GAME, SoundManager.TRANSITION_SHORT_DURATION);
        }

        private void AnimationToBlackHole(Control pObject)
        {

            float lCurrentDuration = ANIMATION_DURATION - ANIMATION_DELAY_MAX * GD.Randf();
            float lCurrentDelay = ANIMATION_DELAY_MAX * GD.Randf();

            animation.InterpolateProperty
                (
                pObject,
                PROPERTY_RECT_GLOBAL_POSITION,
                pObject.RectGlobalPosition,
                singularity.RectGlobalPosition - pObject.RectPivotOffset,
                lCurrentDuration,
                Tween.TransitionType.Expo,
                Tween.EaseType.In,
                delay: lCurrentDelay
                );

            animation.InterpolateProperty
                (
                pObject,
                PROPERTY_RECT_SCALE,
                pObject.RectScale,
                Vector2.Zero,
                lCurrentDuration,
                Tween.TransitionType.Back,
                Tween.EaseType.In,
                delay: lCurrentDelay
                );

            animation.InterpolateProperty
                (
                pObject,
                PROPERTY_RECT_ROTATION,
                pObject.RectRotation,
                360f - 720f * GD.Randf(),
                lCurrentDuration,
                Tween.TransitionType.Expo,
                Tween.EaseType.In,
                delay: lCurrentDelay
                );
        }
    }
}