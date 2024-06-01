using Godot;
using System;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName.Game
{
    public class Ui_Manager : CanvasLayer
    {
        static private Ui_Manager instance;

        [Export] NodePath PlayButtonPath;
        [Export] NodePath SetingButtonPath;
        [Export] NodePath QuitButtonPath;

        Button PlayButton;
        Button SetingButton;
        Button QuitButton;

		static public Ui_Manager GetInstance () {
			if (instance == null) instance = new Ui_Manager();
		    return instance;
		}

        private Ui_Manager ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(Ui_Manager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            PlayButton = GetNode<Button>(PlayButtonPath);
            SetingButton = GetNode<Button>(SetingButtonPath);
            QuitButton = GetNode<Button>(QuitButtonPath);

            PlayButton.Connect("pressed", this, nameof(onPlayPressed));
            SetingButton.Connect("pressed", this, nameof(onSettingPressed));
            QuitButton.Connect("pressed", this, nameof(onQuitPressed));
        }

        private void onPlayPressed()
        {
            GD.Print("play");
        }

        private void onSettingPressed()
        {
            GD.Print("Setting");
        }

        private void onQuitPressed()
        {
            GetTree().Quit();
        }

        private void acctualizeTheHud()
        {

        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}