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

        [Export] NodePath IronCountLabelPath;
        [Export] NodePath FoodCountLabelPath;

        [Export] NodePath seltterCountLabelPath;
        [Export] NodePath BlackHoleTurnLabelPath;

        [Export] NodePath AcctionLabelPath;

        Button PlayButton;
        Button SetingButton;
        Button QuitButton;

        Label IronCountLabel;
        Label FoodCountLabel;

        Label seltterCountLabel;
        Label BlackHoleTurnLabel;

        Label AcctionLabel;

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

            PlayButton = GetNode<Button>(PlayButtonPath);
            SetingButton = GetNode<Button>(SetingButtonPath);
            QuitButton = GetNode<Button>(QuitButtonPath);

            IronCountLabel = GetNode<Label>(IronCountLabelPath);
            FoodCountLabel = GetNode<Label>(FoodCountLabelPath);

            seltterCountLabel = GetNode<Label>(seltterCountLabelPath);
            BlackHoleTurnLabel = GetNode<Label>(BlackHoleTurnLabelPath);
            AcctionLabel = GetNode<Label>(AcctionLabelPath);

            //PlayButton.Connect("pressed", this, nameof(onPlayPressed));
            //SetingButton.Connect("pressed", this, nameof(onSettingPressed));
            //QuitButton.Connect("pressed", this, nameof(onQuitPressed));

            UpdateHud();

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

        public void UpdateHud()
        {
            IronCountLabel.Text = ResourceManager.GetInstance().iron.ToString();
            FoodCountLabel.Text = ResourceManager.GetInstance().food.ToString();

            seltterCountLabel.Text = ResourceManager.GetInstance().settlers.ToString();

            BlackHoleTurnLabel.Text = ResourceManager.GetInstance().BlackHoleCooldown.ToString();
            AcctionLabel.Text = ResourceManager.GetInstance().acction.ToString();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}