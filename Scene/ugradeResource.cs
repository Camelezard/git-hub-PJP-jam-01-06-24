using Godot;
using System;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName{
	
    public class ugradeResource : Node2D
    {
        int liftime = 100;
        private ugradeResource ():base() {}

        [Export] private Texture FoodTextur;
        [Export] private Texture ronTextur;

        [Export] private NodePath iconPath;
        private TextureRect icon;

        public enum enumaterial
        {
            iron,food
        }
        public enumaterial cellType = enumaterial.food;

        public override void _Ready()
        {
            base._Ready();
            icon = GetNode<TextureRect>(iconPath);

            if (cellType == enumaterial.food) icon.Texture = ronTextur;
            else icon.Texture = FoodTextur;
        }

        public override void _Process(float pDelta)
        {
            GlobalPosition += Vector2.Up;
            liftime--;
            if (liftime <= 0) QueueFree();
        }


    }
}