using Godot;
using System;
using Com.IsartDigital.ProjectName.Game;

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

            playButton.Connect("pressed", this, nameof(PlayPressed));
            settingButton.Connect("pressed", this, nameof(SettingPressed));
            quitButton.Connect("pressed", this, nameof(QuitPressed));
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
	}
}