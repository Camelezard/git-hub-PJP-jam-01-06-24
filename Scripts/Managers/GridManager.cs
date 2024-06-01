
using Com.IsartDigital.ProjectName.Game;
using Com.IsartDigital.ProjectName.Utils;
using Godot;
using System.Collections.Generic;
using static Com.IsartDigital.ProjectName.Game.Cell;
//Author : Sophia Solignac
namespace Com.IsartDigital.CCM.Managers
{

    public class GridManager : Node2D
    {
        static private GridManager instance;

        private Vector2 gridSize = Vector2.Zero;
        public Vector2 GridSize { get => gridSize;}

        private const float START_CELL_SIZE = 64;
        private float cell_size;
        private float half_cell_size; 
        
        [Export(PropertyHint.Range, "2,100")]
        public float Cell_size
        {
            get => cell_size;
            set
            {
                cell_size = value;
                half_cell_size = cell_size / 2;
            }
        }

        private Cell[,] grid; // [0,0] is bottom left (the blackhole); 
        private Node2D gridContainer;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Signals


        static public GridManager GetInstance()
        {
            if (instance == null) instance = new GridManager();
            return instance;
        }

        private GridManager() : base()
        {
            Name = GetClass();
        }

        public override void _Ready()
        {
            if (instance != null && GetNodeOrNull(GetClass()) != null)
            {
                GD.Print("Object already exist");
                QueueFree();
                return;
            }
            instance = this;
            half_cell_size = cell_size / 2;
            GD.Randomize();
            GetViewport().Connect(Signals.Ui.SCREEN_RESIZED, this, nameof(OnScreenSizeChanged));
            Cell_size = START_CELL_SIZE;
            CreateGrid(5,5);
        }
        public override void _Process(float delta)
        {
            GD.Print(VectorToGrid(GetGlobalMousePosition()));
        }

        private void OnScreenSizeChanged()
        {
        }

        #region CellHandle
        [Export] Dictionary<string, PackedScene> cellFactory = new Dictionary<string, PackedScene>();

        #endregion

        public void CreateGrid(int pSizeX,int pSizeY)
        {
            if (grid != null)
            {
                foreach (Cell lCurrentCell in grid)
                {
                    lCurrentCell.QueueFree();
                }
            }
            if (gridContainer != null) { gridContainer.QueueFree(); }
            gridContainer = new Node2D();
            AddChild(gridContainer);
            grid = new Cell[pSizeX,pSizeY];
            grid[0, 0] = CreateCell(CellType.Void,new Coordinates(0,0)); // Create the void
        }

        public Cell CreateCell(CellType pCellType, Coordinates pCoordinates)
        {
            Cell cell = cellFactory[pCellType.ToString()].Instance<Cell>();
            cell.gridCoordinates = pCoordinates; 
            cell.cellType = pCellType;
            cell.Position = GridToVector(pCoordinates);
            gridContainer.AddChild(cell);
            return cell;
        }

        Vector2 GridToVector(Coordinates pCoords)
        {
            return new Vector2(pCoords.x * cell_size + half_cell_size, -pCoords.y * cell_size - half_cell_size);
        }
        Coordinates VectorToGrid(Vector2 pCoords)
        {
            return new Coordinates(Mathf.FloorToInt((pCoords.x ) / cell_size), -Mathf.FloorToInt((pCoords.y + cell_size) / cell_size));
        }
    }
    
}