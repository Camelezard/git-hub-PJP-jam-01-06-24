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
        private const string PROPERTY_RECT_SCALE = "rect_scale";
        private const string PROPERTY_RECT_POSITION = "rect_position";
        private const string PROPERTY_RECT_ROTATION = "rect_rotation";
        private const float ANIMATION_DELAY_MAX = .5f;
        private const float ANIMATION_DURATION = 1.25f;
        private const float ANIMATION_VORTEX_END_ROTATION = 720f;


        // Properties
        [Export] private NodePath vortexPath;
        [Export] private NodePath singularityPath;
        [Export] private List<NodePath> allAnimatedObjectPath;
        private List<Control> allAnimatedObject = new List<Control>();
        private Control singularity;
        private Control vortex;
        private Tween animation = new Tween();


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

            SetAnimationProperties();
        }


        private void PlayPressed()
        {
            Main.instance.startLevelOne();
            Hide();
        }

        private void SettingPressed()
        {

        }

        private void QuitPressed()
        {
            GetTree().Quit();
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // ANIMATIONS

        private void SetAnimationProperties()
        {
            GD.Randomize();
            AddChild(animation);
            singularity = (Control)GetNode(singularityPath);
            vortex = (Control)GetNode(vortexPath);
            vortex.RectPivotOffset = vortex.RectSize * .5f;
            foreach (NodePath lCurrentObjectPath in allAnimatedObjectPath)
                allAnimatedObject.Add((Control)GetNode(lCurrentObjectPath));
        }

        private void AnimationPlayButton()
        {
            Vector2 lSingularity = singularity.RectPosition;

            foreach (Control lCurrentObject in allAnimatedObject)
                AnimationToBlackHole(lCurrentObject, lSingularity);

            animation.InterpolateProperty
                (
                    vortex,
                    PROPERTY_RECT_ROTATION,
                    vortex.RectRotation,
                    ANIMATION_VORTEX_END_ROTATION,
                    ANIMATION_DURATION + ANIMATION_DELAY_MAX,
                    Tween.TransitionType.Elastic,
                    Tween.EaseType.InOut
                );

            animation.Start();

            animation.InterpolateCallback(this, ANIMATION_DURATION + ANIMATION_DELAY_MAX, nameof(PlayPressed));

            SoundManager.GetInstance().Play(SoundManager.MusicType.IN_GAME, SoundManager.TRANSITION_NORMAL_DURATION);
        }

        private void AnimationToBlackHole(Control lObject, Vector2 lSingularity)
        {
            float lCurrentDuration = ANIMATION_DURATION - ANIMATION_DELAY_MAX * GD.Randf();
            float lCurrentDelay =ANIMATION_DELAY_MAX * GD.Randf();

            animation.InterpolateProperty
                (
                lObject,
                PROPERTY_RECT_POSITION,
                lObject.RectPosition,
                lSingularity,
                lCurrentDuration,
                Tween.TransitionType.Expo,
                Tween.EaseType.In,
                delay: lCurrentDelay
                );

            animation.InterpolateProperty
                (
                lObject,
                PROPERTY_RECT_SCALE,
                lObject.RectScale,
                Vector2.Zero,
                lCurrentDuration,
                Tween.TransitionType.Back,
                Tween.EaseType.In,
                delay: lCurrentDelay
                );
        }
	}
}