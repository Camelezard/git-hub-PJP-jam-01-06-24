
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

        private const float START_CELL_SIZE = 100;
        private float cell_size;
        private float half_cell_size;

        public List<Cell> numberOfHouses = new List<Cell>();

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
            CreateGrid(50,50);

            initialisateTheLevelOne();
        }
        public override void _Process(float delta)
        {
            //GD.Print(VectorToGrid(GetGlobalMousePosition()));

            if(Input.IsActionJustPressed("ui_accept"))
            {
                Cell lCel = grid[VectorToGrid(GetGlobalMousePosition()).x, VectorToGrid(GetGlobalMousePosition()).y];
                lCel.Rotation = 1;

            }
        }

        private void OnScreenSizeChanged()
        {
        }

        #region CellHandle
        [Export] PackedScene cellFactory;

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
            Cell cell = cellFactory.Instance<Cell>();
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

        private void initialisateTheLevelOne()
       {
           List<string> lMap = new List<string>
           {
               "     XXWXXX     ",
               "   XXWXXXXXWX   ",
               " #XXXXX#XXXXWXX ",
               "WXXX#XXXXXWXX#XX",
               "XXXWXXWXHHXXX#XX",
               "XX##XXX#HHWWXXXX",
               "XXXXXWXXXXX#XXXW",
               "XX#XXXXWXXXXWXXX",
               " XXXXWXXX#XXXXX ",
               "   XWXXXXWXXX   ",
               "     XXWXXX     "
           };
           
           CellType lCellType = CellType.Void;
            

           for (int y = lMap.Count-1; y >= 0; y--)
           {
               string line = lMap[y];
               for (int x = 0; x < line.Length; x++)
               {
                   char character = line[x];

                   switch (character)
                   {
                        case ' ':
                            lCellType = CellType.Void;
                            break;
                        case 'X':
                            lCellType = CellType.Empty;
                            break;
                        case '#':
                           lCellType = CellType.IronSpot;
                           break;
                       case 'W':
                           lCellType = CellType.FoodSpot;
                           break;
                        case 'H':
                            lCellType = CellType.House;
                            break;
                        default:
                           break;
                   }

                    grid[x,y] = CreateCell(lCellType, new Coordinates(x,y));

                    if (lCellType == CellType.House)
                    {
                        numberOfHouses.Add(grid[x, y]);
                        GD.Print(numberOfHouses.Count);
                        grid[x, y].CellSprite.Offset = new Vector2(0, -15);
                    }
                    else if (lCellType == CellType.IronSpot) grid[x, y].CellSprite.Offset = new Vector2(0, -1);
                }
           }
       }

        private void checkNumberOfHouseInTheWorld()
        {

        }
    }
    
}