using Godot;
using System;
using System.Collections.Generic;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName.Game
{
	
    public class ResourceManager : Node2D
    {
        static private ResourceManager instance;

        [Export] public int initailIron = 15;
        [Export] public int initailFood = 15;

        [Export] public int initailSettlers = 35;

        [Export] public int ironGain = 3;
        [Export] public int FoodGain = 5;

        [Export] public int initialBlackholeCooldown = 5;        
        

        public int iron { get; set; }
        public int food { get; set; }

        public int settlers { get; set; }
        public int acction { get; set; }

        public int BlackHoleCooldown { get; set; }

        public abstract class Building
        {
            public abstract int IronCost { get; }
            public abstract int FoodCost { get; }
        }

        public class EmptySpot : Building
        {
            public override int IronCost => 1;
            public override int FoodCost => 1;
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

        public class FoodSpot : Building
        {
            public override int IronCost => 2;
            public override int FoodCost => 0;
        }

        public class IronSpot : Building
        {
            public override int IronCost => 0;
            public override int FoodCost => 2;
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

            iron = initailIron;
            food = initailFood;

            settlers = initailSettlers;
            BlackHoleCooldown = initialBlackholeCooldown;

            StartTheTurn();
        }

        public bool canBuilThisBulding(Cell.CellType cellType)
        {
            if (cellType == Cell.CellType.Empty)
            {
                EmptySpot EmptyTile = new EmptySpot();
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.IronCost) return true;
            }
            else if (cellType == Cell.CellType.House)
            {
                House EmptyTile = new House();
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost) return true;
            }
            else if (cellType == Cell.CellType.IronSpot)
            {
                IronSpot EmptyTile = new IronSpot();
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost) return true;
            }
            else if (cellType == Cell.CellType.FoodSpot)
            {
                FoodSpot EmptyTile = new FoodSpot();
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost) return true;
            }

            return false;
        }

        public void PayConstructionResources(Cell.CellType cellType)
        {
            if (cellType == Cell.CellType.Empty)
            {
                EmptySpot EmptyTile = new EmptySpot();

                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost)
                {
                    iron -= EmptyTile.IronCost;
                    food -= EmptyTile.FoodCost;
                }
            }
            else if (cellType == Cell.CellType.Empty)
            {
                IronSpot EmptyTile = new IronSpot();
            
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost)
                {
                    iron -= EmptyTile.IronCost;
                    food -= EmptyTile.FoodCost;
                }
            }
            else if (cellType == Cell.CellType.Empty)
            {
                IronSpot EmptyTile = new IronSpot();
            
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost)
                {
                    iron -= EmptyTile.IronCost;
                    food -= EmptyTile.FoodCost;
                }
            }
            else if (cellType == Cell.CellType.Empty)
            {
                FoodSpot EmptyTile = new FoodSpot();
            
                if (iron >= EmptyTile.IronCost && food >= EmptyTile.FoodCost)
                {
                    iron -= EmptyTile.IronCost;
                    food -= EmptyTile.FoodCost;
                }
            }
            GD.Print("IronLeft = " + iron);
            GD.Print("FoodLeft = " + food);
        }

        public void ColectIron()
        {
            iron += ironGain;
            UseAcction();

            Ui_Manager.GetInstance().acctualizeTheHud();
        }

        public void ColectFood()
        {
            food += FoodGain;
            UseAcction();

            Ui_Manager.GetInstance().acctualizeTheHud();
        }

        public void CraftAtile()
        {
            UseAcction();
        }

        public void UseAcction(int acctionNumber = 1)
        {
            acction -= acctionNumber;
            if (acction <= 0) EndTheTurn();
        }
        
        public void EndTheTurn()
        {
            BlackHoleCooldown--;

            if (BlackHoleCooldown <= 0) { }//*blackHole*//
            else StartTheTurn();
        }

        public void StartTheTurn()
        {
            acction = settlers;
            food -= settlers;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}