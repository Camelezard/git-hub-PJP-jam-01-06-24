using Godot;
using System;
using Com.IsartDigital.ProjectName.Game;
using Com.IsartDigital.CCM.Managers;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName{
	
    public class Vortex : Node2D
    {
        static private Vortex instance;

        public int radius = 0;

        private Vector2 targetVector = Vector2.Zero;

        [Export] float speed = 5;
        [Export] float rotationSpeed = 5;

        static public Vortex GetInstance () {
			if (instance == null) instance = new Vortex();
		    return instance;
		}

        private Vortex ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(Vortex) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        public override void _Process(float pDelta)
        {
            GlobalPosition += new Vector2(speed,0) * pDelta;
            RotationDegrees -= rotationSpeed * pDelta;

            radius = GridManager.GetInstance().BlackHoleConcertion(GlobalPosition + Vector2.Right * 300);
            GridManager.GetInstance().BlackHoleDestruction(radius);
        }

        private void ApplyNewTarget()
        {

        }

        public void BlackHoleTurn()
        {
            
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}