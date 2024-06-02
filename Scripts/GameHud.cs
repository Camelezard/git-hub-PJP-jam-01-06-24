using Godot;
using System;
using Com.IsartDigital.ProjectName.Game;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName{
	
    public class GameHud : Control
    {
        static private GameHud instance;

        public Label ironLabel;
        public Label foodLabel;

        public Label settlerLabel;
        public Label acctionLabel;

        public Label BlsckHoleCooldownLabel;

        [Export] NodePath ironLabelPath;
        [Export] NodePath foodLabelPath;
        
        [Export] NodePath acctionLabelPath;
        [Export] NodePath settlerLabelPath;
        
        [Export] NodePath BlsckHoleCooldownLabelPath;

        static public GameHud GetInstance () {
			if (instance == null) instance = new GameHud();
		    return instance;
		}

        private GameHud ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(GameHud) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            ironLabel = GetNode<Label>(ironLabelPath);
            foodLabel = GetNode<Label>(foodLabelPath);

            settlerLabel = GetNode<Label>(settlerLabelPath);
            acctionLabel = GetNode<Label>(acctionLabelPath);

            BlsckHoleCooldownLabel = GetNode<Label>(BlsckHoleCooldownLabelPath);
        }


        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}