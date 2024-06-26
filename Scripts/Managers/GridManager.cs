
using Com.IsartDigital.ProjectName.Game;
using Com.IsartDigital.ProjectName.Utils;
using Godot;
using System.Collections.Generic;
using static Com.IsartDigital.ProjectName.Game.Cell;
using Com.IsartDigital.ProjectName;
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

        private Vector2 tilemapSize = new Vector2();

        [Export] PackedScene UpgradParticleFactory;

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

        public Cell[,] grid; // [0,0] is bottom left (the blackhole); 
        private Node2D gridContainer;

        Cell FarestHouse;

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

            serchCraftTile();

            checkFarestHouse();
        }
        public override void _Process(float delta)
        {
            //GD.Print(VectorToGrid(GetGlobalMousePosition()));

            if(Input.IsActionJustPressed("left_clic") && ConstructionBox.instance == null)
            {
                checkTheTileSelected();
                checkNumberOfHouseInTheWorld();
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

        public int BlackHoleConcertion(Vector2 lPosition)
        {
            return VectorToGrid(lPosition).x;
        }

        public Cell SettlersConcertion(Vector2 lPosition)
        {
            return grid[ VectorToGrid(lPosition).x, VectorToGrid(lPosition).y];
        }

        private void initialisateTheLevelOne()
       {
            List<string> lMap = new List<string>
           {

               "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBXXXXXXXXXX ",
               "B X XXIXX    X    H           XXWXWXXXXXX ",
               "B   XX XH          F         XXWXWXXXXXXX ",
               "B   XFXIX X  X        XH     XXWWXWXXXXXX ",
               "B XXXX      XHI        X      XXWWXXXXXXX ",
               "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBXXXXXXXXXX "
           };

            string line = lMap[0];
            

           CellType lCellType = CellType.Void;
            
           for (int y = lMap.Count-1; y >= 0; y--)
           {
               line = lMap[y];
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
                        case 'I':
                           lCellType = CellType.IronSpot;
                           break;
                       case 'F':
                           lCellType = CellType.FoodSpot;
                           break;
                        case 'H':
                            lCellType = CellType.House;
                            break;
                        case 'B':
                            lCellType = CellType.BlackHole;
                            break;
                        case 'W':
                            lCellType = CellType.Win;
                            break;
                        default:
                           break;
                   }

                    grid[x,y] = CreateCell(lCellType, new Coordinates(x,y));

                    if (lCellType == CellType.House || lCellType == CellType.Win)
                    {
                        numberOfHouses.Add(grid[x, y]);
                        grid[x, y].CellSprite.Offset = new Vector2(0, -15);
                        grid[x, y].settlersin = 10;
                        grid[x, y].ShowTheNumberOfSettlerIn();
                    }
                    else if (lCellType == CellType.IronSpot) grid[x, y].CellSprite.Offset = new Vector2(0, -1);
                    else grid[x, y].CellSprite.Offset = Vector2.Zero;
               }
           }

            tilemapSize = new Vector2(line.Length, lMap.Count);
            GD.Print(tilemapSize);
        }

        private void checkNumberOfHouseInTheWorld()
        {
            //int lNumberOfsettlersMax = numberOfHouses.Count * 5;
            //int lNumberOfSettlers = ResourceManager.GetInstance().settlers;
            //if(lNumberOfSettlers > lNumberOfsettlersMax) { ResourceManager.GetInstance().settlers = lNumberOfsettlersMax; }
            //
            //GD.Print(ResourceManager.GetInstance().settlers);
        }

        private void checkFarestHouse()
        {
            
            //foreach (Cell house in numberOfHouses)
            //{
            //    if (house.gridCoordinates.x < 12) GD.Print(house.GlobalPosition);
            //    FarestHouse = house;
            //}
        }

        public void BlackHoleDestruction(int destructionRadiu = 1)
        {
            //for (int i = 0; i < destructionRadiu; i++)
            //{
            //    for (int j = 0; j < destructionRadiu; j++)
            //    {
            //        grid[i, j].Visible = false;
            //    }
            //}

            for (int i = 0; i < destructionRadiu - 1; i++)
            {
                for (int j = (int)tilemapSize.y - 2; j >= 1; j--)
                {
                    grid[i, j].Visible = false;
                    if (grid[i, j].cellType == CellType.House && grid[i, j].settlersin > 0)
                    {
                        ResourceManager.settlerDead += grid[i, j].settlersin;
                        grid[i, j].settlersin = 0;
                        GD.Print(ResourceManager.settlerDead);
                    }
                }
            }
        }

        private void checkTheTileSelected()
        {
            Coordinates corodoneeOfTheTile = new Coordinates( VectorToGrid(GetGlobalMousePosition()).x, VectorToGrid(GetGlobalMousePosition()).y);
            Cell lCel = grid[1,1];

            if (corodoneeOfTheTile.x >= 0 &&
                corodoneeOfTheTile.x < tilemapSize.x && 
                corodoneeOfTheTile.y >= 0 &&
                corodoneeOfTheTile.y < tilemapSize.y) lCel = grid[corodoneeOfTheTile.x, corodoneeOfTheTile.y];

            //if(grid.Co)

            switch (lCel.cellType)
            {
                case CellType.Void:

                if (corodoneeOfTheTile.x >= 0
                && corodoneeOfTheTile.y >= 0
                && corodoneeOfTheTile.x < tilemapSize.x
                && corodoneeOfTheTile.y < tilemapSize.y)
                {

                     if (grid[corodoneeOfTheTile.x - 1, corodoneeOfTheTile.y].cellType != CellType.Void ||
                     grid[corodoneeOfTheTile.x + 1, corodoneeOfTheTile.y].cellType != CellType.Void ||
                     grid[corodoneeOfTheTile.x, corodoneeOfTheTile.y - 1].cellType != CellType.Void ||
                     grid[corodoneeOfTheTile.x, corodoneeOfTheTile.y + 1].cellType != CellType.Void
                     )
                     lCel.creatConstructorBox();
                }
                    

                    break;
                case CellType.Empty:
                    break;
                case CellType.House:
                    lCel.SpawnSettler();
                    break;
                case CellType.IronSpot:
                    ResourceManager.GetInstance().ColectIron();
                    ugradeResource lupg = (ugradeResource)UpgradParticleFactory.Instance();
                    lupg.GlobalPosition = lCel.GlobalPosition;
                    lupg.cellType = ugradeResource.enumaterial.food;
                    AddChild(lupg);

                    break;
                case CellType.FoodSpot:
                    ResourceManager.GetInstance().ColectFood();
                    lupg = (ugradeResource)UpgradParticleFactory.Instance();
                    lupg.GlobalPosition = lCel.GlobalPosition;
                    lupg.cellType = ugradeResource.enumaterial.iron;
                    AddChild(lupg);
                    break;
                default:
                    break;
            }
        }

        public void serchCraftTile()
        {
            Cell lCel = grid[1, 1];
            
            for (int i = 0; i < (int)tilemapSize.x - 14; i++)
            {
                for (int j = (int)tilemapSize.y - 2; j >= 1; j--)
                {
                    if (grid[i, j].cellType == CellType.Void)
                    {
                        lCel = grid[i, j];
            
                     if ((grid[i - 1, j].cellType != CellType.Void && grid[i - 1, j].cellType != CellType.BlackHole) ||
                     (grid[i + 1, j].cellType != CellType.Void && grid[i + 1, j].cellType != CellType.BlackHole) ||
                     (grid[i, j - 1].cellType != CellType.Void && grid[i, j - 1].cellType != CellType.BlackHole) ||
                     (grid[i, j + 1].cellType != CellType.Void && grid[i, j + 1].cellType != CellType.BlackHole)
                     ) grid[i, j].CellSprite.Texture = GD.Load<Texture>("res://Assets/Textures/Graphic/Sprite/Select.png");
                    }
                }
            }
        }

        private void CanCraft()
        {
            //for (int i = 0; i < 20 - 1; i++)
            //{
            //    for (int j = (int)tilemapSize.y - 2; j >= 1; j--)
            //    {
            //        grid[i, j].CellSprite.Texture = GD.Load<Texture>("res://Assets/Textures/Graphic/Sprite/Select.png");
            //    }
            //}
            //
        }
    }
    
}