using Godot;
using System;
using System.Collections.Generic;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName.Game
{
	
    public class ResourceManager : Node
    {
        static private ResourceManager instance;

        [Export] public int initailIron = 5;
        [Export] public int initailFood = 5;

        [Export] public int initailSettler = 5;        
        

        public int irons { get; set; }
        public int foods { get; set; }

        public int settlers { get; set; }

        public abstract class Building
        {
            public abstract int IronCost { get; }
            public abstract int FoodCost { get; }
        }

        public class House : Building
        {
            public override int IronCost => 1;
            public override int FoodCost => 5;
        }

        public class School : Building
        {
            public override int IronCost => 5;
            public override int FoodCost => 1;
        }

        static public ResourceManager GetInstance () {
			if (instance == null) instance = new ResourceManager();
		    return instance;
		}

        private ResourceManager ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(ResourceManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;


        }

        public bool chekRverificationOfRonstructionResources(Cell.CellType cellType)
        {
            if (cellType == Cell.CellType.Empty)
            {
                House EmptyTile = new House();
                if (EmptyTile.IronCost >= irons && EmptyTile.FoodCost >= foods) return true;
            }

            return false;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}