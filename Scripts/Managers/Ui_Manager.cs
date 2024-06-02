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
        [Export] NodePath HelpButtonPath;

        [Export] PackedScene sceneHelpBox;
        HelpOutside helpBox;

        Button HelpButton;

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

            HelpButton = GetNode<Button>(HelpButtonPath);
            HelpButton.Connect("pressed", this, nameof(OnHelpPressed));
        }

        public void MainMenu()
        {

        }

        private void OnPlayPressed()
        {
            GD.Print("play");
        }

        private void OnSettingPressed()
        {
            GD.Print("Setting");
        }

        private void OnQuitPressed()
        {
            GetTree().Quit();
        }
        private void OnHelpPressed()
        {
            if (helpBox == null)
            {
                helpBox = sceneHelpBox.Instance<HelpOutside>();
                AddChild(helpBox);
            }
            helpBox.Visible = true;
        }
        private void UpdateHud()
        {

        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}