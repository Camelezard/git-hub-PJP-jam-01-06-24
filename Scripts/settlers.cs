using Godot;
using System;
using Com.IsartDigital.CCM.Managers;
using Com.IsartDigital.ProjectName.Game;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName{
	
    public class settlers : Area2D
    {

        [Export]private float speed = 200;
        [Export]private float pv = 200;
        [Export] NodePath ProgressBarPath;

        private Vector2 InitialPos = Vector2.Zero;

        ProgressBar lProgressBar;

        private settlers ():base() {}

        public override void _Ready()
        {
            lProgressBar = GetNode<ProgressBar>(ProgressBarPath);
            lProgressBar.Value = pv;
            lProgressBar.MaxValue = pv;

            ZIndex = 2;

            InitialPos = GlobalPosition;
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

            Cell actualCell = GridManager.GetInstance().SettlersConcertion(GlobalPosition);

            if (actualCell.cellType == Cell.CellType.Void) QueueFree();
            else if (actualCell.cellType == Cell.CellType.House && GlobalPosition.x > InitialPos.x+100)
            { 
                QueueFree();
                actualCell.settlersin++;
                actualCell.ShowTheNumberOfSettlerIn();
            }

            Position += new Vector2(speed,0) * delta;
            lProgressBar.Value = pv;

            if (pv <= 0) QueueFree();

            pv--;
        }

    }
}