using Godot;
using System;

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

        public override void _Ready()
		{
			playButton = GetNode<Button>(PlayButtonPath);
			settingButton = GetNode<Button>(SettingButtonPath);
			quitButton = GetNode<Button>(QuitButtonPath);
		}

	}
}