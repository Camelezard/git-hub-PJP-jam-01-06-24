using Godot;
using System;
using static Com.IsartDigital.ProjectName.Game.Cell;
using Com.IsartDigital.ProjectName.Game;

//Author : Daniel Degott
namespace Com.IsartDigital.ProjectName{
	
    public class ConstructionBox : Control
    {
        static private ConstructionBox instance;
		
		static public ConstructionBox GetInstance () {
			if (instance == null) instance = new ConstructionBox();
		    return instance;
		}

        [Export] private NodePath EmptyButtonPath;
        [Export] private NodePath HouseButtonPath;
        [Export] private NodePath IronButtonPath;
        [Export] private NodePath FoodButtonPath;

        private TextureButton EmptyButton;
        private TextureButton HouseButton;
        private TextureButton IronButton;
        private TextureButton FoodButton;

        private ConstructionBox ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(ConstructionBox) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            EmptyButton = GetNode<TextureButton>(EmptyButtonPath);
            HouseButton = GetNode<TextureButton>(HouseButtonPath);
            IronButton = GetNode<TextureButton>(IronButtonPath);
            FoodButton = GetNode<TextureButton>(FoodButtonPath);

            EmptyButton.Connect("pressed",this,nameof(emptyPressed));
            HouseButton.Connect("pressed",this,nameof(housePressed));
            IronButton.Connect("pressed",this,nameof(IronPressed));
            FoodButton.Connect("pressed",this,nameof(FoodPressed));

            
        }

        private void emptyPressed()
        {
            Cell lcell = (Cell)GetParent();
            if (ResourceManager.GetInstance().canBuilThisBulding(CellType.Empty))
            {
                ResourceManager.GetInstance().PayConstructionResources(CellType.Empty);
                lcell.CreateTile(CellType.Empty);
                QueueFree();
            }

        }        
        
        private void housePressed()
        {
            Cell lcell = (Cell)GetParent();

            if (ResourceManager.GetInstance().canBuilThisBulding(CellType.House)) 
            {
                ResourceManager.GetInstance().PayConstructionResources(CellType.House);
                lcell.CreateTile(CellType.House);
                QueueFree();
            }

        }        

        private void IronPressed()
        {
            Cell lcell = (Cell)GetParent();
            if (ResourceManager.GetInstance().canBuilThisBulding(CellType.IronSpot))
            {
                ResourceManager.GetInstance().PayConstructionResources(CellType.IronSpot);
                lcell.CreateTile(CellType.IronSpot);
                QueueFree();
            }
        }    
        
        private void FoodPressed()
        {
            Cell lcell = (Cell)GetParent();
            if (ResourceManager.GetInstance().canBuilThisBulding(CellType.FoodSpot))
            {
                ResourceManager.GetInstance().PayConstructionResources(CellType.FoodSpot);
                lcell.CreateTile(CellType.FoodSpot);
                QueueFree();
            }
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}